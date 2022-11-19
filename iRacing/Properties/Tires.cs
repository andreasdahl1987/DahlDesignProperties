using IRacingReader;

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

            Base.AddProp("PitServiceLFPCold", 0);
            Base.AddProp("PitServiceRFPCold", 0);
            Base.AddProp("PitServiceLRPCold", 0);
            Base.AddProp("PitServiceRRPCold", 0);
        }

        public void DataUpdate()
        {
            LFCold = irData.Telemetry.LFcoldPressure;
            RFCold = irData.Telemetry.RFcoldPressure;
            LRCold = irData.Telemetry.LRcoldPressure;
            RRCold = irData.Telemetry.RRcoldPressure;

            Base.SetProp("PitServiceLFPCold", LFCold);
            Base.SetProp("PitServiceRFPCold", RFCold);
            Base.SetProp("PitServiceLRPCold", LRCold);
            Base.SetProp("PitServiceRRPCold", RRCold);
        }
    }
}
