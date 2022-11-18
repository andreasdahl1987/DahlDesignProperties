using IRacingReader;

namespace User.PluginSdkDemo.iRacing
{
    public class Properties
    {
        private readonly DahlDesign Base;

        DataSampleEx irData;

        public Tires tires;
        public Engine engine;
        public Drivetrain drivetrain;

        public Properties(DahlDesign dahlDesign)
        {
            Base = dahlDesign;
            tires = new Tires(Base, irData);
            engine = new Engine(Base, irData);
            drivetrain = new Drivetrain(Base, irData);
        }

        public void DataUpdate()
        {
            if (Base.gameData?.NewData?.GetRawDataObject() is DataSampleEx)
            {
                irData = Base.gameData.NewData.GetRawDataObject() as DataSampleEx;
            }

            engine.DataUpdate();
            drivetrain.DataUpdate();
            tires.DataUpdate();

        }
    }
}
