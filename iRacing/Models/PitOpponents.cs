using System;

namespace DahlDesign.Plugin
{
    public class pitOpponents
    {
        private double? gap;
        private string name;
        private double? trackPosition;
        private bool isSameClass;
        private int classDifference;
        private int position;
        private TimeSpan bestLap;
        private bool isAhead;
        private bool isFaster;
        private bool usedRealGap;

        // constructor
        public pitOpponents(double? gap, string name, double? trackPosition, bool isSameClass, int classDifference, int position, TimeSpan bestLap, bool isAhead, bool isFaster, bool usedRealGap)
        {
            this.gap = gap;
            this.name = name;
            this.trackPosition = trackPosition;
            this.isSameClass = isSameClass;
            this.classDifference = classDifference;
            this.position = position;
            this.bestLap = bestLap;
            this.isAhead = isAhead;
            this.isFaster = isFaster;
            this.usedRealGap = usedRealGap;
        }
        // utility functions for the class
        public double? Gap
        {
            get 
            { 
                return gap; 
            }
            set 
            { 
                gap = value; 
            }
        }
        public string Name
        {
            get 
            { 
                return name; 
            }
            set
            { 
                name = value; 
            }
        }

        public double? TrackPosition
        {
            get
            {
                return trackPosition;
            }
            set
            {
                trackPosition = value;
            }
        }
        public bool IsSameClass
        {
            get 
            { 
                return isSameClass; 
            }
            set 
            { 
                isSameClass = value; 
            }
        }

        public int ClassDifference
        {
            get
            {
                return classDifference;
            }
            set
            {
                classDifference = value;
            }
        }

        public int Position
        {
            get 
            { 
                return position; 
            }
            set 
            { 
                position = value; 
            }
        }

        public TimeSpan BestLap
        {
            get
            {
                return bestLap;
            }
            set
            {
                bestLap = value;
            }
        }

        public bool IsAhead
        {
            get
            {
                return isAhead;
            }
            set
            {
                isAhead = value;
            }
        }

        public bool IsFaster
        {
            get
            {
                return isFaster;
            }
            set
            {
                isFaster = value;
            }
        }

        public bool UsedRealGap
        {
            get
            {
                return usedRealGap;
            }
            set
            {
                usedRealGap = value;
            }
        }


    }
}
