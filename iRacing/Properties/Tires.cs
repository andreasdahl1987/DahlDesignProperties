using IRacingReader;
using SimHub.Plugins;

namespace DahlDesign.Plugin.iRacing
{
    public class Tires
    {
        private readonly DahlDesign Base;

        private readonly DataSampleEx irData;

        float LFCold = 0;
        float RFCold = 0;
        float LRCold = 0;
        float RRCold = 0;

        public Tires(DahlDesign dahlDesign, DataSampleEx irdata)
        {
            Base = dahlDesign;
            irData = irdata;

            Base.AttachDelegate("PitServiceLFPCold", () => LFCold);
            Base.AttachDelegate("PitServiceRFPCold", () => RFCold);
            Base.AttachDelegate("PitServiceLRPCold", () => LRCold);
            Base.AttachDelegate("PitServiceRRPCold", () => RRCold);
        }

        public void DataUpdate()
        {
            LFCold = irData.Telemetry.LFcoldPressure;
            RFCold = irData.Telemetry.RFcoldPressure;
            LRCold = irData.Telemetry.LRcoldPressure;
            RRCold = irData.Telemetry.RRcoldPressure;
        }
    }
}
