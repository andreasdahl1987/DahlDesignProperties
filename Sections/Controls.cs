namespace User.PluginSdkDemo.Categories
{
    public class Controls
    {
        private readonly DahlDesign Base;

        //public bool ShowBrakeThrottle;

        private string ShowMap = "Controls.ShowMap";
        private string ShowBrakeThrottle = "Controls.ShowBrakeThrottle";

        public Controls(DahlDesign dahlDesign)
        {
            Base = dahlDesign;

            Base.AddProp(ShowMap, Base.Settings.ShowMapEnabled);
            Base.AddProp(ShowBrakeThrottle, Base.Settings.ShowBrakeThrottleGaugesEnabled);

            Base.AddAction("Controls.MapToggle", (a, b) =>
            {                               
                Base.Settings.ShowMapEnabled = !Base.Settings.ShowMapEnabled;
            });

            Base.AddAction("Controls.ShowBrakeThrottleGaugesToggle",  (a, b) =>
            {
                Base.Settings.ShowBrakeThrottleGaugesEnabled = !Base.Settings.ShowBrakeThrottleGaugesEnabled;
            });
        }

        public void DataUpdate()
        {         
            Base.SetProp(ShowMap, Base.Settings.ShowMapEnabled);
            Base.SetProp(ShowBrakeThrottle, Base.Settings.ShowBrakeThrottleGaugesEnabled);
        }
    }
}
