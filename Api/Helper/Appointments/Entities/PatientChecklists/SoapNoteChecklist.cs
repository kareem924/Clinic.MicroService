using System;

namespace Portals.Extivita.Core.Appointments.Entities.PatientChecklists
{
    public class SoapNoteChecklist
    {
        public string Subjective { get; set; }
        public string Objective { get; set; }
        public string Assessment { get; set; }
        public string Plan { get; set; }
        public string Provider { get; set; }
        public string Signature { get; set; }
        public DateTime Date { get; set; }

    }
}