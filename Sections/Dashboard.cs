using System.Collections.ObjectModel;
using SimHub.Plugins.OutputPlugins.GraphicalDash;

namespace DahlDesign.Plugin.Categories
{
    public class Dashboard
    {
        private readonly DahlDesign Base;

        public Screen LeftScreen;
        public Screen RightScreen;
        public Dashboard(DahlDesign dahlDesign)
        {
            Base = dahlDesign;

            LeftScreen = new Screen(new string[] { "TimeAttack", "Sector", "LapTime", "Quali", "Race", "Race2", "Race3", "ShiftPoint", "Map" });
            RightScreen = new Screen(new string[] { "Practice", "Stint", "Q1", "Q2", "Race1", "Race2", "Ralley", "Track" });

            Base.AddProp("ShowMapEnabled", Base.Settings.ShowMapEnabled);
            Base.AddProp("ShowBrakeThrottleGaugesEnabled", Base.Settings.ShowBrakeThrottleGaugesEnabled);

            Base.AddProp("SW1Enabled", Base.Settings.SW1Enabled);
            Base.AddProp("DashLEDEnabled", Base.Settings.DashLEDEnabled);
            Base.AddProp("DashType", Base.Settings.DashType);
            Base.AddProp("LapInfoScreen", Base.Settings.LapInfoScreen);
            Base.AddProp("ShiftTimingAssist", Base.Settings.ShiftTimingAssist);
            Base.AddProp("ShiftWarning", Base.Settings.ShiftWarning);
            Base.AddProp("ARBswapped", Base.Settings.SupercarSwapPosition);
            Base.AddProp("ARBstiffForward", Base.Settings.SupercarARBDirection);
            Base.AddProp("CenterDashType", "");
            Base.AddProp("MenuType", "");
            Base.AddProp("Dashboard.LeftScreen", Base.Settings.LeftScreen);
            Base.AddProp("Dashboard.RightScreen", Base.Settings.RightScreen);
            try
            {
                LeftScreen.screenID = System.Convert.ToInt32(Base.Settings.LeftScreen);              //setup left startup screen      
            }
            catch (System.Exception) { }
            try
            {
                RightScreen.screenID = System.Convert.ToInt32(Base.Settings.RightScreen);            //setup right startup screen 
            }
            catch (System.Exception) { }

            Base.AddAction(
                "Controls.MapToggle",
                (a, b) => Base.Settings.ShowMapEnabled = !Base.Settings.ShowMapEnabled);

            Base.AddAction(
                "Controls.ShowBrakeThrottleGaugesToggle",
                (a, b) => Base.Settings.ShowBrakeThrottleGaugesEnabled = !Base.Settings.ShowBrakeThrottleGaugesEnabled);

            Base.AddAction("Controls.Dashboard.LeftScreen.Next", (a, b) => LeftScreen.Next());
            Base.AddAction("Controls.Dashboard.LeftScreen.Previous", (a, b) => LeftScreen.Previous());

            Base.AddAction("Controls.Dashboard.RightScreen.Next", (a, b) => RightScreen.Next());
            Base.AddAction("Controls.Dashboard.RightScreen.Previous", (a, b) => RightScreen.Previous());
        }

        public void DataUpdate()
        {
            Base.SetProp("ShowMapEnabled", Base.Settings.ShowMapEnabled);
            Base.SetProp("ShowBrakeThrottleGaugesEnabled", Base.Settings.ShowBrakeThrottleGaugesEnabled);

            Base.SetProp("Dashboard.LeftScreen", LeftScreen.screenID);
            Base.SetProp("Dashboard.RightScreen", RightScreen.screenID);

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
