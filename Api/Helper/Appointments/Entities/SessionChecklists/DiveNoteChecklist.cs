using System;
using System.Collections.Generic;
using Portals.Extivita.Core.Appointments.Entities.PatientChecklists;
using Portals.Extivita.Core.Authentication.Entities;

namespace Portals.Extivita.Core.Appointments.Entities.SessionChecklists
{
	public class DiveNoteChecklist : ChecklistWithSignature
	{
		public Depth Depth0 { get; set; }
		
		public Depth Depth16 { get; set; }
		
		public Depth Depth16_25 { get; set; }
		
		public Depth Depth25 { get; set; }
		
		public Depth Depth25_33 { get; set; }
		
		public Depth Depth33_1 { get; set; }
		
		public Depth Depth33_2 { get; set; }
		
		public Depth Depth0_2 { get; set; }
	}

	public class PreDiveChecklist : ChecklistWithSignature
	{
	}
}