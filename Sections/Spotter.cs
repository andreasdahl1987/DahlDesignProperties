using System;
using SimHub.Plugins;

namespace DahlDesign.Plugin.Categories
{
    public class Spotter : SectionBase
    {
        public Spotter(DahlDesign dahlDesign) : base(dahlDesign) { }
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
            bool spotLeft = Convert.ToBoolean(GameData.SpotterCarLeft);                         //Spotter call left
            bool spotRight = Convert.ToBoolean(GameData.SpotterCarRight);                       //Spotter call left

            double trackLength = GameData.TrackLength;

            if (spotLeft && !leftTaken) //Catch and lock
            {
                leftTaken = true;
                double? gap = 0;
                for (int i = 0; i < GameData.OpponentsAheadOnTrack.Count; i++)
                {
                    if (((GameData.OpponentsAheadOnTrack[i].TrackPositionPercentToPlayer != 0 && gap == 0) || GameData.OpponentsAheadOnTrack[i].TrackPositionPercentToPlayer * trackLength < gap) && GameData.OpponentsAheadOnTrack[i].Name != carNameRight)
                    {
                        carNameLeft = GameData.OpponentsAheadOnTrack[i].Name;
                        carPositionLeft = GameData.OpponentsAheadOnTrack[i].TrackPositionPercentToPlayer * trackLength;
                        gap = carPositionLeft;
                    }
                }
                for (int i = 0; i < GameData.OpponentsBehindOnTrack.Count; i++)
                {
                    if (((GameData.OpponentsBehindOnTrack[i].TrackPositionPercentToPlayer != 0 && gap == 0) || -1 * GameData.OpponentsBehindOnTrack[i].TrackPositionPercentToPlayer * trackLength < gap) && GameData.OpponentsBehindOnTrack[i].Name != carNameRight)
                    {
                        carNameLeft = GameData.OpponentsBehindOnTrack[i].Name;
                        carPositionLeft = GameData.OpponentsBehindOnTrack[i].TrackPositionPercentToPlayer * trackLength;
                        gap = carPositionLeft;
                    }
                }
            }

            if (leftTaken) //Read
            {
                for (int i = 0; i < GameData.OpponentsAheadOnTrack.Count; i++)
                {
                    if (GameData.OpponentsAheadOnTrack[i].Name == carNameLeft)
                    {
                        carPositionLeft = GameData.OpponentsAheadOnTrack[i].TrackPositionPercentToPlayer * trackLength;
                        break;
                    }
                }
                for (int i = 0; i < GameData.OpponentsBehindOnTrack.Count; i++)
                {
                    if (GameData.OpponentsBehindOnTrack[i].Name == carNameLeft)
                    {
                        carPositionLeft = GameData.OpponentsBehindOnTrack[i].TrackPositionPercentToPlayer * trackLength;
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
                for (int i = 0; i < GameData.OpponentsAheadOnTrack.Count; i++)
                {
                    if (((GameData.OpponentsAheadOnTrack[i].TrackPositionPercentToPlayer != 0 && gap == 0) || GameData.OpponentsAheadOnTrack[i].TrackPositionPercentToPlayer * trackLength < gap) && GameData.OpponentsAheadOnTrack[i].Name != carNameLeft)
                    {
                        carNameRight = GameData.OpponentsAheadOnTrack[i].Name;
                        carPositionRight = GameData.OpponentsAheadOnTrack[i].TrackPositionPercentToPlayer * trackLength;
                        gap = carPositionRight;
                    }
                }
                for (int i = 0; i < GameData.OpponentsBehindOnTrack.Count; i++)
                {
                    if (((GameData.OpponentsBehindOnTrack[i].TrackPositionPercentToPlayer != 0 && gap == 0) || -1 * GameData.OpponentsBehindOnTrack[i].TrackPositionPercentToPlayer * trackLength < gap) && GameData.OpponentsBehindOnTrack[i].Name != carNameLeft)
                    {
                        carNameRight = GameData.OpponentsBehindOnTrack[i].Name;
                        carPositionRight = GameData.OpponentsBehindOnTrack[i].TrackPositionPercentToPlayer * trackLength;
                        gap = carPositionRight;
                    }
                }
            }

            if (rightTaken) //Read
            {
                for (int i = 0; i < GameData.OpponentsAheadOnTrack.Count; i++)
                {
                    if (GameData.OpponentsAheadOnTrack[i].Name == carNameRight)
                    {
                        carPositionRight = GameData.OpponentsAheadOnTrack[i].TrackPositionPercentToPlayer * trackLength;
                        break;
                    }
                }
                for (int i = 0; i < GameData.OpponentsBehindOnTrack.Count; i++)
                {
                    if (GameData.OpponentsBehindOnTrack[i].Name == carNameRight)
                    {
                        carPositionRight = GameData.OpponentsBehindOnTrack[i].TrackPositionPercentToPlayer * trackLength;
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
