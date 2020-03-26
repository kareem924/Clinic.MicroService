namespace Portals.Extivita.Core.Appointments.Entities.SessionChecklists
{
    public class InsideAttendantChecklist
    {
        public bool IsHoodsConnectToBibs { get; set; }

        public bool IsExhaustOnBibsControlIsOn { get; set; }

        public bool IsOxygenOnBibsUnitIsOff { get; set; }

        public bool IsEmergencyExhaustIsOff { get; set; }

        public bool IsAllPatientsHasUnOpenedWaterBottle { get; set; }

        public bool IsNoMetalIsInsideChamber { get; set; }

        public bool IsNoTrashOpenedWaterBottles { get; set; }
        public bool IsCleanBlanketsAreIsInsideChamber { get; set; }

    }
}