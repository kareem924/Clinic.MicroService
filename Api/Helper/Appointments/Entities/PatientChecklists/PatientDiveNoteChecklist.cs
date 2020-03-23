namespace Portals.Extivita.Core.Appointments.Entities.PatientChecklists
{
    public class PatientDiveNoteChecklist
    {
        public double Temp { get; set; }
        public double Pulse { get; set; }
        public double Resp { get; set; }
        public double BpLower { get; set; }
        public double BpHigher { get; set; }
        public double SpoTwo { get; set; }
        public bool NewEarPain { get; set; }
        public bool NewSinusPain { get; set; }
        public bool NewShortnessofBreath { get; set; }
        public bool NewVisualChanges { get; set; }
        public bool ChangeEarPain { get; set; }
        public bool ChangeSinusPain { get; set; }
        public bool ChangeShortnessofBreath { get; set; }
        public bool ChangeVisualChanges { get; set; }
        public bool ResolvedEarPain { get; set; }
        public bool ResolvedSinusPain { get; set; }
        public bool ResolvedShortnessofBreath { get; set; }
        public bool ResolvedVisualChanges { get; set; }
        public string StaffNotes { get; set; }
        public string Comment { get; set; }
        public bool RequireDoctorApproval{ get; set; }
    }
}