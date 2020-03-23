namespace Portals.Extivita.Core.Appointments.Entities.SessionChecklists
{
    public class ChamberOperatorChecklist
    {
        public bool IsAmDailyGaugeFilledOut { get; set; }

        public bool IsCompressorAndChillerTurnedOn { get; set; }

        public bool IsConsolePowerAndComputerTurnedOn { get; set; }

        public bool IsLoggedInToComputer { get; set; }

        public bool IsFansLightsChamberTvAndCameraTurnedOn { get; set; }

        public bool IsTempSwitchOnCool { get; set; }

        public bool IsNitrogenPurgeSwitchIsTurnedOnAtLeast5MinsPriorToTurningOnTviInsideChamber { get; set; }
    }
}