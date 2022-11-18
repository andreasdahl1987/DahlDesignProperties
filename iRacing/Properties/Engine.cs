using System;
using IRacingReader;

namespace User.PluginSdkDemo.iRacing
{
    public class Engine
    {


        public bool boost;
        public int MGU;
        public double battery;

        private readonly DahlDesign Base;
        private readonly DataSampleEx irData;

        public Engine(DahlDesign dahlDesign, DataSampleEx irdata)
        {
            Base = dahlDesign;
            irData = irdata;
        }

        public void DataUpdate()
        {
            irData.Telemetry.TryGetValue("ManualBoost", out object rawBoost);
            boost = Convert.ToBoolean(rawBoost);                                               //Boost

            irData.Telemetry.TryGetValue("PowerMGU_K", out object rawMGU);
            MGU = Convert.ToInt32(rawMGU);                                                      //MGU-K current

            irData.Telemetry.TryGetValue("EnergyERSBatteryPct", out object rawBattery);
            battery = Convert.ToDouble(rawBattery);                                          //Battery

        }
    }
}
