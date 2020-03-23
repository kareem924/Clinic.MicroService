using System;
using Portals.Extivita.Core.Authentication.Entities;
using Portals.Extivita.Core.ClinicServices.Entities;

namespace Portals.Extivita.Core.Appointments.Entities
{
    public interface IHaveSessionDetails : IPrescriptionDetails
    {
        Chamber Chamber { get; }

        Room Room { get; }

        User Employee { get; }

        PrimaryService PrimaryService { get; }
    }

    public interface IPrescriptionDetails
    {
        TimeSpan Duration { get; }

        string BreathingGas { get; }

        string PressureATA { get; }
    }

    public interface IBasicSessionDetails
    {
        PrimaryServiceType PrimaryServiceType { get; }

        Guid? RoomId { get; }

        Guid? ChamberId { get; }

        Guid? EmployeeId { get; }
    }

    public interface ISessionDetails : IBasicSessionDetails
    {
        DateTime DateTime { get; }
    }
}