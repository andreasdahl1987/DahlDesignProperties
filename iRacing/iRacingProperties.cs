using IRacingReader;

namespace User.PluginSdkDemo.iRacing
{
    public class Properties
    {
        private readonly DahlDesign Base;
        private readonly iRacing.Data iRacingDataBase;

        DataSampleEx irData;

        public Tires tires;
        public Engine engine;
        public Drivetrain drivetrain;
        public Laps laps;

        public Properties(DahlDesign dahlDesign, iRacing.Data data)
        {
            Base = dahlDesign;
            iRacingDataBase = data;

            tires = new Tires(Base, irData);
            engine = new Engine(Base, irData);
            drivetrain = new Drivetrain(Base, irData);
            laps = new Laps(Base, irData, iRacingDataBase);
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
            laps.DataUpdate();

        }
    }
}
