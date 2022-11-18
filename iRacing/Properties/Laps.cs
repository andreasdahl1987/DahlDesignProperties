using System;
using System.Collections.Generic;
using IRacingReader;

namespace User.PluginSdkDemo.iRacing
{
    public class Laps
    {
        private readonly DahlDesign Base;
        private readonly DataSampleEx irData;
        private readonly iRacing.Data irDB;


        /// <summary>Lap distance</summary>
        public double trackPosition;
        /// <summary>Completed laps</summary>
        public int completedLaps;
        public int currentLap;
        /// <summary>Total laps</summary>
        public int totalLaps;
        /// <summary>Current lap time</summary>
        public TimeSpan currentLapTime;
        /// <summary>Last lap time</summary>
        public TimeSpan lastLapTime;

        /// <summary>Joker lap</summary>
        public bool onJokerLap;
        public bool jokerThisLap = false;
        public int jokerLapCount = 0;
        public bool jokerLapChecker = false;


        List<double> lapDeltaCurrent = new List<double> { };
        public List<double> lapDeltaLast = new List<double> { };
        public List<double> lapDeltaSessionBest = new List<double> { };
        public List<double> lapDeltaRecord = new List<double> { };

        List<double> lapDeltaLastChange = new List<double> { };
        List<double> lapDeltaSessionBestChange = new List<double> { };
        List<double> lapDeltaLapRecordChange = new List<double> { };
        List<double> lastChunks = new List<double> { };
        List<double> SBChunks = new List<double> { };
        List<double> LRChunks = new List<double> { };

        int myDeltaIndexOld = -1;

        int deltaChangeChunks = 20;
        public int trackSections = 60;

        public bool findLapRecord = true;



        public LapRecords lapRecords;


        public Laps(DahlDesign dahlDesign, DataSampleEx irdata, iRacing.Data iRacingBase)
        {
            Base = dahlDesign;
            irData = irdata;
            irDB = iRacingBase;

            lapRecords = new LapRecords();


            Base.AddProp("DeltaLastLapChange", "");
            Base.AddProp("DeltaSessionBestChange", "");
            Base.AddProp("DeltaLapRecordChange", "");
            Base.AddProp("JokerThisLap", false);
            Base.AddProp("JokerCount", 0);
        }

