using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Portals.Extivita.Core.Appointments.Entities;
using Portals.Extivita.Core.ClinicServices.Entities;
using Portals.Extivita.SharedKernel.Domain.Repositories;
using Portals.Extivita.SharedKernel.Domain.UnitOfWork;
using Portals.Extivita.SharedKernel.Helpers;

namespace Portals.Extivita.Core.Appointments.Services
{
    public class TherapySessionService : ITherapySessionService
    {
        private readonly IRepository<TherapySession> _therapySessionRepository;
        private readonly IRepository<TimeSlot> _timeSlotRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TherapySessionService> _logger;

        public TherapySessionService(
            IRepository<TherapySession> therapySessionRepository,
            IRepository<TimeSlot> timeSlotRepository,
            IUnitOfWork unitOfWork,
            ILogger<TherapySessionService> logger)
        {
            _therapySessionRepository = therapySessionRepository;
            _timeSlotRepository = timeSlotRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<TherapySession> GetOrCreateIfNotExistsAsync(ISessionDetails details)
        {
            var sessionSpecification = TherapySessionSpecification.ByDetailsOrderDesc(details);
            var therapySession = await _therapySessionRepository.GetSingleOrDefaultAsync(sessionSpecification);
            if (therapySession != null)
            {
                return therapySession;
            }
            var timeSlotSpecification = TimeSlotSpecification.ByDetailsOrderByDateDesc(details);
            var timeSlot = await _timeSlotRepository.GetFirstOrDefaultAsync(timeSlotSpecification);
            Check.NotNull(timeSlot, nameof(timeSlot));
            therapySession = new TherapySession(details.DateTime, timeSlot);
            _therapySessionRepository.Add(therapySession);
            _logger.LogInformation($"Created therapySession with Id = {therapySession.Id}");
            return therapySession;
        }

        public async Task<IHaveSessionDetails> GetSessionDetails(ISessionDetails details)
        {
            var sessionSpecification = TherapySessionSpecification.ByDetailsOrderDesc(details);
            var therapySession = await _therapySessionRepository.GetFirstOrDefaultAsync(sessionSpecification);
            if (therapySession != null)
            {
                return therapySession;
            }
            var timeSlotSpecification = TimeSlotSpecification.ByDetailsOrderByDateDesc(details);
            var timeSlot = await _timeSlotRepository.GetFirstOrDefaultAsync(timeSlotSpecification);
            Check.NotNull(timeSlot, nameof(timeSlot));
            return timeSlot;
        }
    }
}