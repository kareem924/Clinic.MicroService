using System;

namespace Portals.Extivita.Core.Appointments.Entities.SessionChecklists
{
    public abstract class ChecklistWithSignature
    {
        public string PrintName { get; set; }

        public string Signature { get; set; }

        public DateTime Date { get; set; }
    }
}