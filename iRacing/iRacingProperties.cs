using IRacingReader;

namespace User.PluginSdkDemo.iRacing
{
    public class Properties
    {
        private readonly DahlDesign Base;

        DataSampleEx irData;

        readonly Tires tires;
        public Engine engine;

        public Properties(DahlDesign dahlDesign)
        {
            Base = dahlDesign;
            tires = new Tires(Base, irData);
            engine = new Engine(Base, irData);
        }

        public void DataUpdate()
        {
            if (Base.gameData?.NewData?.GetRawDataObject() is DataSampleEx) 
            { 
                irData = Base.gameData.NewData.GetRawDataObject() as DataSampleEx; 
            }

            engine.DataUpdate();

            if (Base.counter == 47)
            {
                tires.DataUpdate();
            }

        }
    }
}
