namespace User.PluginSdkDemo.Category
{
    public class Foo
    {
        private readonly DahlDesign Base;

        public bool FooSetting;        

        public Foos(DahlDesign dahlDesign)
        {
            Base = dahlDesign;

            Base.AddProp("Foo.Item", FooSetting);
            
            Base.AddAction("Foo.ItemAction", this.GetType(), (a, b) =>
            {
                FooSetting = !FooSetting;
            });
        }

        public void DataUpdate()
        {
            Base.SetProp("Foo.Item", FooSetting);            
        }
    }
}
