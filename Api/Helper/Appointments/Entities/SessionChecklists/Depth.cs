using System;

namespace Portals.Extivita.Core.Appointments.Entities.SessionChecklists
{
	public struct Depth
	{
		public TimeSpan Time { get; set; }
		
		public float Temprature { get; set; }
		
		public float O2 { get; set; }
		
		public string Notes { get; set; }
	}
}