using System.ComponentModel.DataAnnotations;

namespace Portals.Extivita.Core.Appointments.Enums
{
    public enum AppointmentStatus
    {
        [Display(Name = "Not Arrived")]
        NotArrived = 1,
        [Display(Name = "Arrived")]
        CheckedIn,
        [Display(Name = "Pre Dive Vital")]
        PreDiveVital,
        [Display(Name = "Medical History Change")]
        MedicalHistoryChange,
        [Display(Name = "Changing In")]
        ChangingIn,
        [Display(Name = "Ready")]
        Ready,
        [Display(Name = "In Dive")]
        InDive,
        [Display(Name = "In Consult")]
        InConsult,
        [Display(Name = "Changing Out")]
        ChangingOut,
        [Display(Name = "Checked Out")]
        CheckedOut,
        [Display(Name = "Pending - Post Dive")]
        PendingPostDive,
        Canceled
    }
}
