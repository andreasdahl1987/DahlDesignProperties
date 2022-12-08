using SimHub.Plugins;

namespace DahlDesign.Plugin.Categories
{
    /// <summary>
    /// Smooth Gear
    /// </summary>
    public class SmoothGear : SectionBase
    {

        public SmoothGear(DahlDesign dahlDesign) : base(dahlDesign) { }

        //class variables go here
        int neutralCounter = 0;
        public string smoothGear = "";


        /// <summary>
        /// Class initialization, called once at game start
        /// </summary>
        /// <param name="pluginManager"></param>
        public override void Init(PluginManager pluginManager)
        {
            Base.AttachDelegate("SmoothGear", () => smoothGear);
        }

        /// <summary>
        /// Called 60 times per second by SimHub
        /// </summary>
        public override void DataUpdate()
        {
            if (GameData.Gear != "N")
            {
                smoothGear = GameData.Gear;
                neutralCounter = 0;
            }

            if (GameData.Gear == "N")
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
        }
    }
}
