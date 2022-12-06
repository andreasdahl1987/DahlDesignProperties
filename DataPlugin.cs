using DahlDesign.Plugin.Categories;
using GameReaderCommon;
using SimHub.Plugins;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DahlDesign.Plugin
{
    [PluginDescription("Dahl Design Properties")]
    [PluginAuthor("Andreas Dahl")]
    [PluginName("DahlDesign")]

    public class DahlDesign : IPlugin, IDataPlugin, IWPFSettingsV2
    {
        public DataPluginSettings Settings;
        /// <summary>Instance of the current plugin manager</summary>
        public PluginManager PluginManager { get; set; }
        public iRacing.Data iRacing;
        public Categories.Dashboard Dashboard;
        public Categories.DDC DDC;
        public int counter = 0;
        public ImageSource PictureIcon => this.ToIcon(Properties.Resources.Dahl_icon);
        public string LeftMenuTitle => "Dahl Design";
        //public bool gameRunning;
        //public string gameName;
        //public GameData gameData;
        public DahlGameData dahlGameData = new DahlGameData();
        public List<SectionBase> Sections = new List<SectionBase>();

        /// <summary>
        /// Called once after plugins startup
        /// Plugins are rebuilt at game change
        /// </summary>
        /// <param name="pluginManager"></param>
        public void Init(PluginManager pluginManager)
        {
            SimHub.Logging.Current.Info("Starting plugin");

            // Load settings
            Settings = this.ReadCommonSettings<DataPluginSettings>("GeneralSettings", () => new DataPluginSettings());

            InitSections();
        }

        private void InitSections()
        {
            Dashboard = null;
            DDC = null;
            iRacing = null;

            Dashboard = new Categories.Dashboard(this);
            DDC = new Categories.DDC(this);
            iRacing = new iRacing.Data(this);

            Sections.Add(Dashboard);
            Sections.Add(DDC);
            Sections.Add(iRacing);
            Sections.Add(new Tires(this));
            Sections.Add(new iRacingSpotter(this));
        }

        public void DataUpdate(PluginManager pluginManager, ref GameData data)
        {
            //gameRunning = data.GameRunning;
            //gameName = data.GameName;
            //gameData = data;

            //FRAME COUNTER FOR CPU SAVING
            //Counters used: 1,2,3,4,5,6,7,8,9,10,11,14,15,17,20,22,24,25,27,30,33,35,36,38,39,40,43,45,47,50,51,52,53,54,55,59  

            counter++;

            //Resetting counter
            if (counter > 59)
            {
                counter = 0;
            }

            if (data.GameRunning && data.NewData != null)
            {
                dahlGameData.GameData = data;
                dahlGameData.GameName = data.GameName;
                dahlGameData.SetGameData();

                foreach (SectionBase section in Sections)
                {
                    section.DataUpdate();
                }
            } else
            {
                Sections.Clear();
                InitSections();
            }
        }

        public void End(PluginManager pluginManager)
        {
            // Save settings
            this.SaveCommonSettings("GeneralSettings", Settings);
        }

        /// <summary>
        /// Returns the settings control, return null if no settings control is required
        /// </summary>
        /// <param name="pluginManager"></param>
        /// <returns></returns>
        public System.Windows.Controls.Control GetWPFSettingsControl(PluginManager pluginManager)
        {
            return new SettingsControl(this) { DataContext = Settings }; ;
        }

        public void AddProp(string PropertyName, dynamic defaultValue) => PluginManager.AddProperty(PropertyName, GetType(), defaultValue);
        public void SetProp(string PropertyName, dynamic value) => PluginManager.SetPropertyValue(PropertyName, GetType(), value);
        public dynamic GetProp(string PropertyName) => PluginManager.GetPropertyValue(PropertyName);
        public bool HasProp(string PropertyName) => PluginManager.GetAllPropertiesNames().Contains(PropertyName);
        public void AddEvent(string EventName) => PluginManager.AddEvent(EventName, GetType());
        public void TriggerEvent(string EventName) => PluginManager.TriggerEvent(EventName, GetType());
        public void AddAction(string ActionName, Action<PluginManager, string> ActionBody)
            => PluginManager.AddAction(ActionName, GetType(), ActionBody);

        public void TriggerAction(string ActionName) => PluginManager.TriggerAction(ActionName);
    }
}