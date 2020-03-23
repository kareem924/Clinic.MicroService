using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Portals.Extivita.Core.Appointments.Entities;
using Portals.Extivita.Core.Payments.Entities;
using Portals.Extivita.Core.Payments.Services;
using Portals.Extivita.SharedKernel.Domain.Repositories;
using Portals.Extivita.SharedKernel.Jobs;
using Portals.Extivita.SharedKernel.Jobs.Attributes;

namespace Portals.Extivita.Core.Appointments.Services.Jobs
{
    [DailyJob(EveryDay, SixAm)]
    //[MinutelyJob(3)]
    public class AutoPaymentJop : IPeriodicJob
    {
        private const int EveryDay = 1;
        private const int SixAm = 6;
        private readonly IRepository<Appointment> _appointmentRepository;
        private readonly ICheckoutService _checkoutService;
        private readonly IPriceCalculationService _priceCalculationService;
        private readonly ILogger<AutoPaymentJop> _logger;

        public AutoPaymentJop(
            IRepository<Appointment> appointmentRepository,
            ICheckoutService checkoutService,
            IPriceCalculationService priceCalculationService,
            ILogger<AutoPaymentJop> logger)
        {
            _appointmentRepository = appointmentRepository;
            _checkoutService = checkoutService;
            _priceCalculationService = priceCalculationService;
            _logger = logger;
        }

        public async Task RunAsync()
        {
            var todayUnpaidDives = await GetTodayDivesWithoutPayment();
            _logger.LogInformation($"Starting auto-payment found \"{todayUnpaidDives.Count()}\" unpaid dives");
            foreach (var dive in todayUnpaidDives)
            {
                await PayFor(dive);
            }
        }

        private async Task<IReadOnlyCollection<Appointment>> GetTodayDivesWithoutPayment()
        {
            var dives = await _appointmentRepository.GetAllAsync(
                AppointmentSpecification.ForTodayDivesWithoutPayment().NotCanceled());
            return dives;
        }

        private async Task PayFor(Appointment appointment)
        {
            var availablePayments = await _checkoutService.GetAppointmentPaymentMethodsAsync(appointment.Id);
            var summary = await _priceCalculationService.CalculatePriceAsync(appointment);
            var paymentMethod = summary.Total == decimal.Zero
                ? availablePayments.SingleOrDefault(payment => payment.Type == PaymentType.FreePayment)
                : GetAutoPaymentMethod(availablePayments);
            if (paymentMethod is null)
            {
                _logger.LogCritical(
                    "No auto-payment method found for patient {patientId}, appointment {appointmentId}",
                    appointment.Patient.Id,
                    appointment.Id);
                return;
            }
            _logger.LogDebug("Found payment-method {paymentMethod}", paymentMethod.Type);
            var paymentInfo = new ProcessPaymentInfo(paymentMethod);
            try
            {
                var transaction = await _checkoutService.ProcessAppointmentPaymentAsync(appointment, paymentInfo);
                _logger.LogInformation("Auto-payment is completed with {transactionId}", transaction.Id);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Auto-payment Failed to process payment.");
            }
        }

        private PaymentMethod GetAutoPaymentMethod(IReadOnlyCollection<PaymentMethod> availablePayments)
        {
            var autoPayment = availablePayments
                    .SingleOrDefault(payment => payment.Type == PaymentType.PatientCredit) ??
                availablePayments
                    .FirstOrDefault(payment => payment.Type == PaymentType.CreditCard && payment.IsDefault) ??
                availablePayments.FirstOrDefault(payment => payment.Type == PaymentType.CreditCard);
            return autoPayment;
        }
    }
}