using Portals.Extivita.Core.ClinicServices.Entities;

namespace Portals.Extivita.Core.Appointments.Entities
{
    public static class Extensions
    {
        public static bool IsDive(this PrimaryServiceType serviceType)
        {
            return serviceType == PrimaryServiceType.Dive;
        }

        public static bool IsConsultation(this PrimaryServiceType serviceType)
        {
            return serviceType == PrimaryServiceType.Consultation;
        }

        public static bool IsOrientation(this PrimaryServiceType serviceType)
        {
            return serviceType == PrimaryServiceType.Orientation;
        }

        public static bool IsPhoneConsultation(this PrimaryServiceType serviceType)
        {
            return serviceType == PrimaryServiceType.PhoneConsultation;
        }

        public static bool HasSamePrescriptionDetails(this IPrescriptionDetails left, IPrescriptionDetails right)
        {
            return
                left.PressureATA == right.PressureATA &&
                left.BreathingGas == right.BreathingGas &&
                left.Duration == right.Duration;
        }
    }
}