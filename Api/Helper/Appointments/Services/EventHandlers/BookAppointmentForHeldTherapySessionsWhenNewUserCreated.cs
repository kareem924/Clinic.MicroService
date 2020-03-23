using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Portals.Extivita.Core.Appointments.Entities;
using Portals.Extivita.Core.Authentication.Entities;
using Portals.Extivita.Core.Authentication.Events;
using Portals.Extivita.SharedKernel.Domain.Repositories;
using Portals.Extivita.SharedKernel.Domain.UnitOfWork;

namespace Portals.Extivita.Core.Appointments.Services.EventHandlers
{
    public class BookAppointmentForHeldTherapySessionsWhenNewUserCreated : INotificationHandler<UserCreatedEvent>
    {
        private readonly IRepository<TherapySession> _therapySessionRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BookAppointmentForHeldTherapySessionsWhenNewUserCreated> _logger;

        public BookAppointmentForHeldTherapySessionsWhenNewUserCreated(
                IRepository<TherapySession> therapySessionRepository,
                IRepository<User> userRepository,
                IUnitOfWork unitOfWork,
                ILogger<BookAppointmentForHeldTherapySessionsWhenNewUserCreated> logger)
        {
            _therapySessionRepository = therapySessionRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Handle(UserCreatedEvent userCreatedEvent, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{userCreatedEvent}", userCreatedEvent);
            if (string.IsNullOrEmpty(userCreatedEvent.AnonymousUserId))
            {
                return;
            }
            var specification =
                    TherapySessionSpecification.ForOnHoldSessionsByHeldForId(userCreatedEvent.AnonymousUserId);
            var session = await _therapySessionRepository.GetSingleOrDefaultAsync(specification);
            if (session is null)
            {
                _logger.LogCritical(
                        "Couldn't find any hold sessions Held for {anonymousUserId} after {userId} created",
                        userCreatedEvent.AnonymousUserId,
                        userCreatedEvent.UserId);
                return;
            }
            var patient = await _userRepository.GetByIdAsync(Guid.Parse(userCreatedEvent.UserId));
            var appointment = new Appointment(patient);
            session.Book(appointment, Enumerable.Empty<ChamberSeat>().ToArray());
            _logger.LogInformation("Book {appointment}", appointment);
            await _unitOfWork.CompleteAsync();
        }
    }
}
