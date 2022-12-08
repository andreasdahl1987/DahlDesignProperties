using System;
using SimHub.Plugins;

namespace DahlDesign.Plugin.Categories
{
    /// <summary>
    /// Acceleration stopwatch
    /// </summary>
    public class Acceleration : SectionBase
    {

        public Acceleration(DahlDesign dahlDesign) : base(dahlDesign) { }

        //delegates
        double propAccelerationTo100KPH = 0;
        double propAccelerationTo200KPH = 0;

        //class variables
        bool accelerationStart = false;
        int accelerationPremature = 0;
        bool oneHundered = false;
        bool twoHundered = false;
        TimeSpan stopWatch = new TimeSpan(0);
        TimeSpan globalClock;

        /// <summary>
        /// Class initialization, called once at game start
        /// </summary>
        /// <param name="pluginManager"></param>
        public override void Init(PluginManager pluginManager)
        {
            Initialize();

            Base.AttachDelegate("AccelerationTo100KPH", () => propAccelerationTo100KPH);
            Base.AttachDelegate("AccelerationTo200KPH", () => propAccelerationTo200KPH);
        }

        /// <summary>
        /// Called 60 times per second by SimHub
        /// </summary>
        public override void DataUpdate()
        {
            globalClock = TimeSpan.FromTicks(DateTime.Now.Ticks);

            if (GameData.Gear != "N" && GameData.SpeedLocal < 0.5 && GameData.Rpms > 300)
            {
                accelerationStart = true;
            }
            else if (accelerationPremature == 1)
            {
                propAccelerationTo200KPH = 0;
            }
            else if (accelerationPremature == 2)
            {
                propAccelerationTo100KPH = 0;
                propAccelerationTo200KPH = 0;
            }

            if (!accelerationStart && GameData.SpeedLocal > 0.5)
            {
                if (!oneHundered && !twoHundered)
                {
                    accelerationPremature = 2;
                }
                else if (!twoHundered)
                {
                    accelerationPremature = 1;
                }
            }

            if (accelerationStart)
            {
                stopWatch = globalClock;
                Initialize();
            }

            if (!accelerationStart && GameData.SpeedLocal > 0.5)
            {
                if (!oneHundered)
                {
                    propAccelerationTo100KPH = globalClock.TotalSeconds - stopWatch.TotalSeconds;
                }
                if (!twoHundered)
                {
                    propAccelerationTo200KPH = globalClock.TotalSeconds - stopWatch.TotalSeconds;
                }

            }

            if (GameData.SpeedLocal > 100 && !oneHundered)
            {
                oneHundered = true;
                accelerationPremature = 1;
            }

            if (GameData.SpeedLocal > 200 && !twoHundered)
            {
                twoHundered = true;
                accelerationPremature = 0;
            }
        }

        private void Initialize()
        {
            oneHundered = false;
            twoHundered = false;
            propAccelerationTo100KPH = 0;
            propAccelerationTo200KPH = 0;
            accelerationStart = false;
        }
    }
}
