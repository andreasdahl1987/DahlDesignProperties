using GameReaderCommon;
using SimHub.Plugins;

namespace DahlDesign.Plugin.Categories
{
    public abstract class SectionBase
    {
        protected readonly DahlDesign Base;
        protected DahlGameData GameDataAll { get => Base.dahlGameData; }
        protected StatusDataBase GameData { get => Base.dahlGameData.GameData.NewData; }
        protected IRacingReader.DataSampleEx IRData { get => Base.dahlGameData.IRData; }

        public SectionBase(DahlDesign dahlDesign)
        {
            Base = dahlDesign;
            Init(dahlDesign.PluginManager);
        }

        public abstract void Init(PluginManager pluginManager);

        public abstract void DataUpdate();
    }
}