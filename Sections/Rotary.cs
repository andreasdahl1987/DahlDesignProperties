using SimHub.Plugins;

namespace DahlDesign.Plugin.Categories
{
    /// <summary>
    /// Rotary button class
    /// </summary>
    public class Rotary : SectionBase
    {

        public Rotary(DahlDesign dahlDesign) : base(dahlDesign) { }

        public int pitMenuRotary { get; set; }
        public int inCarRotary { get; set; }
        public string rotaryType { get; set; }
        public bool pitMenuRequirementMet { get; set; }

        /// <summary>
        /// Class initialization, called once at game start
        /// </summary>
        /// <param name="pluginManager"></param>
        public override void Init(PluginManager pluginManager)
        {
            pitMenuRotary = 12; //Starting on strat page
            inCarRotary = 0;
            rotaryType = "Single";
            pitMenuRequirementMet = false;


            Base.AttachDelegate("MenuType", () => rotaryType);
            Base.AttachDelegate("CoupleInCarToPit", () => Base.Settings.CoupleInCarToPit);

            //
            // Pit Rotary
            //
            Base.AttachDelegate("PitMenu", () => pitMenuRotary);
            

            for (int i = 1; i < 13; i++)
            {
                string propIndex = $"{i:0}";
                int lel = i;

                Base.AddAction($"PitMenu{propIndex}", (a, b) =>
                {
                    pitMenuRotary = i;
                    if (Base.Settings.DDSEnabled)
                    {
                        inCarRotary = 0;
                    }
                });
            }

            Base.AddAction("PitMenuInc", (a, b) =>
            {
                pitMenuRotary++;
                if (pitMenuRotary > 12)
                {
                    pitMenuRotary = 1;
                }
            });

            Base.AddAction("PitMenuDec", (a, b) =>
            {
                pitMenuRotary--;
                if (pitMenuRotary < 1)
                {
                    pitMenuRotary = 12;
                }
            });

            //
            // InCar Rotary
            //

            Base.AttachDelegate("InCarMenu", () => inCarRotary);

            for (int i = 1; i < 13; i++)
            {
                string propIndex = $"{i:0}";

                Base.AddAction($"InCarMenu{propIndex}", (a, b) =>
                {
                    inCarRotary = i;
                    if (Base.Settings.DDSEnabled)
                    {
                        if (rotaryType == "Single")
                        {
                            pitMenuRotary = inCarRotary;
                        }
                        else
                        {
                            pitMenuRotary = 0;
                        }
                    }
                });
            }

            Base.AddAction("InCarMenuInc", (a, b) =>
            {
                inCarRotary++;
                if (inCarRotary > 12)
                {
                    inCarRotary = 1;
                }
            });

            Base.AddAction("InCarMenuDec", (a, b) =>
            {
                inCarRotary--;
                if (inCarRotary < 1)
                {
                    inCarRotary = 12;
                }
            });

        }

        /// <summary>
        /// Called 60 times per second by SimHub
        /// </summary>
        public override void DataUpdate()
        {
            // access to iRacing data with IRdata
            // access to GameData with GameData
            if (!Base.Settings.CoupleInCarToPit) // Ignore all of this if we explicitly state that coupling the InCar to Pit is off in settings)
            {
                pitMenuRequirementMet = true;
            }
            else if (
                inCarRotary == 0 && pitMenuRotary != 0 ||
                rotaryType == "Single" ||
                (rotaryType != "Single" && rotaryType != "Default" && inCarRotary == 12))
            {
                pitMenuRequirementMet = true;
            }
            else
            {
                pitMenuRequirementMet = false;
            }

            if (rotaryType == "Single" && pitMenuRotary == 0)
            {
                pitMenuRotary = inCarRotary;
            }

        }
       
        public bool PitMenu(int position)
        {
            return pitMenuRotary == position && pitMenuRequirementMet;
        }
    }
}
