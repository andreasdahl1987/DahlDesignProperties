namespace DahlDesign.Plugin.Categories
{
    public class Dashboard
    {
        private readonly DahlDesign Base;

 
        public Dashboard(DahlDesign dahlDesign)
        {
            Base = dahlDesign;

            Base.AddProp("ShowMapEnabled", Base.Settings.ShowMapEnabled);
            Base.AddProp("ShowBrakeThrottleGaugesEnabled", Base.Settings.ShowBrakeThrottleGaugesEnabled);

            Base.AddProp("SW1Enabled",  Base.Settings.SW1Enabled);
            Base.AddProp("DashLEDEnabled", Base.Settings.DashLEDEnabled);
            Base.AddProp("DashType", Base.Settings.DashType);
            Base.AddProp("LapInfoScreen", Base.Settings.LapInfoScreen);
            Base.AddProp("ShiftTimingAssist", Base.Settings.ShiftTimingAssist);
            Base.AddProp("ShiftWarning", Base.Settings.ShiftWarning);
            Base.AddProp("ARBswapped", Base.Settings.SupercarSwapPosition);
            Base.AddProp("ARBstiffForward", Base.Settings.SupercarARBDirection);
            Base.AddProp("CenterDashType", "");
            Base.AddProp("MenuType", "");

            Base.AddAction(
                "Controls.MapToggle",
                (a, b) => Base.Settings.ShowMapEnabled = !Base.Settings.ShowMapEnabled);

            Base.AddAction(
                "Controls.ShowBrakeThrottleGaugesToggle",
                (a, b) => Base.Settings.ShowBrakeThrottleGaugesEnabled = !Base.Settings.ShowBrakeThrottleGaugesEnabled);
        }

        public void DataUpdate()
        {
            Base.SetProp("ShowMapEnabled", Base.Settings.ShowMapEnabled);
            Base.SetProp("ShowBrakeThrottleGaugesEnabled", Base.Settings.ShowBrakeThrottleGaugesEnabled);

            if (Base.counter != 2)
                return;


            Base.SetProp("DDUstartLED", Base.Settings.DDUstartLED);
            Base.SetProp("SW1startLED", Base.Settings.SW1startLED);
            Base.SetProp("DDUEnabled", Base.Settings.DDUEnabled);
            Base.SetProp("SW1Enabled", Base.Settings.SW1Enabled);
            Base.SetProp("DashLEDEnabled", Base.Settings.DashLEDEnabled);
            Base.SetProp("DashType", Base.Settings.DashType);
            Base.SetProp("LapInfoScreen", Base.Settings.LapInfoScreen);
            Base.SetProp("ShiftTimingAssist", Base.Settings.ShiftTimingAssist);
            Base.SetProp("ShiftWarning", Base.Settings.ShiftWarning);
            Base.SetProp("ARBswapped", Base.Settings.SupercarSwapPosition);
            Base.SetProp("ARBstiffForward", Base.Settings.SupercarARBDirection);
            Base.SetProp("SmallFuelIncrement", Base.Settings.SmallFuelIncrement);
            Base.SetProp("LargeFuelIncrement", Base.Settings.LargeFuelIncrement);
            Base.SetProp("CoupleInCarToPit", Base.Settings.CoupleInCarToPit);
        }
    }
}
