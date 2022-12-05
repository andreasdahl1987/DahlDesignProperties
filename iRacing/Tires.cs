using IRacingReader;
using SimHub.Plugins;

namespace DahlDesign.Plugin.Categories
{
    public class Tires : SectionBase
    {

        public Tires(DahlDesign dahlDesign) : base(dahlDesign) { }
        private DataSampleEx irData;

        float LFCold = 0;
        float RFCold = 0;
        float LRCold = 0;
        float RRCold = 0;

        public override void Init(PluginManager pluginManager)
        {
            if (NewData?.GetRawDataObject() is DataSampleEx) { irData = NewData.GetRawDataObject() as DataSampleEx; }

            Base.AttachDelegate("PitServiceLFPCold", () => LFCold);
            Base.AttachDelegate("PitServiceRFPCold", () => RFCold);
            Base.AttachDelegate("PitServiceLRPCold", () => LRCold);
            Base.AttachDelegate("PitServiceRRPCold", () => RRCold);
        }

        public override void DataUpdate()
        {
            if (Base.counter != 47)
            {
                return;
            }

            if (irData != null)
            {
                LFCold = irData.Telemetry.LFcoldPressure;
                RFCold = irData.Telemetry.RFcoldPressure;
                LRCold = irData.Telemetry.LRcoldPressure;
                RRCold = irData.Telemetry.RRcoldPressure;
            }
        }
    }
}