        public void DataUpdate()
        {
            if (!Base.gameRunning)
                DataIdle();

            trackPosition = irData.Telemetry.LapDistPct;
            completedLaps = Base.gameData.NewData.CompletedLaps;
            currentLap = Base.gameData.NewData.CurrentLap;
            totalLaps = Base.gameData.NewData.TotalLaps;
            currentLapTime = Base.gameData.NewData.CurrentLapTime;
            lastLapTime = Base.gameData.NewData.LastLapTime;

            irData.Telemetry.TryGetValue("SessionOnJokerLap", out object rawisOnJoker);             //Joker lap
            onJokerLap = Convert.ToBoolean(rawisOnJoker);
            //-------------------------------------
            //-------RX JOKER DETECTION------------
            //-------------------------------------

            if (onJokerLap)
            {
                jokerThisLap = true;
            }

            Base.SetProp("JokerThisLap", jokerThisLap);
            Base.SetProp("JokerCount", jokerLapCount);

            //----------------------------------------------------
            //--------CHECK FOR BEST LAP--------------------------
            //----------------------------------------------------

            // ##TODO## NOT WORKING
            // LapRecords.lapFetch(ref findLapRecord, csvAdress, ref csvIndex, Base.track, Base.carModel,
            // ref lapRecord, ref lapDeltaRecord, lapDeltaSections);




            //-----------------------------------------------------------------------------
            //----------------------LAP DELTA TIMING---------------------------------------
            //-----------------------------------------------------------------------------

            for (int i = 0; i < lapRecords.lapDeltaSections + 1; i++)
            {
                lapDeltaCurrent.Add(-1);
                lapDeltaSessionBest.Add(-1);
                lapDeltaLast.Add(-1);
                lapDeltaRecord.Add(-1);
                lapDeltaLastChange.Add(0);
                lapDeltaSessionBestChange.Add(0);
                lapDeltaLapRecordChange.Add(0);
            }

            for (int i = 0; i < deltaChangeChunks; i++)
            {
                lastChunks.Add(0);
                SBChunks.Add(0);
                LRChunks.Add(0);
            }

            int myDeltaIndex = ((int)((trackPosition * lapRecords.lapDeltaSections) * 100)) / 100;

            if (myDeltaIndex >= lapRecords.lapDeltaSections)
            {
                myDeltaIndex = trackSections - 1;
            }
            if (myDeltaIndex < 0)
            {
                myDeltaIndex = 0;
                myDeltaIndexOld = 0;
            }

            double deltaLastLap = 0;
            double deltaSessionBest = 0;
            double deltaLapRecord = 0;


            if (myDeltaIndex != myDeltaIndexOld)
            {
                myDeltaIndexOld = myDeltaIndex;

                lapDeltaCurrent[myDeltaIndex + 1] = currentLapTime.TotalMilliseconds;

                if (currentLapTime.TotalSeconds < 2 && lapDeltaCurrent[0] != 1)
                {
                    lapDeltaCurrent[0] = 1; //This lap recording checked for full-length
                }

                bool passCheck = (irDB.pit == 0 && (myDeltaIndex > 5 || (myDeltaIndex > 3 && lapDeltaLast[myDeltaIndex + 1] < 10000 && lapDeltaCurrent[myDeltaIndex + 1] < 10000)));

                //Setting last lap delta
                if (passCheck && lapDeltaLast[myDeltaIndex + 1] > 0 && lapDeltaCurrent[myDeltaIndex + 1] > 0)
                {
                    deltaLastLap = (lapDeltaCurrent[myDeltaIndex + 1] - lapDeltaLast[myDeltaIndex + 1]) / 1000;
                    lapDeltaLastChange[myDeltaIndex] = deltaLastLap;
                    Base.SetProp("DeltaLastLap", deltaLastLap);
                }

                if (lapDeltaLast[myDeltaIndex + 1] == -1)
                {
                    Base.SetProp("DeltaLastLap", 0);
                }

                if (myDeltaIndex > 5 && lapDeltaSessionBest[myDeltaIndex + 1] < 10000)
                {
                    passCheck = false;
                }

                //Setting session best lap delta
                if (passCheck && lapDeltaSessionBest[myDeltaIndex + 1] > 0 && lapDeltaCurrent[myDeltaIndex + 1] > 0)
                {
                    deltaSessionBest = (lapDeltaCurrent[myDeltaIndex + 1] - lapDeltaSessionBest[myDeltaIndex + 1]) / 1000;
                    lapDeltaSessionBestChange[myDeltaIndex] = deltaSessionBest;
                    Base.SetProp("DeltaSessionBest", deltaSessionBest);
                }
                if (lapDeltaSessionBest[myDeltaIndex + 1] == -1)
                {
                    Base.SetProp("DeltaSessionBest", 0);
                }


                //Setting lap record delta
                bool recordCheck = (irDB.pit == 0 && (myDeltaIndex > 5 || (myDeltaIndex > 3 && lapDeltaRecord[myDeltaIndex + 1] < 10000 && lapDeltaCurrent[myDeltaIndex + 1] < 10000)));
                if (recordCheck && lapDeltaRecord[myDeltaIndex + 1] > 0 && lapDeltaCurrent[myDeltaIndex + 1] > 0)
                {
                    deltaLapRecord = (lapDeltaCurrent[myDeltaIndex + 1] - lapDeltaRecord[myDeltaIndex + 1]) / 1000;
                    lapDeltaLapRecordChange[myDeltaIndex] = deltaLapRecord;
                    Base.SetProp("DeltaLapRecord", deltaLapRecord);
                }
                if (lapDeltaRecord[myDeltaIndex + 1] == -1)
                {
                    Base.SetProp("DeltaLapRecord", 0);
                }


                if (myDeltaIndex == 0) //last section, copy to last lap. Further copy to session/ATB on lap changes. (from last lap)
                {
                    for (int i = 0; i < lapRecords.lapDeltaSections + 1; i++)
                    {
                        lapDeltaLast[i] = lapDeltaCurrent[i];
                        lapDeltaCurrent[i] = -1;
                    }
                }
            }

            int chunkSize = lapRecords.lapDeltaSections / deltaChangeChunks;
            int currentChunk = myDeltaIndex / chunkSize;
            bool changeStarted = false;
            double changeSum = 0;
            double firstOfChunk = 0;
            double lastOfChunk = 0;

            if (lapDeltaLast[myDeltaIndex + 1] > 0)
            {
                for (int i = currentChunk * chunkSize; i < myDeltaIndex + 1; i++)
                {
                    if (!changeStarted)
                    {
                        firstOfChunk = lapDeltaLastChange[i];
                    }
                    changeStarted = true;

                    if (i == myDeltaIndex)
                    {
                        lastOfChunk = lapDeltaLastChange[i];
                    }
                }
            }

            if (changeStarted)
            {
                changeSum = lastOfChunk - firstOfChunk;
            }

            lastChunks[currentChunk] = changeSum;

            string lastResult = string.Join(",", lastChunks); //push result as string

            changeStarted = false;
            changeSum = 0;
            firstOfChunk = 0;
            lastOfChunk = 0;

            for (int i = currentChunk * chunkSize; i < myDeltaIndex + 1; i++)
            {
                if (!changeStarted)
                {
                    firstOfChunk = lapDeltaSessionBestChange[i];
                }
                changeStarted = true;

                if (i == myDeltaIndex)
                {
                    lastOfChunk = lapDeltaSessionBestChange[i];
                }
            }

            if (changeStarted)
            {
                changeSum = lastOfChunk - firstOfChunk;
            }

            SBChunks[currentChunk] = changeSum;

            string SBResult = string.Join(",", SBChunks); //push result as string

            changeStarted = false;
            changeSum = 0;
            firstOfChunk = 0;
            lastOfChunk = 0;

            for (int i = currentChunk * chunkSize; i < myDeltaIndex + 1; i++)
            {
                if (!changeStarted)
                {
                    firstOfChunk = lapDeltaLapRecordChange[i];
                }
                changeStarted = true;

                if (i == myDeltaIndex)
                {
                    lastOfChunk = lapDeltaLapRecordChange[i];
                }
            }

            if (changeStarted)
            {
                changeSum = lastOfChunk - firstOfChunk;
            }

            LRChunks[currentChunk] = changeSum;

            string LRResult = string.Join(",", LRChunks); //push result as string



            Base.SetProp("DeltaLastLapChange", lastResult);
            Base.SetProp("DeltaSessionBestChange", SBResult);
            Base.SetProp("DeltaLapRecordChange", LRResult);


            DataClear();
        }

        void DataClear()
        {
            if (Base.counter == 59)
            {
                lapDeltaCurrent.Clear();
                lapDeltaSessionBest.Clear();
                lapDeltaLast.Clear();
                lapDeltaRecord.Clear();
                lapDeltaLastChange.Clear();
                lapDeltaSessionBestChange.Clear();
                lapDeltaLapRecordChange.Clear();
                lastChunks.Clear();
                SBChunks.Clear();
                LRChunks.Clear();


                for (int i = 0; i < lapRecords.lapDeltaSections + 1; i++)
                {
                    lapDeltaCurrent.Add(-1);
                    lapDeltaSessionBest.Add(-1);
                    lapDeltaLast.Add(-1);
                    lapDeltaRecord.Add(-1);
                    lapDeltaLastChange.Add(0);
                    lapDeltaSessionBestChange.Add(0);
                    lapDeltaLapRecordChange.Add(0);
                }
                for (int i = 0; i < deltaChangeChunks; i++)
                {
                    lastChunks.Add(0);
                    SBChunks.Add(0);
                    LRChunks.Add(0);
                }
            }
        }

        void DataIdle()
        {
            jokerThisLap = false;
            jokerLapChecker = false;
        }


    }
}
