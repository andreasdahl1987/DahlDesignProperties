namespace User.PluginSdkDemo.Category
{
    public class Switches
    {
        private readonly DahlDesign Base;

        public bool ShowMap;
        public bool ShowBrakeThrottle;

        public Switches(DahlDesign dahlDesign)
        {
            Base = dahlDesign;

            Base.AddProp("Switch.ShowMap", false);
            Base.AddProp("Switch.ShowBrakeThrottleGauges", ShowBrakeThrottleGauges);

            Base.AddAction("Switch.MapToggle", this.GetType(), (a, b) =>
            {                               
                Settings.ShowMapEnabled = !Settings.ShowMapEnabled;
            });

            Base.AddAction("Switch.ShowBrakeThrottleGauges", this.GetType(), (a, b) =>
            {
                ShowBrakeThrottleGauges = !ShowBrakeThrottleGauges;
            });
        }

        public void DataUpdate()
        {         
            Base.SetProp("Switch.ShowMap", ShowMap);
            pBase.SetProp("Switch.ShowBrakeThrottleGauges", ShowBrakeThrottleGauges);
        }
    }
}
