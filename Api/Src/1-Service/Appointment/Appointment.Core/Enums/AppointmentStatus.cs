using System.ComponentModel.DataAnnotations;

namespace Appointment.Core.Enums
{
    public enum AppointmentStatus
    {
        [Display(Name = "Not Arrived")]
        NotArrived = 1,
        [Display(Name = "Arrived")]
        CheckedIn,
        [Display(Name = "Canceled")]
        Canceled
    }
}
