using System;
using GameReaderCommon;

namespace User.PluginSdkDemo.iRacing
{
    class iRacingSpotter
    {
        private bool leftTaken = false;
        private bool rightTaken = false;
        public string carNameLeft = "";
        public string carNameRight = "";
        public double? carPositionLeft = 0;
        public double? carPositionRight = 0;

        public void Spotter(GameReaderCommon.GameData gameData, double trackLength)
        {
            bool spotLeft = Convert.ToBoolean(gameData.NewData.SpotterCarLeft);                         //Spotter call left
            bool spotRight = Convert.ToBoolean(gameData.NewData.SpotterCarRight);                       //Spotter call left

            if (spotLeft && !leftTaken) //Catch and lock
            {
                leftTaken = true;
                double? gap = 0;
                for (int i = 0; i < gameData.NewData.OpponentsAheadOnTrack.Count; i++)
                {
                    if (((gameData.NewData.OpponentsAheadOnTrack[i].TrackPositionPercentToPlayer != 0 && gap == 0) || gameData.NewData.OpponentsAheadOnTrack[i].TrackPositionPercentToPlayer * trackLength < gap) && gameData.NewData.OpponentsAheadOnTrack[i].Name != carNameRight)
                    {
                        carNameLeft = gameData.NewData.OpponentsAheadOnTrack[i].Name;
                        carPositionLeft = gameData.NewData.OpponentsAheadOnTrack[i].TrackPositionPercentToPlayer * trackLength;
                        gap = carPositionLeft;
                    }
                }
                for (int i = 0; i < gameData.NewData.OpponentsBehindOnTrack.Count; i++)
                {
                    if (((gameData.NewData.OpponentsBehindOnTrack[i].TrackPositionPercentToPlayer != 0 && gap == 0) || -1 * gameData.NewData.OpponentsBehindOnTrack[i].TrackPositionPercentToPlayer * trackLength < gap) && gameData.NewData.OpponentsBehindOnTrack[i].Name != carNameRight)
                    {
                        carNameLeft = gameData.NewData.OpponentsBehindOnTrack[i].Name;
                        carPositionLeft = gameData.NewData.OpponentsBehindOnTrack[i].TrackPositionPercentToPlayer * trackLength;
                        gap = carPositionLeft;
                    }
                }
            }

            if (leftTaken) //Read
            {
                for (int i = 0; i < gameData.NewData.OpponentsAheadOnTrack.Count; i++)
                {
                    if (gameData.NewData.OpponentsAheadOnTrack[i].Name == carNameLeft)
                    {
                        carPositionLeft = gameData.NewData.OpponentsAheadOnTrack[i].TrackPositionPercentToPlayer * trackLength;
                        break;
                    }
                }
                for (int i = 0; i < gameData.NewData.OpponentsBehindOnTrack.Count; i++)
                {
                    if (gameData.NewData.OpponentsBehindOnTrack[i].Name == carNameLeft)
                    {
                        carPositionLeft = gameData.NewData.OpponentsBehindOnTrack[i].TrackPositionPercentToPlayer * trackLength;
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
                for (int i = 0; i < gameData.NewData.OpponentsAheadOnTrack.Count; i++)
                {
                    if (((gameData.NewData.OpponentsAheadOnTrack[i].TrackPositionPercentToPlayer != 0 && gap == 0) || gameData.NewData.OpponentsAheadOnTrack[i].TrackPositionPercentToPlayer * trackLength < gap) && gameData.NewData.OpponentsAheadOnTrack[i].Name != carNameLeft)
                    {
                        carNameRight = gameData.NewData.OpponentsAheadOnTrack[i].Name;
                        carPositionRight = gameData.NewData.OpponentsAheadOnTrack[i].TrackPositionPercentToPlayer * trackLength;
                        gap = carPositionRight;
                    }
                }
                for (int i = 0; i < gameData.NewData.OpponentsBehindOnTrack.Count; i++)
                {
                    if (((gameData.NewData.OpponentsBehindOnTrack[i].TrackPositionPercentToPlayer != 0 && gap == 0) || -1 * gameData.NewData.OpponentsBehindOnTrack[i].TrackPositionPercentToPlayer * trackLength < gap) && gameData.NewData.OpponentsBehindOnTrack[i].Name != carNameLeft)
                    {
                        carNameRight = gameData.NewData.OpponentsBehindOnTrack[i].Name;
                        carPositionRight = gameData.NewData.OpponentsBehindOnTrack[i].TrackPositionPercentToPlayer * trackLength;
                        gap = carPositionRight;
                    }
                }
            }

            if (rightTaken) //Read
            {
                for (int i = 0; i < gameData.NewData.OpponentsAheadOnTrack.Count; i++)
                {
                    if (gameData.NewData.OpponentsAheadOnTrack[i].Name == carNameRight)
                    {
                        carPositionRight = gameData.NewData.OpponentsAheadOnTrack[i].TrackPositionPercentToPlayer * trackLength;
                        break;
                    }
                }
                for (int i = 0; i < gameData.NewData.OpponentsBehindOnTrack.Count; i++)
                {
                    if (gameData.NewData.OpponentsBehindOnTrack[i].Name == carNameRight)
                    {
                        carPositionRight = gameData.NewData.OpponentsBehindOnTrack[i].TrackPositionPercentToPlayer * trackLength;
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
