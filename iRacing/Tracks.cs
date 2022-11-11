using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.PluginSdkDemo
{


    public class Tracks
    {
        private int trackType;
        private bool hasExempt;
        private double exemptOne;
        private double exemptOneMargin;
        private double exemptTwo;
        private double exemptTwoMargin;
        private bool hasCutOff;
        private double cutOff;
        private double pitStopBase;
        private double pitStopMaxSpeed;
        private double pitStopCornerSpeed;
        private double pitStopBrakeDistance;
        private double pitStopAcceleration;
        private bool hasAnimatedCrew;
        private string pitFastSide;



        public Tracks(
            string id, 
            int trackType,
            bool hasExempt, 
            double exemptOne,
            double exemptOneMargin,
            double exemptTwo,
            double exemptTwoMargin,
            bool hasCutOff, 
            double cutOff, 
            double pitStopBase, 
            double pitStopMaxSpeed, 
            double pitStopCornerSpeed, 
            double pitStopBrakeDistance, 
            double pitStopAcceleration,
            bool hasAnimatedCrew,
            string pitFastSide)
        {
            this.trackType = trackType;
            this.hasExempt = hasExempt;
            this.exemptOne = exemptOne;
            this.exemptOneMargin = exemptOneMargin;
            this.exemptTwo = exemptTwo;
            this.exemptTwoMargin = exemptTwoMargin;
            this.hasCutOff = hasCutOff;
            this.cutOff = cutOff;
            this.pitStopBase = pitStopBase;
            this.pitStopMaxSpeed = pitStopMaxSpeed;
            this.pitStopCornerSpeed = pitStopCornerSpeed;
            this.pitStopBrakeDistance = pitStopBrakeDistance;
            this.pitStopAcceleration = pitStopAcceleration;
            this.hasAnimatedCrew = hasAnimatedCrew;
            this.pitFastSide = pitFastSide;
            Id = id;
        }

        public string Id;

        public int TrackType
        {
            get { return trackType; }
            set { trackType = value; }
        }


        public bool HasExempt
        {
            get { return hasExempt; }
            set { hasExempt = value; }
        }
        public double ExemptOne
        {
            get { return exemptOne; }
            set { exemptOne = value; }
        }
        public double ExemptOneMargin
        {
            get { return exemptOneMargin; }
            set { exemptOneMargin = value; }
        }

        public double ExemptTwo
        {
            get { return exemptTwo; }
            set { exemptTwo = value; }
        }

        public double ExemptTwoMargin
        {
            get { return exemptTwoMargin; }
            set { exemptTwoMargin = value; }
        }

        public bool HasCutOff
        {
            get { return hasCutOff; }
            set { hasCutOff = value; }
        }

        public double CutOff
        {
            get { return cutOff; }
            set { cutOff = value; }
        }
        public double PitStopBase
        {
            get { return pitStopBase; }
            set { pitStopBase = value; }
        }

        public double PitStopMaxSpeed
        {
            get { return pitStopMaxSpeed; }
            set { pitStopMaxSpeed = value; }
        }
        public double PitStopCornerSpeed
        {
            get { return pitStopCornerSpeed; }
            set { pitStopCornerSpeed = value; }
        }

        public double PitStopBrakeDistance
        {
            get { return pitStopBrakeDistance; }
            set { pitStopBrakeDistance = value; }
        }

        public double PitStopAcceleration
        {
            get { return pitStopAcceleration; }
            set { pitStopAcceleration = value; }
        }

        public bool HasAnimatedCrew
        {
            get { return hasAnimatedCrew; }
            set { hasAnimatedCrew = value; }
        }

        public string PitFastSide
        {
            get { return pitFastSide; }
            set { pitFastSide = value; }
        }


    }



}
