using Portals.Extivita.Core.Appointments.Entities;
using Portals.Extivita.Core.Payments.Entities;
using Portals.Extivita.Core.Payments.Services;
using Portals.Extivita.SharedKernel.Domain.Repositories.Specification;
using Portals.Extivita.SharedKernel.Domain.Repositories.Specification.Sorting;
using System;
using System.Linq;

namespace Portals.Extivita.Core.Appointments.Services
{
    public sealed class OrderItemSpecification : Specification<OrderedItemBase>
    {
        public OrderItemSpecification()
        {
            AddIncludes();
        }

        public static OrderItemSpecification ById(Guid id)
        {
            var result = new OrderItemSpecification();
            result.Where(orderItem => orderItem.Id == id);
            return result;
        }

        public static OrderItemSpecification ByTransactionId(Guid transactionId)
        {
            var result = new OrderItemSpecification();
            result.Where(
                orderedItem => orderedItem.Transactions
                    .Any(transaction => transaction.Id == transactionId));

            return result;
        }

        public static OrderItemSpecification ForFilter(
           TransactionFilter filter,
           Guid? userId = null)
        {
            var specification = new OrderItemSpecification
            {

            };
            if (userId.HasValue)
            {
                specification.Where(orderItem => orderItem.Patient.Id == userId);
            }
            specification.Where(orderItem => orderItem.Transactions.Any());
            ApplyFilter(specification, filter);
            return specification;
        }

        private static void ApplyFilter(OrderItemSpecification specification, TransactionFilter filter)
        {
            if (filter.From.HasValue)
            {
                specification.Where(transaction => transaction.CreationDate.Date >= filter.From.Value.Date);
            }
            if (filter.To.HasValue)
            {
                specification.Where(transaction => transaction.CreationDate <= filter.To.Value.Date);
            }
            if (filter.paymentMethod.HasValue)
            {
                specification.Where(orderItem => orderItem.Transactions
                    .Any(Transaction =>
                         Transaction.PaymentMethod.Type == filter.paymentMethod));
            }
            if (filter.Status.HasValue)
            {
                specification.Where(orderItem => orderItem.Transactions
                   .Any(Transaction =>
                        filter.Status == TransactionStatus.All ||
                        Transaction.Status == filter.Status));
            }
            if (!string.IsNullOrEmpty(filter.Patient))
            {
                filter.Patient = filter.Patient.Trim();
                specification.Where(orderItem =>
                    orderItem.Patient.FirstName == filter.Patient ||
                    orderItem.Patient.LastName == filter.Patient ||
                    orderItem.Patient.Email == filter.Patient);
            }
        }

        private void AddIncludes()
        {
            this
                .Include(orderItem => orderItem.Patient)
                .Include(orderItem => orderItem.Transactions)
                    .ThenInclude(transaction => transaction.PaymentMethod)
                .Include(orderItem => (orderItem as Appointment).TherapySession.PrimaryService)
                .Include(orderItem => (orderItem as Appointment).TherapySession.Chamber)
                .Include(orderItem => (orderItem as Appointment).TherapySession.Room)
                .Include(orderItem => (orderItem as OrderedPackage).Package);
        }

    }
}
