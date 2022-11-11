using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.PluginSdkDemo
{
	public class Cars
	{
		public string Id;
		public int ShiftPoint1;
		public int ShiftPoint2;
		public int ShiftPoint3;
		public int ShiftPoint4;
		public int ShiftPoint5;
		public int ShiftPoint6;
		public int ShiftPoint7;
		public bool HasAntiStall;
		public bool HasDRS;
		public bool HasTCtog;
		public bool HasTCtimer;
		public int TCOffPosition;
		public bool HasABS;
		public bool HasTC;
		public bool HasABStog;
		public int ABSOffPosition;
		public int MapHigh;
		public int MapLow;
		public bool HasNoBoost;
		public bool HasOvertake;
		public string RotaryType;
		public string DashType;
		public double RevLim;
		public int IdleRPM;
		public double ClutchBitePoint;
		public double ClutchSpin;
		public double ClutchIdealRangeStart;
		public double ClutchIdealRangeStop;
		public double ClutchGearRelease;
		public double ClutchTimeRelease;
		public double ClutchGearReleased;
		public double ClutchTimeReleased;
		public bool HighPower;
		public double LaunchThrottle;
		public double PitMaxSpeed;
		public double PitCornerSpeed;
		public double PitBrakeDistance;
		public double PitAcceleration;
		public double PitFuelFillRate;
		public bool PitHasAnimatedCrew;
		public double PitAniBaseTime;
		public double PitAniSlowAdd;
		public double PitBaseTime;
		public double PitSlowAdd;
		public CrewType CrewType;
		public bool PitMultitask;
		public bool PitHasWindscreen;
		public AnimationType AnimationType;
		public double RevSpeed;

		public Cars(string id, bool hasAntiStall, bool hasDRS, bool hasTCtog,bool hasTCtimer, int TCoffPosition, bool hasABS, bool hasTC, bool hasABStog, int ABSoffPosition, int mapHigh, int mapLow,bool hasNoBoost,bool hasOvertake,string rotaryType,string dashType,int shiftPoint1,int shiftPoint2,int shiftPoint3,int shiftPoint4,int shiftPoint5,int shiftPoint6,int shiftPoint7,double revLim,int idleRPM,double clutchBitePoint,double clutchSpin,double clutchIdealRangeStart,double clutchIdealRangeStop,double clutchGearRelease,double clutchTimeRelease,double clutchGearReleased,double clutchTimeReleased,bool highPower, double launchThrottle, double pitMaxSpeed,double pitCornerSpeed,double pitBrakeDistance,double pitAcceleration,double pitFuelFillRate,bool pitHasAnimatedCrew,double pitAniBaseTime,double pitAniSlowAdd,double pitBaseTime,double pitSlowAdd,CrewType crewType,bool pitMultitask,bool pitHasWindscreen,AnimationType animationType,double revSpeed)
		{
			Id = id;
			HasAntiStall = hasAntiStall;
			HasDRS = hasDRS;
			HasTCtog = hasTCtog;
			HasTCtimer = hasTCtimer;
			TCOffPosition = TCoffPosition;
			HasABS = hasABS;
			HasTC = hasTC;
			HasABStog = hasABStog;
			ABSOffPosition = ABSoffPosition;
			MapHigh = mapHigh;
			MapLow = mapLow;
			HasNoBoost = hasNoBoost;
			HasOvertake = hasOvertake;
			RotaryType = rotaryType;
			DashType = dashType;
			ShiftPoint1 = shiftPoint1;
			ShiftPoint2 = shiftPoint2;
			ShiftPoint3 = shiftPoint3;
			ShiftPoint4 = shiftPoint4;
			ShiftPoint5 = shiftPoint5;
			ShiftPoint6 = shiftPoint6;
			ShiftPoint7 = shiftPoint7;
			RevLim = revLim;
			IdleRPM = idleRPM;
			ClutchBitePoint = clutchBitePoint;
			ClutchSpin = clutchSpin;
			ClutchIdealRangeStart = clutchIdealRangeStart;
			ClutchIdealRangeStop = clutchIdealRangeStop;
			ClutchGearRelease = clutchGearRelease;
			ClutchTimeRelease = clutchTimeRelease;
			ClutchGearReleased = clutchGearReleased;
			ClutchTimeReleased = clutchTimeReleased;
			HighPower = highPower;
			LaunchThrottle = launchThrottle;
			PitMaxSpeed = pitMaxSpeed;
			PitCornerSpeed = pitCornerSpeed;
			PitBrakeDistance = pitBrakeDistance;
			PitAcceleration = pitAcceleration;
			PitFuelFillRate = pitFuelFillRate;
			PitHasAnimatedCrew = pitHasAnimatedCrew;
			PitAniBaseTime = pitAniBaseTime;
			PitAniSlowAdd = pitAniSlowAdd;
			PitBaseTime = pitBaseTime;
			PitSlowAdd = pitSlowAdd;
			CrewType = crewType;
			PitMultitask = pitMultitask;
			PitHasWindscreen = pitHasWindscreen;
			AnimationType = animationType;
			RevSpeed = revSpeed;
		}


	}
}
