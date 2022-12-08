using IRacingReader;
using iRacingSDK;
using GameReaderCommon;
using DahlDesign.Plugin.iRacing;

namespace DahlDesign.Plugin
{
    public class DahlGameData
    {
        public GameReaderCommon.GameData GameData { get; set; }
        public IRacingReader.DataSampleEx IRData { get; set; }
        public string GameName { get; set; }


        public void SetGameData()
        {
            if (GameData == null)
                return;

            if (GameData.GameName == "IRacing")
            {
                IRData = GameData.NewData.GetRawDataObject() as DataSampleEx;
            }
        }
    }

}
