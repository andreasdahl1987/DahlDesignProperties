using System;
using SimHub.Plugins;

namespace DahlDesign.Plugin.Categories
{
    /// <summary>
    /// Radio toggle/name
    /// </summary>
    public class Radio : SectionBase
    {

        public Radio(DahlDesign dahlDesign) : base(dahlDesign) { }

        //class variables go here
        bool radio = false;
        string radioName = "";
        int radioPosition;
        bool radioIsSpectator;

        /// <summary>
        /// Class initialization, called once at game start
        /// </summary>
        /// <param name="pluginManager"></param>
        public override void Init(PluginManager pluginManager)
        {
            // Add your delegates here

            Base.AttachDelegate("Radio", () => radio);
            Base.AttachDelegate("RadioName", () => radioName);
            Base.AttachDelegate("RadioPosition", () => radioPosition);
            Base.AttachDelegate("RadioIsSpectator", () => radioIsSpectator);

            Base.AddAction("RadioPressed", (a, b) => radio = true);
            Base.AddAction("RadioReleased", (a, b) => radio = false);
        }

        /// <summary>
        /// Called 60 times per second by SimHub
        /// </summary>
        public override void DataUpdate()
        {
            if (IRData.Telemetry.RadioTransmitCarIdx != -1)
            {
                radioName = IRData.SessionData.DriverInfo.Drivers[IRData.Telemetry.RadioTransmitCarIdx].UserName;
                radioIsSpectator = Convert.ToBoolean(IRData.SessionData.DriverInfo.Drivers[IRData.Telemetry.RadioTransmitCarIdx].IsSpectator);

                if (radioName == Globals.aheadGlobal)
                {
                    radioPosition = Globals.realPosition - 1;
                }
                else if (radioName == Globals.behindGlobal)
                {
                    radioPosition = Globals.realPosition + 1;
                }
                else
                {
                    radioPosition = IRData.Telemetry.CarIdxClassPosition[IRData.Telemetry.RadioTransmitCarIdx];
                }
            }
            else
            {
                radioName = "";
                radioIsSpectator = false;
            }

            radioName = radioName.ToUpper();

            if (IRData.Telemetry.RadioTransmitCarIdx != -1)
            {
                radio = false;
            }
        }
    }
}
