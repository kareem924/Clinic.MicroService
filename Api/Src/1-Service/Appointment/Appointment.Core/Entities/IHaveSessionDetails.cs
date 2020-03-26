using System;
using System.Collections.Generic;
using System.Text;
using Appointment.Core.Enums;

namespace Appointment.Core.Entities
{
    public interface IHaveSessionDetails
    {
        Guid ConsultantId { get; }

        ServiceType Service { get; }
    }


    public interface ISessionDetails : IBasicSessionDetails
    {
        DateTime DateTime { get; }
    }

    public interface IBasicSessionDetails
    {
        PrimaryServiceType PrimaryServiceType { get; }

        Guid? ConsultantId { get; }
    }
}
