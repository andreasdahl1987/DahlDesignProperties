using DahlDesign.Plugin.iRacing;
using GameReaderCommon;
using SimHub.Plugins;
using System;
using System.Linq;
using System.Windows.Markup;
using System.Windows.Media;
//using ACSharedMemory;

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
        public Categories.Dashboard Dashboard;
        public Categories.DDC DDC;
        public iRacing.Data iRacing;
        const long TicksFor60Fps = 10000000 / 60;
        internal bool UpdateAt60Fps;
        internal long LastRan60Fps;
        public int counter = 0;
        public ImageSource PictureIcon => this.ToIcon(Properties.Resources.Dahl_icon);
        public string LeftMenuTitle => "Dahl Design";
        public bool gameRunning;
        public string gameName;
        public GameData gameData;

#if DEBUG
        public double performanceSkippedFrames = 0;
        /// <summary>
        /// List of 'ticks' needed for each frame.
        /// One tick is 100ns. There are 10000 ticks in a millisecond
        /// </summary>
        public long[] performanceFrames = new long[60];
        public long performanceTimerStart = 0;
#endif

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

            Dashboard = new Categories.Dashboard(this);
            DDC = new Categories.DDC(this);
            iRacing = new iRacing.Data(this);

#if DEBUG
            this.AttachDelegate("Performance.SkippedFrames", () => performanceSkippedFrames);
            this.AttachDelegate("Performance.FramesSum", () => performanceFrames.Sum());
            this.AttachDelegate("Performance.Frames", () => String.Join("\r\n ", performanceFrames
                  .Select((d, i) => i + "=" + d).ToArray()));
#endif
        }

        public void DataUpdate(PluginManager pluginManager, ref GameData data)
        {
            gameRunning = data.GameRunning;
            gameName = data.GameName;
            gameData = data;

            //FRAME COUNTER FOR CPU SAVING
            //Counters used: 1,2,3,4,5,6,7,8,9,10,11,14,15,17,20,22,24,25,27,30,33,35,36,38,39,40,43,45,47,50,51,52,53,54,55,59  
            long nowTicks = DateTime.Now.Ticks;
            UpdateAt60Fps = nowTicks - LastRan60Fps >= TicksFor60Fps;

            if (UpdateAt60Fps)
            {
                LastRan60Fps = DateTime.Now.Ticks;
                counter++;
#if DEBUG
                performanceTimerStart = nowTicks;
#endif
                //Resetting counter
                if (counter > 59)
                {
                    counter = 0;
                }
            } else
            {
#if DEBUG
                performanceSkippedFrames++;
#endif
                return;
            }

            Dashboard.DataUpdate();
            DDC.DataUpdate();
            iRacing.DataUpdate();
            iRacing.DataUpdateIdle();

#if DEBUG
            if (DateTime.Now.Ticks - performanceTimerStart > 0)
                performanceFrames[counter] = (DateTime.Now.Ticks - performanceTimerStart);
#endif
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
    }
}