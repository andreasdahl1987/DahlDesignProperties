using System;
using IRacingReader;

namespace User.PluginSdkDemo.iRacing
{
    public class Drivetrain
    {
        private readonly DahlDesign Base;
        private readonly DataSampleEx irData;

        public string gear;
        public double gearRatio;
        public string smoothGear = "";
        int neutralCounter = 0;


        public Drivetrain(DahlDesign dahlDesign, DataSampleEx irdata)
        {
            Base = dahlDesign;
            irData = irdata;

            Base.AddProp("SmoothGear", "");
        }

        public void DataUpdate()
        {
            gear = Base.gameData.NewData.Gear;
            gearRatio = Convert.ToDouble(Base.GetProp("GameRawData.SessionData.CarSetup.Chassis.Rear.DropGearARatio")); //Gear ratio

            //----------------------------------------------
            //--------SMOOTH GEAR---------------------------
            //----------------------------------------------

            if (gear != "N")
            {
                smoothGear = gear;
                neutralCounter = 0;
            }

            if (gear == "N")
            {
                neutralCounter++;
            }

            if (neutralCounter > 6)
            {
                smoothGear = "N";
                neutralCounter = 0;
            }
            if (Base.DDC.button8Mode == 1)
            {
                smoothGear = "N";
            }

            Base.SetProp("SmoothGear", smoothGear);
        }
    }
}
