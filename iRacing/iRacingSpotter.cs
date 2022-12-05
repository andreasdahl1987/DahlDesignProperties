using System;
using SimHub.Plugins;

namespace DahlDesign.Plugin.Categories
{
    public class iRacingSpotter : SectionBase
    {
        public iRacingSpotter(DahlDesign dahlDesign) : base(dahlDesign) { }
        private bool leftTaken = false;
        private bool rightTaken = false;
        public string carNameLeft = "";
        public string carNameRight = "";
        public double? carPositionLeft = 0;
        public double? carPositionRight = 0;

        public override void Init(PluginManager pluginManager)
        {
            Base.AttachDelegate("LeftCarGap", () => carPositionLeft);
            Base.AttachDelegate("LeftCarName", () => carNameLeft);
            Base.AttachDelegate("RightCarGap", () => carPositionRight);
            Base.AttachDelegate("RightCarName", () => carNameRight);
        }

        public override void DataUpdate()
        {
            bool spotLeft = Convert.ToBoolean(NewData.SpotterCarLeft);                         //Spotter call left
            bool spotRight = Convert.ToBoolean(NewData.SpotterCarRight);                       //Spotter call left

            double trackLength = NewData.TrackLength;

            if (spotLeft && !leftTaken) //Catch and lock
            {
                leftTaken = true;
                double? gap = 0;
                for (int i = 0; i < NewData.OpponentsAheadOnTrack.Count; i++)
                {
                    if (((NewData.OpponentsAheadOnTrack[i].TrackPositionPercentToPlayer != 0 && gap == 0) || NewData.OpponentsAheadOnTrack[i].TrackPositionPercentToPlayer * trackLength < gap) && NewData.OpponentsAheadOnTrack[i].Name != carNameRight)
                    {
                        carNameLeft = NewData.OpponentsAheadOnTrack[i].Name;
                        carPositionLeft = NewData.OpponentsAheadOnTrack[i].TrackPositionPercentToPlayer * trackLength;
                        gap = carPositionLeft;
                    }
                }
                for (int i = 0; i < NewData.OpponentsBehindOnTrack.Count; i++)
                {
                    if (((NewData.OpponentsBehindOnTrack[i].TrackPositionPercentToPlayer != 0 && gap == 0) || -1 * NewData.OpponentsBehindOnTrack[i].TrackPositionPercentToPlayer * trackLength < gap) && NewData.OpponentsBehindOnTrack[i].Name != carNameRight)
                    {
                        carNameLeft = NewData.OpponentsBehindOnTrack[i].Name;
                        carPositionLeft = NewData.OpponentsBehindOnTrack[i].TrackPositionPercentToPlayer * trackLength;
                        gap = carPositionLeft;
                    }
                }
            }

            if (leftTaken) //Read
            {
                for (int i = 0; i < NewData.OpponentsAheadOnTrack.Count; i++)
                {
                    if (NewData.OpponentsAheadOnTrack[i].Name == carNameLeft)
                    {
                        carPositionLeft = NewData.OpponentsAheadOnTrack[i].TrackPositionPercentToPlayer * trackLength;
                        break;
                    }
                }
                for (int i = 0; i < NewData.OpponentsBehindOnTrack.Count; i++)
                {
                    if (NewData.OpponentsBehindOnTrack[i].Name == carNameLeft)
                    {
                        carPositionLeft = NewData.OpponentsBehindOnTrack[i].TrackPositionPercentToPlayer * trackLength;
                        break;
                    }
                }
            }


            if (!spotLeft) //Release
            {
                leftTaken = false;
                carNameLeft = "";
                carPositionLeft = 0;
            }


            //RIGHT SIDE

            if (spotRight && !rightTaken) //Catch and lock
            {
                rightTaken = true;
                double? gap = 0;
                for (int i = 0; i < NewData.OpponentsAheadOnTrack.Count; i++)
                {
                    if (((NewData.OpponentsAheadOnTrack[i].TrackPositionPercentToPlayer != 0 && gap == 0) || NewData.OpponentsAheadOnTrack[i].TrackPositionPercentToPlayer * trackLength < gap) && NewData.OpponentsAheadOnTrack[i].Name != carNameLeft)
                    {
                        carNameRight = NewData.OpponentsAheadOnTrack[i].Name;
                        carPositionRight = NewData.OpponentsAheadOnTrack[i].TrackPositionPercentToPlayer * trackLength;
                        gap = carPositionRight;
                    }
                }
                for (int i = 0; i < NewData.OpponentsBehindOnTrack.Count; i++)
                {
                    if (((NewData.OpponentsBehindOnTrack[i].TrackPositionPercentToPlayer != 0 && gap == 0) || -1 * NewData.OpponentsBehindOnTrack[i].TrackPositionPercentToPlayer * trackLength < gap) && NewData.OpponentsBehindOnTrack[i].Name != carNameLeft)
                    {
                        carNameRight = NewData.OpponentsBehindOnTrack[i].Name;
                        carPositionRight = NewData.OpponentsBehindOnTrack[i].TrackPositionPercentToPlayer * trackLength;
                        gap = carPositionRight;
                    }
                }
            }

            if (rightTaken) //Read
            {
                for (int i = 0; i < NewData.OpponentsAheadOnTrack.Count; i++)
                {
                    if (NewData.OpponentsAheadOnTrack[i].Name == carNameRight)
                    {
                        carPositionRight = NewData.OpponentsAheadOnTrack[i].TrackPositionPercentToPlayer * trackLength;
                        break;
                    }
                }
                for (int i = 0; i < NewData.OpponentsBehindOnTrack.Count; i++)
                {
                    if (NewData.OpponentsBehindOnTrack[i].Name == carNameRight)
                    {
                        carPositionRight = NewData.OpponentsBehindOnTrack[i].TrackPositionPercentToPlayer * trackLength;
                        break;
                    }
                }
            }

            if (!spotRight) //Release
            {
                rightTaken = false;
                carNameRight = "";
                carPositionRight = 0;
            }
        }
    }
}
