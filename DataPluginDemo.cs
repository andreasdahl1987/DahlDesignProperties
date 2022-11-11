using GameReaderCommon;
using SimHub.Plugins;
using System;
using System.Windows.Media;
using System.Linq;
using IRacingReader;
using ACSharedMemory;


namespace User.PluginSdkDemo
{
    [PluginDescription("Dahl Design DDU Properties")]
    [PluginAuthor("Andreas Dahl")]
    [PluginName("DahlDesign")]

    public class DahlDesign : IPlugin, IDataPlugin, IWPFSettingsV2
    {
        static readonly string version = "1.10.1";

        public DataPluginDemoSettings Settings;

        /// <summary>Instance of the current plugin manager</summary>
        public PluginManager PluginManager { get; set; }

        public Categories.Dashboard Dashboard;
        public Categories.DDC DDC;
        public Categories.iRacing iRacing;

        public int counter = 0;

        public ImageSource PictureIcon => this.ToIcon(Properties.Resources.Dahl_icon);

        public string LeftMenuTitle => "Dahl Design";

        public bool gameRunning;
        public string gameName;
        public GameData gameData;


        public void DataUpdate(PluginManager pluginManager, ref GameData data)
        {

            //---------------------------------------------------
            //----------------GETTING DATA------------------------
            //----------------------------------------------------
            gameRunning = data.GameRunning;
            gameName = data.GameName;
            gameData = data;

            Dashboard.DataUpdate();

            //FRAME COUNTER FOR CPU SAVING
            counter++;
            //Counters used: 1,2,3,4,5,6,7,8,9,10,11,14,15,17,20,22,24,25,27,30,33,35,36,38,39,40,43,45,47,50,51,52,53,54,55,59  

            DDC.DataUpdate();
            iRacing.DataUpdate();


            //Resetting counter
            if (counter > 59)
            {
                counter = 0;
            }

            iRacing.DataUpdateIdle();
        }

        public void End(PluginManager pluginManager)
        {
            // Save settings
            this.SaveCommonSettings("GeneralSettings", Settings);
        }

        /// <summary>Returns the settings control, return null if no settings control is required</summary>
        /// <param name="pluginManager"></param>
        /// <returns></returns>
        public System.Windows.Controls.Control GetWPFSettingsControl(PluginManager pluginManager)
        {
            return new SettingsControlDemo(this) { DataContext = Settings }; ;
        }

        /// <summary>
        /// Called once after plugins startup
        /// Plugins are rebuilt at game change
        /// </summary>
        /// <param name="pluginManager"></param>
        public void Init(PluginManager pluginManager)
        {
            SimHub.Logging.Current.Info("Starting plugin");

            // Load settings
            Settings = this.ReadCommonSettings<DataPluginDemoSettings>("GeneralSettings", () => new DataPluginDemoSettings());

            Dashboard = new Categories.Dashboard(this);
            DDC = new Categories.DDC(this);
            iRacing = new Categories.iRacing(this);

            //Update property
            pluginManager.AddProperty("Version", this.GetType(), version);
        }

        public void AddProp(string PropertyName, dynamic defaultValue) => PluginManager.AddProperty(PropertyName, GetType(), defaultValue);
        public void SetProp(string PropertyName, dynamic value) => PluginManager.SetPropertyValue(PropertyName, GetType(), value);
        public dynamic GetProp(string PropertyName) => PluginManager.GetPropertyValue(PropertyName);
        public bool HasProp(string PropertyName) => PluginManager.GetAllPropertiesNames().Contains(PropertyName);
        public void AddEvent(string EventName) => PluginManager.AddEvent(EventName, GetType());
        public void TriggerEvent(string EventName) => PluginManager.TriggerEvent(EventName, GetType());
        public void AddAction(string ActionName, Action<PluginManager, string> ActionBody) => PluginManager.AddAction(ActionName, GetType(), ActionBody);
    }
}