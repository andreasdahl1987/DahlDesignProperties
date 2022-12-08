using SimHub.Plugins;

namespace DahlDesign.Plugin.Categories
{
    /// <summary>
    /// Template example class
    /// </summary>
    public class Template : SectionBase
    {

        public Template(DahlDesign dahlDesign) : base(dahlDesign) { }

        //class variables go here
        double templateVariable = 0;

        /// <summary>
        /// Class initialization, called once at game start
        /// </summary>
        /// <param name="pluginManager"></param>
        public override void Init(PluginManager pluginManager)
        {
            // Add your delegates here

            Base.AttachDelegate("Template", () => templateVariable);
        }

        /// <summary>
        /// Called 60 times per second by SimHub
        /// </summary>
        public override void DataUpdate()
        {
            // access to iRacing data with IRdata
            // access to GameData with GameData

            // assign something to your delegated variable
            templateVariable = GameData.something;
        }
    }
}
