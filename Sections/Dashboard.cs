using System.Collections.ObjectModel;
using SimHub.Plugins.OutputPlugins.GraphicalDash;

namespace DahlDesign.Plugin.Categories
{
    public class Dashboard
    {
        private readonly DahlDesign Base;

        public Screen LeftScreen;
        public Screen RightScreen;
        public Screen DeltaScreen;

       

        public Dashboard(DahlDesign dahlDesign)
        {
            Base = dahlDesign;

            LeftScreen = new Screen(new string[] { "Time1", "Time2", "Time3", "Qualy", "Race1", "Race2", "Race3", "Practice1", "Practice2" });
            RightScreen = new Screen(new string[] { "Stint1", "Stint2", "Qualy1", "Qualy2", "Race1", "Race2", "Race3", "Track" });
            DeltaScreen = new Screen(new string[] { "LastLap", "SessionBest", "LapRecord", "FuelSave", "FuelTarget" });

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
            Base.AddProp("Dashboard.DeltaScreen", Base.Settings.DeltaScreen);

            Base.AddAction(
                "Controls.MapToggle",
                (a, b) => Base.Settings.ShowMapEnabled = !Base.Settings.ShowMapEnabled);

            Base.AddAction(
                "Controls.ShowBrakeThrottleGaugesToggle",
                (a, b) => Base.Settings.ShowBrakeThrottleGaugesEnabled = !Base.Settings.ShowBrakeThrottleGaugesEnabled);

            Base.AddAction("Controls.Dashboard.LeftScreen.Next", (a, b) => LeftScreen.Next());
            Base.AddAction("Controls.Dashboard.LeftScreen.Previous", (a, b) => LeftScreen.Previous());

            Base.AddAction("Controls.Dashboard.LeftScreen.Time1", (a, b) => LeftScreen.screenID=1);
            Base.AddAction("Controls.Dashboard.LeftScreen.Time2", (a, b) => LeftScreen.screenID=2);
            Base.AddAction("Controls.Dashboard.LeftScreen.Time3", (a, b) => LeftScreen.screenID=3);
            Base.AddAction("Controls.Dashboard.LeftScreen.Qualy", (a, b) => LeftScreen.screenID=4);
            Base.AddAction("Controls.Dashboard.LeftScreen.Race1", (a, b) => LeftScreen.screenID=5);
            Base.AddAction("Controls.Dashboard.LeftScreen.Race2", (a, b) => LeftScreen.screenID=6);
            Base.AddAction("Controls.Dashboard.LeftScreen.Race3", (a, b) => LeftScreen.screenID=7);
            Base.AddAction("Controls.Dashboard.LeftScreen.Practice1", (a, b) => LeftScreen.screenID=8);
            Base.AddAction("Controls.Dashboard.LeftScreen.Practice2", (a, b) => LeftScreen.screenID=9);

            Base.AddAction("Controls.Dashboard.RightScreen.Next", (a, b) => RightScreen.Next());
            Base.AddAction("Controls.Dashboard.RightScreen.Previous", (a, b) => RightScreen.Previous());

            Base.AddAction("Controls.Dashboard.RightScreen.Stint1", (a, b) => RightScreen.screenID=1);
            Base.AddAction("Controls.Dashboard.RightScreen.Stint2", (a, b) => RightScreen.screenID=2);
            Base.AddAction("Controls.Dashboard.RightScreen.Qualy1", (a, b) => RightScreen.screenID=3);
            Base.AddAction("Controls.Dashboard.RightScreen.Qualy2", (a, b) => RightScreen.screenID=4);
            Base.AddAction("Controls.Dashboard.RightScreen.Race1", (a, b) => RightScreen.screenID=5);
            Base.AddAction("Controls.Dashboard.RightScreen.Race2", (a, b) => RightScreen.screenID=6);
            Base.AddAction("Controls.Dashboard.RightScreen.Race3", (a, b) => RightScreen.screenID=7);
            Base.AddAction("Controls.Dashboard.RightScreen.Track", (a, b) => RightScreen.screenID=8);

            Base.AddAction("Controls.Dashboard.DeltaScreen.Next", (a, b) => DeltaScreen.Next());
            Base.AddAction("Controls.Dashboard.DeltaScreen.Previous", (a, b) => DeltaScreen.Previous());

            Base.AddAction("Controls.Dashboard.DeltaScreen.LastLap", (a, b) => DeltaScreen.screenID = 1);
            Base.AddAction("Controls.Dashboard.DeltaScreen.SessionBest", (a, b) => DeltaScreen.screenID = 2);
            Base.AddAction("Controls.Dashboard.DeltaScreen.LapRecord", (a, b) => DeltaScreen.screenID = 3);
            Base.AddAction("Controls.Dashboard.DeltaScreen.FuelSave", (a, b) => DeltaScreen.screenID = 4);
            Base.AddAction("Controls.Dashboard.DeltaScreen.FuelTarget", (a, b) => DeltaScreen.screenID = 5);
        }

        public void DataUpdate()
        {
            if (Base.gameName != "IRacing" || !Base.gameRunning) return;
        
            Base.SetProp("ShowMapEnabled", Base.Settings.ShowMapEnabled);
            Base.SetProp("ShowBrakeThrottleGaugesEnabled", Base.Settings.ShowBrakeThrottleGaugesEnabled);

            string session = Base.gameData.NewData.SessionTypeName;

            if (Base.iRacing.sessionHolder != session)
            {
                if (session == "Practice" || session == "Warmup" || session == "Offline testing")
                {
                     LeftScreen.screenID = System.Convert.ToInt32(Base.Settings.LeftPracticeScreen);
                     RightScreen.screenID = System.Convert.ToInt32(Base.Settings.RightPracticeScreen);
                }
                else if (session == "Race")
                {
                    LeftScreen.screenID = System.Convert.ToInt32(Base.Settings.LeftRaceScreen);
                    RightScreen.screenID = System.Convert.ToInt32(Base.Settings.RightRaceScreen);
                }
                else if (session == "Lone Qualify" || session == "Open Qualify")
                {
                    LeftScreen.screenID = System.Convert.ToInt32(Base.Settings.LeftQualyScreen);
                    RightScreen.screenID = System.Convert.ToInt32(Base.Settings.RightQualyScreen);
                }
            }

            Base.SetProp("Dashboard.LeftScreen", LeftScreen.screenID);
            Base.SetProp("Dashboard.RightScreen", RightScreen.screenID);
            Base.SetProp("DashBoard.DeltaScreen", DeltaScreen.screenID);

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
