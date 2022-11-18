using System;
using IRacingReader;

namespace User.PluginSdkDemo.iRacing
{
    public class Tires
    {


        public double slipLF;
        public double slipRF;
        public double slipLR;
        public double slipRR;
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

            Base.AddProp("SlipLF", 0);
            Base.AddProp("SlipRF", 0);
            Base.AddProp("SlipLR", 0);
            Base.AddProp("SlipRR", 0);
        }

        public void DataUpdate()
        {
            slipLF = Convert.ToDouble(Base.GetProp("ShakeITMotorsV3Plugin.Export.WheelSlip.FrontLeft"));  //Wheel slip
            slipRF = Convert.ToDouble(Base.GetProp("ShakeITMotorsV3Plugin.Export.WheelSlip.FrontRight"));  //Wheel slip
            slipLR = Convert.ToDouble(Base.GetProp("ShakeITMotorsV3Plugin.Export.WheelSlip.RearLeft"));  //Wheel slip
            slipRR = Convert.ToDouble(Base.GetProp("ShakeITMotorsV3Plugin.Export.WheelSlip.RearRight"));  //Wheel slip

            //Wheel slip
            if (!Base.Settings.WheelSlipLEDs || slipLF < 25 || slipRF < 25)
            {
                slipLF = 0;
                slipLR = 0;
                slipRF = 0;
                slipRR = 0;
            }

            if (slipLF < 40 && slipLF > slipRF)
            {
                slipRF = 0;
                slipRR = 0;
            }
            else if (slipRF < 40 && slipRF > slipLF)
            {
                slipLF = 0;
                slipLR = 0;
            }

            if (slipLF == 0 && slipLR == 0)
            {
                Base.SetProp("SlipLF", 0);
                Base.SetProp("SlipRF", 0);
                Base.SetProp("SlipLR", 0);
                Base.SetProp("SlipRR", 0);
            }

            if (Base.counter == 47)
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
}
