using IRacingReader;

namespace User.PluginSdkDemo.iRacing
{
    public class Properties
    {
        private readonly DahlDesign Base;

        DataSampleEx irData;

        readonly Tires tires;

        public Properties(DahlDesign dahlDesign)
        {
            Base = dahlDesign;
            tires = new Tires(Base, irData);
        }

        public void DataUpdate()
        {
            if (Base.gameData?.NewData?.GetRawDataObject() is DataSampleEx) 
            { 
                irData = Base.gameData.NewData.GetRawDataObject() as DataSampleEx; 
            }

            if (Base.counter == 47)
            {
                tires.DataUpdate();
            }

        }
    }
}
