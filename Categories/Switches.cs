namespace User.PluginSdkDemo.Category
{
    public class Switches
    {
        private readonly DahlDesign Base;

        public bool ShowMapEnabled;

        public Switches(DahlDesign dahlDesign)
        {
            Base = dahlDesign;

            Base.AddProp("Switch.ShowMap", false);            

            Base.AddAction("MapToggle", this.GetType(), (a, b) =>
            {                               
                Settings.ShowMapEnabled = !Settings.ShowMapEnabled;
            });
        }

        public void DataUpdate()
        {         
            Base.SetProp("Switch.ShowMap", ShowMapEnabled);            
        }
    }
}
