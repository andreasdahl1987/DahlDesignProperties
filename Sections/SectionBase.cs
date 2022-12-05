using GameReaderCommon;
using SimHub.Plugins;

namespace DahlDesign.Plugin.Categories
{
    public abstract class SectionBase
    {
        protected readonly DahlDesign Base;
        protected StatusDataBase NewData { get => Base.gameData?.NewData; }
       
        public SectionBase(DahlDesign dahlDesign)
        {
            Base = dahlDesign;
            Init(dahlDesign.PluginManager);
        }

        public abstract void Init(PluginManager pluginManager);

        public abstract void DataUpdate();
    }
}