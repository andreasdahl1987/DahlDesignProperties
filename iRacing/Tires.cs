using IRacingReader;
using SimHub.Plugins;

namespace DahlDesign.Plugin.Categories
{
    public class Tires : SectionBase
    {

        public Tires(DahlDesign dahlDesign) : base(dahlDesign) { }
        //private DataSampleEx irData;

        float LFCold = 0;
        float RFCold = 0;
        float LRCold = 0;
        float RRCold = 0;

        float LFSet = 0;
        float RFSet = 0;
        float LRSet = 0;
        float RRSet = 0;

        public override void Init(PluginManager pluginManager)
        {
            //if (NewData?.GetRawDataObject() is DataSampleEx) { irData = NewData.GetRawDataObject() as DataSampleEx; }

            Base.AttachDelegate("PitServiceLFPCold", () => LFCold);
            Base.AttachDelegate("PitServiceRFPCold", () => RFCold);
            Base.AttachDelegate("PitServiceLRPCold", () => LRCold);
            Base.AttachDelegate("PitServiceRRPCold", () => RRCold);

            Base.AttachDelegate("PitServiceLFPSet", () => LFSet);
            Base.AttachDelegate("PitServiceRFPSet", () => RFSet);
            Base.AttachDelegate("PitServiceLRPSet", () => LRSet);
            Base.AttachDelegate("PitServiceRRPSet", () => RRSet);
        }

        public override void DataUpdate()
        {
            if (Base.counter != 47)
            {
                return;
            }

            if (IRData != null)
            {
                LFCold = IRData.Telemetry.LFcoldPressure;
                RFCold = IRData.Telemetry.RFcoldPressure;
                LRCold = IRData.Telemetry.LRcoldPressure;
                RRCold = IRData.Telemetry.RRcoldPressure;

                LFSet = IRData.Telemetry.PitSvLFP;
                RFSet = IRData.Telemetry.PitSvRFP;
                LRSet = IRData.Telemetry.PitSvLRP;
                RRSet = IRData.Telemetry.PitSvRRP;
            }
        }
    }
}
