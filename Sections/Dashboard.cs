using System.Collections.ObjectModel;
using SimHub.Plugins;
using SimHub.Plugins.OutputPlugins.GraphicalDash;

namespace DahlDesign.Plugin.Categories
{
    public class Dashboard : SectionBase
    {
        public Dashboard(DahlDesign dahlDesign) : base(dahlDesign) { }
        public Screen LeftScreen;
        public Screen RightScreen;
        public Screen DeltaScreen;
        public bool DashStartup;
        public double DeltaBarSensitivity;

       

        public override void Init(PluginManager pluginManager)
        {
            LeftScreen = new Screen(new string[] { "Time1", "Time2", "Time3", "Qualy", "Race1", "Race2", "Race3", "Practice1", "Practice2" });
            RightScreen = new Screen(new string[] { "Stint1", "Stint2", "Qualy1", "Qualy2", "Race1", "Race2", "Race3", "Track" });
            DeltaScreen = new Screen(new string[] { "LastLap", "SessionBest", "LapRecord", "FuelSave", "FuelTarget" });
            
            Base.AttachDelegate("ShowMapEnabled", () => Base.Settings.ShowMapEnabled);
            Base.AttachDelegate("ShowBrakeThrottleGaugesEnabled", () => Base.Settings.ShowBrakeThrottleGaugesEnabled);

            Base.AttachDelegate("SW1Enabled", () => Base.Settings.SW1Enabled);
            Base.AttachDelegate("DashLEDEnabled", () => Base.Settings.DashLEDEnabled);
            Base.AttachDelegate("DashType", () => Base.Settings.DashType);
            Base.AttachDelegate("LapInfoScreen", () => Base.Settings.LapInfoScreen);
            Base.AttachDelegate("ShiftTimingAssist", () => Base.Settings.ShiftTimingAssist);
            Base.AttachDelegate("ShiftWarning", () => Base.Settings.ShiftWarning);
            Base.AttachDelegate("ARBswapped", () => Base.Settings.SupercarSwapPosition);
            Base.AttachDelegate("ARBstiffForward", () => Base.Settings.SupercarARBDirection);
     
            

            
            Base.AttachDelegate("Dashboard.LeftScreen", () => LeftScreen.screenID);
            LeftScreen.screenID = Base.Settings.LeftScreen;
            
            Base.AttachDelegate("Dashboard.RightScreen", () => RightScreen.screenID);
            RightScreen.screenID = Base.Settings.RightScreen;

            DashStartup = true; 
            Base.AttachDelegate("Dashboard.DeltaScreen", () => DeltaScreen.screenID);
            DeltaScreen.screenID = Base.Settings.DeltaScreen;

            Base.AttachDelegate("DeltaBarSensitivity", () => Base.Settings.DeltaRoadSensitivity);
            Base.AttachDelegate("Dashboard.ShowGenericSplashEnabled", () => Base.Settings.ShowGenericSplashEnabled);

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

        public override void DataUpdate()
        {
            if (Base.gameName != "IRacing" || !Base.gameRunning) return;
        
            string session = NewData.SessionTypeName;

            if (Base.iRacing.sessionHolder != session)
            {
                if (session == "Practice" || session == "Warmup" || session == "Offline Testing")
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

            if (DashStartup)
            {
                DashStartup = !DashStartup;
                DeltaScreen.screenID = System.Convert.ToInt32(Base.Settings.DeltaScreenStartup);
            }
                        
            //Delta sensitivity
            if (Base.iRacing.trackType == 0)
            {
                DeltaBarSensitivity = Base.Settings.DeltaRoadSensitivity;
            }
            else if (Base.iRacing.trackType < 5)
            {
                DeltaBarSensitivity = Base.Settings.DeltaRallySensitivity;
            }
            else
            {
                DeltaBarSensitivity = Base.Settings.DeltaOvalSensitivity;
            }
        }
    }
}
