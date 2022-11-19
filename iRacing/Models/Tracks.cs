namespace DahlDesign.Plugin
{


    public class Tracks
    {
        public string Id {get; set;}
        public int TrackType {get; set;}
        public bool HasExempt {get; set;}
        public double ExemptOne {get; set;}
        public double ExemptOneMargin {get; set;}
        public double ExemptTwo {get; set;}
        public double ExemptTwoMargin {get; set;}
        public bool HasCutOff {get; set;}
        public double CutOff {get; set;}
        public double PitStopBase {get; set;}
        public double PitStopMaxSpeed {get; set;}
        public double PitStopCornerSpeed {get; set;}
        public double PitStopBrakeDistance {get; set;}
        public double PitStopAcceleration {get; set;}
        public bool HasAnimatedCrew {get; set;}
        public string PitFastSide {get; set;}



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
            this.TrackType = trackType;
            this.HasExempt = hasExempt;
            this.ExemptOne = exemptOne;
            this.ExemptOneMargin = exemptOneMargin;
            this.ExemptTwo = exemptTwo;
            this.ExemptTwoMargin = exemptTwoMargin;
            this.HasCutOff = hasCutOff;
            this.CutOff = cutOff;
            this.PitStopBase = pitStopBase;
            this.PitStopMaxSpeed = pitStopMaxSpeed;
            this.PitStopCornerSpeed = pitStopCornerSpeed;
            this.PitStopBrakeDistance = pitStopBrakeDistance;
            this.PitStopAcceleration = pitStopAcceleration;
            this.HasAnimatedCrew = hasAnimatedCrew;
            this.PitFastSide = pitFastSide;
            this.Id = id;
        }

    }
}
