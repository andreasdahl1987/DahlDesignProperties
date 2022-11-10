using GameReaderCommon;
using SimHub.Plugins;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Reflection;
using IRacingReader;
using System.Windows.Forms;
using ACSharedMemory;


namespace User.PluginSdkDemo
{

    [PluginDescription("Dahl Design DDU Properties")]
    [PluginAuthor("Andreas Dahl")]
    [PluginName("DahlDesign")]

    

    public class DahlDesign : IPlugin, IDataPlugin, IWPFSettingsV2
    {


        public DataPluginDemoSettings Settings;

        /// <summary>
        /// Instance of the current plugin manager
        /// </summary>

        public PluginManager PluginManager { get; set; }

        public ImageSource PictureIcon => this.ToIcon(Properties.Resources.Dahl_icon);

        public string LeftMenuTitle => "Dahl Design";

        //CSV file adress
        string csvAdress = "";
        int csvIndex = 0;

        //Declaring global variables

        string myClassColor = "";
        int myClassColorIndex = 0;

        List<pitOpponents> pitStopOpponents = new List<pitOpponents> { };
        List<pitOpponents> finalList = new List<pitOpponents> { };

        int trackSections = 60;
        List<double> realGapOpponentDelta = new List<double> { };
        List<double> realGapOpponentRelative = new List<double> { };

        List<List<TimeSpan>> realGapPoints = new List<List<TimeSpan>> { };
        List<List<bool>> realGapLocks = new List<List<bool>> { };
        List<List<bool>> realGapChecks = new List<List<bool>> { };

        List<double> lapDeltaCurrent = new List<double> { };
        List<double> lapDeltaLast = new List<double> { };
        List<double> lapDeltaSessionBest = new List<double> { };
        List<double> lapDeltaRecord = new List<double> { };

        List<double> lapDeltaLastChange = new List<double> { };
        List<double> lapDeltaSessionBestChange = new List<double> { };
        List<double> lapDeltaLapRecordChange = new List<double> { };
        List<double> lastChunks = new List<double> { };
        List<double> SBChunks = new List<double> { };
        List<double> LRChunks = new List<double> { };
        bool findLapRecord = true;

        int myDeltaIndexOld = -1;
        int lapDeltaSections = 120;
        int deltaChangeChunks = 20;


        int counter = 0;

        bool pitMenuRequirementMet = false;

        int roadOff = 0;
        bool outLap = false;
        double cutoff = 0.02;
        bool iRIdle = true;
        bool statusReadyToFetch = false;
        bool lineCross = false;


        int currentSector = 0;
        bool sector1to2 = false;
        bool sector2to3 = false;
        bool sectorExempt = false;
        double currentSector1Time = 0;
        double currentSector2Time = 0;
        double currentSector3Time = 0;
        int currentSector1Status = 0;
        int currentSector2Status = 0;
        int currentSector3Status = 0;
        int lastSectorStatusHolder = 0;
        int sector1StatusHolder = 0;
        double sector1TimeHolder = 0;
        int sector2Incidents = 0;
        int sector3Incidents = 0;
        bool currentLapTimeStarted = false;

        double oneThird = 1d / 3d;
        double twoThirds = 2d / 3d;

        TimeSpan lapRecord = new TimeSpan(0);
        TimeSpan sessionBestLap = new TimeSpan(0);
        double sessionBestSector1 = 0;
        double sessionBestSector2 = 0;
        double sessionBestSector3 = 0;
        double sector1Pace = 0;
        double sector2Pace = 0;
        double sector3Pace = 0;

        int currentTape = 0;
        int currentPWS = 0;
        double currentFrontWing = 0;
        double currentRearWing = 0;
     
        TimeSpan lastLapHolder;
        TimeSpan lastLapChecker;
        static TimeSpan listFiller = new TimeSpan(0);
        List<TimeSpan> lapTimeList = new List<TimeSpan> { listFiller, listFiller, listFiller, listFiller, listFiller, listFiller, listFiller, listFiller };
        List<double> sector1TimeList = new List<double> { 0, 0, 0, 0, 0, 0, 0, 0 };
        List<double> sector2TimeList = new List<double> { 0, 0, 0, 0, 0, 0, 0, 0 };
        List<double> sector3TimeList = new List<double> { 0, 0, 0, 0, 0, 0, 0, 0 };
        
        int lastStatusHolder = 0;
        List<int> lapStatusList = new List<int> {0,0,0,0,0,0,0,0};
        List<int> sector1StatusList = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 };
        List<int> sector2StatusList = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 };
        List<int> sector3StatusList = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 };

        List<double> fuelTargetDeltas = new List<double> { 0, 0, 0, 0, 0, 0, 0, 0 };
        double fuelTargetDeltaCumulative = 0;
        double fuelTargetDelta = 0;

        string classLeaderName = "";
        double? classLeaderRealGap = 0;

        string carModelHolder = "";
        string trackHolder = "";
        string sessionHolder = "";

        List<double?> carAheadGap = new List<double?> { };
        List<double?> carAheadRaceGap = new List<double?> { };
        List<string> carAheadName = new List<string> { };
        List<bool> carAheadIsInPit = new List<bool> { };
        List<bool> carAheadIsClassLeader = new List<bool> { };
        List<string> carAheadClassColor = new List<string> { };
        List<int> carAheadClassDifference = new List<int> { };
        List<int> carAheadPosition = new List<int> { };
        List<bool> carAheadIsAhead = new List<bool> { };
        List<string> carAheadLicence = new List<string> { };
        List<long> carAheadiRating = new List<long> { };
        List<TimeSpan> carAheadBestLap = new List<TimeSpan> { };
        List<long> carAheadJokerLaps = new List<long> { };
        List<int> carAheadLapsSincePit = new List<int> { };
        List<int> carAheadP2PCount = new List<int> { };
        List<bool> carAheadP2PStatus = new List<bool> { };
        List<double?> carAheadRealGap = new List<double?> { };
        List<double?> carAheadRealRelative = new List<double?> { };

        string aheadGlobal = "";

        List<double?> carBehindGap = new List<double?> { };
        List<double?> carBehindRaceGap = new List<double?> { };
        List<string> carBehindName = new List<string> { };
        List<bool> carBehindIsInPit = new List<bool> { };
        List<bool> carBehindIsClassLeader = new List<bool> { };
        List<string> carBehindClassColor = new List<string> { };
        List<int> carBehindClassDifference = new List<int> { };
        List<int> carBehindPosition = new List<int> { };
        List<bool> carBehindIsAhead = new List<bool> { };
        List<string> carBehindLicence = new List<string> { };
        List<long> carBehindiRating = new List<long> { };
        List<TimeSpan> carBehindBestLap = new List<TimeSpan> { };
        List<long> carBehindJokerLaps = new List<long> { };
        List<int> carBehindLapsSincePit = new List<int> { };
        List<int> carBehindP2PCount = new List<int> { };
        List<bool> carBehindP2PStatus = new List<bool> { };
        List<double?> carBehindRealGap = new List<double?> { };
        List<double?> carBehindRealRelative = new List<double?> { };

        string behindGlobal = "";

        List<int> sessionCarsLap = new List<int> { };
        List<int> sessionCarsLapsSincePit = new List<int> { };

        double pitBox = 0;
        double awayFromPits = 0;
        bool hasPitted = false;
        bool hasApproached = false;

        int validStintLaps = 0;
        int invalidStintLaps = 0;
        TimeSpan stintTimer = new TimeSpan(0);
        TimeSpan pushTimer = new TimeSpan(0);
        TimeSpan stintTimeTotal = new TimeSpan(0);
        int stintLapsTotal = 0;
        bool stintLapsCheck = false;

        int qualyPosition = 0;
        bool raceFinished = false;

        double? aheadGap = 0;
        string aheadClass = "";
        int aheadClassPosition = 0;

        int realPosition = 0;
        int hotLapPosition = 0;
        bool isRaceLeader = false;
        List<string> finishedCars = new List<string> { };

        double myExpectedLapTime = 0;
        double? leaderDecimal = 0;
        double? lapLapsRemaining = 0;
        double? timeLapsRemaining = 0;
        bool lapRaceFinished = false;
        bool timeRaceFinished = false;
        bool timedOut = false;
        bool timeBasedChecker = false;
        int? timeLapCounter = 0;
        bool warmup = false;
        int qLap1Status = 0;
        int qLap2Status = 0;
        TimeSpan qLap1Time = new TimeSpan(0);
        TimeSpan qLap2Time = new TimeSpan(0);
        bool qLapStarted2 = false;

        bool isLapLimited = false;
        bool isTimeLimited = false;

        double pace = 0;
        double? remainingLaps = 0;

        int myIR = 0;
        double IRchange = 0;

        bool jokerThisLap = false;
        int jokerLapCount = 0;
        bool jokerLapChecker = false;

        //Track parameters
        int trackType = 0; //Track type: 0 = Road, 1-3 = RX, 4 = Dirt road w/o joker, 5 = Dirt Oval, 6 = Short oval, 7 = oval, 8 = super speedway
        bool hasExempt = false;
        double exemptOne = 0;
        double exemptOneMargin = 0;
        double exemptTwo = 0;
        double exemptTwoMargin = 0;
        bool hasCutOff = false;
        double cutoffValue = 0;
        double pitStopBase = 0;
        double pitStopMaxSpeed = 0;
        double pitStopCornerSpeed = 0;
        double pitStopBrakeDistance = 0;
        double pitStopAcceleration = 0;
        bool trackHasAnimatedCrew = false;
        string pitFastSide = "Right";

        //Car parameters
        string carId = "";
        bool hasAntiStall = false;
        bool hasDRS = false;
        bool hasTCtog = false;
        bool hasTCtimer = false;
        int TCoffPosition = -1;
        bool hasABS = false;
        bool hasTC = false;
        bool hasABStog = false;
        int ABSoffPosition = -1;
        int mapHigh = -1;
        int mapLow = -1;
        bool hasNoBoost = false;
        bool hasOvertake = false;
        string rotaryType = "Single";
        string dashType= "Default";
        int shiftPoint1 = 0;
        int shiftPoint2 = 0;
        int shiftPoint3 = 0;
        int shiftPoint4 = 0;
        int shiftPoint5 = 0;
        int shiftPoint6 = 0;
        int shiftPoint7 = 0;
        int currentShiftPoint = 0;
        double shiftPointAdjustment = 0;
        double shiftLightRPM = 0;
        int lastShiftPoint = 0;
        double revLim = 0;
        int idleRPM = 0;
        double clutchBitePoint = 0;
        double clutchSpin = 0;
        double clutchIdealRangeStart = 0;
        double clutchIdealRangeStop = 0;
        double clutchGearRelease = 0;
        double clutchTimeRelease = 0;
        double clutchGearReleased = 0;
        double clutchTimeReleased = 0;
        bool highPower = false;
        double launchThrottle = 0;
        double pitMaxSpeed = 0;
        double pitCornerSpeed = 0;
        double pitBrakeDistance = 0;
        double pitAcceleration = 0;
        double pitFuelFillRate = 0;
        bool carHasAnimatedCrew = false;
        double pitAniBaseTime = 0;
        double pitAniSlowAdd = 0;
        double pitBaseTime = 0;
        double pitSlowAdd = 0;
        CrewType pitCrewType = CrewType.SingleTyre;
        bool pitMultitask = false;
        bool pitHasWindscreen = false;
        AnimationType animaionType = AnimationType.Analog;
        double revSpeed = 1;
     
        int ERSlapCounter = 0;
        int ERSreturnMode = 0;
        bool ERSstartingLap = false;
        int ERSChangeCount = 0;
        int W12ERSRef = 0;
        

        double pitStopDuration = 0;
        bool LFTog = false;
        bool RFTog = false;
        bool LRTog = false;
        bool RRTog = false;
        bool fuelTog = false;
        bool WSTog = false;
        bool repairTog = false;

        bool sessionScreen = false;
        bool scenicActive = false;
        bool camToolActive = false;
        bool UIHidden = false;
        bool useAutoShotSelection = false;
        bool useTemporaryEdits = false;
        bool useKeyAcceleration = false;
        bool useKey10xAcceleration = false;
        bool useMouseAimMode = false;

        bool savePitTimerLock = false;
        TimeSpan savePitTimerSnap = new TimeSpan (0);
        TimeSpan slowestLapTimeSpanCopy = new TimeSpan(0);
        double minFuelPush = 0;
        double maxFuelPush = 0;
        double fuelPerLapOffset = 0;
        bool onlyThrough = true;

        bool fuelTargetCheck = false;
        double oldFuelValue = 0;
        double commandMinFuel = 0;
        double commandMaxFuel = 500;

        float LFCold = 0;
        float RFCold = 0;
        float LRCold = 0;
        float RRCold = 0;

        double minimumCornerSpeed = 0;
        double straightLineSpeed = 0;
        double highestThrottle = 0;
        bool throttleLift = false;

        string smoothGear = "";
        int neutralCounter = 0;

        //Buttons
        int pitMenuRotary = 12; //Starting on strat page
        int inCarRotary = 0;

        bool TCactive = false;
        double TCOffTimer = 0;
        bool TCLimiter = false;
        TimeSpan TCtimer = new TimeSpan(0);

        bool p2pActive = false;
        int p2pCounter = -1;
        int myTireCompound = -1;
        int myDRSCount = -1;

        bool radio = false;
        string radioName = "";
        int radioPosition;
        bool radioIsSpectator;

        bool NBpressed = false;
        bool NBactive = false;
        bool NBspeedLim = false;
        bool NBvalue = false;

        int fuelSaveDelta = 0;
        bool plusButtonCheck = false;
        bool minusButtonCheck = false;
        bool OKButtonCheck = false;
        bool upshift = false;
        bool downshift = false;
        bool launchPressed = false;
        bool launchReleased = false;
        bool launchActive = false;
        bool paceCheck = false;
        bool pacePressed = false;
        bool paceReleased = false;
        bool pitPressed = false;
        bool pitReleased = false;
        bool pitScreenEnable = false;
        bool bitePointPressed = false;
        bool bitePointReleased = false;
        bool bitePointAdjust = false;
        bool LEDwarningActive = false;
        bool spotMode = false;

        TimeSpan stopWatch = new TimeSpan(0);
        bool accelerationStart = false;
        int accelerationPremature = 0;
        bool oneHundered = false;
        bool twoHundered = false;
        TimeSpan reactionTime = new TimeSpan(0);
        string reactionGear = "";
        double reactionPush = 0;

        TimeSpan offTrackTimer = new TimeSpan(0);
        bool offTrack = false;

        double TCrpm = 0;
        double TCthrottle = 0;
        bool TCon = false;
        int TCduration = 0;
        string TCgear = "";
        int TCgearCD = 0;
        int TCreleaseCD = 0;
        int TCdropCD = 0;
        double TCPushTimer = 0;
        int tcBumpCounter = 0;
        bool tcBump = false;
        int[] roadTextures = { 1, 2, 9, 11, 12};

        int brakeClock = 0;
        int brakeClockBase = 0;
        List<double> brakeCurve = new List<double> { };
        bool brakeTrigger = false;
        bool brakeTriggerCheck = false;
        double brakeMax = 0;

        int throttleClock = 0;
        int throttleClockBase = 0;
        List<double> throttleCurve = new List<double> { };
        bool throttleTrigger = false;
        bool throttleTriggerCheck = false;

        bool watchOn = false;
        bool watchReset = false;
        bool watchStopper = false;
        bool watchSplit = false;
        TimeSpan watchSplitTime = new TimeSpan(0);
        TimeSpan watchTimer = new TimeSpan(0);
        double watchResult = 0;
        double watchSnap = 0;

        string carNameLeft = "";
        string carNameRight = "";
        double? carPositionLeft = 0;
        double? carPositionRight = 0;
        bool leftTaken = false;
        bool rightTaken = false;

        double RPMtracker = 0;
        bool RPMgearShift = false;
        double RPMlastGear = 0;
        string RPMgear = "";

        //Switches
        int encoder1Mode = 0;
        int encoder2Mode = 0;
        int encoder3Mode = 0;
        int encoder4Mode = 0;
        int encoder5Mode = 0;
        int encoder6Mode = 0;
        int encoder7Mode = 0;
        int encoder8Mode = 0;
        int DDSmode = 0;
        int bitePointMode = 0;
        int dualClutchesMode = 0;
        int encoder15Mode = 0;

        int button1Mode = 0;
        int button2Mode = 0;
        int button3Mode = 0;
        int button4Mode = 0;
        int button5Mode = 0;
        int button6Mode = 0;
        int button7Mode = 0;
        int button8Mode = 0;
        int button9Mode = 0;
        int button10Mode = 0;
        int button11Mode = 0;
        int button15Mode = 0;
        int button16Mode = 0;

        List<string> classColors = new List<string> { "0xffda59", "0x33ceff", "0xff5888", "0xae6bff", "0x53ff77" };
        //1: light yellow
        //2: vivid cyan
        //3: light pink
        //4: very light violet
        //5: light lime green

        List<Tracks> trackInfo = new List<Tracks> { };
        List<Cars> carInfo = new List<Cars> { };

        DataSampleEx irData;

        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
        //--------------------DATA FLOW STARTS HERE-----------------------------------
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------


        public void DataUpdate(PluginManager pluginManager, ref GameData data)
        {


            //SETTINGS
            if (counter == 2)
            {
                pluginManager.SetPropertyValue("DDUstartLED", this.GetType(), Settings.DDUstartLED);
                pluginManager.SetPropertyValue("SW1startLED", this.GetType(), Settings.SW1startLED);
                pluginManager.SetPropertyValue("DDUEnabled", this.GetType(), Settings.DDUEnabled);
                pluginManager.SetPropertyValue("SW1Enabled", this.GetType(), Settings.SW1Enabled);
                pluginManager.SetPropertyValue("DashLEDEnabled", this.GetType(), Settings.DashLEDEnabled);
                pluginManager.SetPropertyValue("DashType", this.GetType(), Settings.DashType);
                pluginManager.SetPropertyValue("LapInfoScreen", this.GetType(), Settings.LapInfoScreen);
                pluginManager.SetPropertyValue("ShiftTimingAssist", this.GetType(), Settings.ShiftTimingAssist);
                pluginManager.SetPropertyValue("ShiftWarning", this.GetType(), Settings.ShiftWarning);
                pluginManager.SetPropertyValue("ARBswapped", this.GetType(), Settings.SupercarSwapPosition);
                pluginManager.SetPropertyValue("ARBstiffForward", this.GetType(), Settings.SupercarARBDirection);
                pluginManager.SetPropertyValue("SmallFuelIncrement", this.GetType(), Settings.SmallFuelIncrement);
                pluginManager.SetPropertyValue("LargeFuelIncrement", this.GetType(), Settings.LargeFuelIncrement);
                pluginManager.SetPropertyValue("CoupleInCarToPit", this.GetType(), Settings.CoupleInCarToPit);
            }

            pluginManager.SetPropertyValue("ShowBrakeThrottleGaugesEnabled", this.GetType(), Settings.ShowBrakeThrottleGaugesEnabled);

            //---------------------------------------------------
            //----------------GETTING DATA------------------------
            //----------------------------------------------------
            bool gameRunning = data.GameRunning;
            string gameName = data.GameName;


            //FRAME COUNTER FOR CPU SAVING
            counter++;
            //Counters used: 1,2,3,4,5,6,7,8,9,10,11,14,15,17,20,22,24,25,27,30,33,35,36,38,39,40,43,45,47,50,51,52,53,54,55,59  


            //----------------------------------------------------------------------------
            //----------------------------------------------------------------------------
            //----------------------------------------------------------------------------
            //--------------------DDC CALCULATIONS------------------------------------
            //----------------------------------------------------------------------------
            //----------------------------------------------------------------------------
            //----------------------------------------------------------------------------

            pluginManager.SetPropertyValue("DDCDDSEnabled", this.GetType(), false);
            pluginManager.SetPropertyValue("DDCclutchEnabled", this.GetType(), false);

            bool controllerEnabled = Settings.DDCEnabled;  

            var controllerSearch = pluginManager.GetPropertyValue("JoystickPlugin." + Settings.DDC + "_Z");
            if (controllerSearch == null)
            {
                controllerEnabled = false;
            }
            
            pluginManager.SetPropertyValue("DDCEnabled", this.GetType(), controllerEnabled);

            if (Settings.SW1Enabled)
            {
                int encoderField = Convert.ToInt32(pluginManager.GetPropertyValue("JoystickPlugin." + Settings.DDC + "_Z")); //Encoder field
                encoder1Mode = (encoderField & 1);
                encoder2Mode = (encoderField & 2) >> 1;
                encoder3Mode = (encoderField & 4) >> 2;

                string bitField = Convert.ToString(encoderField, 2).PadLeft(16, '0');

                encoder5Mode = (encoderField & 768) >> 8;
                encoder6Mode = (encoderField & 3072) >> 10;
                encoder7Mode = (encoderField & 12288) >> 12;
                encoder8Mode = (encoderField & 16384) >> 14;

                pluginManager.SetPropertyValue("SW1HandbrakeActive", this.GetType(), encoder1Mode);
                pluginManager.SetPropertyValue("SW1QuickSwitchMode", this.GetType(), encoder2Mode);
                
                pluginManager.SetPropertyValue("SW1DDSMode", this.GetType(), encoder5Mode);
                pluginManager.SetPropertyValue("SW1ClutchMode", this.GetType(), encoder7Mode);
                pluginManager.SetPropertyValue("SW1BiteSetting", this.GetType(), encoder6Mode);
                pluginManager.SetPropertyValue("SW1QuickSwitchActive", this.GetType(), encoder8Mode);


                int buttonField = Convert.ToInt32(pluginManager.GetPropertyValue("JoystickPlugin." + Settings.DDC + "_Y")); //Buttonfield
                button1Mode = buttonField & 1;
                button2Mode = (buttonField & 2) >> 1;
                button3Mode = (buttonField & 4) >> 2;
                button4Mode = (buttonField & 8) >> 3;
                button5Mode = (buttonField & 16) >> 4;
                button6Mode = (buttonField & 32) >> 5;
                button7Mode = (buttonField & 64) >> 6;
                button8Mode = (buttonField & 128) >> 7;
                button9Mode = (buttonField & 256) >> 8;
                button10Mode = (buttonField & 512) >> 9;
                button11Mode = (buttonField & 15360) >> 10;
                button15Mode = (buttonField & 16384) >> 14;
                button16Mode = (buttonField & 32768) >> 15;

                double clutchValue = Convert.ToInt32(pluginManager.GetPropertyValue("JoystickPlugin." + Settings.DDC + "_X")) / 655.35;
                double bitePointValue = Convert.ToInt32(pluginManager.GetPropertyValue("JoystickPlugin." + Settings.DDC + "_RY")) / 655.35;
                double brakeValue = Convert.ToInt32(pluginManager.GetPropertyValue("JoystickPlugin." + Settings.DDC + "_RZ")) / 655.35;
                double throttleValue = Convert.ToInt32(pluginManager.GetPropertyValue("JoystickPlugin." + Settings.DDC + "_Slider0")) / 655.35;

                pluginManager.SetPropertyValue("SW1RadioButtonMode", this.GetType(), button1Mode);
                pluginManager.SetPropertyValue("SW1RightRotaryMode", this.GetType(), button2Mode);
                pluginManager.SetPropertyValue("SW1LeftRotaryMode", this.GetType(), button3Mode);
                pluginManager.SetPropertyValue("SW1MagicToggleMode", this.GetType(), button4Mode);
                pluginManager.SetPropertyValue("SW1RightToggleMode", this.GetType(), button5Mode);
                pluginManager.SetPropertyValue("SW1LeftToggleMode", this.GetType(), button6Mode);
                pluginManager.SetPropertyValue("SW1ShifterMode", this.GetType(), button7Mode);
                pluginManager.SetPropertyValue("SW1NeutralActive", this.GetType(), button8Mode);
                pluginManager.SetPropertyValue("SW1ThrottleHoldActive", this.GetType(), button9Mode);
                pluginManager.SetPropertyValue("SW1MagicToggleActive", this.GetType(), button10Mode);
                pluginManager.SetPropertyValue("SW1Preset", this.GetType(), button11Mode + 1);
                pluginManager.SetPropertyValue("SW1NeutralMode", this.GetType(), button15Mode);

                pluginManager.SetPropertyValue("SW1Clutch", this.GetType(), Math.Round(clutchValue, 1));
                pluginManager.SetPropertyValue("SW1BitePoint", this.GetType(), Math.Round(bitePointValue, 1));
                pluginManager.SetPropertyValue("SW1Brake", this.GetType(), Math.Round(brakeValue, 1));
                pluginManager.SetPropertyValue("SW1Throttle", this.GetType(), Math.Round(throttleValue, 1));

                pluginManager.SetPropertyValue("DDCDDSEnabled", this.GetType(), Settings.DDSEnabled);
                pluginManager.SetPropertyValue("DDCclutchEnabled", this.GetType(), Settings.DDCclutchEnabled);
            }

            else if (controllerEnabled)
            {
                int encoderField = Convert.ToInt32(pluginManager.GetPropertyValue("JoystickPlugin." + Settings.DDC + "_Z")); //Encoder field
                encoder1Mode = encoderField & 1;
                encoder2Mode = (encoderField & 2) >> 1;
                encoder3Mode = (encoderField & 4) >> 2;
                encoder4Mode = (encoderField & 8) >> 3;
                encoder5Mode = (encoderField & 16) >> 4;
                encoder6Mode = (encoderField & 32) >> 5;
                encoder7Mode = (encoderField & 64) >> 6;
                encoder8Mode = (encoderField & 128) >> 7;

                DDSmode = (encoderField & 768) >> 8;
                bitePointMode = (encoderField & 3072) >> 10;
                dualClutchesMode = (encoderField & 12288) >> 12;
                encoder15Mode = (encoderField & 16384) >> 14;


                pluginManager.SetPropertyValue("DDCR1", this.GetType(), encoder1Mode);
                pluginManager.SetPropertyValue("DDCR2", this.GetType(), encoder2Mode);
                pluginManager.SetPropertyValue("DDCR3", this.GetType(), encoder3Mode);
                pluginManager.SetPropertyValue("DDCR4", this.GetType(), encoder4Mode);
                pluginManager.SetPropertyValue("DDCR5", this.GetType(), encoder5Mode);
                pluginManager.SetPropertyValue("DDCR6", this.GetType(), encoder6Mode);
                pluginManager.SetPropertyValue("DDCR7", this.GetType(), encoder7Mode);
                pluginManager.SetPropertyValue("DDCR8", this.GetType(), encoder8Mode);
                pluginManager.SetPropertyValue("DDCR15", this.GetType(), encoder15Mode);


                pluginManager.SetPropertyValue("DDCDDSMode", this.GetType(), DDSmode);
                pluginManager.SetPropertyValue("DDCclutchMode", this.GetType(), dualClutchesMode);
                pluginManager.SetPropertyValue("DDCbiteSetting", this.GetType(), bitePointMode); 

                int buttonField = Convert.ToInt32(pluginManager.GetPropertyValue("JoystickPlugin." + Settings.DDC + "_Y")); //Buttonfield
                button1Mode = buttonField & 1;
                button2Mode = (buttonField & 2) >> 1;
                button3Mode = (buttonField & 4) >> 2;
                button4Mode = (buttonField & 8) >> 3;
                button5Mode = (buttonField & 16) >> 4;
                button6Mode = (buttonField & 32) >> 5;
                button7Mode = (buttonField & 64) >> 6;
                button8Mode = (buttonField & 128) >> 7;
                button9Mode = (buttonField & 256) >> 8;
                button10Mode = (buttonField & 512) >> 9;
                button11Mode = (buttonField & 15360) >> 10;
                button15Mode = (buttonField & 16384) >> 14;
                button16Mode = (buttonField & 32768) >> 15;

                double clutchValue = Convert.ToInt32(pluginManager.GetPropertyValue("JoystickPlugin." + Settings.DDC + "_X")) / 655.35;
                double bitePointValue = Convert.ToInt32(pluginManager.GetPropertyValue("JoystickPlugin." + Settings.DDC + "_RY")) / 655.35;
                double brakeValue = Convert.ToInt32(pluginManager.GetPropertyValue("JoystickPlugin." + Settings.DDC + "_RZ")) / 655.35;
                double throttleValue = Convert.ToInt32(pluginManager.GetPropertyValue("JoystickPlugin." + Settings.DDC + "_Slider0")) / 655.35;

                pluginManager.SetPropertyValue("DDCB1", this.GetType(), button1Mode);
                pluginManager.SetPropertyValue("DDCB2", this.GetType(), button2Mode);
                pluginManager.SetPropertyValue("DDCB3", this.GetType(), button3Mode);
                pluginManager.SetPropertyValue("DDCB4", this.GetType(), button4Mode);

                pluginManager.SetPropertyValue("DDCthrottleHoldActive", this.GetType(), button6Mode);
                pluginManager.SetPropertyValue("DDCmagicActive", this.GetType(), button7Mode);
                pluginManager.SetPropertyValue("DDCquickSwitchMode", this.GetType(), button8Mode);
                pluginManager.SetPropertyValue("DDCquickSwitchActive", this.GetType(), button9Mode);
                pluginManager.SetPropertyValue("DDChandbrakeActive", this.GetType(), button10Mode);
                pluginManager.SetPropertyValue("DDCPreset", this.GetType(), button11Mode+1);
                pluginManager.SetPropertyValue("DDCneutralMode", this.GetType(), button15Mode);
                pluginManager.SetPropertyValue("DDCneutralActive", this.GetType(), button5Mode);

                pluginManager.SetPropertyValue("DDCclutch", this.GetType(), Math.Round(clutchValue, 1));
                pluginManager.SetPropertyValue("DDCbitePoint", this.GetType(), Math.Round(bitePointValue, 1));
                pluginManager.SetPropertyValue("DDCbrake", this.GetType(), Math.Round(brakeValue, 1));
                pluginManager.SetPropertyValue("DDCthrottle", this.GetType(), Math.Round(throttleValue, 1));

                pluginManager.SetPropertyValue("DDCDDSEnabled", this.GetType(), Settings.DDSEnabled);
                pluginManager.SetPropertyValue("DDCclutchEnabled", this.GetType(), Settings.DDCclutchEnabled);
            }


            //----------------------------------------------------------------------------
            //----------------------------------------------------------------------------
            //----------------------------------------------------------------------------
            //--------------------IRACING CALCULATIONS------------------------------------
            //----------------------------------------------------------------------------
            //----------------------------------------------------------------------------
            //----------------------------------------------------------------------------


            if (gameName == "IRacing" && gameRunning)
            {

                //Gaining access to raw data
                if (data?.NewData?.GetRawDataObject() is DataSampleEx) { irData = data.NewData.GetRawDataObject() as DataSampleEx; }

                //Updating relevant data
                TimeSpan globalClock = TimeSpan.FromTicks(DateTime.Now.Ticks);

                irData.Telemetry.TryGetValue("PlayerCarTeamIncidentCount", out object rawIncidents);
                int incidents = Convert.ToInt32(rawIncidents);                                          //Incidents

                irData.Telemetry.TryGetValue("PlayerCarInPitStall", out object rawStall);
                int pitStall = Convert.ToInt32(rawStall);                                               //Pit Stall

                irData.Telemetry.TryGetValue("ManualBoost", out object rawBoost);
                bool boost = Convert.ToBoolean(rawBoost);                                               //Boost

                irData.Telemetry.TryGetValue("PowerMGU_K", out object rawMGU);
                int MGU = Convert.ToInt32(rawMGU);                                                      //MGU-K current

                irData.Telemetry.TryGetValue("EnergyERSBatteryPct", out object rawBattery);
                double battery = Convert.ToDouble(rawBattery);                                          //Battery

                irData.Telemetry.TryGetValue("DRS_Status", out object rawDRS);
                int DRSState = Convert.ToInt32(rawDRS);                                                 //DRS state

                double slipLF = Convert.ToDouble(pluginManager.GetPropertyValue("ShakeITMotorsV3Plugin.Export.WheelSlip.FrontLeft"));  //Wheel slip
                double slipRF = Convert.ToDouble(pluginManager.GetPropertyValue("ShakeITMotorsV3Plugin.Export.WheelSlip.FrontRight"));  //Wheel slip
                double slipLR = Convert.ToDouble(pluginManager.GetPropertyValue("ShakeITMotorsV3Plugin.Export.WheelSlip.RearLeft"));  //Wheel slip
                double slipRR = Convert.ToDouble(pluginManager.GetPropertyValue("ShakeITMotorsV3Plugin.Export.WheelSlip.RearRight"));  //Wheel slip

                double trackPosition = irData.Telemetry.LapDistPct;                                     //Lap distance
                bool spotLeft = Convert.ToBoolean(data.NewData.SpotterCarLeft);                         //Spotter call left
                bool spotRight = Convert.ToBoolean(data.NewData.SpotterCarRight);                       //Spotter call left
                int completedLaps = data.NewData.CompletedLaps;                                         //Completed laps
                int currentLap = data.NewData.CurrentLap;                                               //Current lap
                int totalLaps = data.NewData.TotalLaps;                                                 //Total laps
                TimeSpan currentLapTime = data.NewData.CurrentLapTime;                                  //Current lap time
                int pit = data.NewData.IsInPit;                                                         //Pit
                int pitLimiter = data.NewData.PitLimiterOn;                                             //Pit limiter on/off
                string gear = data.NewData.Gear;                                                        //Gear
                double fuelAvgLap = Convert.ToDouble(pluginManager.GetPropertyValue("DataCorePlugin.Computed.Fuel_LitersPerLap")); //Fuel avg lap
                int black = data.NewData.Flag_Black;                                                    //Black flag
                int white = data.NewData.Flag_White;                                                    //White flag
                int checkered = data.NewData.Flag_Checkered;                                            //Checkered flag
                TimeSpan lastLapTime = data.NewData.LastLapTime;                                        //Last Lap Time 
                string carModel = data.NewData.CarModel;                                                //Car model
                string track = data.NewData.TrackName;                                                  //Track name
                string session = data.NewData.SessionTypeName;                                          //Session type
                TimeSpan timeLeft = data.NewData.SessionTimeLeft;                                       //Session time left
                double pitLocation = irData.SessionData.DriverInfo.DriverPitTrkPct;                     //Pit location
                double trackLength = data.NewData.TrackLength;                                          //Track length
                double defaultRevLim = data.NewData.CarSettings_MaxRPM;                                 //Default rev limiter
                int pitSpeedLimit = 0;                                                                  //Pit speed limit
                if (irData.SessionData.WeekendInfo.TrackPitSpeedLimit != null)
                {
                    if (Convert.ToInt32(irData.SessionData.WeekendInfo.TrackPitSpeedLimit.Substring(0, 1)) != 0)
                    {
                        pitSpeedLimit = Convert.ToInt32(irData.SessionData.WeekendInfo.TrackPitSpeedLimit.Substring(0, 2));
                    }
                }

                int ERSlimit = 0;
                if (pitSpeedLimit > 70)
                {
                    ERSlimit = 76;
                }
                else
                {
                    ERSlimit = 52;
                }
                int sessionNumber = irData.Telemetry.SessionNum;                                        //Session number, to find correct session
                string trackConfig = irData.SessionData.WeekendInfo.TrackType;                          //Track type name
                int greenFlag = data.NewData.Flag_Green;                                                //Green flag

                irData.Telemetry.TryGetValue("dcTractionControlToggle", out object rawTCswitch);        //In-game TC toggle
                bool TCswitch = Convert.ToBoolean(rawTCswitch);

                irData.Telemetry.TryGetValue("dcABSToggle", out object rawABSswitch);                   //In-game ABS toggle
                bool ABSswitch = Convert.ToBoolean(rawABSswitch);

                irData.Telemetry.TryGetValue("dcABS", out object rawABS);                               //In-game ABS switch position
                int ABS = Convert.ToInt32(rawABS);

                irData.Telemetry.TryGetValue("dcTractionControl", out object rawTC);                    //In-game TC switch position
                int TC = Convert.ToInt32(rawTC);

                irData.Telemetry.TryGetValue("PlayerTrackSurfaceMaterial", out object rawSurface);      //Track surface type
                int surface = Convert.ToInt32(rawSurface);

                double stintLength = data.NewData.StintOdo;                                             //Stint length
                int opponents = data.NewData.Opponents.Count;                                           //All opponents
                int classOpponents = data.NewData.PlayerClassOpponentsCount;                            //Class opponents
                double fuel = data.NewData.Fuel;                                                        //Fuel on tank

                irData.Telemetry.TryGetValue("SessionState", out object rawSessionState);
                int sessionState = Convert.ToInt32(rawSessionState);                                    //Session State

                irData.Telemetry.TryGetValue("PlayerTrackSurface", out object rawtrackLocation);
                int trackLocation = Convert.ToInt32(rawtrackLocation);                                  //TrkLoc

                irData.Telemetry.TryGetValue("dpWingFront", out object rawWingFront);                   //Front wing setting
                double wingFront = Math.Round(Convert.ToDouble(rawWingFront), 2);

                irData.Telemetry.TryGetValue("dpWingRear", out object rawWingRear);                     //Rear wing setting
                double wingRear = Math.Round(Convert.ToDouble(rawWingRear), 1);

                irData.Telemetry.TryGetValue("dpQTape", out object rawtape);                            //Tape
                int tape = Convert.ToInt16(rawtape);

                irData.Telemetry.TryGetValue("dpPowerSteering", out object rawPWS);                     //Powersteering
                int PWS = Convert.ToInt16(rawPWS);

                double gearRatio = Convert.ToDouble(pluginManager.GetPropertyValue("GameRawData.SessionData.CarSetup.Chassis.Rear.DropGearARatio")); //Gear ratio

                irData.Telemetry.TryGetValue("SessionOnJokerLap", out object rawisOnJoker);             //Joker lap
                bool onJokerLap = Convert.ToBoolean(rawisOnJoker);

                irData.Telemetry.TryGetValue("PlayerCarIdx", out object rawPlayerIdx);                  //My CarIdx
                int myCarIdx = Convert.ToInt32(rawPlayerIdx);

                irData.Telemetry.TryGetValue("CarIdxP2P_Count", out object p2pCount);                   //P2P Counts
                irData.Telemetry.TryGetValue("CarIdxP2P_Status", out object p2pStatus);                 //P2P Statuses
                irData.Telemetry.TryGetValue("CarIdxBestLapTime", out object BestLapTimes);             //BestLapTimes
                irData.Telemetry.TryGetValue("CarIdxTireCompound", out object tireCompounds);           //Tire compounds

                bool furled = Convert.ToBoolean(pluginManager.GetPropertyValue("GameRawData.Telemetry.SessionFlagsDetails.IsFurled"));  //Furled flag

                irData.Telemetry.TryGetValue("LRshockVel", out object rawLRShockVel);                   //Left rear shock
                double LRShockVel = Convert.ToDouble(rawLRShockVel);

                irData.Telemetry.TryGetValue("LRshockVel", out object rawRRShockVel);                   //Right rear shock
                double RRShockVel = Convert.ToDouble(rawRRShockVel);

                irData.Telemetry.TryGetValue("DRS_Count", out object rawDRSCount);                      //DRS Count
                if (rawDRSCount != null)
                {
                    myDRSCount = Convert.ToInt32(rawDRSCount);
                }
                else
                {
                    myDRSCount = 0;
                }

                var estimatedLapTime = (TimeSpan)(pluginManager.GetPropertyValue("PersistantTrackerPlugin.EstimatedLapTime")); //EstimatedLapTime

                if (data.NewData.OpponentsAheadOnTrack.Count > 0)
                {
                    aheadGap = data.NewData.OpponentsAheadOnTrack[0].GaptoPlayer;                       //Ahead GAP
                    aheadClass = data.NewData.OpponentsAheadOnTrack[0].CarClass;                        //Ahead Class
                    aheadClassPosition = data.NewData.OpponentsAheadOnTrack[0].PositionInClass;         //Ahead Position (class)
                }
                string myClass = data.NewData.CarClass;                                                 //My Class
                int myPosition = irData.Telemetry.PlayerCarClassPosition;                               //My Position (class)
                double throttle = data.NewData.Throttle;                                                //Throttle application
                double brake = data.NewData.Brake;                                                      //Brake application
                double clutch = data.NewData.Clutch;                                                    //Clutch application
                double speed = data.NewData.SpeedLocal;                                                 //Speed
                double rpm = data.NewData.Rpms;                                                         //RPM value

                double plannedFuel = Convert.ToDouble(irData.Telemetry.PitSvFuel);                      //Planned fuel
                double maxFuel = data.NewData.MaxFuel;
                float plannedLFPressure = irData.Telemetry.PitSvLFP;                                    //Planned LF pressure
                float plannedRFPressure = irData.Telemetry.PitSvRFP;                                    //Planned RF pressure
                float plannedLRPressure = irData.Telemetry.PitSvLRP;                                    //Planned LR pressure
                float plannedRRPressure = irData.Telemetry.PitSvRRP;                                    //Planned RR pressure

                int cam = irData.Telemetry.CamCameraState;                                              //Cam state
                sessionScreen = Convert.ToBoolean(cam & 1);
                scenicActive = Convert.ToBoolean(cam & 2);
                camToolActive = Convert.ToBoolean(cam & 4);
                UIHidden = Convert.ToBoolean(cam & 8);
                useAutoShotSelection = Convert.ToBoolean(cam & 16);
                useTemporaryEdits = Convert.ToBoolean(cam & 32);
                useKeyAcceleration = Convert.ToBoolean(cam & 64);
                useKey10xAcceleration = Convert.ToBoolean(cam & 128);
                useMouseAimMode = Convert.ToBoolean(cam & 256);

                int pitInfo = irData.Telemetry.PitSvFlags;                                              //Pit stop toggles
                LFTog = Convert.ToBoolean(pitInfo & 1);
                RFTog = Convert.ToBoolean(pitInfo & 2);
                LRTog = Convert.ToBoolean(pitInfo & 4);
                RRTog = Convert.ToBoolean(pitInfo & 8);
                fuelTog = Convert.ToBoolean(pitInfo & 16);
                WSTog = Convert.ToBoolean(pitInfo & 32);
                repairTog = Convert.ToBoolean(pitInfo & 64);




                //-----------------------------------------------
                //--------TIRE ATTRIBUTES------------------------
                //-----------------------------------------------

                if (counter == 47)
                {
                    LFCold = irData.Telemetry.LFcoldPressure;
                    RFCold = irData.Telemetry.RFcoldPressure;
                    LRCold = irData.Telemetry.LRcoldPressure;
                    RRCold = irData.Telemetry.RRcoldPressure;

                    pluginManager.SetPropertyValue("PitServiceLFPCold", this.GetType(), LFCold);
                    pluginManager.SetPropertyValue("PitServiceRFPCold", this.GetType(), RFCold);
                    pluginManager.SetPropertyValue("PitServiceLRPCold", this.GetType(), LRCold);
                    pluginManager.SetPropertyValue("PitServiceRRPCold", this.GetType(), RRCold);
                }

                //----------------------------------------------
                //--------SMOOTH GEAR---------------------------
                //----------------------------------------------

                if (gear != "N")
                {
                    smoothGear = gear;
                    neutralCounter = 0;
                }

                if (gear == "N")
                {
                    neutralCounter++;
                }

                if (neutralCounter > 6)
                {
                    smoothGear = "N";
                    neutralCounter = 0;
                }
                if (button8Mode == 1)
                {
                    smoothGear = "N";
                }

                //----------------------------------------------
                //--------SoF AND IR LOSS/GAIN------------------
                //----------------------------------------------

                if (counter == 8)
                {
                    List<double?> iratings = new List<double?> { };
                    double weight = 1600 / Math.Log(2);
                    double posCorr = (classOpponents / 2 - realPosition) / 100;

                    for (int i = 0; i < opponents; i++)
                    {
                        if (data.NewData.Opponents[i].CarClass == myClass)
                        {
                            iratings.Add(data.NewData.Opponents[i].IRacing_IRating);
                        }
                        else
                        {
                            iratings.Add(0);
                        }
                    }

                    List<double> filtered = new List<double> { };
                    double valueHolder = 0;

                    for (int a = 0; a < iratings.Count; a++)
                    {
                        valueHolder = Convert.ToDouble(iratings[a]);
                        if (valueHolder != 0)
                        {
                            filtered.Add(valueHolder);
                        }
                    }

                    double sum = 0;
                    double IRscore = 0;

                    if (filtered.Count >= classOpponents)
                    {
                        for (int e = 0; e < classOpponents; e++)
                        {
                            sum += Math.Pow(2, -filtered[e] / 1600);
                            IRscore += (1 - Math.Exp(-myIR / weight)) * Math.Exp(-filtered[e] / weight) / ((1 - Math.Exp(-filtered[e] / weight)) * Math.Exp(-myIR / weight) + (1 - Math.Exp(-myIR / weight)) * Math.Exp(-filtered[e] / weight));
                        }
                    }

                    if (IRscore != 0)
                    {
                        IRscore = IRscore - 0.5;
                    }

                    double SoF = 0;

                    if (sum != 0)
                    {
                        SoF = Math.Round(weight * Math.Log(classOpponents / sum));
                        if (session == "Race" && !raceFinished && sessionState > 3)
                        {
                            IRchange = Math.Round((classOpponents - realPosition - IRscore - posCorr) * 200 / classOpponents);
                        }

                    }

                    pluginManager.SetPropertyValue("SoF", this.GetType(), SoF);
                    pluginManager.SetPropertyValue("IRchange", this.GetType(), IRchange);

                }

                //----------------------------------------------
                //--------OFF TRACK REGISTRATION----------------
                //----------------------------------------------
                if ((session == "Race" || session == "Practice" || session == "Open Qualify") && sessionState > 3)
                {
                    if ((trackLocation != 0 && !(pit != 1 && speed < 10) && !(awayFromPits > 2 && stintLength < 400 && stintLength > 20)) || ((currentLap == 1 || currentLap == 0) && stintLength < 400 && session == "Race"))
                    {
                        offTrackTimer = globalClock;
                    }
                    if (globalClock.TotalSeconds - offTrackTimer.TotalSeconds > 1 && speed < 150)
                    {
                        offTrack = true;
                    }
                    if (offTrack && globalClock.TotalSeconds - offTrackTimer.TotalSeconds < 1 && speed > 50)
                    {
                        offTrack = false;
                    }
                }


                //-----------------------------------------------
                //--------TRACK ATTRIBUTES UPDATE----------------
                //-----------------------------------------------

                if (counter == 1)
                {
                    //Resetting values to default

                    trackType = 0;
                    hasExempt = false;
                    exemptOne = 0;
                    exemptOneMargin = 0;
                    exemptTwo = 0;
                    exemptTwoMargin = 0;
                    hasCutOff = false;
                    cutoffValue = 0;
                    pitStopBase = 25;
                    pitStopMaxSpeed = 0;
                    pitStopCornerSpeed = 0;
                    pitStopBrakeDistance = 0;
                    pitStopAcceleration = 0;
                    trackHasAnimatedCrew = false;
                    pitFastSide = "Right";

                    //Extracting info from track list

                    for (int i = 0; i < trackInfo.Count; i++)
                    {
                        if (track == trackInfo[i].Id)
                        {
                            trackType = trackInfo[i].TrackType;
                            hasExempt = trackInfo[i].HasExempt;
                            exemptOne = trackInfo[i].ExemptOne;
                            exemptOneMargin = trackInfo[i].ExemptOneMargin;
                            exemptTwo = trackInfo[i].ExemptTwo;
                            exemptTwoMargin = trackInfo[i].ExemptTwoMargin;
                            hasCutOff = trackInfo[i].HasCutOff;
                            cutoffValue = trackInfo[i].CutOff;
                            pitStopBase = trackInfo[i].PitStopBase;
                            pitStopMaxSpeed = trackInfo[i].PitStopMaxSpeed;
                            pitStopCornerSpeed = trackInfo[i].PitStopCornerSpeed;
                            pitStopBrakeDistance = trackInfo[i].PitStopBrakeDistance;
                            pitStopAcceleration = trackInfo[i].PitStopAcceleration;
                            trackHasAnimatedCrew = trackInfo[i].HasAnimatedCrew;
                            pitFastSide = trackInfo[i].PitFastSide;

                            break;
                        }
                    }

                    if (hasCutOff)
                    {
                        cutoff = cutoffValue;
                    }
                    else
                    {
                        cutoff = 0.02;
                    }

                    if (trackType == 0)
                    {
                        if (trackConfig == "short oval")
                        {
                            trackType = 6;
                        }
                        else if (trackConfig == "medium oval")
                        {
                            trackType = 7;
                        }
                        else if (trackConfig == "super speedway")
                        {
                            trackType = 8;
                        }
                        else if (trackConfig == "dirt oval")
                        {
                            trackType = 5;
                        }
                        else if (trackConfig == "dirt road course")
                        {
                            trackType = 4;
                        }
                    }

                }

                //-----------------------------------------------------------------------------
                //----------------------CAR ATTRIBUTES UPDATE----------------------------------
                //-----------------------------------------------------------------------------


                if (counter == 14)
                {

                    //Resetting values to default
                    carId = "";
                    hasAntiStall = false;
                    hasDRS = false;
                    hasTCtog = false;
                    hasTCtimer = false;
                    TCoffPosition = -1;
                    hasABStog = false;
                    hasABS = false;
                    hasTC = false;
                    ABSoffPosition = -1;
                    mapHigh = -1;
                    mapLow = -1;
                    hasNoBoost = false;
                    hasOvertake = false;
                    rotaryType = "Single";
                    dashType = "Default";
                    shiftPoint1 = 0;
                    shiftPoint2 = 0;
                    shiftPoint3 = 0;
                    shiftPoint4 = 0;
                    shiftPoint5 = 0;
                    shiftPoint6 = 0;
                    shiftPoint7 = 0;
                    revLim = defaultRevLim;
                    idleRPM = 0;
                    clutchBitePoint = 40;
                    clutchSpin = 0;
                    clutchIdealRangeStart = 0;
                    clutchIdealRangeStop = 0;
                    clutchGearRelease = 1;
                    clutchTimeRelease = 0;
                    clutchGearReleased = 1;
                    clutchTimeReleased = 100;
                    highPower = false;
                    launchThrottle = 0;
                    pitMaxSpeed = 1;
                    pitCornerSpeed = 1;
                    pitBrakeDistance = 1;
                    pitAcceleration = 1;
                    pitFuelFillRate = 2.7;
                    carHasAnimatedCrew = false;
                    pitAniBaseTime = 0;
                    pitAniSlowAdd = 0;
                    pitBaseTime = 0;
                    pitSlowAdd = 0;
                    pitCrewType = CrewType.SingleTyre;
                    pitMultitask = true;
                    pitHasWindscreen = true;
                    animaionType = AnimationType.Analog;
                    revSpeed = 1;
                    


                    for (int i = 0; i < carInfo.Count; i++)
                    {
                        if (carModel == carInfo[i].Id)
                        {
                            carId = carInfo[i].Id;
                            hasAntiStall = carInfo[i].HasAntiStall;
                            hasDRS = carInfo[i].HasDRS;
                            hasTCtog = carInfo[i].HasTCtog;
                            hasTCtimer = carInfo[i].HasTCtimer;
                            TCoffPosition = carInfo[i].TCOffPosition;
                            hasABStog = carInfo[i].HasABStog;
                            hasABS = carInfo[i].HasABS;
                            hasTC = carInfo[i].HasTC;
                            ABSoffPosition = carInfo[i].ABSOffPosition;
                            mapHigh = carInfo[i].MapHigh;
                            mapLow = carInfo[i].MapLow;
                            hasNoBoost = carInfo[i].HasNoBoost;
                            hasOvertake = carInfo[i].HasOvertake;
                            rotaryType = carInfo[i].RotaryType;
                            dashType = carInfo[i].DashType;
                            shiftPoint1 = carInfo[i].ShiftPoint1;
                            shiftPoint2 = carInfo[i].ShiftPoint2;
                            shiftPoint3 = carInfo[i].ShiftPoint3;
                            shiftPoint4 = carInfo[i].ShiftPoint4;
                            shiftPoint5 = carInfo[i].ShiftPoint5;
                            shiftPoint6 = carInfo[i].ShiftPoint6;
                            shiftPoint7 = carInfo[i].ShiftPoint7;
                            revLim = carInfo[i].RevLim;
                            idleRPM = carInfo[i].IdleRPM;
                            clutchBitePoint = carInfo[i].ClutchBitePoint;
                            clutchSpin = carInfo[i].ClutchSpin;
                            clutchIdealRangeStart = carInfo[i].ClutchIdealRangeStart;
                            clutchIdealRangeStop = carInfo[i].ClutchIdealRangeStop;
                            clutchGearRelease = carInfo[i].ClutchGearRelease;
                            clutchTimeRelease = carInfo[i].ClutchTimeRelease;
                            clutchGearReleased = carInfo[i].ClutchGearReleased;
                            clutchTimeReleased = carInfo[i].ClutchTimeReleased;
                            highPower = carInfo[i].HighPower;
                            launchThrottle = carInfo[i].LaunchThrottle;
                            pitMaxSpeed = carInfo[i].PitMaxSpeed;
                            pitCornerSpeed = carInfo[i].PitCornerSpeed;
                            pitBrakeDistance = carInfo[i].PitBrakeDistance;
                            pitAcceleration = carInfo[i].PitAcceleration;
                            pitFuelFillRate = carInfo[i].PitFuelFillRate;
                            carHasAnimatedCrew = carInfo[i].PitHasAnimatedCrew;
                            pitAniBaseTime = carInfo[i].PitAniBaseTime;
                            pitAniSlowAdd = carInfo[i].PitAniSlowAdd;
                            pitBaseTime = carInfo[i].PitBaseTime;
                            pitSlowAdd = carInfo[i].PitSlowAdd;
                            pitCrewType = carInfo[i].CrewType;
                            pitMultitask = carInfo[i].PitMultitask;
                            pitHasWindscreen = carInfo[i].PitHasWindscreen;
                            animaionType = carInfo[i].AnimationType;
                            revSpeed = carInfo[i].RevSpeed;
                            break;
                        }
                    }

                    if (Settings.DashType != "Automatic Selection")
                    {
                        dashType = Settings.DashType;
                    }
                    
                    if (p2pCount != null)
                    {
                        p2pCounter = ((int[])p2pCount)[myCarIdx];
                    }
                    else
                    {
                        p2pCounter = -1;
                    }

                    if (p2pStatus != null)
                    {
                        p2pActive = ((bool[])p2pStatus)[myCarIdx];
                    }
                    else
                    {
                        p2pActive = false;
                    }

                    if (tireCompounds != null)
                    {
                        myTireCompound = ((int[])tireCompounds)[myCarIdx];
                    }
                    else
                    {
                        myTireCompound = -1;
                    }

                    //No pit stop tracks
                    if (trackType > 0 && trackType < 5)
                    {
                        rotaryType = "Default";
                    }

                    //Supercar gear ratio bite point setting
                    if (dashType == "Supercar")
                    {
                        switch (gearRatio)
                        {
                            case 0.85:
                                clutchBitePoint = 28;
                                clutchSpin = 0;
                                clutchIdealRangeStart = 28;
                                clutchIdealRangeStop = 31;
                                launchThrottle = 100;

                                break;
                            case 0.931:
                                clutchBitePoint = 30.0;
                                clutchSpin = 29.0;
                                clutchIdealRangeStart = 29.5;
                                clutchIdealRangeStop = 33;
                                launchThrottle = 85;
                                break;
                            case 0.96:
                                clutchBitePoint = 30.0;
                                clutchSpin = 29.5;
                                clutchIdealRangeStart = 31.0;
                                clutchIdealRangeStop = 34;
                                launchThrottle = 85;
                                break;
                            case 1:
                                clutchBitePoint = 32.0;
                                clutchSpin = 31.5;
                                clutchIdealRangeStart = 32.0;
                                clutchIdealRangeStop = 35;
                                launchThrottle = 80;
                                break;
                            case 1.042:
                                clutchBitePoint = 34.0;
                                clutchSpin = 33.0;
                                clutchIdealRangeStart = 34.0;
                                clutchIdealRangeStop = 36;
                                launchThrottle = 75;
                                break;
                            case 1.074:
                                clutchBitePoint = 34.0;
                                clutchSpin = 33.0;
                                clutchIdealRangeStart = 35.0;
                                clutchIdealRangeStop = 37;
                                launchThrottle = 70;
                                break;
                            case 1.13:
                                clutchBitePoint = 36.0;
                                clutchSpin = 35.0;
                                clutchIdealRangeStart = 35.5;
                                clutchIdealRangeStop = 38;
                                launchThrottle = 67;
                                break;
                        }
                    }



                    pluginManager.SetPropertyValue("LaunchBitePoint", this.GetType(), clutchBitePoint);
                    pluginManager.SetPropertyValue("LaunchSpin", this.GetType(), clutchSpin);
                    pluginManager.SetPropertyValue("LaunchIdealRangeStart", this.GetType(), clutchIdealRangeStart);
                    pluginManager.SetPropertyValue("LaunchIdealRangeStop", this.GetType(), clutchIdealRangeStop);
                    pluginManager.SetPropertyValue("LaunchGearRelease", this.GetType(), clutchGearRelease);
                    pluginManager.SetPropertyValue("LaunchGearReleased", this.GetType(), clutchGearReleased);
                    pluginManager.SetPropertyValue("LaunchTimeRelease", this.GetType(), clutchTimeRelease);
                    pluginManager.SetPropertyValue("LaunchTimeReleased", this.GetType(), clutchTimeReleased);
                    pluginManager.SetPropertyValue("HighPower", this.GetType(), highPower);
                    pluginManager.SetPropertyValue("LaunchThrottle", this.GetType(), launchThrottle);

                    pluginManager.SetPropertyValue("OptimalShiftGear1", this.GetType(), shiftPoint1);
                    pluginManager.SetPropertyValue("OptimalShiftGear2", this.GetType(), shiftPoint2);
                    pluginManager.SetPropertyValue("OptimalShiftGear3", this.GetType(), shiftPoint3);
                    pluginManager.SetPropertyValue("OptimalShiftGear4", this.GetType(), shiftPoint4);
                    pluginManager.SetPropertyValue("OptimalShiftGear5", this.GetType(), shiftPoint5);
                    pluginManager.SetPropertyValue("OptimalShiftGear6", this.GetType(), shiftPoint6);
                    pluginManager.SetPropertyValue("OptimalShiftGear7", this.GetType(), shiftPoint7);

                    pluginManager.SetPropertyValue("HasTC", this.GetType(), hasTCtimer || hasTCtog || hasTC);
                    pluginManager.SetPropertyValue("HasABS", this.GetType(), hasABS);
                    pluginManager.SetPropertyValue("HasDRS", this.GetType(), hasDRS);
                    pluginManager.SetPropertyValue("HasAntiStall", this.GetType(), hasAntiStall);
                    pluginManager.SetPropertyValue("HasOvertake", this.GetType(), hasOvertake);
                    pluginManager.SetPropertyValue("MapHigh", this.GetType(), mapHigh);
                    pluginManager.SetPropertyValue("MapLow", this.GetType(), mapLow);

                    pluginManager.SetPropertyValue("AnimationType", this.GetType(), (int)animaionType);

                    pluginManager.SetPropertyValue("TrueRevLimiter", this.GetType(), revLim);
                    pluginManager.SetPropertyValue("IdleRPM", this.GetType(), idleRPM);

                    pluginManager.SetPropertyValue("CenterDashType", this.GetType(), dashType);
                    pluginManager.SetPropertyValue("MenuType", this.GetType(), rotaryType);

                }


                //----------------------------------------------------
                //--------CHECK FOR BEST LAP--------------------------
                //----------------------------------------------------

                LapRecords.lapFetch(ref findLapRecord, csvAdress, ref csvIndex, track, carModel, ref lapRecord, ref lapDeltaRecord, lapDeltaSections);

                //----------------------------------------------------
                //--------F3.5 DRS COUNT------------------------------
                //----------------------------------------------------

                int DRSleft = 8 - myDRSCount;

                if (DRSleft < 0 || session != "Race")
                {
                    DRSleft = 0;
                }

                pluginManager.SetPropertyValue("P2PCount", this.GetType(), p2pCounter);
                pluginManager.SetPropertyValue("P2PStatus", this.GetType(), p2pActive);
                pluginManager.SetPropertyValue("DRSCount", this.GetType(), DRSleft);

                //Special considerations

                //Indycar P2P

                if (carId == "Dallara IR18")
                {
                    if (p2pActive)
                    {
                        revLim = 12300;
                        shiftPoint1 = 12250;
                        shiftPoint2 = 12270;
                        shiftPoint3 = 12280;
                        shiftPoint4 = 12280;
                        shiftPoint5 = 12280;
                    }

                    pluginManager.SetPropertyValue("OptimalShiftGear1", this.GetType(), shiftPoint1);
                    pluginManager.SetPropertyValue("OptimalShiftGear2", this.GetType(), shiftPoint2);
                    pluginManager.SetPropertyValue("OptimalShiftGear3", this.GetType(), shiftPoint3);
                    pluginManager.SetPropertyValue("OptimalShiftGear4", this.GetType(), shiftPoint4);
                    pluginManager.SetPropertyValue("OptimalShiftGear5", this.GetType(), shiftPoint5);
                    pluginManager.SetPropertyValue("OptimalShiftGear6", this.GetType(), shiftPoint6);
                    pluginManager.SetPropertyValue("OptimalShiftGear7", this.GetType(), shiftPoint7);
                    pluginManager.SetPropertyValue("TrueRevLimiter", this.GetType(), revLim);
                }



                //-----------------------------------------------
                //--------------DRS------------------------------
                //-----------------------------------------------
                string DRSpush = "";
                switch (DRSState)
                {
                    case 0:
                        DRSpush = "None";
                        break;
                    case 1:
                        DRSpush = "Acquired";
                        if (carId == "Formula Renault 3.5")
                        {
                            DRSpush = "None";
                        }
                        break;
                    case 2:
                        DRSpush = "Ready";
                        if (carId == "Formula Renault 3.5")
                        {
                            DRSpush = "Open";
                        }
                        break;
                    case 3:
                        DRSpush = "Open";
                        break;
                }

                pluginManager.SetPropertyValue("DRSState", this.GetType(), DRSpush);

                //----------------------------------------------
                //-------SHIFT LIGHT/SHIFT POINT PER GEAR-------
                //----------------------------------------------



                switch (gear)
                {
                    case "1":
                        currentShiftPoint = shiftPoint1;
                        shiftPointAdjustment = 4;
                        break;

                    case "2":
                        currentShiftPoint = shiftPoint2;
                        shiftPointAdjustment = 2;
                        break;

                    case "3":
                        currentShiftPoint = shiftPoint3;
                        shiftPointAdjustment = 1.5;
                        break;

                    case "4":
                        currentShiftPoint = shiftPoint4;
                        shiftPointAdjustment = 1;
                        break;

                    case "5":
                        currentShiftPoint = shiftPoint5;
                        shiftPointAdjustment = 0.8;
                        break;

                    case "6":
                        currentShiftPoint = shiftPoint6;
                        shiftPointAdjustment = 0.7;
                        break;

                    case "7":
                        currentShiftPoint = shiftPoint7;
                        shiftPointAdjustment = 0.4;
                        break;
                    case "8":
                        currentShiftPoint = Convert.ToInt32(revLim);
                        shiftPointAdjustment = 0;
                        break;
                }
                double amplifier = 1;

                if (gear == "N" && smoothGear == "N")
                {
                    currentShiftPoint = shiftPoint1;
                    shiftPointAdjustment = 0;
                }

                if (boost || MGU > 200000)
                {
                    amplifier = amplifier + 0.3;
                }

                if (hasDRS && DRSpush == "Open")
                {
                    amplifier = amplifier + 0.15;
                }

                double revSpeedCopy = revSpeed * amplifier;

                shiftLightRPM = currentShiftPoint - (Settings.ReactionTime * shiftPointAdjustment * revSpeedCopy);
                double throttleFraction = throttle - 30;
                if (throttleFraction < 0)
                {
                    throttleFraction = 0;
                }
                shiftLightRPM = shiftLightRPM + ((currentShiftPoint - shiftLightRPM) * (1 - throttleFraction / 70));
                if (currentShiftPoint == 0)
                {
                    shiftLightRPM = revLim;
                }


                if (rpm < shiftLightRPM)
                {
                    reactionTime = globalClock;
                    reactionGear = gear;
                }

                if (gear != reactionGear && gear == "N")
                {
                    reactionPush = globalClock.TotalMilliseconds - reactionTime.TotalMilliseconds - 40;
                    reactionGear = gear;
                }

                pluginManager.SetPropertyValue("OptimalShiftCurrentGear", this.GetType(), currentShiftPoint);
                pluginManager.SetPropertyValue("OptimalShiftLastGear", this.GetType(), lastShiftPoint);
                pluginManager.SetPropertyValue("ShiftLightRPM", this.GetType(), shiftLightRPM);
                pluginManager.SetPropertyValue("ReactionTime", this.GetType(), Math.Round(reactionPush));

                //-------------------------------------
                //-------MCLAREN MP4-30 ERS TARGET-----
                //-------------------------------------



                if (carId == "Mclaren MP4-30" || carId == "Mercedes W12")
                {
                    irData.Telemetry.TryGetValue("dcMGUKDeployMode", out object rawERSMode);
                    int ERSselectedMode = Convert.ToInt32(rawERSMode);
                    int W12ERS = ERSselectedMode;
                    int ERSstartMode = 0;
                    if (pluginManager.GetPropertyValue("GameRawData.SessionData.CarSetup.DriveBrake.PowerUnitConfig.TargetBatterySoc") != null)
                    {
                        ERSstartMode = Convert.ToInt32(Convert.ToString(pluginManager.GetPropertyValue("GameRawData.SessionData.CarSetup.DriveBrake.PowerUnitConfig.TargetBatterySoc")).Substring(0, 2));
                    }

                    if (W12ERS != W12ERSRef)
                    {
                        if (speed > ERSlimit)
                        {
                            ERSChangeCount--;
                        }
                        W12ERSRef = W12ERS;
                        if (ERSChangeCount < 0)
                        {
                            ERSChangeCount = 0;
                        }
                    }

                    if (ERSstartingLap)
                    {
                        ERSreturnMode = ERSstartMode;
                    }

                    if (currentLap != ERSlapCounter)
                    {
                        ERSlapCounter = currentLap;
                        ERSreturnMode = ERSselectedMode;
                        ERSstartingLap = false;
                    }

                    pluginManager.SetPropertyValue("ERSCharges", this.GetType(), ERSChangeCount);
                    pluginManager.SetPropertyValue("ERSTarget", this.GetType(), ERSreturnMode);
                }
                else
                {
                    pluginManager.SetPropertyValue("ERSTarget", this.GetType(), 0);
                    pluginManager.SetPropertyValue("ERSCharges", this.GetType(), 0);
                }


                //-------------------------------------
                //-------RX JOKER DETECTION------------
                //-------------------------------------

                if (onJokerLap)
                {
                    jokerThisLap = true;
                }

                //----------------------------------
                //----ACCELERATION STOPWATCH--------
                //----------------------------------

                if (gear != "N" && speed < 0.5 && rpm > 300)
                {
                    accelerationStart = true;
                }
                else if (accelerationPremature == 1)
                {
                    pluginManager.SetPropertyValue("AccelerationTo200KPH", this.GetType(), 0);
                }
                else if (accelerationPremature == 2)
                {
                    pluginManager.SetPropertyValue("AccelerationTo100KPH", this.GetType(), 0);
                    pluginManager.SetPropertyValue("AccelerationTo200KPH", this.GetType(), 0);
                }

                if (!accelerationStart && speed > 0.5)
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
                    oneHundered = false;
                    twoHundered = false;
                    pluginManager.SetPropertyValue("AccelerationTo100KPH", this.GetType(), 0);
                    pluginManager.SetPropertyValue("AccelerationTo200KPH", this.GetType(), 0);
                    accelerationStart = false;
                }

                if (!accelerationStart && speed > 0.5)
                {
                    if (!oneHundered)
                    {
                        pluginManager.SetPropertyValue("AccelerationTo100KPH", this.GetType(), globalClock.TotalSeconds - stopWatch.TotalSeconds);
                    }
                    if (!twoHundered)
                    {
                        pluginManager.SetPropertyValue("AccelerationTo200KPH", this.GetType(), globalClock.TotalSeconds - stopWatch.TotalSeconds);
                    }

                }

                if (speed > 100 && !oneHundered)
                {
                    oneHundered = true;
                    accelerationPremature = 1;
                }

                if (speed > 200 && !twoHundered)
                {
                    twoHundered = true;
                    accelerationPremature = 0;
                }

                //----------------------------------------------------
                //------------Spotter calculations--------------------
                //----------------------------------------------------

                //LEFT SIDE

                if (spotLeft && !leftTaken) //Catch and lock
                {
                    leftTaken = true;
                    double? gap = 0;
                    for (int i = 0; i < data.NewData.OpponentsAheadOnTrack.Count; i++)
                    {
                        if (((data.NewData.OpponentsAheadOnTrack[i].TrackPositionPercentToPlayer != 0 && gap == 0) || data.NewData.OpponentsAheadOnTrack[i].TrackPositionPercentToPlayer * trackLength < gap) && data.NewData.OpponentsAheadOnTrack[i].Name != carNameRight)
                        {
                            carNameLeft = data.NewData.OpponentsAheadOnTrack[i].Name;
                            carPositionLeft = data.NewData.OpponentsAheadOnTrack[i].TrackPositionPercentToPlayer * trackLength;
                            gap = carPositionLeft;
                        }
                    }
                    for (int i = 0; i < data.NewData.OpponentsBehindOnTrack.Count; i++)
                    {
                        if (((data.NewData.OpponentsBehindOnTrack[i].TrackPositionPercentToPlayer != 0 && gap == 0) || -1 * data.NewData.OpponentsBehindOnTrack[i].TrackPositionPercentToPlayer * trackLength < gap) && data.NewData.OpponentsBehindOnTrack[i].Name != carNameRight)
                        {
                            carNameLeft = data.NewData.OpponentsBehindOnTrack[i].Name;
                            carPositionLeft = data.NewData.OpponentsBehindOnTrack[i].TrackPositionPercentToPlayer * trackLength;
                            gap = carPositionLeft;
                        }
                    }
                }

                if (leftTaken) //Read
                {
                    for (int i = 0; i < data.NewData.OpponentsAheadOnTrack.Count; i++)
                    {
                        if (data.NewData.OpponentsAheadOnTrack[i].Name == carNameLeft)
                        {
                            carPositionLeft = data.NewData.OpponentsAheadOnTrack[i].TrackPositionPercentToPlayer * trackLength;
                            break;
                        }
                    }
                    for (int i = 0; i < data.NewData.OpponentsBehindOnTrack.Count; i++)
                    {
                        if (data.NewData.OpponentsBehindOnTrack[i].Name == carNameLeft)
                        {
                            carPositionLeft = data.NewData.OpponentsBehindOnTrack[i].TrackPositionPercentToPlayer * trackLength;
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
                    for (int i = 0; i < data.NewData.OpponentsAheadOnTrack.Count; i++)
                    {
                        if (((data.NewData.OpponentsAheadOnTrack[i].TrackPositionPercentToPlayer != 0 && gap == 0) || data.NewData.OpponentsAheadOnTrack[i].TrackPositionPercentToPlayer * trackLength < gap) && data.NewData.OpponentsAheadOnTrack[i].Name != carNameLeft)
                        {
                            carNameRight = data.NewData.OpponentsAheadOnTrack[i].Name;
                            carPositionRight = data.NewData.OpponentsAheadOnTrack[i].TrackPositionPercentToPlayer * trackLength;
                            gap = carPositionRight;
                        }
                    }
                    for (int i = 0; i < data.NewData.OpponentsBehindOnTrack.Count; i++)
                    {
                        if (((data.NewData.OpponentsBehindOnTrack[i].TrackPositionPercentToPlayer != 0 && gap == 0) || -1 * data.NewData.OpponentsBehindOnTrack[i].TrackPositionPercentToPlayer * trackLength < gap) && data.NewData.OpponentsBehindOnTrack[i].Name != carNameLeft)
                        {
                            carNameRight = data.NewData.OpponentsBehindOnTrack[i].Name;
                            carPositionRight = data.NewData.OpponentsBehindOnTrack[i].TrackPositionPercentToPlayer * trackLength;
                            gap = carPositionRight;
                        }
                    }
                }

                if (rightTaken) //Read
                {
                    for (int i = 0; i < data.NewData.OpponentsAheadOnTrack.Count; i++)
                    {
                        if (data.NewData.OpponentsAheadOnTrack[i].Name == carNameRight)
                        {
                            carPositionRight = data.NewData.OpponentsAheadOnTrack[i].TrackPositionPercentToPlayer * trackLength;
                            break;
                        }
                    }
                    for (int i = 0; i < data.NewData.OpponentsBehindOnTrack.Count; i++)
                    {
                        if (data.NewData.OpponentsBehindOnTrack[i].Name == carNameRight)
                        {
                            carPositionRight = data.NewData.OpponentsBehindOnTrack[i].TrackPositionPercentToPlayer * trackLength;
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

                pluginManager.SetPropertyValue("LeftCarGap", this.GetType(), carPositionLeft);
                pluginManager.SetPropertyValue("LeftCarName", this.GetType(), carNameLeft);
                pluginManager.SetPropertyValue("RightCarGap", this.GetType(), carPositionRight);
                pluginManager.SetPropertyValue("RightCarName", this.GetType(), carNameRight);


                //----------------------------------
                //-------TRIGGERED STOPWATCH--------
                //----------------------------------

                // -- Idle clock
                if (!watchOn && watchReset)
                {
                    watchTimer = globalClock;
                    watchResult = 0;
                }

                // -- Clock is started
                if (watchOn)
                {
                    watchReset = false;
                    watchResult = globalClock.TotalSeconds - watchTimer.TotalSeconds + watchSnap;
                    watchStopper = true;
                }

                //Split is captured
                if (watchOn && watchSplit)
                {
                    if (watchSplitTime.TotalSeconds == 0)
                    {
                        watchSplitTime = TimeSpan.FromSeconds(watchResult);
                    }
                    else
                    {
                        watchSplitTime = TimeSpan.FromSeconds(0);
                    }
                    watchSplit = false;
                }

                // --Clock is stopped, begin clocking the waiting time
                if (!watchOn && !watchReset)
                {
                    watchTimer = globalClock;
                    if (watchStopper)
                    {
                        watchSnap = watchResult;
                        watchStopper = false;
                    }
                }

                pluginManager.SetPropertyValue("StopWatchSplit", this.GetType(), watchSplitTime);
                pluginManager.SetPropertyValue("StopWatch", this.GetType(), TimeSpan.FromSeconds(watchResult));

                //----------------------------------
                //----------MISC--------------------
                //----------------------------------

                //Wheel slip
                if (!Settings.WheelSlipLEDs || slipLF < 25 || slipRF < 25)
                {
                    slipLF = 0;
                    slipLR = 0;
                    slipRF = 0;
                    slipRR = 0;
                }

                if (slipLF < 40 && slipLF > slipRF)
                {
                    slipRF = 0;
                    slipRR = 0;
                }
                else if (slipRF < 40 && slipRF > slipLF)
                {
                    slipLF = 0;
                    slipLR = 0;
                }

                if (slipLF == 0 && slipLR == 0)
                {
                    pluginManager.SetPropertyValue("SlipLF", this.GetType(), 0);
                    pluginManager.SetPropertyValue("SlipRF", this.GetType(), 0);
                    pluginManager.SetPropertyValue("SlipLR", this.GetType(), 0);
                    pluginManager.SetPropertyValue("SlipRR", this.GetType(), 0);
                }


                //OvertakeMode
                bool overtakeMode = false;

                if (throttle == 100 && rpm > 300 && speed > 10)
                {
                    overtakeMode = true;
                }


                //Idle property
                if (sessionScreen && !spotMode)
                {
                    iRIdle = true;
                }
                else
                {
                    iRIdle = false;
                }

                //Identifying my class color and iRating
                if (counter == 2)
                {
                    for (int i = 0; i < irData.SessionData.DriverInfo.CompetingDrivers.Length; i++)
                    {
                        if (data.NewData.PlayerName == irData.SessionData.DriverInfo.CompetingDrivers[i].UserName)
                        {
                            myClassColor = irData.SessionData.DriverInfo.CompetingDrivers[i].CarClassColor;
                            myClassColorIndex = classColors.IndexOf(myClassColor);
                            myIR = Convert.ToInt32(irData.SessionData.DriverInfo.CompetingDrivers[i].IRating);
                            pluginManager.SetPropertyValue("MyClassColor", this.GetType(), myClassColor);
                            break;
                        }
                    }
                }

                //Looking for exempt sector
                if (hasExempt && ((trackPosition > exemptOne && trackPosition < (exemptOne + exemptOneMargin)) || (trackPosition > exemptTwo && trackPosition < (exemptTwo + exemptTwoMargin))))
                {
                    sectorExempt = true;
                }
                else
                {
                    sectorExempt = false;
                }

                //----------------------------------------------------
                //--------------BUTTONS-------------------------------
                //----------------------------------------------------

                //Pit commands
                if (!Settings.CoupleInCarToPit) // Ignore all of this if we explicitly state that coupling the InCar to Pit is off in settings)
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

                bool aheadPlayerReady = false;
                bool behindPlayerReady = false;

                if (data.NewData.OpponentsAheadOnTrack.Count > 0)
                {
                    aheadPlayerReady = true;
                }
                if (data.NewData.OpponentsBehindOnTrack.Count > 0)
                {
                    behindPlayerReady = true;
                }

                if (rotaryType == "Single" && pitMenuRotary == 0)
                {
                    pitMenuRotary = inCarRotary;
                }

                if (plusButtonCheck)
                {
                    if (pitMenuRotary == 1 && pitMenuRequirementMet)
                    {
                        string pushPit = "";

                        if (commandMaxFuel == 0)
                        {
                            pushPit = "#clear ws$";
                        }
                        else
                        {
                            pushPit = "#clear fuel " + Convert.ToString(commandMaxFuel) + "l ws$";
                        }

                        PitCommands.iRacingChat(pushPit);
                    }
                    else if (pitMenuRotary == 2 && pitMenuRequirementMet)
                    {
                        launchActive = !launchActive;
                    }
                    else if (pitMenuRotary == 3 && pitMenuRequirementMet)
                    {
                        PitCommands.iRacingChat("#lf +3kpa rf +3kpa lr +3kpa rr +3kpa$");
                    }
                    else if (pitMenuRotary == 4 && pitMenuRequirementMet)
                    {
                        if (pitCrewType < CrewType.LeftRight || pitCrewType == CrewType.All)
                        {
                            PitCommands.iRacingChat("#lf +3kpa rf +3kpa$");
                        }
                        else if (pitCrewType == CrewType.LeftRight)
                        {
                            PitCommands.iRacingChat("#lf +3kpa lr +3kpa$");
                        }
                    }
                    else if (pitMenuRotary == 5 && pitMenuRequirementMet)
                    {
                        if (pitCrewType < CrewType.LeftRight || pitCrewType == CrewType.All)
                        {
                            PitCommands.iRacingChat("#lr +3kpa rr +3kpa$");
                        }
                        else if (pitCrewType == CrewType.LeftRight)
                        {
                            PitCommands.iRacingChat("#rf +3kpa rr +3kpa$");
                        }
                    }
                    else if (pitMenuRotary == 6 && pitMenuRequirementMet && aheadPlayerReady)
                    {
                        PitCommands.iRacingChat("/" + data.NewData.OpponentsAheadOnTrack[0].CarNumber + " " + Settings.AheadPlayerText);
                    }
                    else if (pitMenuRotary == 7 && pitMenuRequirementMet)
                    {
                        PitCommands.iRacingChat("#fuel +" + Settings.SmallFuelIncrement + "l$");
                    }
                    else if (pitMenuRotary == 8 && pitMenuRequirementMet)
                    {
                        PitCommands.iRacingChat("#fuel +" + Settings.LargeFuelIncrement + "l$");
                    }
                    else if (pitMenuRotary == 9 && pitMenuRequirementMet)
                    {
                        watchSplit = true;
                    }

                    else if (pitMenuRotary == 10 && pitMenuRequirementMet)
                    {
                        Settings.ShowMapEnabled = !Settings.ShowMapEnabled;
                    }

                    else if (pitMenuRotary == 11 && pitMenuRequirementMet)
                    {
                        savePitTimerLock = true;
                        savePitTimerSnap = slowestLapTimeSpanCopy;
                    }

                    else if (pitMenuRotary == 12 && pitMenuRequirementMet)
                    {
                        //pluginManager.TriggerAction("ShakeITBSV3Plugin.MainFeedbackLevelIncrement");
                        fuelPerLapOffset = fuelPerLapOffset + Settings.fuelOffsetIncrement;
                    }


                    plusButtonCheck = false;
                }

                if (minusButtonCheck)
                {
                    if (pitMenuRotary == 1 && pitMenuRequirementMet)
                    {
                        string pushPit = "";
                        if (commandMinFuel == 0)
                        {
                            pushPit = "#clear ws$";
                        }
                        else
                        {
                            pushPit = "#clear fuel " + Convert.ToString(commandMinFuel) + "l ws$";
                        }
                        PitCommands.iRacingChat(pushPit);
                    }
                    else if (pitMenuRotary == 2 && pitMenuRequirementMet)
                    {
                        paceCheck = !paceCheck;
                    }
                    else if (pitMenuRotary == 3 && pitMenuRequirementMet)
                    {
                        PitCommands.iRacingChat("#lf -3kpa rf -3kpa lr -3kpa rr -3kpa$");
                    }
                    else if (pitMenuRotary == 4 && pitMenuRequirementMet)
                    {
                        if (pitCrewType < CrewType.LeftRight || pitCrewType == CrewType.All)
                        {
                            PitCommands.iRacingChat("#rf -3kpa lf -3kpa$");
                        }
                        else if (pitCrewType == CrewType.LeftRight)
                        {
                            PitCommands.iRacingChat("#lf -3kpa lr -3kpa$");
                        }
                    }
                    else if (pitMenuRotary == 5 && pitMenuRequirementMet)
                    {
                        if (pitCrewType < CrewType.LeftRight || pitCrewType == CrewType.All)
                        {
                            PitCommands.iRacingChat("#lr -3kpa rr -3kpa$");
                        }
                        else if (pitCrewType == CrewType.LeftRight)
                        {
                            PitCommands.iRacingChat("#rf -3kpa rr -3kpa$");
                        }
                    }
                    else if (pitMenuRotary == 6 && pitMenuRequirementMet && behindPlayerReady)
                    {
                        string driverText = "/#" + data.NewData.OpponentsBehindOnTrack[0].CarNumber + " " + Settings.BehindPlayerText;
                        PitCommands.iRacingChat(driverText);
                    }
                    else if (pitMenuRotary == 7 && pitMenuRequirementMet)
                    {
                        PitCommands.iRacingChat("#fuel -" + Settings.SmallFuelIncrement + "l$");
                    }
                    else if (pitMenuRotary == 8 && pitMenuRequirementMet)
                    {
                        PitCommands.iRacingChat("#fuel -" + Settings.LargeFuelIncrement + "l$");
                    }

                    else if (pitMenuRotary == 9 && pitMenuRequirementMet)
                    {
                        watchTimer = globalClock;
                        watchSnap = 0;
                        watchReset = true;
                        watchResult = 0;
                        watchSplit = false;
                    }

                    else if (pitMenuRotary == 10 && pitMenuRequirementMet)
                    {
                        pitScreenEnable = !pitScreenEnable;

                    }

                    else if (pitMenuRotary == 11 && pitMenuRequirementMet)
                    {
                        savePitTimerLock = false;
                    }

                    else if (pitMenuRotary == 12 && pitMenuRequirementMet)
                    {
                        //pluginManager.TriggerAction("ShakeITBSV3Plugin.MainFeedbackLevelDecrement");
                        if ((fuelAvgLap + fuelPerLapOffset - Settings.fuelOffsetIncrement) > 0)
                        {
                            fuelPerLapOffset = fuelPerLapOffset - Settings.fuelOffsetIncrement;
                        }
                        else
                        {
                            fuelPerLapOffset = -fuelAvgLap;
                        }
                    }

                    minusButtonCheck = false;
                }

                if (OKButtonCheck)
                {
                    if (pitMenuRotary == 1 && pitMenuRequirementMet)
                    {
                        PitCommands.iRacingChat("#clear$");
                        fuelPerLapOffset = 0;
                    }
                    else if (pitMenuRotary == 2 && pitMenuRequirementMet)
                    {
                        PitCommands.iRacingChat("#!fr$");
                    }
                    else if (pitMenuRotary == 3 && pitMenuRequirementMet)
                    {
                        PitCommands.iRacingChat("#!cleartires$");
                    }
                    else if (pitMenuRotary == 4 && pitMenuRequirementMet)
                    {
                        if (pitCrewType < CrewType.LeftRight)
                        {
                            PitCommands.iRacingChat("#!lf !rf$");
                        }
                        else if (pitCrewType == CrewType.LeftRight)
                        {
                            PitCommands.iRacingChat("#!lf !lr$");
                        }
                        else
                        {
                            PitCommands.iRacingChat("#!cleartires$");
                        }
                    }
                    else if (pitMenuRotary == 5 && pitMenuRequirementMet)
                    {
                        if (pitCrewType < CrewType.LeftRight)
                        {
                            PitCommands.iRacingChat("#!lr !rr$");
                        }
                        else if (pitCrewType == CrewType.LeftRight)
                        {
                            PitCommands.iRacingChat("#!rf !rr$");
                        }
                        else
                        {
                            PitCommands.iRacingChat("#!cleartires$");
                        }
                    }
                    else if (pitMenuRotary == 6 && pitMenuRequirementMet)
                    {
                        PitCommands.iRacingChat("#!ws$");
                    }
                    else if (pitMenuRotary == 7 && pitMenuRequirementMet)
                    {
                        PitCommands.iRacingChat("#!fuel$");
                    }
                    else if (pitMenuRotary == 8 && pitMenuRequirementMet)
                    {
                        PitCommands.iRacingChat("#!fuel$");
                    }
                    else if (pitMenuRotary == 9 && pitMenuRequirementMet)
                    {
                        watchOn = !watchOn;
                    }
                    else if (pitMenuRotary == 10 && pitMenuRequirementMet)
                    {
                        spotMode = !spotMode;
                    }
                    else if (pitMenuRotary == 11 && pitMenuRequirementMet)
                    {
                        fuelSaveDelta++;
                        if (fuelSaveDelta > 4)
                        {
                            fuelSaveDelta = 0;
                        }
                    }
                    else if (pitMenuRotary == 12 && pitMenuRequirementMet)
                    {
                        Settings.fuelPerLapTarget = fuelAvgLap + fuelPerLapOffset;
                    }

                    OKButtonCheck = false;
                }

                //Launch control
                if (launchPressed)
                {
                    launchActive = !launchActive;
                    launchPressed = false;
                }

                if (launchReleased)
                {
                    launchActive = false;
                    launchPressed = false;
                    launchReleased = false;
                }
                pluginManager.SetPropertyValue("LaunchScreen", this.GetType(), launchActive);

                //Pit screen

                if (pitPressed)
                {
                    pitScreenEnable = !pitScreenEnable;
                    pitPressed = false;
                }

                if (pitReleased)
                {
                    pitScreenEnable = false;
                    pitPressed = false;
                    pitReleased = false;
                }
                pluginManager.SetPropertyValue("PitScreen", this.GetType(), pitScreenEnable);


                //Pace screen
                if (pacePressed)
                {
                    paceCheck = !paceCheck;
                    pacePressed = false;
                }

                if (paceReleased)
                {
                    paceCheck = false;
                    pacePressed = false;
                    paceReleased = false;
                }

                pluginManager.SetPropertyValue("PaceCheck", this.GetType(), paceCheck);

                //Bite adjust
                if (bitePointPressed)
                {
                    bitePointAdjust = !bitePointAdjust;
                    bitePointPressed = false;
                }

                if (bitePointReleased)
                {
                    bitePointAdjust = false;
                    bitePointPressed = false;
                    bitePointReleased = false;
                }
                pluginManager.SetPropertyValue("BitePointAdjust", this.GetType(), bitePointAdjust);

                //Radio toggle/name

                if (irData.Telemetry.RadioTransmitCarIdx != -1)
                {
                    radioName = irData.SessionData.DriverInfo.Drivers[irData.Telemetry.RadioTransmitCarIdx].UserName;
                    radioIsSpectator = Convert.ToBoolean(irData.SessionData.DriverInfo.Drivers[irData.Telemetry.RadioTransmitCarIdx].IsSpectator);

                    if (radioName == aheadGlobal)
                    {
                        radioPosition = realPosition - 1;
                    }
                    else if (radioName == behindGlobal)
                    {
                        radioPosition = realPosition + 1;
                    }
                    else
                    {
                        radioPosition = irData.Telemetry.CarIdxClassPosition[irData.Telemetry.RadioTransmitCarIdx];
                    }
                }
                else
                {
                    radioName = "";
                    radioIsSpectator = false;
                }

                radioName = radioName.ToUpper();

                if (irData.Telemetry.RadioTransmitCarIdx != -1)
                {
                    radio = false;
                }

                pluginManager.SetPropertyValue("Radio", this.GetType(), radio);
                pluginManager.SetPropertyValue("RadioName", this.GetType(), radioName);
                pluginManager.SetPropertyValue("RadioPosition", this.GetType(), radioPosition);
                pluginManager.SetPropertyValue("RadioIsSpectator", this.GetType(), radioIsSpectator);



                //No boost
                if (hasNoBoost)
                {
                    if (speed > 80)
                    {
                        NBspeedLim = true;
                    }

                    if (NBpressed)
                    {
                        NBactive = !NBactive;
                        NBpressed = false;
                    }

                    if (NBactive)
                    {
                        NBvalue = true;
                    }

                    if (speed < 80 && NBspeedLim || boost || !NBactive || MGU > 0 || battery == 1)
                    {
                        NBvalue = false;
                        NBspeedLim = false;
                        NBactive = false;
                    }

                    pluginManager.SetPropertyValue("NoBoost", this.GetType(), NBvalue);
                }

                //TC off toggle
                if (hasTCtimer)
                {

                    if (!TCLimiter) //Idle state
                    {
                        TCtimer = globalClock;
                    }

                    TCOffTimer = globalClock.TotalSeconds - TCtimer.TotalSeconds; //ticks/seconds, something = 0 in idle state

                    if (TCactive) //Activated, sets timer to 5, keeps tractionTimer updated as long as button is held, starts the 5 second count-up when released
                    {
                        TCOffTimer = 5;
                        TCtimer = globalClock;
                        TCLimiter = true;
                    }

                    if (globalClock.TotalSeconds - TCtimer.TotalSeconds > 5) //Ends the 5 second count-up 
                    {
                        TCLimiter = false;
                    }

                    TCPushTimer = 5 - TCOffTimer; //Refining the result
                    if (TCOffTimer > 5)
                    {
                        TCPushTimer = 0;
                    }

                    if (TCOffTimer == 5)
                    {
                        TCPushTimer = 5;
                    }
                    if (TCOffTimer == 0)
                    {
                        TCPushTimer = 0;
                    }

                    pluginManager.SetPropertyValue("TCoffTimer", this.GetType(), TimeSpan.FromSeconds(TCPushTimer));

                }
                else
                {
                    pluginManager.SetPropertyValue("TCoffTimer", this.GetType(), new TimeSpan(0));
                }

                //-----------------------------------------
                //----------TC EMULATION-------------------
                //-----------------------------------------

                //Materials on road: 2

                if (Settings.WheelSlipLEDs || ((hasTCtog && TCswitch) || (hasTCtimer && TCPushTimer == 0)) && !(pitLimiter == 1 && speed > 0.9 * pitSpeedLimit) && TC != TCoffPosition)
                {

                    if (TCrpm * 0.998 > rpm || TCdropCD > 0)  //Main filter
                    {
                        TCdropCD++;
                        if (TCdropCD > 3 && gear == TCgear)
                        {
                            TCdropCD = 0;
                        }
                    }

                    int TCgearLimit = 25;

                    if (carId == "Porsche 911 GT3.R") //Rediculous wobbly RPM on gear shift on this car
                    {
                        TCgearLimit = 40;
                    }

                    if (upshift || TCgearCD > 0 || downshift) //Stop registering TC after gear shift
                    {
                        TCgearCD++;
                    }
                    if (TCgearCD > TCgearLimit)
                    {
                        TCgearCD = 0;
                        TCgear = gear;
                        TCthrottle = throttle;
                        TCrpm = rpm;
                    }


                    if (roadTextures.Contains(surface) && (Math.Abs(LRShockVel) > 0.13 || Math.Abs(RRShockVel) > 0.13))  //Filter on bumps
                    {
                        tcBumpCounter = 1;
                    }
                    if (tcBumpCounter > 0)
                    {
                        tcBump = true;
                        tcBumpCounter++;
                    }
                    if (tcBumpCounter > 20)
                    {
                        tcBumpCounter = 0;
                        tcBump = false;
                    }

                    if ((TCthrottle == 0 && throttle > 0) || TCreleaseCD > 0)  //Filter on heavy throttle application
                    {
                        TCreleaseCD++;
                        if (TCreleaseCD > 25)
                        {
                            TCreleaseCD = 0;
                        }
                    }


                    if (!tcBump && TCreleaseCD == 0 && gear == TCgear && TCdropCD == 0 && (TCthrottle < throttle || TCthrottle == 100 && throttle == 100) && (throttle > 30 || trackLocation == 0) && TCrpm * 0.995 > rpm && rpm < 0.98 * revLim && speed < 200 && rpm > idleRPM * 1.3)
                    {
                        TCon = true;
                        TCthrottle = throttle;
                        TCrpm = rpm;
                        TCduration = 0;
                    }
                    else if (TCdropCD == 0)
                    {
                        TCthrottle = throttle;
                        TCrpm = rpm;
                    }
                    if (TCon)
                    {
                        TCduration++;
                    }
                    if (TCduration > 20)
                    {
                        TCon = false;
                        TCduration = 0;
                    }

                    //Running wheel slip through the filter
                    if (!tcBump && TCreleaseCD == 0 && gear == TCgear && TCdropCD == 0 && (((TCthrottle < throttle || TCthrottle == 100 && throttle == 100) && (throttle > 30 || trackLocation == 0)) || (slipLF == 100 || slipRF == 100)))
                    {
                        pluginManager.SetPropertyValue("SlipLF", this.GetType(), slipLF);
                        pluginManager.SetPropertyValue("SlipRF", this.GetType(), slipRF);
                        pluginManager.SetPropertyValue("SlipLR", this.GetType(), slipLR);
                        pluginManager.SetPropertyValue("SlipRR", this.GetType(), slipRR);
                    }

                    if ((hasTCtog && TCswitch) || (hasTCtimer && TCPushTimer == 0)) //Push active TC, check again that calculations has been done because of TC, and not because of wheel slip calc
                    {
                        pluginManager.SetPropertyValue("TCActive", this.GetType(), TCon);
                    }

                }

                if (!hasTC || TCPushTimer > 0 || (TC == TCoffPosition && TCoffPosition != -1) || (hasTCtog && !TCswitch))
                {
                    pluginManager.SetPropertyValue("TCToggle", this.GetType(), false);
                }

                else
                {
                    pluginManager.SetPropertyValue("TCToggle", this.GetType(), true);
                }

                //-----------------------------------------
                //----------ABS TOGGLE---------------------
                //-----------------------------------------

                if (hasABStog || ABSoffPosition > -1)
                {
                    if ((!ABSswitch && hasABStog) || ABSoffPosition == ABS)
                    {
                        pluginManager.SetPropertyValue("ABSToggle", this.GetType(), false);
                    }
                    else
                    {
                        pluginManager.SetPropertyValue("ABSToggle", this.GetType(), true);
                    }
                }
                else
                {
                    pluginManager.SetPropertyValue("ABSToggle", this.GetType(), false);
                }

                //-------------------------------------
                //-------RPM TRACKER-------------------
                //-------------------------------------

                if (rpm > 300 && rpm > RPMtracker && !upshift && clutch == 0)
                {
                    RPMtracker = rpm;
                }

                if (RPMgear != gear && gear != "N")
                {
                    RPMlastGear = RPMtracker;
                    RPMgearShift = true;
                    pluginManager.SetPropertyValue("LastGearMaxRPM", this.GetType(), RPMlastGear);
                    pluginManager.SetPropertyValue("LastGear", this.GetType(), RPMgear);

                    switch (RPMgear)
                    {
                        case "1":
                            lastShiftPoint = shiftPoint1;
                            break;

                        case "2":
                            lastShiftPoint = shiftPoint2;
                            break;

                        case "3":
                            lastShiftPoint = shiftPoint3;
                            break;

                        case "4":
                            lastShiftPoint = shiftPoint4;
                            break;

                        case "5":
                            lastShiftPoint = shiftPoint5;
                            break;

                        case "6":
                            lastShiftPoint = shiftPoint6;
                            break;

                        case "7":
                            lastShiftPoint = shiftPoint7;
                            break;
                    }

                    RPMgear = gear;
                    RPMtracker = 0;
                    upshift = false;
                    downshift = false;
                }

                if (brake == 0)
                {
                    RPMgearShift = false;
                }

                if (RPMgearShift && brake > 0 || pit == 1) //slowing down
                {
                    RPMtracker = 0;
                }


                //-----------------------------------------
                //----------Lap calculations---------------
                //-----------------------------------------
                stintLapsCheck = false;

                if ((currentLapTime.TotalSeconds > 6 && trackPosition > 0.15 && trackPosition < twoThirds) || pit == 1)
                {
                    currentLapTimeStarted = true;
                }
                if (trackPosition > twoThirds)
                {
                    currentLapTimeStarted = false;
                }

                if (stintLength > 0) //Starting new stint
                {
                    hasPitted = true;
                }

                int lapStatus = 1; //Lap status calculation: 1 = Valid lap, 2 = Invalid lap, 3 = Out lap, 4 = Penalty, 5 = Pit lane

                if (outLap)
                {
                    lapStatus = 3;
                    stintLapsCheck = true;
                }
                if ((incidents > roadOff || furled) && !outLap)
                {
                    lapStatus = 2;
                }

                if (trackPosition > (1 - cutoff)) //Approaching start/finish line
                {
                    sector1StatusHolder = currentSector1Status;
                    sector1TimeHolder = currentSector1Time;
                    statusReadyToFetch = true;
                    jokerLapChecker = false;
                    lastLapChecker = lastLapTime;
                }

                if (trackPosition < cutoff) //Crossing start/finish line
                {
                    roadOff = incidents;
                    outLap = false;
                    lineCross = true;
                    if (jokerThisLap)
                    {
                        jokerLapChecker = true;
                        jokerThisLap = false;
                    }

                }
                if (counter == 11)
                {
                    if (currentLapTime.TotalSeconds > 6 && trackPosition > 0.1 && trackPosition < 0.3333) //Stuf that happens a bit into lap
                    {
                        if (lastLapChecker.TotalSeconds == lastLapTime.TotalSeconds)
                        {
                            pluginManager.SetPropertyValue("CurrentSector3Time", this.GetType(), new TimeSpan(0));
                            pluginManager.SetPropertyValue("CurrentSector3Delta", this.GetType(), 0);
                        }
                    }
                }

                if (trackPosition > 0.5) //Getting halfways
                {
                    lineCross = false;
                }

                if (pit == 1 || iRIdle) //If in pit or idle
                {
                    outLap = true;
                    roadOff = incidents;
                    lapStatus = 5;
                }
                if (black == 1) //Black flag
                {
                    lapStatus = 4;
                }
                if (jokerThisLap || jokerLapChecker)
                {
                    lapStatus = 6;
                }

                //Sector calculations
                if (trackPosition > twoThirds)
                {
                    if (!(sectorExempt) && !(currentSector == 1 && lapStatus != 5)) //Not update sector if jump to exempt area of track or in driving backwards.
                    {
                        currentSector = 3;
                        sector1to2 = false;
                    }
                }
                else if (trackPosition > oneThird && trackPosition < twoThirds)
                {
                    if (!(sectorExempt) && !(currentSector == 3 && lapStatus != 5))
                    {
                        currentSector = 2;
                        sector2to3 = false;
                    }
                }
                else
                {
                    if (!(sectorExempt) && !(currentSector == 2 && lapStatus != 5))
                    {
                        currentSector = 1;
                        sector1to2 = false;
                        sector2to3 = false;
                    }
                }

                if (currentSector == 3) //Updating lap time and status
                {
                    currentSector3Time = currentLapTime.TotalSeconds - currentSector2Time - currentSector1Time;
                    if (currentSector3Time < 0 || currentSector1Time == 0 || currentSector2Time == 0)
                    {
                        currentSector3Time = 0;
                    }

                    if (!sector2to3)
                    {
                        lastSectorStatusHolder = lapStatus;
                        currentSector3Status = 1;
                        sector2to3 = true;
                        sector3Incidents = incidents;
                    }
                    if (sector2to3 && sector3Incidents != incidents)
                    {
                        if (lapStatus == 3)
                        {
                            currentSector3Status = 2;
                            lastSectorStatusHolder = 2;
                        }
                        else
                        {
                            currentSector3Status = lapStatus;
                            lastSectorStatusHolder = lapStatus;
                        }

                        sector3Incidents = incidents;

                    }
                    if (sector2to3 && lastSectorStatusHolder != lapStatus && lapStatus != 3)
                    {
                        currentSector3Status = lapStatus;
                        lastSectorStatusHolder = lapStatus;
                    }

                    pluginManager.SetPropertyValue("CurrentSector3Time", this.GetType(), TimeSpan.FromSeconds(currentSector3Time));
                    pluginManager.SetPropertyValue("CurrentSector3Status", this.GetType(), currentSector3Status);
                    pluginManager.SetPropertyValue("CurrentSector3Delta", this.GetType(), 0);

                    if (currentSector2Time > 0 && sessionBestSector2 > 0)
                    {
                        double delta = currentSector2Time - sessionBestSector2;
                        pluginManager.SetPropertyValue("CurrentSector2Delta", this.GetType(), Math.Round(delta, 3));
                    }
                    else
                    {
                        pluginManager.SetPropertyValue("CurrentSector2Delta", this.GetType(), 0);
                    }
                }

                else if (currentSector == 2)
                {
                    currentSector2Time = currentLapTime.TotalSeconds - currentSector1Time;
                    if (currentSector2Time < 0 || currentSector1Time == 0)
                    {
                        currentSector2Time = 0;
                    }
                    if (!sector1to2)
                    {
                        lastSectorStatusHolder = lapStatus;
                        currentSector2Status = 1;
                        sector1to2 = true;
                        sector2Incidents = incidents;
                    }
                    if (sector1to2 && sector2Incidents != incidents)
                    {
                        if (lapStatus == 3)
                        {
                            currentSector2Status = 2;
                            lastSectorStatusHolder = 2;
                        }
                        else
                        {
                            currentSector2Status = lapStatus;
                            lastSectorStatusHolder = lapStatus;
                        }

                        sector2Incidents = incidents;

                    }
                    if (sector1to2 && lastSectorStatusHolder != lapStatus && lapStatus != 3)
                    {
                        currentSector2Status = lapStatus;
                        lastSectorStatusHolder = lapStatus;
                    }

                    pluginManager.SetPropertyValue("CurrentSector2Time", this.GetType(), TimeSpan.FromSeconds(currentSector2Time));
                    pluginManager.SetPropertyValue("CurrentSector2Status", this.GetType(), currentSector2Status);
                    pluginManager.SetPropertyValue("CurrentSector2Delta", this.GetType(), 0);

                    if (currentSector1Time > 0 && sessionBestSector1 > 0)
                    {
                        double delta = currentSector1Time - sessionBestSector1;
                        pluginManager.SetPropertyValue("CurrentSector1Delta", this.GetType(), Math.Round(delta, 3));
                    }
                    else
                    {
                        pluginManager.SetPropertyValue("CurrentSector1Delta", this.GetType(), 0);
                    }
                }
                else if (currentLapTimeStarted) //sector 1
                {
                    currentSector1Time = currentLapTime.TotalSeconds;
                    currentSector1Status = lapStatus;

                    pluginManager.SetPropertyValue("CurrentSector1Time", this.GetType(), TimeSpan.FromSeconds(currentSector1Time));
                    pluginManager.SetPropertyValue("CurrentSector1Status", this.GetType(), currentSector1Status);
                    pluginManager.SetPropertyValue("CurrentSector1Delta", this.GetType(), 0);
                }

                pluginManager.SetPropertyValue("CurrentSector", this.GetType(), currentSector);

                if (pitBox > 0 && !hasApproached) //If jumped to pit box, not taking a proper inlap
                {
                    if (trackPosition > 0.5)
                    {
                        currentSector = 3;
                    }
                    else
                    {
                        currentSector = 1;
                    }

                    pluginManager.SetPropertyValue("CurrentSector", this.GetType(), currentSector);

                    currentSector1Time = 0;
                    currentSector2Time = 0;
                    currentSector3Time = 0;
                    currentSector1Status = 0;
                    currentSector2Status = 0;
                    currentSector3Status = 0;

                    pluginManager.SetPropertyValue("CurrentSector3Time", this.GetType(), TimeSpan.FromSeconds(currentSector3Time));
                    pluginManager.SetPropertyValue("CurrentSector3Status", this.GetType(), currentSector3Status);
                    pluginManager.SetPropertyValue("CurrentSector3Delta", this.GetType(), 0);
                    pluginManager.SetPropertyValue("CurrentSector2Time", this.GetType(), TimeSpan.FromSeconds(currentSector2Time));
                    pluginManager.SetPropertyValue("CurrentSector2Status", this.GetType(), currentSector2Status);
                    pluginManager.SetPropertyValue("CurrentSector2Delta", this.GetType(), 0);
                    pluginManager.SetPropertyValue("CurrentSector1Time", this.GetType(), TimeSpan.FromSeconds(currentSector1Time));
                    pluginManager.SetPropertyValue("CurrentSector1Status", this.GetType(), currentSector1Status);
                    pluginManager.SetPropertyValue("CurrentSector1Delta", this.GetType(), 0);


                }


                //Sector calculations finished

                if (lineCross && statusReadyToFetch)     //Updating values at finish line crossing
                {
                    lastStatusHolder = lapStatus;
                    lastSectorStatusHolder = currentSector3Status;
                    statusReadyToFetch = false;
                    if (pit != 1)
                    {
                        outLap = false;
                    }

                }

                if (lastLapHolder != lastLapTime && (lastLapTime != new TimeSpan(0)))  //New lap time arrives, update certain lists and values
                {
                    ERSChangeCount = 4;
                    lapStatusList.Insert(0, lastStatusHolder);

                    if (lastStatusHolder == 1)
                    {
                        validStintLaps++;
                    }
                    if (lastStatusHolder == 2)
                    {
                        invalidStintLaps++;
                    }
                    if (lastStatusHolder == 6)
                    {
                        jokerLapCount++;
                        jokerLapChecker = false;
                    }

                    lapStatusList.RemoveAt(8); //Making sure list doesnt grow untill infility
                    if (lapStatusList[0] != 0)
                    {
                        lapTimeList.Insert(0, lastLapTime);

                        //Checking for session best lap
                        if ((lapTimeList[0].TotalSeconds < sessionBestLap.TotalSeconds || sessionBestLap.TotalSeconds == 0) && lapStatusList[0] == 1)
                        {
                            sessionBestLap = lapTimeList[0];
                            for (int i = 0; i < lapDeltaSections + 1; i++) //Keep hold of the timings on that lap
                            {
                                lapDeltaSessionBest[i] = lapDeltaLast[i];
                            }
                        }
                        
                        //Checking for lap record
                        if (lapRecord.TotalSeconds == 0 && lapStatusList[0] == 1)
                        {
                            LapRecords.addLapRecord(track, carModel, lapTimeList[0].TotalMilliseconds,lapDeltaLast, csvAdress, ref csvIndex);
                            for (int i = 0; i < lapDeltaSections + 1; i++) //Keep hold of the timings on that lap
                            {
                                lapDeltaRecord[i] = lapDeltaLast[i];
                            }
                            findLapRecord = true;
                        }
                        else if (lapTimeList[0].TotalSeconds < lapRecord.TotalSeconds && lapStatusList[0] == 1)
                        {
                            LapRecords.replaceLapRecord(track, carModel, lapTimeList[0].TotalMilliseconds, lapDeltaLast, csvAdress, csvIndex);
                            findLapRecord=true;
                        }
                        
                        lapTimeList.RemoveAt(8); //Making sure list doesnt grow untill infinity
                    }
                    lastLapHolder = lastLapTime;

                    //Sectors
                    sector1StatusList.Insert(0, sector1StatusHolder);
                    sector1StatusList.RemoveAt(8);
                    sector1TimeList.Insert(0, sector1TimeHolder);
                    if ((sector1TimeList[0] < sessionBestSector1 || sessionBestSector1 == 0) && sector1StatusList[0] == 1)
                    {
                        sessionBestSector1 = sector1TimeList[0];
                    }
                    sector1TimeList.RemoveAt(8);

                    sector2StatusList.Insert(0, currentSector2Status);
                    sector2StatusList.RemoveAt(8);
                    sector2TimeList.Insert(0, currentSector2Time);
                    if ((sector2TimeList[0] < sessionBestSector2 || sessionBestSector2 == 0) && sector2StatusList[0] == 1)
                    {
                        sessionBestSector2 = sector2TimeList[0];
                    }
                    sector2TimeList.RemoveAt(8);

                    sector3StatusList.Insert(0, lastSectorStatusHolder);
                    sector3StatusList.RemoveAt(8);
                    sector3TimeList.Insert(0, lapTimeList[0].TotalSeconds - sector1TimeHolder - currentSector2Time);
                    currentSector3Time = sector3TimeList[0];
                    pluginManager.SetPropertyValue("CurrentSector3Time", this.GetType(), TimeSpan.FromSeconds(currentSector3Time));
                    if (currentSector3Time > 0 && sessionBestSector3 > 0)
                    {
                        double delta = currentSector3Time - sessionBestSector3;
                        pluginManager.SetPropertyValue("CurrentSector3Delta", this.GetType(), Math.Round(delta, 3));
                    }
                    else
                    {
                        pluginManager.SetPropertyValue("CurrentSector3Delta", this.GetType(), 0);
                    }
                    if ((sector3TimeList[0] < sessionBestSector3 || sessionBestSector3 == 0) && sector3StatusList[0] == 1)
                    {
                        sessionBestSector3 = sector3TimeList[0];
                    }
                    sector3TimeList.RemoveAt(8);

                    currentSector1Time = 0;
                    currentSector2Time = 0;
                    currentSector3Time = 0;

                    if (lastStatusHolder != 3)
                    {
                        fuelTargetDeltas.Insert(0, fuelTargetDelta);
                        fuelTargetDeltas.RemoveAt(8);

                        fuelTargetDeltaCumulative = fuelTargetDeltaCumulative + fuelTargetDelta;
                    }
                }

                //----------------------------------------------------
                //------------Hotlap live position--------------------
                //----------------------------------------------------

                if (counter == 17)
                {
                    int position = 0;
                    for (int i = 0; i < opponents; i++)
                    {
                        if (estimatedLapTime.TotalSeconds > 0 && data.NewData.Opponents[i].BestLapTime.TotalSeconds > 0 && estimatedLapTime.TotalSeconds > data.NewData.Opponents[i].BestLapTime.TotalSeconds && data.NewData.Opponents[i].CarClass == myClass && !data.NewData.Opponents[i].IsPlayer)
                        {
                            position++;
                        }

                    }
                    if (opponents > 1 && !(session == "Race" && currentLap == 1))
                    {
                        position++;
                    }

                    if (estimatedLapTime.TotalSeconds == 0)
                    {
                        position = 0;
                    }

                    pluginManager.SetPropertyValue("HotlapLivePosition", this.GetType(), position);

                }

                //----------------------------------------------------
                //---------Pit box location calculations--------------
                //----------------------------------------------------

                bool boxApproach = false;

                pitBox = (pitLocation - trackPosition) * trackLength;
                if (pitLocation < 0.2 && trackPosition > 0.8)
                {
                    pitBox = (pitLocation + (1 - trackPosition)) * trackLength;
                }
                else if (pitLocation > 0.8 && trackPosition < 0.2)
                {
                    pitBox = -((1 - pitLocation) + trackPosition) * trackLength;
                }

                awayFromPits = -pitBox;

                if (pitBox > -8 && pitBox < 33 && pit == 1 && pitStall != 1 && hasPitted == true) //Car is approaching the pit box, and can pass by 8 meters. 
                {
                    boxApproach = true;
                    hasApproached = true;
                }

                if (Math.Abs(pitBox) < 2 && pit == 1)   //Car is in the pit box
                {
                    pitBox = 1 - ((pitBox + 2) / 4);
                    validStintLaps = 0;
                    invalidStintLaps = 0;
                    stintLapsCheck = true;
                    fuelTargetDeltaCumulative = 0;
                }
                else pitBox = 0;

                if (pitStall == 1) //Car has spawned or recieved pit stop
                {
                    hasPitted = false;
                    currentFrontWing = wingFront;
                    currentRearWing = wingRear;
                    currentPWS = PWS;
                    currentTape = tape;
                    offTrack = false;
                    offTrackTimer = globalClock;
                }
                if (pit == 0)
                {
                    hasApproached = false;
                }


                bool pitEntry = false;

                if (pitLimiter == 1 && pit == 0 && stintLength > 1000)
                {
                    pitEntry = true;
                }

                bool pitSpeeding = false;

                if (pit == 1 && (Math.Round(speed, 0) - 2.5) > pitSpeedLimit)
                {
                    pitSpeeding = true;
                }

                pluginManager.SetPropertyValue("PitEntry", this.GetType(), pitEntry);
                pluginManager.SetPropertyValue("PitSpeeding", this.GetType(), pitSpeeding);

                //---------------------------------------------------------------
                //-------------Pace calculation, once pr. second-----------------
                //---------------------------------------------------------------


                if (counter == 30) //Race pace pr lap
                {
                    List<double> lapListSeconds = new List<double> { };
                    double fastLap = 0;
                    for (int i = 0; i < lapTimeList.Count; i++)
                    {
                        lapListSeconds.Add(lapTimeList[i].TotalSeconds);
                        if (fastLap == 0 || lapListSeconds[i] != 0 && lapListSeconds[i] < fastLap)
                        {
                            fastLap = lapListSeconds[i];
                        }
                    }

                    List<double> fastList = new List<double> { };
                    List<double> slowList = new List<double> { };
                    double thresholdLap = fastLap * 1.015;
                    double runOffLap = fastLap * 1.05;
                    for (int i = 0; i < lapTimeList.Count; i++)
                    {
                        if ((lapStatusList[i] < 3 && lapStatusList[i] != 0) && !(lapListSeconds[i] > (fastLap + 8) && lapListSeconds[i] > runOffLap)) //Excluding inlaps/outlaps/jokerlaps and laps with accidents (8 sec time loss if that corresponds to 5% or more of normal lap time)
                        {
                            if (lapListSeconds[i] < thresholdLap)
                            {
                                fastList.Add(lapListSeconds[i]);
                            }
                            else
                            {
                                slowList.Add(lapListSeconds[i]);
                            }
                        }
                    }

                    pace = fastList.Count > 0 ? fastList.Average() : 0.0;

                    if (lapListSeconds.Count > 1)
                    {
                        if (lapListSeconds[0] > thresholdLap && lapListSeconds[1] > thresholdLap && lapStatusList[0] < 3 && lapStatusList[1] < 3 && slowList.Count > 1) //Pace is slowing down for some reason, fast acting
                        {
                            pace = (slowList[0] + slowList[1]) / 2;
                        }

                        if (lapListSeconds[0] < fastLap * 1.005 && lapListSeconds[1] < fastLap * 1.005 && lapStatusList[0] == 1 && lapStatusList[1] == 1) //Pace is increasing, two fast valid Laps fast acting
                        {
                            pace = (fastList[0] + fastList[1]) / 2;
                        }
                    }
                    TimeSpan paceTime = TimeSpan.FromSeconds(pace);

                    pluginManager.SetPropertyValue("Pace", this.GetType(), paceTime);

                    if (sessionBestLap.TotalSeconds > 0)
                    {
                        for (int i = 0; i < lapListSeconds.Count; i++)
                        {
                            double delta = Math.Round(lapListSeconds[i] - sessionBestLap.TotalSeconds, 3);
                            if (lapListSeconds[i] > 0)
                            {
                                pluginManager.SetPropertyValue("Lap0" + (i + 1) + "Delta", this.GetType(), delta);
                            }
                        }
                    }


                }

                if (counter == 33) //Sector 1 pace
                {
                    double fastLap = 0;
                    for (int i = 0; i < sector1TimeList.Count; i++)
                    {
                        if (fastLap == 0 || sector1TimeList[i] != 0 && sector1TimeList[i] < fastLap)
                        {
                            fastLap = sector1TimeList[i];
                        }
                    }

                    List<double> fastList = new List<double> { };
                    List<double> slowList = new List<double> { };
                    double thresholdLap = fastLap * 1.015;
                    double runOffLap = fastLap * 1.05;
                    double sectorAverage = 0;
                    int sectorAverageCounter = 0;
                    for (int i = 0; i < sector1TimeList.Count; i++)
                    {
                        if (sector1StatusList[i] < 3 && sector1StatusList[i] != 0)
                        {
                            sectorAverage = sectorAverage + sector1TimeList[i];
                            sectorAverageCounter++;
                        }
                        if ((sector1StatusList[i] < 3 && sector1StatusList[i] != 0) && !(sector1TimeList[i] > (fastLap + 8) && sector1TimeList[i] > runOffLap)) //Excluding inlaps/outlaps/jokerlaps and laps with accidents (8 sec time loss if that corresponds to 5% or more of normal lap time)
                        {
                            if (sector1TimeList[i] < thresholdLap)
                            {
                                fastList.Add(sector1TimeList[i]);
                            }
                            else
                            {
                                slowList.Add(sector1TimeList[i]);
                            }
                        }
                    }

                    sector1Pace = fastList.Count > 0 ? fastList.Average() : 0.0;

                    sectorAverage = sectorAverage / sectorAverageCounter;

                    double sum = 0;
                    int invalids = 0;
                    int valids = 0;
                    double sectorVariance = 0;
                    double sectorScore = 0;
                    for (int i = 0; i < sector1TimeList.Count; i++)
                    {
                        if (sector1StatusList[i] < 3 && sector1StatusList[i] != 0)
                        {
                            if (sector1StatusList[i] == 1)
                            {
                                valids++;
                            }
                            if (sector1StatusList[i] == 2)
                            {
                                invalids++;
                            }
                            sum = sum + ((sector1TimeList[i] - sectorAverage) * (sector1TimeList[i] - sectorAverage));
                        }
                    }

                    if (sectorAverageCounter > 2)
                    {
                        sectorVariance = Math.Sqrt(sum / sectorAverageCounter);
                        sectorScore = 10 / ((1 + sectorVariance) * (1 + (invalids / (valids + invalids))));
                        sectorScore = Math.Round(sectorScore - ((8 - valids - invalids) * 0.4), 1);
                        if (sectorScore < 0)
                        {
                            sectorScore = 0.1;
                        }
                    }

                    if (sector1TimeList.Count > 1)
                    {
                        if (sector1TimeList[0] > thresholdLap && sector1TimeList[1] > thresholdLap && sector1StatusList[0] < 3 && sector1StatusList[1] < 3 && slowList.Count > 1) //Pace is slowing down for some reason, fast acting
                        {
                            sector1Pace = (slowList[0] + slowList[1]) / 2;
                        }

                        if (sector1TimeList[0] < fastLap * 1.005 && sector1TimeList[1] < fastLap * 1.005 && sector1StatusList[0] == 1 && sector1StatusList[1] == 1) //Pace is increasing, two fast valid Laps fast acting
                        {
                            sector1Pace = (fastList[0] + fastList[1]) / 2;
                        }
                    }

                    pluginManager.SetPropertyValue("Sector1Pace", this.GetType(), TimeSpan.FromSeconds(sector1Pace));
                    pluginManager.SetPropertyValue("Sector1Score", this.GetType(), sectorScore);

                }

                if (counter == 43) //Sector 2 pace
                {
                    double fastLap = 0;
                    for (int i = 0; i < sector2TimeList.Count; i++)
                    {
                        if (fastLap == 0 || sector2TimeList[i] != 0 && sector2TimeList[i] < fastLap)
                        {
                            fastLap = sector2TimeList[i];
                        }
                    }

                    List<double> fastList = new List<double> { };
                    List<double> slowList = new List<double> { };
                    double thresholdLap = fastLap * 1.015;
                    double runOffLap = fastLap * 1.05;
                    double sectorAverage = 0;
                    int sectorAverageCounter = 0;
                    for (int i = 0; i < sector2TimeList.Count; i++)
                    {
                        if (sector2StatusList[i] < 3 && sector2StatusList[i] != 0)
                        {
                            sectorAverage = sectorAverage + sector2TimeList[i];
                            sectorAverageCounter++;
                        }
                        if ((sector2StatusList[i] < 3 && sector2StatusList[i] != 0) && !(sector2TimeList[i] > (fastLap + 8) && sector2TimeList[i] > runOffLap)) //Excluding inlaps/outlaps/jokerlaps and laps with accidents (8 sec time loss if that corresponds to 5% or more of normal lap time)
                        {
                            if (sector2TimeList[i] < thresholdLap)
                            {
                                fastList.Add(sector2TimeList[i]);
                            }
                            else
                            {
                                slowList.Add(sector2TimeList[i]);
                            }
                        }
                    }

                    sector2Pace = fastList.Count > 0 ? fastList.Average() : 0.0;

                    sectorAverage = sectorAverage / sectorAverageCounter;

                    double sum = 0;
                    int invalids = 0;
                    int valids = 0;
                    double sectorVariance = 0;
                    double sectorScore = 0;

                    for (int i = 0; i < sector2TimeList.Count; i++)
                    {
                        if (sector2StatusList[i] < 3 && sector2StatusList[i] != 0)
                        {
                            if (sector2StatusList[i] == 1)
                            {
                                valids++;
                            }
                            if (sector2StatusList[i] == 2)
                            {
                                invalids++;
                            }
                            sum = sum + ((sector2TimeList[i] - sectorAverage) * (sector2TimeList[i] - sectorAverage));
                        }
                    }

                    if (sectorAverageCounter > 2)
                    {
                        sectorVariance = Math.Sqrt(sum / sectorAverageCounter);
                        sectorScore = 10 / ((1 + sectorVariance) * (1 + (invalids / (valids + invalids))));
                        sectorScore = Math.Round(sectorScore - ((8 - valids - invalids) * 0.4), 1);
                        if (sectorScore < 0)
                        {
                            sectorScore = 0.1;
                        }
                    }

                    if (sector2TimeList.Count > 1)
                    {
                        if (sector2TimeList[0] > thresholdLap && sector2TimeList[1] > thresholdLap && sector2StatusList[0] < 3 && sector2StatusList[1] < 3 && slowList.Count > 1) //Pace is slowing down for some reason, fast acting
                        {
                            sector2Pace = (slowList[0] + slowList[1]) / 2;
                        }

                        if (sector2TimeList[0] < fastLap * 1.005 && sector2TimeList[1] < fastLap * 1.005 && sector2StatusList[0] == 1 && sector2StatusList[1] == 1) //Pace is increasing, two fast valid Laps fast acting
                        {
                            sector2Pace = (fastList[0] + fastList[1]) / 2;
                        }
                    }

                    pluginManager.SetPropertyValue("Sector2Pace", this.GetType(), TimeSpan.FromSeconds(sector2Pace));
                    pluginManager.SetPropertyValue("Sector2Score", this.GetType(), sectorScore);

                }

                if (counter == 53) //Sector 3 pace
                {
                    double fastLap = 0;
                    for (int i = 0; i < sector3TimeList.Count; i++)
                    {
                        if (fastLap == 0 || sector3TimeList[i] != 0 && sector3TimeList[i] < fastLap)
                        {
                            fastLap = sector3TimeList[i];
                        }
                    }

                    List<double> fastList = new List<double> { };
                    List<double> slowList = new List<double> { };
                    double thresholdLap = fastLap * 1.015;
                    double runOffLap = fastLap * 1.05;
                    double sectorAverage = 0;
                    int sectorAverageCounter = 0;
                    for (int i = 0; i < sector3TimeList.Count; i++)
                    {
                        if (sector3StatusList[i] < 3 && sector3StatusList[i] != 0)
                        {
                            sectorAverage = sectorAverage + sector3TimeList[i];
                            sectorAverageCounter++;
                        }
                        if ((sector3StatusList[i] < 3 && sector3StatusList[i] != 0) && !(sector3TimeList[i] > (fastLap + 8) && sector3TimeList[i] > runOffLap)) //Excluding inlaps/outlaps/jokerlaps and laps with accidents (8 sec time loss if that corresponds to 5% or more of normal lap time)
                        {
                            if (sector3TimeList[i] < thresholdLap)
                            {
                                fastList.Add(sector3TimeList[i]);
                            }
                            else
                            {
                                slowList.Add(sector3TimeList[i]);
                            }
                        }
                    }

                    sector3Pace = fastList.Count > 0 ? fastList.Average() : 0.0;

                    sectorAverage = sectorAverage / sectorAverageCounter;

                    double sum = 0;
                    int invalids = 0;
                    int valids = 0;
                    double sectorVariance = 0;
                    double sectorScore = 0;

                    for (int i = 0; i < sector3TimeList.Count; i++)
                    {
                        if (sector3StatusList[i] < 3 && sector3StatusList[i] != 0)
                        {
                            if (sector3StatusList[i] == 1)
                            {
                                valids++;
                            }
                            if (sector3StatusList[i] == 2)
                            {
                                invalids++;
                            }
                            sum = sum + ((sector3TimeList[i] - sectorAverage) * (sector3TimeList[i] - sectorAverage));
                        }
                    }

                    if (sectorAverageCounter > 2)
                    {
                        sectorVariance = Math.Sqrt(sum / sectorAverageCounter);
                        sectorScore = 10 / ((1 + sectorVariance) * (1 + (invalids / (valids + invalids))));
                        sectorScore = Math.Round(sectorScore - ((8 - valids - invalids) * 0.4), 1);
                        if (sectorScore < 0)
                        {
                            sectorScore = 0.1;
                        }
                    }

                    if (sector3TimeList.Count > 1)
                    {
                        if (sector3TimeList[0] > thresholdLap && sector3TimeList[1] > thresholdLap && sector3StatusList[0] < 3 && sector3StatusList[1] < 3 && slowList.Count > 1) //Pace is slowing down for some reason, fast acting
                        {
                            sector3Pace = (slowList[0] + slowList[1]) / 2;
                        }

                        if (sector3TimeList[0] < fastLap * 1.005 && sector3TimeList[1] < fastLap * 1.005 && sector3StatusList[0] == 1 && sector3StatusList[1] == 1) //Pace is increasing, two fast valid Laps fast acting
                        {
                            sector3Pace = (fastList[0] + fastList[1]) / 2;
                        }
                    }

                    pluginManager.SetPropertyValue("Sector3Pace", this.GetType(), TimeSpan.FromSeconds(sector3Pace));
                    pluginManager.SetPropertyValue("Sector3Score", this.GetType(), sectorScore);
                }

                //----------------------------------------------------------------
                //------------Updating delta values, once pr. second--------------
                //----------------------------------------------------------------

                if (sessionBestSector1 > 0)
                {
                    for (int i = 0; i < sector1TimeList.Count; i++)
                    {
                        double delta = Math.Round(sector1TimeList[i] - sessionBestSector1, 3);
                        if (sector1TimeList[i] > 0)
                        {
                            pluginManager.SetPropertyValue("Lap0" + (i + 1) + "Sector1Delta", this.GetType(), Math.Round(delta, 3));
                        }
                    }
                }

                if (sessionBestSector2 > 0)
                {
                    for (int i = 0; i < sector2TimeList.Count; i++)
                    {
                        double delta = Math.Round(sector2TimeList[i] - sessionBestSector2, 3);
                        if (sector2TimeList[i] > 0)
                        {
                            pluginManager.SetPropertyValue("Lap0" + (i + 1) + "Sector2Delta", this.GetType(), Math.Round(delta, 3));
                        }
                    }
                }

                if (sessionBestSector3 > 0)
                {
                    for (int i = 0; i < sector3TimeList.Count; i++)
                    {
                        double delta = Math.Round(sector3TimeList[i] - sessionBestSector3, 3);
                        if (sector3TimeList[i] > 0)
                        {
                            pluginManager.SetPropertyValue("Lap0" + (i + 1) + "Sector3Delta", this.GetType(), Math.Round(delta, 3));
                        }
                    }
                }


                //---------------------------------------------------------------
                //------Real position calculations, twice pr. second-------------
                //---------------------------------------------------------------

                if (counter == 15 || counter == 45)
                {
                    isRaceLeader = false;
                    realPosition = 1;

                    if (session == "Lone Qualify" || session == "Open Qualify")
                    {
                        qualyPosition = myPosition;
                        realPosition = myPosition;
                        hotLapPosition = myPosition;
                    }

                    else if (session == "Race" && opponents > 1)
                    {
                        isRaceLeader = true;

                        for (int i = 0; i < opponents; i++)
                        {
                            if (data.NewData.Opponents[i].GaptoPlayer < 0)
                            {
                                isRaceLeader = false;
                                if (data.NewData.Opponents[i].CarClass == myClass)
                                {
                                    realPosition++;
                                }
                            }
                            hotLapPosition = 1;
                            if (data.NewData.Opponents[i].BestLapTime.TotalSeconds < sessionBestLap.TotalSeconds && data.NewData.Opponents[i].BestLapTime.TotalSeconds > 0)
                            {
                                hotLapPosition++;
                            }
                            if (sessionBestLap.TotalSeconds == 0)
                            {
                                hotLapPosition = opponents;
                            }
                        }

                        if (data.NewData.Opponents[0].GaptoPlayer == null && data.NewData.Opponents[1].GaptoPlayer == null)
                        {
                            if (aheadClass == myClass && aheadGap != 0)
                            {
                                realPosition = aheadClassPosition + 1;
                            }
                            if (aheadClass != myClass || aheadGap == 0)
                            {
                                realPosition = myPosition;
                            }
                        }
                        if (currentLapTime.TotalSeconds == 0 && qualyPosition > 0)
                        {
                            realPosition = qualyPosition;
                        }
                        if (currentLapTime.TotalSeconds == 0 && qualyPosition == 0)
                        {
                            realPosition = myPosition;
                        }
                        if (currentLapTime.TotalSeconds > 0)
                        {
                            qualyPosition = 0;
                        }

                        if (checkered == 1 && ((trackPosition > 0.1 && trackPosition < 0.15) || (currentLapTime.TotalSeconds > 5 && currentLapTime.TotalSeconds < 10)))
                        {
                            raceFinished = true;
                        }
                        if ((lapRaceFinished || timeRaceFinished)) //Identify all cars with one lap more finished - keep in list, cannot decrement if players DC. 
                        {
                            int position = 1;

                            for (int i = 0; i < opponents; i++)
                            {
                                if (finishedCars.IndexOf(data.NewData.Opponents[i].CarNumber) < 0 && data.NewData.Opponents[i].CurrentLap > currentLap && myClass == data.NewData.Opponents[i].CarClass)
                                {
                                    finishedCars.Add(data.NewData.Opponents[i].CarNumber);
                                }

                                if (data.NewData.Opponents[i].CurrentLap == currentLap && data.NewData.Opponents[i].GaptoPlayer < 0 && myClass == data.NewData.Opponents[i].CarClass)
                                {
                                    position++;
                                }
                            }

                            realPosition = position + finishedCars.Count;
                        }
                        if ((lapRaceFinished || timeRaceFinished) && trackPosition < 0.1 && checkered == 1)
                        {
                            realPosition = 1 + finishedCars.Count;
                        }

                        if (raceFinished)
                        {
                            realPosition = myPosition;
                        }

                    }
                    else
                    {
                        realPosition = myPosition;
                        hotLapPosition = myPosition;

                    }

                    pluginManager.SetPropertyValue("Position", this.GetType(), realPosition);
                    pluginManager.SetPropertyValue("HotLapPosition", this.GetType(), hotLapPosition);
                    pluginManager.SetPropertyValue("RaceFinished", this.GetType(), raceFinished);

                }

                //-------------------------------------------------
                //----Opponents calculations and remaining laps----
                //-------------------------------------------------

                if (counter == 5 || counter == 20 || counter == 35 || counter == 50)
                {
                    //Declaring and resetting

                    double? leaderGap = 0;
                    string leaderName = "";
                    int? leaderCurrentLap = 0;
                    TimeSpan leaderLastLap = new TimeSpan(0);
                    TimeSpan leaderBestLap = new TimeSpan(0);
                    double? leaderTrackPosition = 0;

                    double? classLeaderGap = 0;
                    classLeaderName = "";
                    TimeSpan classLeaderLastLap = new TimeSpan(0);
                    TimeSpan classLeaderBestLap = new TimeSpan(0);

                    double? aheadGap = 0;
                    string aheadName = "";
                    aheadGlobal = aheadName;
                    TimeSpan aheadLastLap = new TimeSpan(0);
                    TimeSpan aheadBestLap = new TimeSpan(0);
                    bool aheadIsConnected = false;
                    bool aheadIsInPit = false;
                    bool aheadSlowLap = false;
                    int aheadOvertakePrediction = 0;
                    int aheadLapsSincePit = -1;
                    int aheadP2PCount = -1;
                    bool aheadP2PActive = false;
                    double? aheadRealGap = 0;

                    double? behindGap = 0;
                    string behindName = "";
                    behindGlobal = behindName;
                    TimeSpan behindLastLap = new TimeSpan(0);
                    TimeSpan behindBestLap = new TimeSpan(0);
                    bool behindIsConnected = false;
                    bool behindIsInPit = false;
                    bool behindSlowLap = false;
                    int behindOvertakePrediction = 0;
                    int behindLapsSincePit = -1;
                    int behindP2PCount = -1;
                    bool behindP2PActive = false;
                    double? behindRealGap = 0;

                    double? luckyDogRealGap = 0;
                    double? luckyDogGap = 0;
                    string luckyDogName = "";
                    int luckyDogPositionsAhead = 0;

                    remainingLaps = 0;

                    int gridSubtract = 0;

                    double totalSessionTime = irData.SessionData.SessionInfo.Sessions[sessionNumber]._SessionTime;                          //Total session time of the session
                    long completedRaceLaps = irData.SessionData.SessionInfo.Sessions[sessionNumber].ResultsLapsComplete;                    //To use for identifying lap race finish

                    double timeLeftSeconds = timeLeft.TotalSeconds;

                    if(Settings.CorrectByPitstop && !onlyThrough)
                    {
                        timeLeftSeconds = timeLeftSeconds - pitStopDuration;
                    }

                    pluginManager.SetPropertyValue("QualyLap1Status", this.GetType(), 0);
                    pluginManager.SetPropertyValue("QualyLap2Status", this.GetType(), 0);
                    pluginManager.SetPropertyValue("QualyLap1Time", this.GetType(), new TimeSpan(0));
                    pluginManager.SetPropertyValue("QualyLap2Time", this.GetType(), new TimeSpan(0));
                    warmup = false;


                    for (int i = 0; i < opponents; i++)
                    {
                        if (data.NewData.Opponents[i].GaptoPlayer < classLeaderGap && data.NewData.Opponents[i].CarClass == myClass)
                        {
                            classLeaderGap = data.NewData.Opponents[i].GaptoPlayer;
                            classLeaderName = data.NewData.Opponents[i].Name;
                            classLeaderLastLap = data.NewData.Opponents[i].LastLapTime;
                            classLeaderBestLap = data.NewData.Opponents[i].BestLapTime;
                        }
                        if (data.NewData.Opponents[i].GaptoPlayer < leaderGap)
                        {
                            leaderGap = data.NewData.Opponents[i].GaptoPlayer;
                            leaderName = data.NewData.Opponents[i].Name;
                            leaderCurrentLap = data.NewData.Opponents[i].CurrentLap;
                            leaderLastLap = data.NewData.Opponents[i].LastLapTime;
                            leaderBestLap = data.NewData.Opponents[i].BestLapTime;
                            leaderTrackPosition = data.NewData.Opponents[i].TrackPositionPercent;
                        }
                        if (data.NewData.Opponents[i].GaptoPlayer < 0 && (aheadGap == 0 || data.NewData.Opponents[i].GaptoPlayer > aheadGap))
                        {
                            aheadGap = data.NewData.Opponents[i].GaptoPlayer;
                            aheadName = data.NewData.Opponents[i].Name;
                            aheadLastLap = data.NewData.Opponents[i].LastLapTime;
                            aheadBestLap = data.NewData.Opponents[i].BestLapTime;
                            aheadIsConnected = data.NewData.Opponents[i].IsConnected;
                            aheadIsInPit = data.NewData.Opponents[i].IsCarInPit;
                        }
                        else if (data.NewData.Opponents[i].GaptoPlayer > 0 && (behindGap == 0 || data.NewData.Opponents[i].GaptoPlayer < behindGap))
                        {
                            behindGap = data.NewData.Opponents[i].GaptoPlayer;
                            behindName = data.NewData.Opponents[i].Name;
                            behindLastLap = data.NewData.Opponents[i].LastLapTime;
                            behindBestLap = data.NewData.Opponents[i].BestLapTime;
                            behindIsConnected = data.NewData.Opponents[i].IsConnected;
                            behindIsInPit = data.NewData.Opponents[i].IsCarInPit;
                        }
                        if ((leaderCurrentLap + leaderTrackPosition) - (data.NewData.Opponents[i].TrackPositionPercent + data.NewData.Opponents[i].CurrentLap) > 1 && data.NewData.Opponents[i].CarClass == myClass && (luckyDogGap == 0 || data.NewData.Opponents[i].GaptoLeader < luckyDogGap))
                        {
                            luckyDogGap = data.NewData.Opponents[i].GaptoPlayer;
                            luckyDogName = data.NewData.Opponents[i].Name;
                            if (data.NewData.Opponents[i].GaptoPlayer < 0)
                            {
                                luckyDogPositionsAhead++;
                            }
                        }
                        else if ((leaderCurrentLap + leaderTrackPosition) - (data.NewData.Opponents[i].TrackPositionPercent + data.NewData.Opponents[i].CurrentLap) > 1 && data.NewData.Opponents[i].CarClass == myClass && data.NewData.Opponents[i].GaptoPlayer < 0)
                        {
                            luckyDogPositionsAhead++;
                        }
                        if ((leaderCurrentLap + leaderTrackPosition)-(currentLap +trackPosition) > 1 && luckyDogGap > 0)
                        {
                            luckyDogGap = 0;
                            luckyDogName = data.NewData.PlayerName;
                            luckyDogPositionsAhead = 0;
                        }

                    }

                    bool inaccurateCalculations = false;

                    myExpectedLapTime = pace;

                    if (lapRecord.TotalSeconds == 0 || (pace > 0 && pace > lapRecord.TotalSeconds * 1.05))
                    {
                        inaccurateCalculations = true;
                    }

                    if (myExpectedLapTime == 0)
                    {
                        myExpectedLapTime = lapRecord.TotalSeconds * 1.05;
                        inaccurateCalculations = true;
                    }
                    if (myExpectedLapTime == 0)
                    {
                        myExpectedLapTime = trackLength / 40;
                        inaccurateCalculations = true;
                    }

                    lapLapsRemaining = totalLaps - currentLap;
                    timeLapsRemaining = timeLeftSeconds / myExpectedLapTime + trackPosition - 1;

                    pluginManager.SetPropertyValue("ApproximateCalculations", this.GetType(), inaccurateCalculations);

                    pluginManager.SetPropertyValue("P1Gap", this.GetType(), leaderGap);
                    pluginManager.SetPropertyValue("P1Name", this.GetType(), leaderName);
                    pluginManager.SetPropertyValue("ClassP1Gap", this.GetType(), classLeaderGap);
                    pluginManager.SetPropertyValue("ClassP1Name", this.GetType(), classLeaderName);
                    if (trackType > 4)
                    {
                        pluginManager.SetPropertyValue("LuckyDogGap", this.GetType(), luckyDogGap);
                        pluginManager.SetPropertyValue("LuckyDogName", this.GetType(), luckyDogName);
                        pluginManager.SetPropertyValue("LuckyDogPositionsAhead", this.GetType(), luckyDogPositionsAhead);
                    }
 
                    //Leader lap times
                    double leaderExpectedLapTime = (leaderLastLap.TotalSeconds * 2 + leaderBestLap.TotalSeconds) / 3;
                    if (leaderLastLap.TotalSeconds == 0)
                    {
                        leaderExpectedLapTime = leaderBestLap.TotalSeconds * 1.01;
                    }
                    if (leaderBestLap.TotalSeconds == 0)
                    {
                        leaderExpectedLapTime = leaderLastLap.TotalSeconds;
                    }

                    double classLeaderExpectedLapTime = (classLeaderLastLap.TotalSeconds * 2 + classLeaderBestLap.TotalSeconds) / 3;
                    if (classLeaderLastLap.TotalSeconds == 0)
                    {
                        classLeaderExpectedLapTime = classLeaderBestLap.TotalSeconds * 1.01;
                    }
                    if (classLeaderBestLap.TotalSeconds == 0)
                    {
                        classLeaderExpectedLapTime = classLeaderLastLap.TotalSeconds;
                    }
                    TimeSpan classLeaderPace = TimeSpan.FromSeconds(classLeaderExpectedLapTime);
                    TimeSpan leaderPace = TimeSpan.FromSeconds(leaderExpectedLapTime);

                    pluginManager.SetPropertyValue("P1Pace", this.GetType(), leaderPace);
                    pluginManager.SetPropertyValue("ClassP1Pace", this.GetType(), classLeaderPace);

                    if (session == "Race") //Race sessions exemptions
                    {
                        leaderDecimal = 0;

                        if (sessionState < 4)
                        {
                            offTrack = false;
                            offTrackTimer = globalClock;
                            timeLeftSeconds = totalSessionTime;
                            if (trackPosition > 0.5 || trackPosition == 0)
                            {
                                gridSubtract = 1;
                            }
                        }

                        if (timeLeft.TotalSeconds == 0 && completedLaps > 0)
                        {
                            timedOut = true;
                        }
                        if (timedOut || timeLeftSeconds < 0)
                        {
                            timeLeftSeconds = 0;
                        }

                        //Leader finishing race in lap based race
                        if (completedRaceLaps == totalLaps)
                        {
                            lapRaceFinished = true;
                        }
                        //Leader finishing race in a time based race
                        if (!timedOut)
                        {
                            timeBasedChecker = true;
                            timeLapCounter = leaderCurrentLap;
                        }
                        if (leaderCurrentLap > timeLapCounter && timeBasedChecker)
                        {
                            timeBasedChecker = false;
                            timeRaceFinished = true;
                        }

                        if (leaderExpectedLapTime == 0)
                        {
                            timeLapsRemaining = (timeLeftSeconds / myExpectedLapTime) + trackPosition - 1; //No grid subtract
                            lapLapsRemaining = lapLapsRemaining + gridSubtract;
                        }

                        //Continuing calculations if we have leader pace and my pace --- and I'm not the leader. 
                        if (leaderExpectedLapTime > 0 && !isRaceLeader)
                        {
                            //Lap limited session calculations

                            double? leaderRaceTime = leaderExpectedLapTime * (totalLaps - leaderCurrentLap + 1 - leaderTrackPosition);
                            double? lapsWhileLeaderRace = leaderRaceTime / myExpectedLapTime;
                            lapLapsRemaining = lapsWhileLeaderRace + trackPosition;

                            //Time limited session calculations
                            double? leaderTimeOut = (timeLeftSeconds / leaderExpectedLapTime) + leaderTrackPosition;
                            leaderDecimal = leaderTimeOut - ((int)(leaderTimeOut * 100)) / 100;
                            double? timeUntillLeaderCheckered = leaderExpectedLapTime * (leaderTimeOut + (1 - leaderDecimal) - leaderTrackPosition);
                            timeLapsRemaining = (timeUntillLeaderCheckered / myExpectedLapTime) + trackPosition;
                            if (isTimeLimited)
                            {
                                pluginManager.SetPropertyValue("P1LapBalance", this.GetType(), leaderDecimal);
                            }
                        }
                        else
                        {
                            pluginManager.SetPropertyValue("P1LapBalance", this.GetType(), 0);
                        }

                        remainingLaps = lapLapsRemaining;
                        isLapLimited = irData.SessionData.SessionInfo.Sessions[sessionNumber].IsLimitedSessionLaps;
                        isTimeLimited = irData.SessionData.SessionInfo.Sessions[sessionNumber].IsLimitedTime;

                        if (isLapLimited && isTimeLimited) //Session is both lap and time limited
                        {
                            if (timeLapsRemaining < lapLapsRemaining + 1)
                            {
                                remainingLaps = timeLapsRemaining;
                            }
                        }
                        else if (isTimeLimited) //Session is strictly time limited
                        {
                            remainingLaps = timeLapsRemaining;
                        }
                        else //Session is strictly lap limited
                        {
                            leaderDecimal = 0;
                            pluginManager.SetPropertyValue("P1LapBalance", this.GetType(), 0);
                        }
                        if (lapRaceFinished || timeRaceFinished)
                        {
                            remainingLaps = 0;
                            pluginManager.SetPropertyValue("P1Finished", this.GetType(), true);
                            pluginManager.SetPropertyValue("P1LapBalance", this.GetType(), 0);
                        }
                        else
                        {
                            pluginManager.SetPropertyValue("P1Finished", this.GetType(), false);
                        }
                    }

                    else if (session == "Lone Qualify") //Qlap status: 1 - Waiting, 2 - Valid lap, not completed. 3 - Ruined lap, completed or not. 4 - Finished valid lap
                    {

                        if (((timeLeftSeconds / myExpectedLapTime) + trackPosition - 1) > lapLapsRemaining + 1)
                        {
                            remainingLaps = lapLapsRemaining + 0.99;
                        }
                        else
                        {
                            remainingLaps = (timeLeftSeconds / myExpectedLapTime) + trackPosition - 1;
                        }

                    }

                    else if (session == "Offline Testing")
                    {
                        remainingLaps = 0;
                    }
                    else
                    {
                        remainingLaps = timeLapsRemaining;
                        if (isLapLimited)
                        {
                            remainingLaps = lapLapsRemaining;
                        }
                    }

                    int truncRemainingLaps = ((int)(remainingLaps * 100)) / 100;
                    double? lapBalance = remainingLaps - truncRemainingLaps;


                    pluginManager.SetPropertyValue("LapsRemaining", this.GetType(), truncRemainingLaps);
                    pluginManager.SetPropertyValue("LapBalance", this.GetType(), lapBalance);

                    pluginManager.SetPropertyValue("AheadPace", this.GetType(), new TimeSpan(0));
                    pluginManager.SetPropertyValue("AheadSlowLap", this.GetType(), false);
                    pluginManager.SetPropertyValue("AheadPrognosis", this.GetType(), 0);
                    pluginManager.SetPropertyValue("AheadLapsToOvertake", this.GetType(), -1);
                    pluginManager.SetPropertyValue("AheadLapsSincePit", this.GetType(), -1);
                    pluginManager.SetPropertyValue("AheadP2PStatus", this.GetType(), false);
                    pluginManager.SetPropertyValue("AheadP2PCount", this.GetType(), -1);

                    pluginManager.SetPropertyValue("BehindPace", this.GetType(), new TimeSpan(0));
                    pluginManager.SetPropertyValue("BehindSlowLap", this.GetType(), false);
                    pluginManager.SetPropertyValue("BehindPrognosis", this.GetType(), 0);
                    pluginManager.SetPropertyValue("BehindLapsToOvertake", this.GetType(), -1);
                    pluginManager.SetPropertyValue("BehindLapsSincePit", this.GetType(), -1);
                    pluginManager.SetPropertyValue("BehindP2PStatus", this.GetType(), false);
                    pluginManager.SetPropertyValue("BehindP2PCount", this.GetType(), -1);

                    pluginManager.SetPropertyValue("AheadName", this.GetType(), "");
                    pluginManager.SetPropertyValue("AheadGap", this.GetType(), 0);
                    pluginManager.SetPropertyValue("AheadBestLap", this.GetType(), new TimeSpan(0));
                    pluginManager.SetPropertyValue("AheadIsConnected", this.GetType(), false);
                    pluginManager.SetPropertyValue("AheadIsInPit", this.GetType(), true);
                    pluginManager.SetPropertyValue("AheadRealGap", this.GetType(), 0);

                    pluginManager.SetPropertyValue("BehindName", this.GetType(), "");
                    pluginManager.SetPropertyValue("BehindGap", this.GetType(), 0);
                    pluginManager.SetPropertyValue("BehindBestLap", this.GetType(), new TimeSpan(0));
                    pluginManager.SetPropertyValue("BehindIsConnected", this.GetType(), false);
                    pluginManager.SetPropertyValue("BehindIsInPit", this.GetType(), true);
                    pluginManager.SetPropertyValue("BehindRealGap", this.GetType(), 0);

                    pluginManager.SetPropertyValue("ClassP1RealGap", this.GetType(), 0);

                    if (session == "Race")
                    {
                        pluginManager.SetPropertyValue("AheadName", this.GetType(), aheadName);
                        pluginManager.SetPropertyValue("AheadGap", this.GetType(), aheadGap);
                        pluginManager.SetPropertyValue("AheadBestLap", this.GetType(), aheadBestLap);
                        pluginManager.SetPropertyValue("AheadIsConnected", this.GetType(), aheadIsConnected);
                        pluginManager.SetPropertyValue("AheadIsInPit", this.GetType(), aheadIsInPit);

                        pluginManager.SetPropertyValue("BehindName", this.GetType(), behindName);
                        pluginManager.SetPropertyValue("BehindGap", this.GetType(), behindGap);
                        pluginManager.SetPropertyValue("BehindBestLap", this.GetType(), behindBestLap);
                        pluginManager.SetPropertyValue("BehindIsConnected", this.GetType(), behindIsConnected);
                        pluginManager.SetPropertyValue("BehindIsInPit", this.GetType(), behindIsInPit);

                        //Calculations of ahead and behind drivers + lucky dog

                        for (int e = 0; e < irData.SessionData.DriverInfo.CompetingDrivers.Length; e++)
                        {
                            if (aheadName == irData.SessionData.DriverInfo.CompetingDrivers[e].UserName)
                            {
                                int carID = Convert.ToInt16(irData.SessionData.DriverInfo.CompetingDrivers[e].CarIdx);

                                aheadRealGap = realGapOpponentDelta[carID];

                                if ((aheadRealGap > aheadGap * 1.25 && aheadRealGap - aheadGap > 10) || (aheadRealGap < aheadGap * 0.75 && aheadRealGap - aheadGap < -10) || aheadRealGap >= 0)
                                {
                                    aheadRealGap = aheadGap;
                                }
                                aheadLapsSincePit = sessionCarsLapsSincePit[carID];
                                if (p2pCount != null)
                                {
                                    aheadP2PCount = ((int[])p2pCount)[carID];
                                }
                                else
                                {
                                    aheadP2PCount = -1;
                                }
                                if (p2pStatus != null)
                                {
                                    aheadP2PActive = ((bool[])p2pStatus)[carID];
                                }
                                else
                                {
                                    aheadP2PActive = false;
                                }

                                break;
                            }
                        }

                        for (int i = 0; i < irData.SessionData.DriverInfo.CompetingDrivers.Length; i++)
                        {
                            if (behindName == irData.SessionData.DriverInfo.CompetingDrivers[i].UserName)
                            {
                                int carID = Convert.ToInt16(irData.SessionData.DriverInfo.CompetingDrivers[i].CarIdx);
                                behindRealGap = realGapOpponentDelta[carID];

                                if ((behindRealGap > behindGap * 1.25 && behindRealGap - behindGap > 10) || (behindRealGap < behindGap * 0.75 && behindRealGap - behindGap < -10) || behindRealGap <= 0)
                                {
                                    behindRealGap = behindGap;
                                }

                                behindLapsSincePit = sessionCarsLapsSincePit[carID];
                                if (p2pCount != null)
                                {
                                    behindP2PCount = ((int[])p2pCount)[carID];
                                }
                                else
                                {
                                    behindP2PCount = -1;
                                }
                                if (p2pStatus != null)
                                {
                                    behindP2PActive = ((bool[])p2pStatus)[carID];
                                }
                                else
                                {
                                    behindP2PActive = false;
                                }

                                break;

                            }
                        }

                        for (int i = 0; i < irData.SessionData.DriverInfo.CompetingDrivers.Length; i++)
                        {
                            if (luckyDogName == irData.SessionData.DriverInfo.CompetingDrivers[i].UserName)
                            {
                                int carID = Convert.ToInt16(irData.SessionData.DriverInfo.CompetingDrivers[i].CarIdx);
                                luckyDogRealGap = realGapOpponentDelta[carID];

                                if ((luckyDogRealGap > luckyDogGap * 1.25 && luckyDogRealGap - luckyDogGap > 10) || (luckyDogRealGap < luckyDogGap * 0.75 && luckyDogRealGap - luckyDogGap < -10) || luckyDogRealGap <= 0)
                                {
                                    luckyDogRealGap = luckyDogGap;
                                }

                                break;

                            }
                        }
                        if ((leaderCurrentLap + leaderTrackPosition) - (currentLap + trackPosition) > 1 && luckyDogGap > 0)
                        {
                            luckyDogRealGap = 0;
                        }

                        //Calculate class P1 real gap

                        for (int e = 0; e < irData.SessionData.DriverInfo.CompetingDrivers.Length; e++)
                        {
                            if (classLeaderName == irData.SessionData.DriverInfo.CompetingDrivers[e].UserName)
                            {
                                int carID = Convert.ToInt16(irData.SessionData.DriverInfo.CompetingDrivers[e].CarIdx);

                                if (carID == myCarIdx)
                                {
                                    classLeaderRealGap = 0;
                                    break;
                                }

                                classLeaderRealGap = realGapOpponentDelta[carID];

                                if ((classLeaderRealGap > classLeaderGap * 1.25 && classLeaderRealGap - classLeaderGap > 10) || (classLeaderRealGap < classLeaderGap * 0.75 && classLeaderRealGap - classLeaderGap < -10) || classLeaderRealGap >= 0)
                                {
                                    classLeaderRealGap = classLeaderGap;
                                }

                                break;
                            }
                        }
                        if (trackType > 4)
                        {
                            pluginManager.SetPropertyValue("LuckyDogRealGap", this.GetType(), luckyDogRealGap);
                        }
                        pluginManager.SetPropertyValue("ClassP1RealGap", this.GetType(), classLeaderRealGap);

                        double overtakeThreshold = -0.5;

                        double aheadBestLapSeconds = aheadBestLap.TotalSeconds;
                        double aheadLastLapSeconds = aheadLastLap.TotalSeconds;
                        double behindBestLapSeconds = behindBestLap.TotalSeconds;
                        double behindLastLapSeconds = behindLastLap.TotalSeconds;


                        if ((aheadBestLapSeconds != 0 || aheadLastLapSeconds != 0) && pace != 0)
                        {
                            double? overtakeGap = aheadRealGap - overtakeThreshold;
                            double aheadPace = (aheadBestLapSeconds + aheadLastLapSeconds * 2) / 3;
                            if (aheadBestLapSeconds == 0)
                            {
                                aheadPace = aheadLastLapSeconds;
                            }

                            if (aheadBestLapSeconds * 1.02 < aheadLastLapSeconds && aheadBestLapSeconds != 0)
                            {
                                aheadPace = aheadLastLapSeconds;
                                aheadSlowLap = true;
                            }

                            double distanceLeft = truncRemainingLaps + (1 - trackPosition);
                            double paceDifference = aheadPace - pace;
                            double? gapOnFinish = overtakeGap + (paceDifference * distanceLeft);
                            double? marginPerLap = gapOnFinish / distanceLeft;

                            if (marginPerLap > 0.7)
                            {
                                aheadOvertakePrediction = 1;
                            }
                            else if (marginPerLap > 0.2)
                            {
                                aheadOvertakePrediction = 2;
                            }
                            else if (marginPerLap > -0.2)
                            {
                                aheadOvertakePrediction = 3;
                            }
                            else if (marginPerLap > -0.7)
                            {
                                aheadOvertakePrediction = 4;
                            }
                            else
                            {
                                aheadOvertakePrediction = 5;
                            }

                            int aheadLapsToOvertake = ((int)(((-overtakeGap / paceDifference) + trackPosition) * 100)) / 100;

                            if (paceDifference < 0 || overtakeGap > -0.5)
                            {
                                aheadLapsToOvertake = -1;
                            }

                            TimeSpan aheadPaceTime = TimeSpan.FromSeconds(aheadPace);

                            pluginManager.SetPropertyValue("AheadName", this.GetType(), aheadName);
                            pluginManager.SetPropertyValue("AheadPace", this.GetType(), aheadPaceTime);
                            pluginManager.SetPropertyValue("AheadSlowLap", this.GetType(), aheadSlowLap);
                            pluginManager.SetPropertyValue("AheadPrognosis", this.GetType(), aheadOvertakePrediction);
                            pluginManager.SetPropertyValue("AheadLapsToOvertake", this.GetType(), aheadLapsToOvertake);
                            pluginManager.SetPropertyValue("AheadLapsSincePit", this.GetType(), aheadLapsSincePit);
                            pluginManager.SetPropertyValue("AheadP2PStatus", this.GetType(), aheadP2PActive);
                            pluginManager.SetPropertyValue("AheadP2PCount", this.GetType(), aheadP2PCount);
                            pluginManager.SetPropertyValue("AheadRealGap", this.GetType(), aheadRealGap);

                            aheadGlobal = aheadName;

                        }

                        if ((behindBestLapSeconds != 0 || behindLastLapSeconds != 0) && pace != 0)
                        {
                            double? overtakeGap = behindRealGap + overtakeThreshold;
                            double behindPace = (behindBestLapSeconds + behindLastLapSeconds * 2) / 3;
                            if (behindBestLapSeconds == 0)
                            {
                                behindPace = behindLastLapSeconds;
                            }

                            if (behindBestLapSeconds * 1.02 < behindLastLapSeconds && behindBestLapSeconds != 0)
                            {
                                behindPace = behindLastLapSeconds;
                                behindSlowLap = true;
                            }

                            double distanceLeft = truncRemainingLaps + (1 - trackPosition);
                            double paceDifference = behindPace - pace;
                            double? gapOnFinish = overtakeGap + (paceDifference * distanceLeft);
                            double? marginPerLap = gapOnFinish / distanceLeft;

                            if (marginPerLap > 0.7)
                            {
                                behindOvertakePrediction = 1;
                            }
                            else if (marginPerLap > 0.2)
                            {
                                behindOvertakePrediction = 2;
                            }
                            else if (marginPerLap > -0.2)
                            {
                                behindOvertakePrediction = 3;
                            }
                            else if (marginPerLap > -0.7)
                            {
                                behindOvertakePrediction = 4;
                            }
                            else
                            {
                                behindOvertakePrediction = 5;
                            }

                            int behindLapsToOvertake = ((int)(((-overtakeGap / paceDifference) + trackPosition) * 100)) / 100;
                            if (paceDifference > 0 || overtakeGap < 0.5)
                            {
                                behindLapsToOvertake = -1;
                            }

                            TimeSpan behindPaceTime = TimeSpan.FromSeconds(behindPace);

                            pluginManager.SetPropertyValue("BehindName", this.GetType(), behindName);
                            pluginManager.SetPropertyValue("BehindPace", this.GetType(), behindPaceTime);
                            pluginManager.SetPropertyValue("BehindSlowLap", this.GetType(), behindSlowLap);
                            pluginManager.SetPropertyValue("BehindPrognosis", this.GetType(), behindOvertakePrediction);
                            pluginManager.SetPropertyValue("BehindLapsSincePit", this.GetType(), behindLapsSincePit);
                            pluginManager.SetPropertyValue("BehindP2PStatus", this.GetType(), behindP2PActive);
                            pluginManager.SetPropertyValue("BehindP2PCount", this.GetType(), behindP2PCount);
                            pluginManager.SetPropertyValue("BehindRealGap", this.GetType(), behindRealGap);

                            behindGlobal = behindName;
                        }
                    }
                }

                //---------------------------------------------
                //------------LONE QUALY-----------------------
                //---------------------------------------------

                if (session == "Lone Qualify")
                {
                    if (currentLapTime.TotalSeconds == 0) //Warmup lap
                    {
                        warmup = true;
                        lapLapsRemaining = 2;
                        if (pit == 1)
                        {
                            lapLapsRemaining = 3;
                        }
                        qLap1Status = 1;
                        qLap2Status = 1;
                    }
                    else if (completedLaps == 0) //1st Q lap
                    {
                        lapLapsRemaining = 1;
                        qLap1Status = 2;
                        qLap1Time = currentLapTime;
                        if (lapStatus > 1)
                        {
                            qLap1Status = 3;
                            qLap1Time = TimeSpan.FromSeconds(0);
                        }
                        qLapStarted2 = false;
                    }
                    else if (completedLaps == 1) //2nd Q lap
                    {
                        if (lapTimeList.Count > 0)
                        {
                            qLap1Time = lapTimeList[0];
                        }
                        if (qLap1Status == 2)
                        {
                            qLap1Status = 4;
                        }
                        if (currentLapTime.TotalSeconds < 5 || (trackPosition > 0.1 && trackPosition < 0.11))
                        {
                            qLapStarted2 = true;
                        }
                        if (qLapStarted2)
                        {
                            qLap2Time = currentLapTime;
                        }
                        lapLapsRemaining = 0;
                        qLap2Status = 2;
                        if (lapStatus > 1)
                        {
                            qLap2Status = 3;
                            qLap2Time = TimeSpan.FromSeconds(0);
                        }
                    }
                    else if (completedLaps == 2) //Completed qualy
                    {
                        lapLapsRemaining = 0;
                        if (lapTimeList.Count > 0 && qLap1Time != lapTimeList[0])
                        {
                            qLap2Time = lapTimeList[0];
                        }
                        if (qLap2Status == 2)
                        {
                            qLap2Status = 4;
                        }
                    }

                    pluginManager.SetPropertyValue("QualyWarmUpLap", this.GetType(), warmup);
                    pluginManager.SetPropertyValue("QualyLap1Status", this.GetType(), qLap1Status);
                    pluginManager.SetPropertyValue("QualyLap2Status", this.GetType(), qLap2Status);
                    pluginManager.SetPropertyValue("QualyLap1Time", this.GetType(), qLap1Time);
                    pluginManager.SetPropertyValue("QualyLap2Time", this.GetType(), qLap2Time);

                }


                //---------------------------------------------
                //-------------AHEAD/BEHIND CARS---------------
                //---------------------------------------------
                if (counter == 9 || counter == 24 || counter == 39 || counter == 54)
                {
                    carAheadGap.Clear();
                    carAheadRaceGap.Clear();
                    carAheadName.Clear();
                    carAheadIsInPit.Clear();
                    carAheadIsClassLeader.Clear();
                    carAheadClassColor.Clear();
                    carAheadClassDifference.Clear();
                    carAheadIsAhead.Clear();
                    carAheadLicence.Clear();
                    carAheadiRating.Clear();
                    carAheadBestLap.Clear();
                    carAheadJokerLaps.Clear();
                    carAheadLapsSincePit.Clear();
                    carAheadPosition.Clear();
                    carAheadP2PCount.Clear();
                    carAheadP2PStatus.Clear();
                    carAheadRealGap.Clear();
                    carAheadRealRelative.Clear();

                    carBehindGap.Clear();
                    carBehindRaceGap.Clear();
                    carBehindName.Clear();
                    carBehindIsInPit.Clear();
                    carBehindIsClassLeader.Clear();
                    carBehindClassColor.Clear();
                    carBehindClassDifference.Clear();
                    carBehindIsAhead.Clear();
                    carBehindLicence.Clear();
                    carBehindiRating.Clear();
                    carBehindBestLap.Clear();
                    carBehindJokerLaps.Clear();
                    carBehindLapsSincePit.Clear();
                    carBehindPosition.Clear();
                    carBehindP2PCount.Clear();
                    carBehindP2PStatus.Clear();
                    carBehindRealGap.Clear();
                    carBehindRealRelative.Clear();

                    //Session car lists

                    //Checking the list regularly

                    for (int i = 0; i < 64; i++)
                    {
                        int trackLoc = Convert.ToInt16(irData.Telemetry.CarIdxTrackSurface[i]);

                        if (trackLoc >= 0)
                        {
                            if (trackLoc == 1 || trackLoc == 2)
                            {
                                sessionCarsLap[i] = irData.Telemetry.CarIdxLap[i];
                                sessionCarsLapsSincePit[i] = 0;
                            }
                            else if (sessionCarsLapsSincePit[i] != -1 || (sessionCarsLapsSincePit[i] == -1 && irData.Telemetry.CarIdxLap[i] - sessionCarsLap[i] > 0))
                            {
                                sessionCarsLapsSincePit[i] = irData.Telemetry.CarIdxLap[i] - sessionCarsLap[i];
                            }
                            else if (sessionCarsLapsSincePit[i] < -1)
                            {
                                sessionCarsLap[i] = irData.Telemetry.CarIdxLap[i];
                                sessionCarsLapsSincePit[i] = -1;
                            }
                        }
                        else
                        {
                            sessionCarsLap[i] = irData.Telemetry.CarIdxLap[i];
                            sessionCarsLapsSincePit[i] = -1;

                        }
                    }

                    //Cars ahead/behind on track calculations

                    for (int i = 0; i < data.NewData.OpponentsAheadOnTrack.Count && i < 5; i++)
                    {
                        carAheadGap.Add(data.NewData.OpponentsAheadOnTrack[i].RelativeGapToPlayer);
                        carAheadRaceGap.Add(data.NewData.OpponentsAheadOnTrack[i].GaptoPlayer);
                        carAheadName.Add(data.NewData.OpponentsAheadOnTrack[i].Name);
                        carAheadIsInPit.Add(data.NewData.OpponentsAheadOnTrack[i].IsCarInPit);
                        carAheadBestLap.Add(data.NewData.OpponentsAheadOnTrack[i].BestLapTime);
                        carAheadPosition.Add(data.NewData.OpponentsAheadOnTrack[i].Position);

                        for (int u = 0; u < irData.SessionData.DriverInfo.CompetingDrivers.Length; u++)
                        {
                            if (irData.SessionData.DriverInfo.CompetingDrivers[u].UserName == data.NewData.OpponentsAheadOnTrack[i].Name)
                            {
                                carAheadLicence.Add(irData.SessionData.DriverInfo.CompetingDrivers[u].LicString);
                                carAheadiRating.Add(irData.SessionData.DriverInfo.CompetingDrivers[u].IRating);
                                carAheadClassColor.Add(irData.SessionData.DriverInfo.CompetingDrivers[u].CarClassColor);
                                carAheadClassDifference.Add((classColors.IndexOf(irData.SessionData.DriverInfo.CompetingDrivers[u].CarClassColor)) - myClassColorIndex);
                                carAheadLapsSincePit.Add(sessionCarsLapsSincePit[Convert.ToInt32(irData.SessionData.DriverInfo.CompetingDrivers[u].CarIdx)]);

                                double? gap = data.NewData.OpponentsAheadOnTrack[i].GaptoPlayer;
                                double? realgap = realGapOpponentDelta[Convert.ToInt32(irData.SessionData.DriverInfo.CompetingDrivers[u].CarIdx)];
                                double? relative = data.NewData.OpponentsAheadOnTrack[i].RelativeGapToPlayer;
                                double? realrelative = realGapOpponentRelative[Convert.ToInt32(irData.SessionData.DriverInfo.CompetingDrivers[u].CarIdx)];

                                if ((gap > realgap * 1.25 && gap - realgap > 10) || (gap < realgap * 0.75 && gap - realgap < -10) || realgap == 0)
                                {
                                    realgap = gap;
                                    if (realgap == null)
                                    {
                                        realgap = 0;
                                    }
                                }
                                if (relative - realrelative > 10 || relative - realrelative < -10 || realrelative >= 0)
                                {
                                    realrelative = relative;
                                }

                                carAheadRealGap.Add(realgap);
                                carAheadRealRelative.Add(realrelative);

                                if (p2pCount != null)
                                {
                                    carAheadP2PCount.Add(((int[])p2pCount)[Convert.ToInt32(irData.SessionData.DriverInfo.CompetingDrivers[u].CarIdx)]);
                                }
                                if (p2pStatus != null)
                                {
                                    carAheadP2PStatus.Add(((bool[])p2pStatus)[Convert.ToInt32(irData.SessionData.DriverInfo.CompetingDrivers[u].CarIdx)]);
                                }

                                if (irData.SessionData.SessionInfo.Sessions[sessionNumber].ResultsPositions != null)
                                {
                                    for (int e = 0; e < irData.SessionData.SessionInfo.Sessions[sessionNumber].ResultsPositions.Length; e++)
                                    {
                                        if (irData.SessionData.DriverInfo.CompetingDrivers[u].CarIdx == irData.SessionData.SessionInfo.Sessions[sessionNumber].ResultsPositions[e].CarIdx)
                                        {
                                            carAheadJokerLaps.Add(irData.SessionData.SessionInfo.Sessions[sessionNumber].ResultsPositions[e].JokerLapsComplete);
                                            break;
                                        }
                                    }
                                }

                                if (carAheadJokerLaps.Count < carAheadName.Count)
                                {
                                    carAheadJokerLaps.Add(0);
                                }

                                break;
                            }
                        }
                        if (data.NewData.OpponentsAheadOnTrack[i].GaptoPlayer < 0 || (data.NewData.OpponentsAheadOnTrack[i].RelativeGapToPlayer != null && data.NewData.OpponentsAheadOnTrack[i].GaptoPlayer == null))
                        {
                            carAheadIsAhead.Add(true);
                        }
                        else
                        {
                            carAheadIsAhead.Add(false);
                        }
                        if (data.NewData.OpponentsAheadOnTrack[i].Name == classLeaderName)
                        {
                            carAheadIsClassLeader.Add(true);
                        }
                        else
                        {
                            carAheadIsClassLeader.Add(false);
                        }

                        pluginManager.SetPropertyValue("CarAhead0" + (i + 1) + "Gap", this.GetType(), carAheadGap[i]);
                        pluginManager.SetPropertyValue("CarAhead0" + (i + 1) + "RaceGap", this.GetType(), carAheadRaceGap[i]);
                        pluginManager.SetPropertyValue("CarAhead0" + (i + 1) + "BestLap", this.GetType(), carAheadBestLap[i]);
                        pluginManager.SetPropertyValue("CarAhead0" + (i + 1) + "Name", this.GetType(), carAheadName[i]);
                        pluginManager.SetPropertyValue("CarAhead0" + (i + 1) + "IRating", this.GetType(), carAheadiRating[i]);
                        pluginManager.SetPropertyValue("CarAhead0" + (i + 1) + "Licence", this.GetType(), carAheadLicence[i]);
                        pluginManager.SetPropertyValue("CarAhead0" + (i + 1) + "IsAhead", this.GetType(), carAheadIsAhead[i]);
                        pluginManager.SetPropertyValue("CarAhead0" + (i + 1) + "IsClassLeader", this.GetType(), carAheadIsClassLeader[i]);
                        pluginManager.SetPropertyValue("CarAhead0" + (i + 1) + "IsInPit", this.GetType(), carAheadIsInPit[i]);
                        pluginManager.SetPropertyValue("CarAhead0" + (i + 1) + "ClassColor", this.GetType(), carAheadClassColor[i]);
                        pluginManager.SetPropertyValue("CarAhead0" + (i + 1) + "ClassDifference", this.GetType(), carAheadClassDifference[i]);
                        pluginManager.SetPropertyValue("CarAhead0" + (i + 1) + "Position", this.GetType(), carAheadPosition[i]);
                        pluginManager.SetPropertyValue("CarAhead0" + (i + 1) + "JokerLaps", this.GetType(), carAheadJokerLaps[i]);
                        pluginManager.SetPropertyValue("CarAhead0" + (i + 1) + "LapsSincePit", this.GetType(), carAheadLapsSincePit[i]);

                        if (carAheadP2PCount.Count > 0)
                        {
                            pluginManager.SetPropertyValue("CarAhead0" + (i + 1) + "P2PCount", this.GetType(), carAheadP2PCount[i]);
                            pluginManager.SetPropertyValue("CarAhead0" + (i + 1) + "P2PStatus", this.GetType(), carAheadP2PStatus[i]);
                        }

                        pluginManager.SetPropertyValue("CarAhead0" + (i + 1) + "RealGap", this.GetType(), carAheadRealGap[i]);
                        pluginManager.SetPropertyValue("CarAhead0" + (i + 1) + "RealRelative", this.GetType(), carAheadRealRelative[i]);

                    }

                    for (int i = data.NewData.OpponentsAheadOnTrack.Count; i < 5; i++) //Clearing the empty ones
                    {
                        pluginManager.SetPropertyValue("CarAhead0" + (i + 1) + "Gap", this.GetType(), 0);
                        pluginManager.SetPropertyValue("CarAhead0" + (i + 1) + "RaceGap", this.GetType(), 0);
                        pluginManager.SetPropertyValue("CarAhead0" + (i + 1) + "BestLap", this.GetType(), new TimeSpan(0));
                        pluginManager.SetPropertyValue("CarAhead0" + (i + 1) + "Name", this.GetType(), "");
                        pluginManager.SetPropertyValue("CarAhead0" + (i + 1) + "IRating", this.GetType(), 0);
                        pluginManager.SetPropertyValue("CarAhead0" + (i + 1) + "Licence", this.GetType(), "");
                        pluginManager.SetPropertyValue("CarAhead0" + (i + 1) + "IsAhead", this.GetType(), false);
                        pluginManager.SetPropertyValue("CarAhead0" + (i + 1) + "IsClassLeader", this.GetType(), false);
                        pluginManager.SetPropertyValue("CarAhead0" + (i + 1) + "IsInPit", this.GetType(), false);
                        pluginManager.SetPropertyValue("CarAhead0" + (i + 1) + "ClassColor", this.GetType(), "");
                        pluginManager.SetPropertyValue("CarAhead0" + (i + 1) + "ClassDifference", this.GetType(), 0);
                        pluginManager.SetPropertyValue("CarAhead0" + (i + 1) + "Position", this.GetType(), 0);
                        pluginManager.SetPropertyValue("CarAhead0" + (i + 1) + "JokerLaps", this.GetType(), 0);
                        pluginManager.SetPropertyValue("CarAhead0" + (i + 1) + "LapsSincePit", this.GetType(), -1);
                        pluginManager.SetPropertyValue("CarAhead0" + (i + 1) + "P2PCount", this.GetType(), -1);
                        pluginManager.SetPropertyValue("CarAhead0" + (i + 1) + "P2PStatus", this.GetType(), false);
                        pluginManager.SetPropertyValue("CarAhead0" + (i + 1) + "RealGap", this.GetType(), 0);
                        pluginManager.SetPropertyValue("CarAhead0" + (i + 1) + "RealRelative", this.GetType(), 0);
                    }

                    for (int i = 0; i < data.NewData.OpponentsBehindOnTrack.Count && i < 5; i++)
                    {
                        carBehindGap.Add(data.NewData.OpponentsBehindOnTrack[i].RelativeGapToPlayer);
                        carBehindRaceGap.Add(data.NewData.OpponentsBehindOnTrack[i].GaptoPlayer);
                        carBehindName.Add(data.NewData.OpponentsBehindOnTrack[i].Name);
                        carBehindIsInPit.Add(data.NewData.OpponentsBehindOnTrack[i].IsCarInPit);
                        carBehindBestLap.Add(data.NewData.OpponentsBehindOnTrack[i].BestLapTime);
                        carBehindPosition.Add(data.NewData.OpponentsBehindOnTrack[i].Position);

                        for (int u = 0; u < irData.SessionData.DriverInfo.CompetingDrivers.Length; u++)
                        {
                            if (irData.SessionData.DriverInfo.CompetingDrivers[u].UserName == data.NewData.OpponentsBehindOnTrack[i].Name)
                            {
                                carBehindLicence.Add(irData.SessionData.DriverInfo.CompetingDrivers[u].LicString);
                                carBehindiRating.Add(irData.SessionData.DriverInfo.CompetingDrivers[u].IRating);
                                carBehindClassColor.Add(irData.SessionData.DriverInfo.CompetingDrivers[u].CarClassColor);
                                carBehindClassDifference.Add((classColors.IndexOf(irData.SessionData.DriverInfo.CompetingDrivers[u].CarClassColor)) - myClassColorIndex);
                                carBehindLapsSincePit.Add(sessionCarsLapsSincePit[Convert.ToInt32(irData.SessionData.DriverInfo.CompetingDrivers[u].CarIdx)]);

                                double? relative = data.NewData.OpponentsBehindOnTrack[i].RelativeGapToPlayer;
                                double? gap = data.NewData.OpponentsBehindOnTrack[i].GaptoPlayer;
                                double? realgap = realGapOpponentDelta[Convert.ToInt32(irData.SessionData.DriverInfo.CompetingDrivers[u].CarIdx)];
                                double? realrelative = realGapOpponentRelative[Convert.ToInt32(irData.SessionData.DriverInfo.CompetingDrivers[u].CarIdx)];

                                if ((gap > realgap * 1.25 && gap - realgap > 10) || (gap < realgap * 0.75 && gap - realgap < -10) || realgap == 0)
                                {
                                    realgap = gap;
                                    if (realgap == null)
                                    {
                                        realgap = 0;
                                    }
                                }
                                if (relative - realrelative > 10 || relative - realrelative < -10 || realrelative <= 0)
                                {
                                    realrelative = relative;
                                }

                                carBehindRealGap.Add(realgap);
                                carBehindRealRelative.Add(realrelative);


                                if (p2pCount != null)
                                {
                                    carBehindP2PCount.Add(((int[])p2pCount)[Convert.ToInt32(irData.SessionData.DriverInfo.CompetingDrivers[u].CarIdx)]);
                                }
                                if (p2pStatus != null)
                                {
                                    carBehindP2PStatus.Add(((bool[])p2pStatus)[Convert.ToInt32(irData.SessionData.DriverInfo.CompetingDrivers[u].CarIdx)]);
                                }


                                if (irData.SessionData.SessionInfo.Sessions[sessionNumber].ResultsPositions != null)
                                {
                                    for (int e = 0; e < irData.SessionData.SessionInfo.Sessions[sessionNumber].ResultsPositions.Length; e++)
                                    {
                                        if (irData.SessionData.DriverInfo.CompetingDrivers[u].CarIdx == irData.SessionData.SessionInfo.Sessions[sessionNumber].ResultsPositions[e].CarIdx)
                                        {
                                            carBehindJokerLaps.Add(irData.SessionData.SessionInfo.Sessions[sessionNumber].ResultsPositions[e].JokerLapsComplete);
                                            break;
                                        }
                                    }
                                }

                                if (carBehindJokerLaps.Count < carBehindName.Count)
                                {
                                    carBehindJokerLaps.Add(0);
                                }

                                break;

                            }
                        }
                        if (data.NewData.OpponentsBehindOnTrack[i].GaptoPlayer < 0)
                        {
                            carBehindIsAhead.Add(true);
                        }
                        else
                        {
                            carBehindIsAhead.Add(false);
                        }
                        if (data.NewData.OpponentsBehindOnTrack[i].Name == classLeaderName)
                        {
                            carBehindIsClassLeader.Add(true);
                        }
                        else
                        {
                            carBehindIsClassLeader.Add(false);
                        }
                        pluginManager.SetPropertyValue("CarBehind0" + (i + 1) + "Gap", this.GetType(), carBehindGap[i]);
                        pluginManager.SetPropertyValue("CarBehind0" + (i + 1) + "RaceGap", this.GetType(), carBehindRaceGap[i]);
                        pluginManager.SetPropertyValue("CarBehind0" + (i + 1) + "BestLap", this.GetType(), carBehindBestLap[i]);
                        pluginManager.SetPropertyValue("CarBehind0" + (i + 1) + "Name", this.GetType(), carBehindName[i]);
                        pluginManager.SetPropertyValue("CarBehind0" + (i + 1) + "IRating", this.GetType(), carBehindiRating[i]);
                        pluginManager.SetPropertyValue("CarBehind0" + (i + 1) + "Licence", this.GetType(), carBehindLicence[i]);
                        pluginManager.SetPropertyValue("CarBehind0" + (i + 1) + "IsAhead", this.GetType(), carBehindIsAhead[i]);
                        pluginManager.SetPropertyValue("CarBehind0" + (i + 1) + "IsClassLeader", this.GetType(), carBehindIsClassLeader[i]);
                        pluginManager.SetPropertyValue("CarBehind0" + (i + 1) + "IsInPit", this.GetType(), carBehindIsInPit[i]);
                        pluginManager.SetPropertyValue("CarBehind0" + (i + 1) + "ClassColor", this.GetType(), carBehindClassColor[i]);
                        pluginManager.SetPropertyValue("CarBehind0" + (i + 1) + "ClassDifference", this.GetType(), carBehindClassDifference[i]);
                        pluginManager.SetPropertyValue("CarBehind0" + (i + 1) + "Position", this.GetType(), carBehindPosition[i]);
                        pluginManager.SetPropertyValue("CarBehind0" + (i + 1) + "JokerLaps", this.GetType(), carBehindJokerLaps[i]);
                        pluginManager.SetPropertyValue("CarBehind0" + (i + 1) + "LapsSincePit", this.GetType(), carBehindLapsSincePit[i]);

                        if (carBehindP2PCount.Count > 0)
                        {
                            pluginManager.SetPropertyValue("CarBehind0" + (i + 1) + "P2PCount", this.GetType(), carBehindP2PCount[i]);
                            pluginManager.SetPropertyValue("CarBehind0" + (i + 1) + "P2PStatus", this.GetType(), carBehindP2PStatus[i]);
                        }

                        pluginManager.SetPropertyValue("CarBehind0" + (i + 1) + "RealGap", this.GetType(), carBehindRealGap[i]);
                        pluginManager.SetPropertyValue("CarBehind0" + (i + 1) + "RealRelative", this.GetType(), carBehindRealRelative[i]);
                    }

                    for (int i = data.NewData.OpponentsBehindOnTrack.Count; i < 5; i++)
                    {
                        pluginManager.SetPropertyValue("CarBehind0" + (i + 1) + "Gap", this.GetType(), 0);
                        pluginManager.SetPropertyValue("CarBehind0" + (i + 1) + "RaceGap", this.GetType(), 0);
                        pluginManager.SetPropertyValue("CarBehind0" + (i + 1) + "BestLap", this.GetType(), new TimeSpan(0));
                        pluginManager.SetPropertyValue("CarBehind0" + (i + 1) + "Name", this.GetType(), "");
                        pluginManager.SetPropertyValue("CarBehind0" + (i + 1) + "IRating", this.GetType(), 0);
                        pluginManager.SetPropertyValue("CarBehind0" + (i + 1) + "Licence", this.GetType(), "");
                        pluginManager.SetPropertyValue("CarBehind0" + (i + 1) + "IsAhead", this.GetType(), false);
                        pluginManager.SetPropertyValue("CarBehind0" + (i + 1) + "IsClassLeader", this.GetType(), false);
                        pluginManager.SetPropertyValue("CarBehind0" + (i + 1) + "IsInPit", this.GetType(), false);
                        pluginManager.SetPropertyValue("CarBehind0" + (i + 1) + "ClassColor", this.GetType(), "");
                        pluginManager.SetPropertyValue("CarBehind0" + (i + 1) + "ClassDifference", this.GetType(), 0);
                        pluginManager.SetPropertyValue("CarBehind0" + (i + 1) + "Position", this.GetType(), 0);
                        pluginManager.SetPropertyValue("CarBehind0" + (i + 1) + "JokerLaps", this.GetType(), 0);
                        pluginManager.SetPropertyValue("CarBehind0" + (i + 1) + "LapsSincePit", this.GetType(), -1);
                        pluginManager.SetPropertyValue("CarBehind0" + (i + 1) + "P2PCount", this.GetType(), -1);
                        pluginManager.SetPropertyValue("CarBehind0" + (i + 1) + "P2PStatus", this.GetType(), false);
                        pluginManager.SetPropertyValue("CarBehind0" + (i + 1) + "RealGap", this.GetType(), 0);
                        pluginManager.SetPropertyValue("CarBehind0" + (i + 1) + "RealRelative", this.GetType(), 0);
                    }
                }



                //---------------------------------------------
                //--------------FUEL CALCULATION + STINT-------
                //---------------------------------------------

                if (counter == 3 || counter == 4 || pit == 1)
                {
                    pluginManager.SetPropertyValue("FuelPerLapTarget", this.GetType(), Settings.fuelPerLapTarget);

                    double fuelLastLap = Convert.ToDouble(pluginManager.GetPropertyValue("DataCorePlugin.Computed.Fuel_LastLapConsumption"));
                    double fuelPerLap = 0;

                    int truncRemainingLaps = ((int)(remainingLaps * 100)) / 100;

                    if (sessionState < 4 && trackPosition == 0 && isLapLimited && !isTimeLimited) //When standing on grid and track position is not updated yet. 
                    {
                        truncRemainingLaps--;
                    }

                    if (counter != 4)
                    {
                        fuelPerLap = fuelAvgLap + Math.Round(fuelPerLapOffset, 2);
                    }
                    else
                    {
                        fuelPerLap = fuelLastLap;
                    }

                    if (counter != 4)
                    {
                        pluginManager.SetPropertyValue("FuelDelta", this.GetType(), 0);
                        pluginManager.SetPropertyValue("FuelPitWindowFirst", this.GetType(), 0);
                        pluginManager.SetPropertyValue("FuelPitWindowLast", this.GetType(), 0);
                        pluginManager.SetPropertyValue("FuelMinimumFuelFill", this.GetType(), 0);
                        pluginManager.SetPropertyValue("FuelMaximumFuelFill", this.GetType(), 0);
                        pluginManager.SetPropertyValue("FuelPitStops", this.GetType(), 0);
                        pluginManager.SetPropertyValue("FuelConserveToSaveAStop", this.GetType(), 0);
                        pluginManager.SetPropertyValue("FuelSlowestFuelSavePace", this.GetType(), new TimeSpan(0));
                        pluginManager.SetPropertyValue("FuelAlert", this.GetType(), false);

                    }
                    else
                    {
                        pluginManager.SetPropertyValue("FuelDeltaLL", this.GetType(), 0);
                        pluginManager.SetPropertyValue("FuelPitWindowFirstLL", this.GetType(), 0);
                        pluginManager.SetPropertyValue("FuelPitWindowLastLL", this.GetType(), 0);
                        pluginManager.SetPropertyValue("FuelMinimumFuelFillLL", this.GetType(), 0);
                        pluginManager.SetPropertyValue("FuelMaximumFuelFillLL", this.GetType(), 0);
                        pluginManager.SetPropertyValue("FuelPitStopsLL", this.GetType(), 0);
                        pluginManager.SetPropertyValue("FuelConserveToSaveAStopLL", this.GetType(), 0);
                        pluginManager.SetPropertyValue("FuelSaveDeltaValue", this.GetType(), 0);
                    }


                    if (fuelPerLap > 0)
                    {
                        double distanceLeft = truncRemainingLaps + 1 - trackPosition;
                        double fuelDelta = fuel - (fuelPerLap * distanceLeft);

                        //Calculating pit window

                        //Room for fuel
                        double roomForFuel = maxFuel - fuel;
                        double roomAfterDelta = roomForFuel + fuelDelta;

                        //Where will I get to with current fuel load
                        double dryPosition = (fuel / fuelPerLap) + currentLap + trackPosition;
                        //Latest possible pit stop on lap:
                        int latestPitLap = ((int)((dryPosition - 1.1) * 100)) / 100;
                        if (fuelDelta > 0 && session != "Offline Testing")
                        {
                            latestPitLap = 0;
                        }

                        //Fuel alert
                        bool fuelAlert = false;
                        if (latestPitLap != 0 && latestPitLap <= currentLap)
                        {
                            fuelAlert = true;
                        }

                        //How much is left on tank on latest possible stop
                        double latestPitFuelLoad = (dryPosition - (latestPitLap + 1)) * fuelPerLap;
                        //The most I can fuel
                        double maxFillOnStop = maxFuel - latestPitFuelLoad;
                        //How far can I get on that tank?
                        double maxDist = maxFuel / fuelPerLap;
                        double maxLaps = ((int)((maxDist - 1.1) * 100)) / 100;
                        //Least amount of fuel to give maximum amount of laps
                        double secondFuelingMinimum = fuelPerLap * maxLaps;

                        //Maximumfueling
                        //How much is left on tank at the end of this lap
                        double thisLapFuelLoad = (fuel - ((1 - myPosition) * fuelPerLap));
                        //Compare that to secondFuelingMinumum           
                        maxFuelPush = secondFuelingMinimum - thisLapFuelLoad;

                        double pitStops = 0;
                        if (remainingLaps != 0)
                        {
                            if (fuelDelta > 0)
                            {
                                pitStops = 1 - (fuelDelta / maxFuel);
                            }
                            if (fuelDelta < 0)
                            {
                                pitStops = 1 - (fuelDelta / maxFillOnStop);
                            }
                            if (fuelDelta < -maxFillOnStop)
                            {
                                pitStops = 2 - ((fuelDelta + maxFillOnStop) / secondFuelingMinimum);
                            }
                        }


                        int truncPitStops = ((int)(pitStops * 100)) / 100;
                        double minimumFueling = (pitStops - truncPitStops) * maxFuel;

                        if (minimumFueling > maxFillOnStop)
                        {
                            minimumFueling = maxFillOnStop - 0.01;
                        }

                        double roomAfterMinFueling = roomForFuel - minimumFueling;

                        minFuelPush = 0;
                        if (fuelDelta < 0)
                        {
                            minFuelPush = -fuelDelta;
                        }
                        if (pitStops > 2)
                        {
                            minFuelPush = minimumFueling;
                        }

                        if (counter != 4)
                        {
                            commandMinFuel = Math.Ceiling(minFuelPush + 0.5);
                            if (minFuelPush == 0)
                            {
                                commandMinFuel = 0;
                            }
                            commandMaxFuel = Math.Ceiling(maxFuelPush + 0.5);
                            if (maxFuelPush == 0)
                            {
                                commandMaxFuel = 500;
                            }

                        }

                        int earliestLap = ((int)((currentLap + trackPosition - (roomAfterDelta / fuelPerLap)) * 100)) / 100;
                        if (pitStops > 2)
                        {
                            earliestLap = ((int)((currentLap + trackPosition - (roomAfterMinFueling / fuelPerLap)) * 100)) / 100;
                        }
                        if (earliestLap <= currentLap || pitStops > 2 && roomAfterMinFueling > 0)
                        {
                            earliestLap = currentLap;
                        }
                        if (fuelDelta > 0)
                        {
                            earliestLap = 0;
                        }

                        double conserveToNotPit = (minFuelPush / distanceLeft) / fuelPerLap;
                        double slowestLapTime = (pitStopDuration / distanceLeft) + myExpectedLapTime;

                        double saveDelta = 0;
                        if (pitStops - truncPitStops > 0.5)
                        {
                            slowestLapTime = 0;
                            if (minFuelPush == 0)
                            {
                                saveDelta = -fuelDelta;
                            }
                            else
                            {
                                saveDelta = minFuelPush - maxFillOnStop;
                            }

                        }
                        else
                        {
                            saveDelta = minFuelPush;
                            if (minFuelPush == 0)
                            {
                                slowestLapTime = 0;
                            }
                        }

                        if (session == "Offline Testing")
                        {
                            slowestLapTime = 0;
                            fuelDelta = 0;
                            pitStops = 0;
                            if (roomForFuel > fuelPerLap)
                            {
                                earliestLap = currentLap;
                            }
                            else
                            {
                                earliestLap = 0;
                            }
                            saveDelta = 0;

                        }

                        TimeSpan slowestLapTimeSpan = TimeSpan.FromSeconds(slowestLapTime);

                        if (raceFinished)
                        {
                            fuelDelta = 0;
                        }

                        if (counter != 4)
                        {

                            slowestLapTimeSpanCopy = slowestLapTimeSpan;

                            pluginManager.SetPropertyValue("FuelDelta", this.GetType(), fuelDelta);
                            pluginManager.SetPropertyValue("FuelPitWindowFirst", this.GetType(), earliestLap);
                            pluginManager.SetPropertyValue("FuelPitWindowLast", this.GetType(), latestPitLap);
                            pluginManager.SetPropertyValue("FuelMinimumFuelFill", this.GetType(), minFuelPush);
                            pluginManager.SetPropertyValue("FuelMaximumFuelFill", this.GetType(), maxFuelPush);
                            pluginManager.SetPropertyValue("FuelPitStops", this.GetType(), pitStops);
                            pluginManager.SetPropertyValue("FuelConserveToSaveAStop", this.GetType(), conserveToNotPit);
                            pluginManager.SetPropertyValue("FuelAlert", this.GetType(), fuelAlert);


                            if (!savePitTimerLock)
                            {
                                pluginManager.SetPropertyValue("FuelSlowestFuelSavePace", this.GetType(), slowestLapTimeSpan);
                            }
                            else
                            {
                                pluginManager.SetPropertyValue("FuelSlowestFuelSavePace", this.GetType(), savePitTimerSnap);
                            }


                            //Stint calculations

                            if ((lapTimeList[0].TotalSeconds == 0 && pit == 0) || pitBox > 0 || (session == "Race" && sessionState == 2) || (session == "Lone Qualify" && pit == 1)) //Update values only when in box, on grid or at end of pit lane for qualy laps. 
                            {
                                stintLapsTotal = latestPitLap - currentLap - 1; //Laps remaining of the stint
                                if ((session == "Race" && sessionState == 2) || (session == "Lone Qualify" && pit == 1) || (lapTimeList[0].TotalSeconds == 0 && pit == 0))
                                {
                                    stintLapsTotal++;
                                }
                                if (fuelDelta > 0) //In case there is no need to fuel to end the sessions
                                {
                                    stintLapsTotal = truncRemainingLaps;
                                }

                                if (pitLocation > 0.8 && !(session == "Race" && sessionState == 2) && !(session == "Lone Qualify" && pit == 1))
                                {
                                    stintLapsTotal--;
                                }
                                stintTimeTotal = TimeSpan.FromSeconds((stintLapsTotal + 2) * myExpectedLapTime);
                                if ((session == "Race" && sessionState == 2) || (session == "Lone Qualify" && pit == 1))
                                {
                                    stintTimeTotal = TimeSpan.FromSeconds(stintLapsTotal * myExpectedLapTime);
                                }
                                if (fuelDelta > 0) //In case there is no need to fuel to end the sessions
                                {
                                    if (isLapLimited)
                                    {
                                        stintTimeTotal = TimeSpan.FromSeconds(truncRemainingLaps * myExpectedLapTime);
                                    }
                                    else
                                    {
                                        double posWhenZero = timeLeft.TotalSeconds / myExpectedLapTime + trackPosition;
                                        int truncPos = ((int)(posWhenZero * 100)) / 100;

                                        stintTimeTotal = TimeSpan.FromSeconds(timeLeft.TotalSeconds + (1 - (posWhenZero - truncPos)) * myExpectedLapTime);
                                    }
                                }
                                if (lapTimeList[0].TotalSeconds == 0 && pit == 0)
                                {
                                    stintTimeTotal = TimeSpan.FromSeconds(stintTimeTotal.TotalSeconds);
                                }
                                if (sessionState > 4) //If session is ending
                                {
                                    stintTimeTotal = new TimeSpan(0);
                                    stintLapsTotal = 0;
                                }
                                pluginManager.SetPropertyValue("StintTotalTime", this.GetType(), stintTimeTotal);
                                pluginManager.SetPropertyValue("StintTotalHotlaps", this.GetType(), stintLapsTotal);
                            }

                        }
                        else
                        {
                            pluginManager.SetPropertyValue("FuelDeltaLL", this.GetType(), fuelDelta);
                            pluginManager.SetPropertyValue("FuelPitWindowFirstLL", this.GetType(), earliestLap);
                            pluginManager.SetPropertyValue("FuelPitWindowLastLL", this.GetType(), latestPitLap);
                            pluginManager.SetPropertyValue("FuelMinimumFuelFillLL", this.GetType(), minFuelPush);
                            pluginManager.SetPropertyValue("FuelMaximumFuelFillLL", this.GetType(), maxFuelPush);
                            pluginManager.SetPropertyValue("FuelPitStopsLL", this.GetType(), pitStops);
                            pluginManager.SetPropertyValue("FuelConserveToSaveAStopLL", this.GetType(), conserveToNotPit);
                            pluginManager.SetPropertyValue("FuelSaveDeltaValue", this.GetType(), saveDelta);


                            //Fuel target calculations

                            fuelTargetDelta = fuelPerLap - Settings.fuelPerLapTarget;
                            if (fuelPerLap == 0)
                            {
                                fuelTargetDelta = 0;
                            }
                            pluginManager.SetPropertyValue("FuelPerLapTargetLastLapDelta", this.GetType(), fuelTargetDelta);
                            pluginManager.SetPropertyValue("FuelTargetDeltaCumulative", this.GetType(), fuelTargetDeltaCumulative);
                        }
                    }

                }

                //----------------------------------------
                //---------Stint timer/lap counter--------
                //----------------------------------------

                if (counter == 7 || counter == 22 || counter == 37 || counter == 52)
                {
                    //Several conditions where stint timer will reset
                    if (iRIdle || pitBox > 0 || (session == "Race" && sessionState < 4) || (session == "Offline Testing" && pit == 1) || pushTimer.TotalHours > 10)
                    {
                        stintTimer = globalClock;
                    }

                    pushTimer = TimeSpan.FromMilliseconds(globalClock.TotalMilliseconds - stintTimer.TotalMilliseconds);

                    pluginManager.SetPropertyValue("StintTimer", this.GetType(), pushTimer);

                    int stintLaps = validStintLaps + invalidStintLaps + 1;

                    if (stintLapsCheck)
                    {
                        stintLaps = stintLaps - 1;
                    }
                    pluginManager.SetPropertyValue("StintCurrentHotlap", this.GetType(), stintLaps);
                }

                //--------------------------------------------------
                //----Minimum Corner Speed and Straight Line Speed--
                //--------------------------------------------------
                if (pit == 1)
                {
                    pluginManager.SetPropertyValue("StraightLineSpeed", this.GetType(), 0);
                    pluginManager.SetPropertyValue("MinimumCornerSpeed", this.GetType(), minimumCornerSpeed);
                }
                else
                {
                    if (throttle > highestThrottle && !throttleLift) //recording highest throttle value
                    {
                        highestThrottle = throttle;
                    }

                    if (!throttleLift && highestThrottle > 0.995) //Updating straight line speed when throttle is on
                    {
                        if (straightLineSpeed < speed)
                        {
                            straightLineSpeed = speed;
                        }
                    }

                    if (throttle < (0.98 * highestThrottle) && !throttleLift) //detecting throttle lift
                    {
                        throttleLift = true;
                        minimumCornerSpeed = speed;
                    }
                    if (throttleLift && throttle < 30 && straightLineSpeed > 50) //posting straight line speed
                    {
                        pluginManager.SetPropertyValue("StraightLineSpeed", this.GetType(), straightLineSpeed);
                    }

                    if (throttleLift) //Throttle has been lifted, start recording throttle again, if speed is lower than recorded MCS, pick it up.
                    {
                        highestThrottle = throttle;
                        if (minimumCornerSpeed == 0 || minimumCornerSpeed > speed)
                        {
                            minimumCornerSpeed = speed;
                        }
                    }

                    if (throttleLift && speed > (1.1 * minimumCornerSpeed)) //Detecting increase in speed, resetting straight line value. 
                    {
                        throttleLift = false;
                        straightLineSpeed = 0;
                    }

                    pluginManager.SetPropertyValue("MinimumCornerSpeed", this.GetType(), minimumCornerSpeed);
                }

                //--------------------------------------------------
                //--------------------BRAKE CURVE-------------------
                //--------------------------------------------------


                if (brake > 0 && !brakeTrigger && brakeClock == 0)
                {
                    brakeTriggerCheck = true;
                }

                if (brakeTrigger)
                {
                    brakeClock++;
                    if (brakeMax < brake)
                    {
                        brakeMax = brake;
                    }
                }

                if (brakeTriggerCheck)
                {
                    brakeTrigger = true;
                    brakeTriggerCheck = false;
                    brakeCurve.Add(Math.Round(brake, 1));
                    brakeClock++;
                }

                if (brakeTrigger && brakeClock > (brakeClockBase + 5) && brakeCurve.Count < 41)
                {
                    brakeCurve.Add(Math.Round(brake, 1));
                    brakeClockBase = brakeClock;
                }

                if (brakeCurve.Count == 40 && brakeTrigger)
                {
                    brakeTrigger = false;
                    string result = string.Join(",", brakeCurve); //push result as string
                    pluginManager.SetPropertyValue("BrakeCurveValues", this.GetType(), result);

                    pluginManager.SetPropertyValue("BrakeCurvePeak", this.GetType(), brakeMax);

                    double auc = 0;
                    for (int i = 0; i < 40; i++)
                    {
                        auc = auc + (0.1 * brakeCurve[i]);
                    }
                    pluginManager.SetPropertyValue("BrakeCurveAUC", this.GetType(), Math.Round(auc, 1));
                }
                if (brakeCurve.Count == 40 && brake == 0)
                {
                    brakeCurve.Clear();
                    brakeClock = 0;
                    brakeClockBase = 0;
                    brakeMax = 0;
                }

                //--------------------------------------------------
                //--------------------THROTTLE CURVE-------------------
                //--------------------------------------------------

                if (throttle > 0 && throttle < 10 && !throttleTrigger && throttleClock == 0)
                {
                    throttleTriggerCheck = true;
                }

                if (throttleTrigger)
                {
                    throttleClock++;
                }

                if (throttleTriggerCheck)
                {
                    throttleTrigger = true;
                    throttleTriggerCheck = false;
                    throttleCurve.Add(Math.Round(throttle, 1));
                    throttleClock++;
                }

                if (throttleTrigger && throttleClock > (throttleClockBase + 3) && throttleCurve.Count < 41)
                {
                    throttleCurve.Add(Math.Round(throttle, 1));
                    throttleClockBase = throttleClock;
                }

                if (throttleCurve.Count == 40 && throttleTrigger)
                {
                    throttleTrigger = false;
                    string result = string.Join(",", throttleCurve); //push result as string
                    pluginManager.SetPropertyValue("ThrottleCurveValues", this.GetType(), result);

                    double agro = 0;
                    for (int i = 0; i < 40; i++)
                    {
                        if (throttleCurve[i] != 100)
                        {
                            agro++;
                        }
                        else if (throttleCurve[i] == 100)
                        {
                            break;
                        }
                    }
                    if (agro == 40)
                    {
                        agro = 0;
                    }
                    pluginManager.SetPropertyValue("ThrottleAgro", this.GetType(), Math.Round((0.06666666667 * agro), 2));
                }
                if (throttleCurve.Count == 40 && throttle == 0)
                {
                    throttleCurve.Clear();
                    throttleClock = 0;
                    throttleClockBase = 0;
                }



                //--------------------------------------------------
                //-------PIT STOP EXIT POSITION CALCULATIONS--------
                //--------------------------------------------------

                if (sessionBestLap.TotalSeconds > 0 && counter == 27 && session == "Race" && pitLimiter != 1)
                {

                    pitStopOpponents.Clear();
                    finalList.Clear();

                    for (int i = 0; i < opponents; i++) //Add all opponents as objects with gap to player and name
                    {
                        if (!data.NewData.Opponents[i].IsPlayer)
                        {

                            double? gap = data.NewData.Opponents[i].GaptoPlayer;
                            double? realgap = 0;
                            bool usingRealGap = true;
                            for (int u = 0; u < irData.SessionData.DriverInfo.CompetingDrivers.Length; u++)
                            {
                                if (irData.SessionData.DriverInfo.CompetingDrivers[u].UserName == data.NewData.Opponents[i].Name)
                                {
                                    realgap = realGapOpponentDelta[Convert.ToInt32(irData.SessionData.DriverInfo.CompetingDrivers[u].CarIdx)];
                                    break;
                                }
                            }

                            if ((gap > realgap * 1.25 && gap - realgap > 10) || (gap < realgap * 0.75 && gap - realgap < -10) || realgap == 0)
                            {
                                realgap = gap;
                                usingRealGap = false;
                                if (realgap == null)
                                {
                                    realgap = 0;
                                }
                            }


                            pitStopOpponents.Add(new pitOpponents(realgap, data.NewData.Opponents[i].Name, data.NewData.Opponents[i].TrackPositionPercent, data.NewData.Opponents[i].CarClass == myClass, 0, 0, data.NewData.Opponents[i].BestLapTime, false, false, usingRealGap));
                        }
                    }

                    pitStopOpponents = pitStopOpponents.OrderBy(entry => entry.Gap).ToList(); //Sort by gap to find position
                    pitStopOpponents.Sort((x, y) => y.IsSameClass.CompareTo(x.IsSameClass));

                    for (int i = 0; i < pitStopOpponents.Count; i++)
                    {
                        double bestLap = pitStopOpponents[i].BestLap.TotalSeconds; //Calculations for gap using player best lap (or my best lap if none)
                        if (pitStopOpponents[i].BestLap.TotalSeconds < sessionBestLap.TotalSeconds)
                        {
                            pitStopOpponents[i].IsFaster = true;
                        }
                        if (bestLap == 0)
                        {
                            bestLap = sessionBestLap.TotalSeconds;
                            pitStopOpponents[i].IsFaster = false;
                        }

                        double correctingFactor = 1;
                        if (!pitStopOpponents[i].UsedRealGap)
                        {
                            correctingFactor = bestLap / sessionBestLap.TotalSeconds;
                        }

                        if ((pitStopOpponents[i].Gap * correctingFactor) - pitStopDuration < 0) //Will this car actually be ahead in race after pit stop?
                        {
                            pitStopOpponents[i].IsAhead = true;
                        }

                        if (pitStopOpponents[i].Gap - pitStopDuration < 10 && pitStopOpponents[i].Gap - pitStopDuration > -10) //If no calculations are needed to adjust for extra laps, just use the gap, as it is likely to be a real meassured gap, and therefore quite accurate
                        {
                            pitStopOpponents[i].Gap = pitStopOpponents[i].Gap - pitStopDuration;
                        }

                        else
                        {
                            double? posOnExit = pitStopOpponents[i].TrackPosition + (pitStopDuration / bestLap); //Updating the gap to show relative gap on exit
                            posOnExit = posOnExit % 1;
                            pitStopOpponents[i].Gap = -1 * ((posOnExit - trackPosition) % 1) * sessionBestLap.TotalSeconds;
                        }

                        if (pitStopOpponents[i].IsSameClass) //Give position to same class cars
                        {
                            pitStopOpponents[i].Position = i + 1;
                        }

                        if (pitStopOpponents[i].IsSameClass && !pitStopOpponents[i].IsAhead)
                        {
                            pitStopOpponents[i].Position++;
                        }
                    }

                    //Futher include only cars in the +/1 10 seconds range

                    int posAfterPit = 1;

                    for (int i = 0; i < pitStopOpponents.Count; i++)
                    {
                        if (pitStopOpponents[i].Gap > -10 && pitStopOpponents[i].Gap < 10)
                        {
                            finalList.Add(pitStopOpponents[i]);
                        }

                        if (pitStopOpponents[i].IsAhead && pitStopOpponents[i].IsSameClass) //Find my position when exiting the pits
                        {
                            posAfterPit++;
                        }
                    }

                    pluginManager.SetPropertyValue("PitExitPosition", this.GetType(), posAfterPit);

                    for (int i = 0; i < finalList.Count && i < 14; i++) //Edit name and find class difference of relevant cars
                    {

                        for (int u = 0; u < irData.SessionData.DriverInfo.CompetingDrivers.Length; u++)
                        {
                            if (irData.SessionData.DriverInfo.CompetingDrivers[u].UserName == finalList[i].Name)
                            {
                                finalList[i].ClassDifference = classColors.IndexOf(irData.SessionData.DriverInfo.CompetingDrivers[u].CarClassColor) - myClassColorIndex;
                                break;
                            }
                        }

                        int letterCount = finalList[i].Name.Split(' ').Last().Length;
                        if (letterCount > 3)
                        {
                            letterCount = 3;
                        }

                        finalList[i].Name = finalList[i].Name.Split(' ').Last().Substring(0, letterCount).ToUpper();

                        pluginManager.SetPropertyValue("PitExitCar" + (i + 1) + "Name", this.GetType(), finalList[i].Name);
                        pluginManager.SetPropertyValue("PitExitCar" + (i + 1) + "Gap", this.GetType(), finalList[i].Gap);
                        pluginManager.SetPropertyValue("PitExitCar" + (i + 1) + "Position", this.GetType(), finalList[i].Position);
                        pluginManager.SetPropertyValue("PitExitCar" + (i + 1) + "ClassDifference", this.GetType(), finalList[i].ClassDifference);
                        pluginManager.SetPropertyValue("PitExitCar" + (i + 1) + "IsAhead", this.GetType(), finalList[i].IsAhead);
                        pluginManager.SetPropertyValue("PitExitCar" + (i + 1) + "IsFaster", this.GetType(), finalList[i].IsFaster);

                    }
                    for (int i = finalList.Count; i < 14; i++)
                    {
                        pluginManager.SetPropertyValue("PitExitCar" + (i + 1) + "Name", this.GetType(), "");
                        pluginManager.SetPropertyValue("PitExitCar" + (i + 1) + "Gap", this.GetType(), 0);
                        pluginManager.SetPropertyValue("PitExitCar" + (i + 1) + "Position", this.GetType(), 0);
                        pluginManager.SetPropertyValue("PitExitCar" + (i + 1) + "ClassDifference", this.GetType(), 0);
                        pluginManager.SetPropertyValue("PitExitCar" + (i + 1) + "IsAhead", this.GetType(), false);
                        pluginManager.SetPropertyValue("PitExitCar" + (i + 1) + "IsFaster", this.GetType(), false);
                    }

                }

                //-----------------------------------------------------------------------------
                //----------------------PIT STOP DURATION--------------------------------------
                //-----------------------------------------------------------------------------

                if (counter == 10 || counter == 25 || counter == 40 || counter == 55)
                {

                    double throughTime = pitStopBase + pitStopBase * ((pitMaxSpeed - 1) * pitStopMaxSpeed + (pitCornerSpeed - 1) * pitStopCornerSpeed - (pitAcceleration - 1) * pitStopAcceleration + (pitBrakeDistance - 1) * pitStopBrakeDistance);

                    double tireTime = 0;

                    //establish toggle bools, front/rear/left/right/all

                    bool pitToggleFront = LFTog && RFTog;
                    bool pitToggleRear = LRTog && RRTog;
                    bool pitToggleLeft = LFTog && LRTog;
                    bool pitToggleRight = RFTog && RRTog;

                    int totalTireNumber = Convert.ToInt16(LFTog) + Convert.ToInt16(RFTog) + Convert.ToInt16(LRTog) + Convert.ToInt16(RRTog);

                    if (carHasAnimatedCrew && trackHasAnimatedCrew)
                    {
                        pitBaseTime = pitAniBaseTime;
                        pitSlowAdd = pitAniSlowAdd;
                    }

                    tireTime = 0;
                    if (totalTireNumber > 0)
                    {

                        if (pitCrewType == CrewType.SingleTyre)
                        {
                            if (totalTireNumber == 4)
                            {
                                tireTime = pitBaseTime * 4 + pitSlowAdd * 2;
                            }
                            else if (totalTireNumber == 3)
                            {
                                tireTime = pitBaseTime * 3 + pitSlowAdd;
                            }
                            else if (totalTireNumber == 2 && ((pitFastSide == "Left" && (RFTog || RRTog)) || (pitFastSide == "Right" && (LFTog || LRTog))))
                            {
                                tireTime = pitBaseTime * 2 + (2 * pitSlowAdd / 3);
                            }
                            else if (totalTireNumber == 2)
                            {
                                tireTime = pitBaseTime * 2 - (pitSlowAdd / 3);
                            }
                            else if (totalTireNumber == 1 && ((pitFastSide == "Left" && (RFTog || RRTog)) || (pitFastSide == "Right" && (LFTog || LRTog))))
                            {
                                tireTime = pitBaseTime + pitSlowAdd;
                            }
                            else
                            {
                                tireTime = pitBaseTime;
                            }
                        }

                        else if (pitCrewType == CrewType.FrontRear)
                        {
                            if (totalTireNumber == 4 || (totalTireNumber == 3 && ((pitFastSide == "Left" && ((pitToggleFront && LRTog) || (pitToggleRear && LFTog))) || (pitFastSide == "Right" && ((pitToggleFront && RRTog) || (pitToggleRear && RFTog)))))
                               || (totalTireNumber == 2 && ((pitFastSide == "Left" && pitToggleRight) || (pitFastSide == "Right" && pitToggleLeft))))
                            {
                                tireTime = pitBaseTime * 2 + (2 * pitSlowAdd / 3);
                            }
                            else if (totalTireNumber == 3 || totalTireNumber == 2 && (pitToggleRight || pitToggleLeft))
                            {
                                tireTime = pitBaseTime * 2 - (pitSlowAdd / 3);
                            }
                            else if (totalTireNumber == 2 || totalTireNumber == 1 && ((pitFastSide == "Left" && (RFTog || RRTog)) || (pitFastSide == "Right" && (LFTog || LRTog))))
                            {
                                tireTime = pitBaseTime + pitSlowAdd;
                            }
                            else
                            {
                                tireTime = pitBaseTime;
                            }
                        }

                        else if (pitCrewType == CrewType.LeftRight)
                        {
                            if (totalTireNumber == 4 || totalTireNumber == 3 || (totalTireNumber == 2 && !pitToggleLeft && !pitToggleRight))
                            {
                                tireTime = pitBaseTime * 2 + (2 * pitSlowAdd / 3);
                            }
                            else if ((pitFastSide == "Left" && (pitToggleRight || RFTog || RRTog)) || (pitFastSide == "Right" && (pitToggleLeft || LFTog || LRTog)))
                            {
                                tireTime = pitBaseTime + pitSlowAdd;
                            }
                            else if (totalTireNumber == 2 || totalTireNumber == 1 && ((pitFastSide == "Left" && (RFTog || RRTog)) || (pitFastSide == "Right" && (LFTog || LRTog))))
                            {
                                tireTime = pitBaseTime + pitSlowAdd;
                            }
                            else
                            {
                                tireTime = pitBaseTime;
                            }
                        }
                        else
                        {
                            tireTime = pitBaseTime;
                        }

                    }

                    if (plannedFuel + fuel > maxFuel)
                    {
                        plannedFuel = maxFuel - fuel;
                    }

                    double fuelTime = 1 + (plannedFuel / pitFuelFillRate) + 0.2;

                    if (!fuelTog)
                    {
                        fuelTime = 0;
                    }

                    if (!fuelTargetCheck)
                    {
                        fuelTargetCheck = true;
                        oldFuelValue = fuel;
                    }

                    if (oldFuelValue >= fuel || !fuelTog || oldFuelValue == 0)
                    {
                        oldFuelValue = fuel;
                    }

                    double fuelTarget = oldFuelValue;
                    if (fuelTog)
                    {
                        fuelTarget = fuelTarget + plannedFuel;
                    }

                    double WStimer = 0;
                    if (pitHasWindscreen && WSTog)
                    {
                        WStimer = 2.5;
                    }

                    double frontWingTime = 0;
                    double rearWingTime = 0;
                    double tapeTime = 0;
                    double powersteerTime = 0;

                    //Front wing
                    double currentFrontWingC = Math.Abs(currentFrontWing - wingFront);
                    if (carId == "Dallara IR18")
                    {
                        currentFrontWingC = currentFrontWingC * 10;
                    }

                    if (currentFrontWingC > 0)
                    {
                        frontWingTime = 2.6 + (currentFrontWingC - 1) * 0.2;
                    }

                    //Rear wing
                    double currentRearWingC = Math.Abs(currentRearWing - wingRear);
                    if (carId == "Dallara IR18")
                    {
                        currentRearWingC = currentRearWingC * 10;
                    }

                    if (currentRearWingC > 0)
                    {
                        rearWingTime = 2.6 + (currentRearWingC - 1) * 0.2;
                    }

                    //PWS
                    int currentPWSC = Math.Abs(currentPWS - PWS);

                    if (currentPWSC > 0)
                    {
                        powersteerTime = 4 + (currentPWSC - 1) * 2;
                    }

                    //Tape
                    int currentTapeC = Math.Abs(currentTape - tape);

                    if (currentTapeC > 0)
                    {
                        tapeTime = 1.2 + (currentTapeC - 1) * 0.2;
                    }

                    double adjustmentTime = Math.Max(Math.Max(Math.Max(frontWingTime, rearWingTime), powersteerTime), tapeTime);

                    double pitTime = Math.Max(Math.Max(Math.Max(tireTime, fuelTime), WStimer), adjustmentTime);
                    if (!pitMultitask)
                    {
                        pitTime = fuelTime + tireTime;
                        if (WStimer > fuelTime + tireTime)
                        {
                            pitTime = WStimer;
                        }
                        if ((adjustmentTime > fuelTime + tireTime) && adjustmentTime > WStimer)
                        {
                            pitTime = adjustmentTime;
                        }

                        if (fuelTog && totalTireNumber > 0)
                        {
                            pitTime = pitTime - pitSlowAdd;
                        }
                    }

                    pitStopDuration = pitTime + throughTime;

                    if (pitStopDuration == throughTime)
                    {
                        onlyThrough = true;
                    }
                    else
                    {
                        onlyThrough = false;
                    }

                    if (pitStall != 1)
                    {
                        pluginManager.SetPropertyValue("PitServiceFuelTarget", this.GetType(), fuelTarget);
                    }

                    pluginManager.SetPropertyValue("PitTimeTires", this.GetType(), tireTime);
                    pluginManager.SetPropertyValue("PitTimeFuel", this.GetType(), fuelTime);
                    pluginManager.SetPropertyValue("PitTimeWindscreen", this.GetType(), WStimer);
                    pluginManager.SetPropertyValue("PitTimeAdjustment", this.GetType(), adjustmentTime);
                    pluginManager.SetPropertyValue("PitTimeDriveThrough", this.GetType(), throughTime);
                    pluginManager.SetPropertyValue("PitTimeService", this.GetType(), pitTime);
                    pluginManager.SetPropertyValue("PitTimeTotal", this.GetType(), pitStopDuration);

                    pluginManager.SetPropertyValue("PitCrewType", this.GetType(), (int)pitCrewType);

                }

                //-----------------------------------------------------------------------------
                //----------------------LAP DELTA TIMING---------------------------------------
                //-----------------------------------------------------------------------------


                int myDeltaIndex = ((int)((trackPosition * lapDeltaSections) * 100)) / 100;

                if (myDeltaIndex >= lapDeltaSections)
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

                    bool passCheck = (pit == 0 && (myDeltaIndex > 5 || (myDeltaIndex > 3 && lapDeltaLast[myDeltaIndex + 1] < 10000 && lapDeltaCurrent[myDeltaIndex + 1] < 10000)));

                    //Setting last lap delta
                    if (passCheck && lapDeltaLast[myDeltaIndex + 1] > 0 && lapDeltaCurrent[myDeltaIndex + 1] > 0)
                    {
                        deltaLastLap = (lapDeltaCurrent[myDeltaIndex + 1] - lapDeltaLast[myDeltaIndex + 1]) / 1000;
                        lapDeltaLastChange[myDeltaIndex] = deltaLastLap;
                        pluginManager.SetPropertyValue("DeltaLastLap", this.GetType(), deltaLastLap);
                    }

                    if (lapDeltaLast[myDeltaIndex + 1] == -1)
                    {
                        pluginManager.SetPropertyValue("DeltaLastLap", this.GetType(), 0);
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
                        pluginManager.SetPropertyValue("DeltaSessionBest", this.GetType(), deltaSessionBest);
                    }
                    if (lapDeltaSessionBest[myDeltaIndex + 1] == -1)
                    {
                        pluginManager.SetPropertyValue("DeltaSessionBest", this.GetType(), 0);
                    }


                    //Setting lap record delta
                    bool recordCheck = (pit == 0 && (myDeltaIndex > 5 || (myDeltaIndex > 3 && lapDeltaRecord[myDeltaIndex + 1] < 10000 && lapDeltaCurrent[myDeltaIndex + 1] < 10000)));
                    if (recordCheck && lapDeltaRecord[myDeltaIndex + 1] > 0 && lapDeltaCurrent[myDeltaIndex + 1] > 0)
                    {
                        deltaLapRecord = (lapDeltaCurrent[myDeltaIndex + 1] - lapDeltaRecord[myDeltaIndex + 1]) / 1000;
                        lapDeltaLapRecordChange[myDeltaIndex] = deltaLapRecord;
                        pluginManager.SetPropertyValue("DeltaLapRecord", this.GetType(), deltaLapRecord);
                    }
                    if (lapDeltaRecord[myDeltaIndex + 1] == -1)
                    {
                        pluginManager.SetPropertyValue("DeltaLapRecord", this.GetType(), 0);
                    }


                    if (myDeltaIndex == 0) //last section, copy to last lap. Further copy to session/ATB on lap changes. (from last lap)
                    {
                        for (int i = 0; i < lapDeltaSections + 1; i++)
                        {
                            lapDeltaLast[i] = lapDeltaCurrent[i];
                            lapDeltaCurrent[i] = -1;
                        }
                    }
                }

                int chunkSize = lapDeltaSections / deltaChangeChunks;
                int currentChunk = myDeltaIndex / chunkSize;
                bool changeStarted = false;
                double changeSum = 0;
                double firstOfChunk = 0;
                double lastOfChunk = 0;

                if (lapDeltaLast[myDeltaIndex+1] > 0)
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



                pluginManager.SetPropertyValue("DeltaLastLapChange", this.GetType(), lastResult);
                pluginManager.SetPropertyValue("DeltaSessionBestChange", this.GetType(), SBResult);
                pluginManager.SetPropertyValue("DeltaLapRecordChange", this.GetType(), LRResult);

                //-----------------------------------------------------------------------------
                //----------------------REAL GAPS----------------------------------------------
                //-----------------------------------------------------------------------------

                int myLap = irData.Telemetry.CarIdxLap[myCarIdx]; //My lap count
                double myLoc = irData.Telemetry.CarIdxLapDistPct[myCarIdx]; //My current track position
                int myDistIndex = ((int)((myLoc * trackSections) * 100)) / 100; //Distance index, dividing track position into sections
                if (myDistIndex >= trackSections)
                {
                    myDistIndex = trackSections - 1;
                }
                int myPrevIndex = myDistIndex - 1;
                if (myPrevIndex == -1)
                {
                    myPrevIndex = trackSections - 1;
                }
                if (myDistIndex < 0)
                {
                    myDistIndex = 0;
                    myPrevIndex = 0;
                }

                if (sessionState == 4 && BestLapTimes!=null)
                {
                    for (int i = 0; i < 64; i++)
                    {
                        if ((int)irData.Telemetry.CarIdxTrackSurface[i] != -1 && irData.Telemetry.CarIdxLap[i] > 0) //Checking if this CarID is in world
                        {
                            int distIndex = ((int)((irData.Telemetry.CarIdxLapDistPct[i] * trackSections) * 100)) / 100;
                            if (distIndex >= trackSections)
                            {
                                distIndex = trackSections - 1;
                            }
                            int distPrevIndex = distIndex - 1;
                            if (distPrevIndex == -1)
                            {
                                distPrevIndex = trackSections - 1;
                            }

                            if (distIndex < 0)
                            {
                                distIndex = 0;
                                distPrevIndex = 0;
                            }

                            int lap = irData.Telemetry.CarIdxLap[i];

                            float bestLapRaw = ((float[])BestLapTimes)[i];
                            double bestLap = Convert.ToDouble(bestLapRaw);
                            if (bestLap < 1)
                            {
                                bestLap = myExpectedLapTime;
                            }
                            double posdiff = myLoc - irData.Telemetry.CarIdxLapDistPct[i];
                            double lapdiff = myLap - lap + posdiff;
                            int truncdiff = ((int)((lapdiff) * 100)) / 100;

                            //Checking if car is in front. 
                            if ((posdiff < 0 && posdiff > -0.5) || posdiff > 0.5)
                            {
                                if (!realGapLocks[distIndex][i] && !realGapChecks[distIndex][i])  //Car arriving at unchecked, closed gate. Opening, snapshotting global clock, setting check. Unchecking previous gate. 
                                {
                                    realGapPoints[distIndex][i] = globalClock;
                                    realGapLocks[distIndex][i] = true;
                                    realGapChecks[distIndex][i] = true;
                                    realGapChecks[distPrevIndex][i] = false;
                                }

                                if (realGapLocks[myDistIndex][i]) //If I just arrived at an open gate , close it and post delta.
                                {
                                    double delta = realGapPoints[myDistIndex][i].TotalSeconds - globalClock.TotalSeconds;

                                    realGapOpponentRelative[i] = delta;

                                    if (lapdiff < -1) 
                                    {
                                        delta = delta - truncdiff * myExpectedLapTime;
                                    }
                                    else if (lapdiff > 0)
                                    {
                                        delta = bestLap + delta + truncdiff*bestLap;
                                    }

                                    realGapOpponentDelta[i] = delta;
                                    realGapLocks[myDistIndex][i] = false;
                                }

                            }

                            else//Assume the car is behind
                            {
                                if (!realGapLocks[myDistIndex][i] && !realGapChecks[myDistIndex][i]) //If I just arrived at a unchecked, closed gate, open and snapshot global clock, checking. Unchecking previous gate.
                                {
                                    realGapPoints[myDistIndex][i] = globalClock;
                                    realGapLocks[myDistIndex][i] = true;
                                    realGapChecks[myDistIndex][i] = true;
                                    realGapChecks[myPrevIndex][i] = false;
                                }
                                
                                //Calculating the total race distance to this car


                                if (realGapLocks[distIndex][i]) //If car just arrived at an open gate, close it and post delta. 
                                {
                                    double delta = globalClock.TotalSeconds - realGapPoints[distIndex][i].TotalSeconds;

                                    realGapOpponentRelative[i] = delta;

                                    if (lapdiff < 0)
                                    {
                                        delta = delta - myExpectedLapTime - truncdiff * myExpectedLapTime;
                                    }
                                    else if (lapdiff > 1)
                                    {
                                        delta = delta + truncdiff * bestLap;
                                    }

                                    realGapOpponentDelta[i] = delta;
                                    realGapLocks[distIndex][i] = false;
                                    

                                }

                            }
                        }
                    }
                }

                
                

                //-----------------------------------------------------------------------------
                //----------------------IDLE AND RESETS----------------------------------------
                //-----------------------------------------------------------------------------

                //Stuf that happens when idle
                if (iRIdle)
                {
                    findLapRecord = true;
                    csvIndex = 0;
                    currentFrontWing = 0;
                    currentRearWing = 0;
                    currentPWS = 0;
                    currentTape = 0;
                    pitBox = 0.5;
                    hasPitted = false;
                    validStintLaps = 0;
                    invalidStintLaps = 0;
                    fuelTargetDeltaCumulative = 0;
                    raceFinished = false;
                    jokerThisLap = false;
                    jokerLapChecker = false;
                    finishedCars = new List<string> { };
                    fuelTargetCheck = false;
                    oldFuelValue = 0;
                    NBactive = false;
                    NBvalue = false;
                    NBspeedLim = false;
                    ERSlapCounter = currentLap;
                    ERSstartingLap = true;
                    TCon = false;
                    TCduration = 0;
                    offTrack = false;
                    commandMinFuel = 0;
                    commandMaxFuel = 500;
                    LEDwarningActive = false;
                    fuelSaveDelta = 0;
                    tcBump = false;
                    tcBumpCounter = 0;

                    //Props that need refresh
                    pluginManager.SetPropertyValue("TCActive", this.GetType(), false);

                    //Session or car or track change
                    if (carModelHolder != carModel || trackHolder != track || sessionHolder != session)
                    {
                        findLapRecord = true;
                        csvIndex = 0;
                        IRchange = 0;
                        ERSChangeCount = 4;
                        savePitTimerLock = false;
                        savePitTimerSnap = new TimeSpan(0);
                        slowestLapTimeSpanCopy = new TimeSpan(0);
                        lapTimeList = new List<TimeSpan> { listFiller, listFiller, listFiller, listFiller, listFiller, listFiller, listFiller, listFiller }; //Reset lap and status lists
                        lapStatusList = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 };
                        sector1TimeList = new List<double> { 0, 0, 0, 0, 0, 0, 0, 0 };
                        sector2TimeList = new List<double> { 0, 0, 0, 0, 0, 0, 0, 0 };
                        sector3TimeList = new List<double> { 0, 0, 0, 0, 0, 0, 0, 0 };
                        sector1StatusList = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 };
                        sector2StatusList = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 };
                        sector3StatusList = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 };
                        fuelTargetDeltas = new List<double> { 0, 0, 0, 0, 0, 0, 0, 0 };
                        sessionBestSector1 = 0;
                        sessionBestSector2 = 0;
                        sessionBestSector3 = 0;
                        sessionBestLap = new TimeSpan(0);
                        qLap1Status = 0;
                        qLap2Status = 0;
                        qLap1Time = new TimeSpan(0);
                        qLap2Time = new TimeSpan(0);
                        lapRaceFinished = false;
                        timeRaceFinished = false;
                        timedOut = false;
                        leaderDecimal = 0;
                        isRaceLeader = false;
                        isLapLimited = false;
                        isTimeLimited = false;
                        jokerLapCount = 0;
                        pluginManager.SetPropertyValue("P1Finished", this.GetType(), false);
                        minFuelPush = 0;
                        maxFuelPush = 0;
                        qLapStarted2 = false;

                        //Props that need refresh
                        pluginManager.SetPropertyValue("TCActive", this.GetType(), false);

                        //Resetting relGap list
                        if (counter == 59)
                        {
                            realGapLocks.Clear();
                            realGapChecks.Clear();
                            realGapPoints.Clear();
                            realGapOpponentDelta.Clear();
                            realGapOpponentRelative.Clear();
                            sessionCarsLapsSincePit.Clear();
                            sessionCarsLap.Clear();

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

                            for (int u = 0; u < trackSections; u++)
                            {
                                List<bool> locks = new List<bool> { };
                                List<bool> checks = new List<bool> { };
                                List<TimeSpan> points = new List<TimeSpan> { };

                                for (int i = 0; i < 64; i++)
                                {
                                    locks.Add(false);
                                    checks.Add(false);
                                    points.Add(TimeSpan.FromSeconds(0));
                                }

                                realGapLocks.Add(locks);
                                realGapChecks.Add(checks);
                                realGapPoints.Add(points);
                            }

                            for (int i = 0; i < 64; i++)
                            {
                                realGapOpponentDelta.Add(0);
                                realGapOpponentRelative.Add(0);
                                sessionCarsLapsSincePit.Add(-1);
                                sessionCarsLap.Add(-1);
                            }

                            for (int i = 0; i < lapDeltaSections + 1; i++)
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
                }

                //Stuf that happens when not idle

                if (!iRIdle)
                {
                    carModelHolder = carModel; //Updating choice of car, track and session
                    trackHolder = track;
                    sessionHolder = session;
                }

                //-----------------------------------------------------------------------------
                //----------------------SETTING GLOBAL PROPERTY VALUES-------------------------
                //-----------------------------------------------------------------------------

                pluginManager.SetPropertyValue("TestProperty", this.GetType(), TCreleaseCD != 0);
                pluginManager.SetPropertyValue("Idle", this.GetType(), iRIdle);
                pluginManager.SetPropertyValue("SmoothGear", this.GetType(), smoothGear);
                pluginManager.SetPropertyValue("TrackEntry", this.GetType(), offTrack);
                pluginManager.SetPropertyValue("FuelSaveDelta", this.GetType(), fuelSaveDelta);
                pluginManager.SetPropertyValue("LEDWarnings", this.GetType(), LEDwarningActive);
                pluginManager.SetPropertyValue("SpotterMode", this.GetType(), spotMode);
                pluginManager.SetPropertyValue("PitSavePaceLock", this.GetType(), savePitTimerLock);
                pluginManager.SetPropertyValue("OvertakeMode", this.GetType(), overtakeMode);
                pluginManager.AddProperty("FuelPerLapOffset", this.GetType(), Math.Round(fuelPerLapOffset,2));

                pluginManager.SetPropertyValue("LapStatus", this.GetType(), lapStatus);
                pluginManager.SetPropertyValue("StintValidLaps", this.GetType(), validStintLaps);
                pluginManager.SetPropertyValue("StintInvalidLaps", this.GetType(), invalidStintLaps);
                pluginManager.SetPropertyValue("SessionBestLap", this.GetType(), sessionBestLap);
                pluginManager.SetPropertyValue("LapRecord", this.GetType(), lapRecord);
                pluginManager.SetPropertyValue("SessionBestSector1", this.GetType(), TimeSpan.FromSeconds(sessionBestSector1));
                pluginManager.SetPropertyValue("SessionBestSector2", this.GetType(), TimeSpan.FromSeconds(sessionBestSector2));
                pluginManager.SetPropertyValue("SessionBestSector3", this.GetType(), TimeSpan.FromSeconds(sessionBestSector3));

                if (sessionBestSector1 > 0 && sessionBestSector2 > 0 && sessionBestSector3 > 0)
                {
                    pluginManager.SetPropertyValue("OptimalLapTime", this.GetType(), TimeSpan.FromSeconds(sessionBestSector1 + sessionBestSector2 + sessionBestSector3));
                }
                else
                {
                    pluginManager.SetPropertyValue("OptimalLapTime", this.GetType(), new TimeSpan(0));
                }

                pluginManager.SetPropertyValue("JokerThisLap", this.GetType(), jokerThisLap);
                pluginManager.SetPropertyValue("JokerCount", this.GetType(), jokerLapCount);

                pluginManager.SetPropertyValue("PitBoxPosition", this.GetType(), pitBox);
                pluginManager.SetPropertyValue("PitBoxApproach", this.GetType(), boxApproach);

                pluginManager.SetPropertyValue("PitToggleLF", this.GetType(), LFTog);
                pluginManager.SetPropertyValue("PitToggleRF", this.GetType(), RFTog);
                pluginManager.SetPropertyValue("PitToggleLR", this.GetType(), LRTog);
                pluginManager.SetPropertyValue("PitToggleRR", this.GetType(), RRTog);
                pluginManager.SetPropertyValue("PitToggleFuel", this.GetType(), fuelTog);
                pluginManager.SetPropertyValue("PitToggleWindscreen", this.GetType(), WSTog);
                pluginManager.SetPropertyValue("PitToggleRepair", this.GetType(), repairTog);

                pluginManager.SetPropertyValue("PitServiceLFPSet", this.GetType(), plannedLFPressure);
                pluginManager.SetPropertyValue("PitServiceRFPSet", this.GetType(), plannedRFPressure);
                pluginManager.SetPropertyValue("PitServiceLRPSet", this.GetType(), plannedLRPressure);
                pluginManager.SetPropertyValue("PitServiceRRPSet", this.GetType(), plannedRRPressure);

                if (counter == 6 || counter == 36) //General lap times refreshed only twice per second
                {


                    pluginManager.SetPropertyValue("Lap01Time", this.GetType(), lapTimeList[0]);
                    pluginManager.SetPropertyValue("Lap02Time", this.GetType(), lapTimeList[1]);
                    pluginManager.SetPropertyValue("Lap03Time", this.GetType(), lapTimeList[2]);
                    pluginManager.SetPropertyValue("Lap04Time", this.GetType(), lapTimeList[3]);
                    pluginManager.SetPropertyValue("Lap05Time", this.GetType(), lapTimeList[4]);
                    pluginManager.SetPropertyValue("Lap06Time", this.GetType(), lapTimeList[5]);
                    pluginManager.SetPropertyValue("Lap07Time", this.GetType(), lapTimeList[6]);
                    pluginManager.SetPropertyValue("Lap08Time", this.GetType(), lapTimeList[7]);

                    pluginManager.SetPropertyValue("Lap01Status", this.GetType(), lapStatusList[0]);
                    pluginManager.SetPropertyValue("Lap02Status", this.GetType(), lapStatusList[1]);
                    pluginManager.SetPropertyValue("Lap03Status", this.GetType(), lapStatusList[2]);
                    pluginManager.SetPropertyValue("Lap04Status", this.GetType(), lapStatusList[3]);
                    pluginManager.SetPropertyValue("Lap05Status", this.GetType(), lapStatusList[4]);
                    pluginManager.SetPropertyValue("Lap06Status", this.GetType(), lapStatusList[5]);
                    pluginManager.SetPropertyValue("Lap07Status", this.GetType(), lapStatusList[6]);
                    pluginManager.SetPropertyValue("Lap08Status", this.GetType(), lapStatusList[7]);

                    pluginManager.SetPropertyValue("Lap01FuelTargetDelta", this.GetType(), fuelTargetDeltas[0]);
                    pluginManager.SetPropertyValue("Lap02FuelTargetDelta", this.GetType(), fuelTargetDeltas[1]);
                    pluginManager.SetPropertyValue("Lap03FuelTargetDelta", this.GetType(), fuelTargetDeltas[2]);
                    pluginManager.SetPropertyValue("Lap04FuelTargetDelta", this.GetType(), fuelTargetDeltas[3]);
                    pluginManager.SetPropertyValue("Lap05FuelTargetDelta", this.GetType(), fuelTargetDeltas[4]);
                    pluginManager.SetPropertyValue("Lap06FuelTargetDelta", this.GetType(), fuelTargetDeltas[5]);
                    pluginManager.SetPropertyValue("Lap07FuelTargetDelta", this.GetType(), fuelTargetDeltas[6]);
                    pluginManager.SetPropertyValue("Lap08FuelTargetDelta", this.GetType(), fuelTargetDeltas[7]);

                    pluginManager.SetPropertyValue("Lap01Sector1Time", this.GetType(), TimeSpan.FromSeconds(sector1TimeList[0]));
                    pluginManager.SetPropertyValue("Lap01Sector2Time", this.GetType(), TimeSpan.FromSeconds(sector2TimeList[0]));
                    pluginManager.SetPropertyValue("Lap01Sector3Time", this.GetType(), TimeSpan.FromSeconds(sector3TimeList[0]));
                    pluginManager.SetPropertyValue("Lap01Sector1Status", this.GetType(), sector1StatusList[0]);
                    pluginManager.SetPropertyValue("Lap01Sector2Status", this.GetType(), sector2StatusList[0]);
                    pluginManager.SetPropertyValue("Lap01Sector3Status", this.GetType(), sector3StatusList[0]);

                    pluginManager.SetPropertyValue("Lap02Sector1Time", this.GetType(), TimeSpan.FromSeconds(sector1TimeList[1]));
                    pluginManager.SetPropertyValue("Lap02Sector2Time", this.GetType(), TimeSpan.FromSeconds(sector2TimeList[1]));
                    pluginManager.SetPropertyValue("Lap02Sector3Time", this.GetType(), TimeSpan.FromSeconds(sector3TimeList[1]));
                    pluginManager.SetPropertyValue("Lap02Sector1Status", this.GetType(), sector1StatusList[1]);
                    pluginManager.SetPropertyValue("Lap02Sector2Status", this.GetType(), sector2StatusList[1]);
                    pluginManager.SetPropertyValue("Lap02Sector3Status", this.GetType(), sector3StatusList[1]);

                    pluginManager.SetPropertyValue("Lap03Sector1Time", this.GetType(), TimeSpan.FromSeconds(sector1TimeList[2]));
                    pluginManager.SetPropertyValue("Lap03Sector2Time", this.GetType(), TimeSpan.FromSeconds(sector2TimeList[2]));
                    pluginManager.SetPropertyValue("Lap03Sector3Time", this.GetType(), TimeSpan.FromSeconds(sector3TimeList[2]));
                    pluginManager.SetPropertyValue("Lap03Sector1Status", this.GetType(), sector1StatusList[2]);
                    pluginManager.SetPropertyValue("Lap03Sector2Status", this.GetType(), sector2StatusList[2]);
                    pluginManager.SetPropertyValue("Lap03Sector3Status", this.GetType(), sector3StatusList[2]);

                    pluginManager.SetPropertyValue("Lap04Sector1Time", this.GetType(), TimeSpan.FromSeconds(sector1TimeList[3]));
                    pluginManager.SetPropertyValue("Lap04Sector2Time", this.GetType(), TimeSpan.FromSeconds(sector2TimeList[3]));
                    pluginManager.SetPropertyValue("Lap04Sector3Time", this.GetType(), TimeSpan.FromSeconds(sector3TimeList[3]));
                    pluginManager.SetPropertyValue("Lap04Sector1Status", this.GetType(), sector1StatusList[3]);
                    pluginManager.SetPropertyValue("Lap04Sector2Status", this.GetType(), sector2StatusList[3]);
                    pluginManager.SetPropertyValue("Lap04Sector3Status", this.GetType(), sector3StatusList[3]);

                    pluginManager.SetPropertyValue("Lap05Sector1Time", this.GetType(), TimeSpan.FromSeconds(sector1TimeList[4]));
                    pluginManager.SetPropertyValue("Lap05Sector2Time", this.GetType(), TimeSpan.FromSeconds(sector2TimeList[4]));
                    pluginManager.SetPropertyValue("Lap05Sector3Time", this.GetType(), TimeSpan.FromSeconds(sector3TimeList[4]));
                    pluginManager.SetPropertyValue("Lap05Sector1Status", this.GetType(), sector1StatusList[4]);
                    pluginManager.SetPropertyValue("Lap05Sector2Status", this.GetType(), sector2StatusList[4]);
                    pluginManager.SetPropertyValue("Lap05Sector3Status", this.GetType(), sector3StatusList[4]);

                    pluginManager.SetPropertyValue("Lap06Sector1Time", this.GetType(), TimeSpan.FromSeconds(sector1TimeList[5]));
                    pluginManager.SetPropertyValue("Lap06Sector2Time", this.GetType(), TimeSpan.FromSeconds(sector2TimeList[5]));
                    pluginManager.SetPropertyValue("Lap06Sector3Time", this.GetType(), TimeSpan.FromSeconds(sector3TimeList[5]));
                    pluginManager.SetPropertyValue("Lap06Sector1Status", this.GetType(), sector1StatusList[5]);
                    pluginManager.SetPropertyValue("Lap06Sector2Status", this.GetType(), sector2StatusList[5]);
                    pluginManager.SetPropertyValue("Lap06Sector3Status", this.GetType(), sector3StatusList[5]);

                    pluginManager.SetPropertyValue("Lap07Sector1Time", this.GetType(), TimeSpan.FromSeconds(sector1TimeList[6]));
                    pluginManager.SetPropertyValue("Lap07Sector2Time", this.GetType(), TimeSpan.FromSeconds(sector2TimeList[6]));
                    pluginManager.SetPropertyValue("Lap07Sector3Time", this.GetType(), TimeSpan.FromSeconds(sector3TimeList[6]));
                    pluginManager.SetPropertyValue("Lap07Sector1Status", this.GetType(), sector1StatusList[6]);
                    pluginManager.SetPropertyValue("Lap07Sector2Status", this.GetType(), sector2StatusList[6]);
                    pluginManager.SetPropertyValue("Lap07Sector3Status", this.GetType(), sector3StatusList[6]);

                    pluginManager.SetPropertyValue("Lap08Sector1Time", this.GetType(), TimeSpan.FromSeconds(sector1TimeList[7]));
                    pluginManager.SetPropertyValue("Lap08Sector2Time", this.GetType(), TimeSpan.FromSeconds(sector2TimeList[7]));
                    pluginManager.SetPropertyValue("Lap08Sector3Time", this.GetType(), TimeSpan.FromSeconds(sector3TimeList[7]));
                    pluginManager.SetPropertyValue("Lap08Sector1Status", this.GetType(), sector1StatusList[7]);
                    pluginManager.SetPropertyValue("Lap08Sector2Status", this.GetType(), sector2StatusList[7]);
                    pluginManager.SetPropertyValue("Lap08Sector3Status", this.GetType(), sector3StatusList[7]);

                    pluginManager.SetPropertyValue("TrackType", this.GetType(), trackType);

                    pluginManager.SetPropertyValue("CurrentFrontWing", this.GetType(), currentFrontWing);
                    pluginManager.SetPropertyValue("CurrentRearWing", this.GetType(), currentRearWing);
                    pluginManager.SetPropertyValue("CurrentPowersteer", this.GetType(), currentPWS);
                    pluginManager.SetPropertyValue("CurrentTape", this.GetType(), currentTape);
                  
                }

            }

            //Resetting counter
            if (counter > 59)
            {
                counter = 0;
            }

            if (!gameRunning) //Stuf that happens when out of game
            {
                fuelPerLapOffset = 0;
                savePitTimerLock = false;
                savePitTimerSnap = new TimeSpan(0);
                slowestLapTimeSpanCopy = new TimeSpan(0);

                pitBox = 0.5;
                hasPitted = false;
                validStintLaps = 0;
                invalidStintLaps = 0;
                fuelTargetDeltaCumulative = 0;
                raceFinished = false;
                jokerThisLap = false;
                jokerLapChecker = false;
                finishedCars = new List<string> { };
                fuelTargetCheck = false;
                oldFuelValue = 0;
                NBactive = false;
                NBvalue = false;
                NBspeedLim = false;
                ERSlapCounter = 0;
                ERSstartingLap = true;
                TCon = false;
                TCduration = 0;
                offTrack = false;
                IRchange = 0;

                //Props that need refresh
                pluginManager.SetPropertyValue("TCActive", this.GetType(), false);
                
                //Refreshing some lists
                if (counter == 59)
                {
                    realGapLocks.Clear();
                    realGapChecks.Clear();
                    realGapPoints.Clear();
                    realGapOpponentDelta.Clear();
                    realGapOpponentRelative.Clear();
                    sessionCarsLapsSincePit.Clear();
                    sessionCarsLap.Clear();

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

                    for (int u = 0; u < trackSections; u++)
                    {
                        List<bool> locks = new List<bool> { };
                        List<bool> checks = new List<bool> { };
                        List<TimeSpan> points = new List<TimeSpan> { };

                        for (int i = 0; i < 64; i++)
                        {
                            locks.Add(false);
                            checks.Add(false);
                            points.Add(TimeSpan.FromSeconds(0));
                        }

                        realGapLocks.Add(locks);
                        realGapChecks.Add(checks);
                        realGapPoints.Add(points);
                    }

                    for (int i = 0; i < 64; i++)
                    {
                        realGapOpponentDelta.Add(0);
                        realGapOpponentRelative.Add(0);
                        sessionCarsLapsSincePit.Add(-1);
                        sessionCarsLap.Add(-1);
                    }

                    for (int i = 0; i < lapDeltaSections + 1; i++)
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


        }

            public void End(PluginManager pluginManager)
            {
                // Save settings
                this.SaveCommonSettings("GeneralSettings", Settings);
            }

            /// <summary>
            /// Returns the settings control, return null if no settings control is required
            /// </summary>
            /// <param name="pluginManager"></param>
            /// <returns></returns>
            public System.Windows.Controls.Control GetWPFSettingsControl(PluginManager pluginManager)
            {
                return new SettingsControlDemo(this) { DataContext = Settings }; ;
            }

        /// <summary>
        /// Called once after plugins startup
        /// Plugins are rebuilt at game change
        /// </summary>
        /// <param name="pluginManager"></param>
        /// 


        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
        //--------------------INITIALIZATION STARTS HERE------------------------------
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------




        public void Init(PluginManager pluginManager)
        {

            SimHub.Logging.Current.Info("Starting plugin");

            // Load settings
            Settings = this.ReadCommonSettings<DataPluginDemoSettings>("GeneralSettings", () => new DataPluginDemoSettings());

            //Find the lap records file
            LapRecords.findCSV(ref csvAdress);

            //Filling some lists

            for ( int u = 0; u < trackSections; u++)
            {
                List<bool> locks = new List<bool> { };
                List<bool> checks = new List<bool> { };
                List<TimeSpan> points = new List<TimeSpan> { };

                for (int i = 0; i < 64; i++)
                {
                    locks.Add(false);
                    checks.Add(false);
                    points.Add(TimeSpan.FromSeconds(0));
                }

                realGapLocks.Add(locks);
                realGapChecks.Add(checks);
                realGapPoints.Add(points);
            }

            for (int i = 0; i < 64; i++)
            {
                realGapOpponentDelta.Add(0);
                realGapOpponentRelative.Add(0);
                sessionCarsLapsSincePit.Add(-1);
                sessionCarsLap.Add(-1);
            }

            for (int i = 0; i < lapDeltaSections + 1; i++)
            {
                lapDeltaCurrent.Add(-1);
                lapDeltaSessionBest.Add(-1);
                lapDeltaLast.Add(-1);
                lapDeltaRecord.Add(-1);
                lapDeltaLastChange.Add(0);
                lapDeltaSessionBestChange.Add(0);
                lapDeltaLapRecordChange.Add(0);
            }

            for(int i = 0; i < deltaChangeChunks; i ++)
            {
                lastChunks.Add(0);
                SBChunks.Add(0);
                LRChunks.Add(0);
            }


            //Test property

            pluginManager.AddProperty("TestProperty", this.GetType(), 0);

            //Update property

            pluginManager.AddProperty("Version", this.GetType(), "1.10.1");

            //Key presses
            pluginManager.AddProperty("FuelSaveDelta", this.GetType(), 0);

            pluginManager.AddProperty("BitePointAdjust", this.GetType(), false);
            pluginManager.AddAction("BitePointPressed", this.GetType(), (a, b) =>
            {
                bitePointPressed = true;
            });
            pluginManager.AddAction("BitePointReleased", this.GetType(), (a, b) =>
            {
                bitePointReleased = true;
            });

            pluginManager.AddProperty("LEDWarnings", this.GetType(), false);
            pluginManager.AddProperty("SpotterMode", this.GetType(), false);

            pluginManager.AddAction("PlusPressed", this.GetType(), (a, b) =>
            {
                plusButtonCheck = true;
            });

            pluginManager.AddAction("MinusPressed", this.GetType(), (a, b) =>
            {
                minusButtonCheck = true;
            });

            pluginManager.AddAction("OKPressed", this.GetType(), (a, b) =>
            {
                OKButtonCheck = true;
            });

            pluginManager.AddAction("Upshift", this.GetType(), (a, b) =>
            {
                upshift = true;
            });

            pluginManager.AddAction("Downshift", this.GetType(), (a, b) =>
            {
                downshift = true;
            });

            pluginManager.AddProperty("LaunchScreen", this.GetType(), false);
            pluginManager.AddAction("LaunchPressed", this.GetType(), (a, b) =>
            {
                launchPressed = true;
            });
            pluginManager.AddAction("LaunchReleased", this.GetType(), (a, b) =>
            {
                launchReleased = true;
            });

            pluginManager.AddProperty("NoBoost", this.GetType(), false);
            pluginManager.AddAction("NBPressed", this.GetType(), (a, b) =>
            {
                NBpressed = true;
            });

            pluginManager.AddProperty("Radio", this.GetType(), false);
            pluginManager.AddProperty("RadioName", this.GetType(), "");
            pluginManager.AddProperty("RadioPosition", this.GetType(), 0);
            pluginManager.AddProperty("RadioIsSpectator", this.GetType(), false);

            pluginManager.AddAction("RadioPressed", this.GetType(), (a, b) =>
            {
                radio = true;
            });
            pluginManager.AddAction("RadioReleased", this.GetType(), (a, b) =>
            {
                radio = false;
            });

            pluginManager.AddProperty("PaceCheck", this.GetType(), false);
            pluginManager.AddAction("PacePressed", this.GetType(), (a, b) =>
            {
                pacePressed = true;
            });
            pluginManager.AddAction("PaceReleased", this.GetType(), (a, b) =>
            {
                paceReleased = true;
            });

            pluginManager.AddProperty("PitScreen", this.GetType(), false);
            pluginManager.AddAction("PitPressed", this.GetType(), (a, b) =>
            {
                pitPressed = true;
            });
            pluginManager.AddAction("PitReleased", this.GetType(), (a, b) =>
            {
                pitReleased = true;
            });

            pluginManager.AddProperty("TCOffTimer", this.GetType(), new TimeSpan(0));
            pluginManager.AddAction("TCPressed", this.GetType(), (a, b) =>
            {
                TCactive = true;
            });
            pluginManager.AddAction("TCReleased", this.GetType(), (a, b) =>
            {
                TCactive = false;
            });

            pluginManager.AddProperty("PitMenu", this.GetType(), 1);
            pluginManager.AddAction("L1", this.GetType(), (a, b) =>
            {
                pitMenuRotary = 1;
                pluginManager.SetPropertyValue("PitMenu", this.GetType(), pitMenuRotary);
                if(Settings.DDSEnabled)
                {
                    inCarRotary = 0;
                    pluginManager.SetPropertyValue("InCarMenu", this.GetType(), inCarRotary);
                }
            });
            pluginManager.AddAction("L2", this.GetType(), (a, b) =>
            {
                pitMenuRotary = 2;
                pluginManager.SetPropertyValue("PitMenu", this.GetType(), pitMenuRotary);
                if (Settings.DDSEnabled)
                {
                    inCarRotary = 0;
                    pluginManager.SetPropertyValue("InCarMenu", this.GetType(), inCarRotary);
                }
            });
            pluginManager.AddAction("L3", this.GetType(), (a, b) =>
            {
                pitMenuRotary = 3;
                pluginManager.SetPropertyValue("PitMenu", this.GetType(), pitMenuRotary);
                if (Settings.DDSEnabled)
                {
                    inCarRotary = 0;
                    pluginManager.SetPropertyValue("InCarMenu", this.GetType(), inCarRotary);
                }
            });
            pluginManager.AddAction("L4", this.GetType(), (a, b) =>
            {
                pitMenuRotary = 4;
                pluginManager.SetPropertyValue("PitMenu", this.GetType(), pitMenuRotary);
                if (Settings.DDSEnabled)
                {
                    inCarRotary = 0;
                    pluginManager.SetPropertyValue("InCarMenu", this.GetType(), inCarRotary);
                }
            });
            pluginManager.AddAction("L5", this.GetType(), (a, b) =>
            {
                pitMenuRotary = 5;
                pluginManager.SetPropertyValue("PitMenu", this.GetType(), pitMenuRotary);
                if (Settings.DDSEnabled)
                {
                    inCarRotary = 0;
                    pluginManager.SetPropertyValue("InCarMenu", this.GetType(), inCarRotary);
                }
            });
            pluginManager.AddAction("L6", this.GetType(), (a, b) =>
            {
                pitMenuRotary = 6;
                pluginManager.SetPropertyValue("PitMenu", this.GetType(), pitMenuRotary);
                if (Settings.DDSEnabled)
                {
                    inCarRotary = 0;
                    pluginManager.SetPropertyValue("InCarMenu", this.GetType(), inCarRotary);
                }
            });
            pluginManager.AddAction("L7", this.GetType(), (a, b) =>
            {
                pitMenuRotary = 7;
                pluginManager.SetPropertyValue("PitMenu", this.GetType(), pitMenuRotary);
                if (Settings.DDSEnabled)
                {
                    inCarRotary = 0;
                    pluginManager.SetPropertyValue("InCarMenu", this.GetType(), inCarRotary);
                }
            });
            pluginManager.AddAction("L8", this.GetType(), (a, b) =>
            {
                pitMenuRotary = 8;
                pluginManager.SetPropertyValue("PitMenu", this.GetType(), pitMenuRotary);
                if (Settings.DDSEnabled)
                {
                    inCarRotary = 0;
                    pluginManager.SetPropertyValue("InCarMenu", this.GetType(), inCarRotary);
                }
            });
            pluginManager.AddAction("L9", this.GetType(), (a, b) =>
            {
                pitMenuRotary = 9;
                pluginManager.SetPropertyValue("PitMenu", this.GetType(), pitMenuRotary);
                if (Settings.DDSEnabled)
                {
                    inCarRotary = 0;
                    pluginManager.SetPropertyValue("InCarMenu", this.GetType(), inCarRotary);
                }
            });
            pluginManager.AddAction("L10", this.GetType(), (a, b) =>
            {
                pitMenuRotary = 10;
                pluginManager.SetPropertyValue("PitMenu", this.GetType(), pitMenuRotary);
                if (Settings.DDSEnabled)
                {
                    inCarRotary = 0;
                    pluginManager.SetPropertyValue("InCarMenu", this.GetType(), inCarRotary);
                }
            });
            pluginManager.AddAction("L11", this.GetType(), (a, b) =>
            {
                pitMenuRotary = 11;
                pluginManager.SetPropertyValue("PitMenu", this.GetType(), pitMenuRotary);
                if (Settings.DDSEnabled)
                {
                    inCarRotary = 0;
                    pluginManager.SetPropertyValue("InCarMenu", this.GetType(), inCarRotary);
                }
            });
            pluginManager.AddAction("L12", this.GetType(), (a, b) =>
            {
                pitMenuRotary = 12;
                pluginManager.SetPropertyValue("PitMenu", this.GetType(), pitMenuRotary);
                if (Settings.DDSEnabled)
                {
                    inCarRotary = 0;
                    pluginManager.SetPropertyValue("InCarMenu", this.GetType(), inCarRotary);
                }
            });
            pluginManager.AddAction("LInc", this.GetType(), (a, b) =>
            {
                pitMenuRotary++;
                if (pitMenuRotary > 12)
                {
                    pitMenuRotary = 1;
                }
                pluginManager.SetPropertyValue("PitMenu", this.GetType(), pitMenuRotary);
            });
            pluginManager.AddAction("LDec", this.GetType(), (a, b) =>
            {
                pitMenuRotary--;
                if (pitMenuRotary < 1)
                {
                    pitMenuRotary = 12;
                }
                pluginManager.SetPropertyValue("PitMenu", this.GetType(), pitMenuRotary);
            });

            pluginManager.AddAction("DeltaInc", this.GetType(), (a, b) =>
            {
                fuelSaveDelta++;
                if (fuelSaveDelta > 4)
                {
                    fuelSaveDelta = 0;
                }
                pluginManager.SetPropertyValue("FuelSaveDelta", this.GetType(), fuelSaveDelta);
            });

            pluginManager.AddAction("DeltaDec", this.GetType(), (a, b) =>
            {
                fuelSaveDelta--;
                if (fuelSaveDelta < 0)
                {
                    fuelSaveDelta = 4;
                }
                pluginManager.SetPropertyValue("FuelSaveDelta", this.GetType(), fuelSaveDelta);
            });

            pluginManager.AddAction("ShowBrakeThrottleGaugesEnabled", this.GetType(), (a, b) =>
            {                               
                Settings.ShowBrakeThrottleGaugesEnabled = !Settings.ShowBrakeThrottleGaugesEnabled;
            });


            pluginManager.AddProperty("PitSavePaceLock", this.GetType(), false);

            pluginManager.AddProperty("InCarMenu", this.GetType(), 0);
            pluginManager.AddAction("R1", this.GetType(), (a, b) =>
            {
                inCarRotary = 1;
                pluginManager.SetPropertyValue("InCarMenu", this.GetType(), inCarRotary);
                if (Settings.DDSEnabled)
                {
                    if (rotaryType == "Single")
                    {
                        pitMenuRotary = inCarRotary;
                        pluginManager.SetPropertyValue("PitMenu", this.GetType(), inCarRotary);
                    }
                    else
                    {
                        pluginManager.SetPropertyValue("PitMenu", this.GetType(), 0);
                    }  
                }
            });
            pluginManager.AddAction("R2", this.GetType(), (a, b) =>
            {
                inCarRotary = 2;
                pluginManager.SetPropertyValue("InCarMenu", this.GetType(), inCarRotary);
                if (Settings.DDSEnabled)
                {
                    if (rotaryType == "Single")
                    {
                        pitMenuRotary = inCarRotary;
                        pluginManager.SetPropertyValue("PitMenu", this.GetType(), inCarRotary);
                    }
                    else
                    {
                        pluginManager.SetPropertyValue("PitMenu", this.GetType(), 0);
                    }
                }
            });
            pluginManager.AddAction("R3", this.GetType(), (a, b) =>
            {
                inCarRotary = 3;
                pluginManager.SetPropertyValue("InCarMenu", this.GetType(), inCarRotary);
                if (Settings.DDSEnabled)
                {
                    if (rotaryType == "Single")
                    {
                        pitMenuRotary = inCarRotary;
                        pluginManager.SetPropertyValue("PitMenu", this.GetType(), inCarRotary);
                    }
                    else
                    {
                        pluginManager.SetPropertyValue("PitMenu", this.GetType(), 0);
                    }
                }
            });
            pluginManager.AddAction("R4", this.GetType(), (a, b) =>
            {
                inCarRotary = 4;
                pluginManager.SetPropertyValue("InCarMenu", this.GetType(), inCarRotary);
                if (Settings.DDSEnabled)
                {
                    if (rotaryType == "Single")
                    {
                        pitMenuRotary = inCarRotary;
                        pluginManager.SetPropertyValue("PitMenu", this.GetType(), inCarRotary);
                    }
                    else
                    {
                        pluginManager.SetPropertyValue("PitMenu", this.GetType(), 0);
                    }
                }
            });
            pluginManager.AddAction("R5", this.GetType(), (a, b) =>
            {
                inCarRotary = 5;
                pluginManager.SetPropertyValue("InCarMenu", this.GetType(), inCarRotary);
                if (Settings.DDSEnabled)
                {
                    if (rotaryType == "Single")
                    {
                        pitMenuRotary = inCarRotary;
                        pluginManager.SetPropertyValue("PitMenu", this.GetType(), inCarRotary);
                    }
                    else
                    {
                        pluginManager.SetPropertyValue("PitMenu", this.GetType(), 0);
                    }
                }
            });
            pluginManager.AddAction("R6", this.GetType(), (a, b) =>
            {
                inCarRotary = 6;
                pluginManager.SetPropertyValue("InCarMenu", this.GetType(), inCarRotary);
                if (Settings.DDSEnabled)
                {
                    if (rotaryType == "Single")
                    {
                        pitMenuRotary = inCarRotary;
                        pluginManager.SetPropertyValue("PitMenu", this.GetType(), inCarRotary);
                    }
                    else
                    {
                        pluginManager.SetPropertyValue("PitMenu", this.GetType(), 0);
                    }
                }
            });
            pluginManager.AddAction("R7", this.GetType(), (a, b) =>
            {
                inCarRotary = 7;
                pluginManager.SetPropertyValue("InCarMenu", this.GetType(), inCarRotary);
                if (Settings.DDSEnabled)
                {
                    if (rotaryType == "Single")
                    {
                        pitMenuRotary = inCarRotary;
                        pluginManager.SetPropertyValue("PitMenu", this.GetType(), inCarRotary);
                    }
                    else
                    {
                        pluginManager.SetPropertyValue("PitMenu", this.GetType(), 0);
                    }
                }
            });
            pluginManager.AddAction("R8", this.GetType(), (a, b) =>
            {
                inCarRotary = 8;
                pluginManager.SetPropertyValue("InCarMenu", this.GetType(), inCarRotary);
                if (Settings.DDSEnabled)
                {
                    if (rotaryType == "Single")
                    {
                        pitMenuRotary = inCarRotary;
                        pluginManager.SetPropertyValue("PitMenu", this.GetType(), inCarRotary);
                    }
                    else
                    {
                        pluginManager.SetPropertyValue("PitMenu", this.GetType(), 0);
                    }
                }
            });
            pluginManager.AddAction("R9", this.GetType(), (a, b) =>
            {
                inCarRotary = 9;
                pluginManager.SetPropertyValue("InCarMenu", this.GetType(), inCarRotary);
                if (Settings.DDSEnabled)
                {
                    if (rotaryType == "Single")
                    {
                        pitMenuRotary = inCarRotary;
                        pluginManager.SetPropertyValue("PitMenu", this.GetType(), inCarRotary);
                    }
                    else
                    {
                        pluginManager.SetPropertyValue("PitMenu", this.GetType(), 0);
                    }
                }
            });
            pluginManager.AddAction("R10", this.GetType(), (a, b) =>
            {
                inCarRotary = 10;
                pluginManager.SetPropertyValue("InCarMenu", this.GetType(), inCarRotary);
                if (Settings.DDSEnabled)
                {
                    if (rotaryType == "Single")
                    {
                        pitMenuRotary = inCarRotary;
                        pluginManager.SetPropertyValue("PitMenu", this.GetType(), inCarRotary);
                    }
                    else
                    {
                        pluginManager.SetPropertyValue("PitMenu", this.GetType(), 0);
                    }
                }
            });
            pluginManager.AddAction("R11", this.GetType(), (a, b) =>
            {
                inCarRotary = 11;
                pluginManager.SetPropertyValue("InCarMenu", this.GetType(), inCarRotary);
                if (Settings.DDSEnabled)
                {
                    if (rotaryType == "Single")
                    {
                        pitMenuRotary = inCarRotary;
                        pluginManager.SetPropertyValue("PitMenu", this.GetType(), inCarRotary);
                    }
                    else
                    {
                        pluginManager.SetPropertyValue("PitMenu", this.GetType(), 0);
                    }
                }
            });
            pluginManager.AddAction("R12", this.GetType(), (a, b) =>
            {
                inCarRotary = 12;
                pluginManager.SetPropertyValue("InCarMenu", this.GetType(), inCarRotary);
                if (Settings.DDSEnabled)
                {
                    if (rotaryType == "Single")
                    {
                        pitMenuRotary = inCarRotary;
                        pluginManager.SetPropertyValue("PitMenu", this.GetType(), inCarRotary);
                    }
                    else
                    {
                        pluginManager.SetPropertyValue("PitMenu", this.GetType(), 0);
                    }
                }
            });
            pluginManager.AddAction("RInc", this.GetType(), (a, b) =>
            {
                inCarRotary++;
                if (inCarRotary > 12)
                {
                    inCarRotary = 1;
                }
                pluginManager.SetPropertyValue("InCarMenu", this.GetType(), inCarRotary);
            });
            pluginManager.AddAction("RDec", this.GetType(), (a, b) =>
            {
                inCarRotary--;
                if (inCarRotary < 1)
                {
                    inCarRotary = 12;
                }
                pluginManager.SetPropertyValue("InCarMenu", this.GetType(), inCarRotary);
            });


            //-----------------------------------------------------
            //-----------------Add tracks--------------------------
            //-----------------------------------------------------

            //Dirt road
            trackInfo.Add(new Tracks("atlanta rallycross long", 2, false, 0, 0, 0, 0, false, 0, 0, 0, 0, 0, 0, false, "None"));
            trackInfo.Add(new Tracks("atlanta rallycross short", 2, false, 0, 0, 0, 0, false, 0, 0, 0, 0, 0, 0, false, "None"));
            trackInfo.Add(new Tracks("brandshatch rallycross", 1, false, 0, 0, 0, 0, false, 0, 0, 0, 0, 0, 0, false, "None"));
            trackInfo.Add(new Tracks("charlotte 2018 2019 rallycrosslong", 3, false, 0, 0, 0, 0, false, 0, 0, 0, 0, 0, 0, false, "None"));
            trackInfo.Add(new Tracks("daytona rallycross long", 2, false, 0, 0, 0, 0, false, 0, 0, 0, 0, 0, 0, false, "None"));
            trackInfo.Add(new Tracks("daytona rallycross short", 2, false, 0, 0, 0, 0, false, 0, 0, 0, 0, 0, 0, false, "None"));
            trackInfo.Add(new Tracks("iowa rallycross", 2, false, 0, 0, 0, 0, false, 0, 0, 0, 0, 0, 0, false, "None"));
            trackInfo.Add(new Tracks("lankebanen hellrx", 1, false, 0, 0, 0, 0, false, 0, 0, 0, 0, 0, 0, false, "None"));
            trackInfo.Add(new Tracks("lankebanen rallyshort", 1, false, 0, 0, 0, 0, false, 0, 0, 0, 0, 0, 0, false, "None"));
            trackInfo.Add(new Tracks("irp rallycross", 1, false, 0, 0, 0, 0, false, 0, 0, 0, 0, 0, 0, false, "None"));
            trackInfo.Add(new Tracks("phoenix rallycross", 1, false, 0, 0, 0, 0, false, 0, 0, 0, 0, 0, 0, false, "None"));
            trackInfo.Add(new Tracks("sonoma rallycross", 1, false, 0, 0, 0, 0, false, 0, 0, 0, 0, 0, 0, false, "None"));
            trackInfo.Add(new Tracks("wildhorse", 4, false, 0, 0, 0, 0, false, 0, 0, 0, 0, 0, 0, false, "None"));
            trackInfo.Add(new Tracks("wildwest full", 4, false, 0, 0, 0, 0, false, 0, 0, 0, 0, 0, 0, false, "None"));

            //Road

            trackInfo.Add(new Tracks("daytona 2011 road", 0, false, 0, 0, 0, 0, false, 0, 19.5, 0.2, 0.3, 0.3, 0.1, true, "Left"));

            trackInfo.Add(new Tracks("silverstone 2019 gp", 0, false, 0, 0, 0, 0, false, 0, 13.5, 0.4, 0.3, 0.1, 0.2, false, "Right"));
            trackInfo.Add(new Tracks("silverstone 2019 international", 0, false, 0, 0, 0, 0, false, 0, 13.5, 0.4, 0.3, 0.1, 0.2, false, "Right"));
            trackInfo.Add(new Tracks("silverstone 2019 national", 0, false, 0, 0, 0, 0, false, 0, 17.5, 0.6, 0.2, 0, 0.2, false, "Right"));

            trackInfo.Add(new Tracks("limerock 2019 gp", 0, false, 0, 0, 0, 0, false, 0, 18.5, 0.6, 0, 0.2, 0.3, false, "Right"));
            trackInfo.Add(new Tracks("limerock 2019 classic", 0, false, 0, 0, 0, 0, false, 0, 18.5, 0.6, 0, 0.2, 0.3, false, "Right"));
            trackInfo.Add(new Tracks("limerock 2019 chicanes", 0, false, 0, 0, 0, 0, false, 0, 18.5, 0.55, 0, 0.2, 0.3, false, "Right"));
            trackInfo.Add(new Tracks("limerock 2019 chicanewestb", 0, false, 0, 0, 0, 0, false, 0, 18.5, 0.55, 0, 0.2, 0.3, false, "Right"));

            trackInfo.Add(new Tracks("longbeach", 0, true, 0.2, 0.05, 0.4, 0.05, false, 0, 20, 0, 0, 0, 0, false, "Right")); //PIT CREW INDY || base || other
            trackInfo.Add(new Tracks("okayama full", 0, false, 0, 0, 0, 0, true, 0.0035, 19, 0.2, 0.2, 0.2, 0.3, false, "Right")); //base || other
            trackInfo.Add(new Tracks("bathurst", 0, false, 0, 0, 0, 0, false, 0, 19.5, 0.7, 0.2, 0.0, 0.2, false, "Left")); 
            trackInfo.Add(new Tracks("snetterton 300", 0, false, 0, 0, 0, 0, true, 0.005, 20, 0, 0, 0, 0, false, "Right")); //base || other
            trackInfo.Add(new Tracks("virginia east", 0, false, 0, 0, 0, 0, true, 0.005, 20, 0, 0, 0, 0, false, "Right")); //base || other

            //-----------------------------------------------------
            //-------------------------Add cars--------------------
            //-----------------------------------------------------

            carInfo.Add(new Cars("Dallara F312 F3", false, false, false, false, -1, false, false, false,-1,-1,-1, false, false, "Single", "BiasOnly", 7200, 7100, 7050, 7000, 6950, 0, 0, 7340, 1932, 25.9, 25.5, 20.0, 32.0, 1, 90, 1, 90, false, 0, 1, 1, 1, 1, 7.0, false, 0, 0, 6, 1.5, CrewType.All, true, false, AnimationType.FormulaRenault, 0.35));
            carInfo.Add(new Cars("Dallara P217 LMP2", true, false, false, true, -1, false, true, false, -1, -1, -1, false, false, "Dallara LMP2", "Dallara LMP2", 8400, 8500, 8600, 8640, 8640, 0, 0, 8690, 2400, 41.3, 41.1, 41.3, 65, 1, 100, 1, 100, false, 0, 1.2, 1.05, 1.1, 1.1, 2.38, false, 0, 0, 10.9, 1.2, CrewType.FrontRear, false, true, AnimationType.LMP2, 0.3));
            carInfo.Add(new Cars("Porsche 919 2016", true, false, false, true, -1, false, true, false, -1, 1, 12, true, false, "Porsche 919", "Porsche 919", 8500, 8500, 8600, 8700, 8700, 8700, 0, 9003, 3000, 42.8, 42.7, 42.8, 53.2, 1, 100, 1, 100, false, 0, 1.2, 1.1, 1.1, 1.4, 3, false, 0, 0, 7.4, 1.4, CrewType.FrontRear, false, true, AnimationType.Porsche, 0.5));
            carInfo.Add(new Cars("Mclaren MP4-30", true, true, false, false, -1, false, false, false, -1, -1, -1, false, false, "Mclaren F1", "Mclaren F1", 12400, 12400, 12160, 11850, 11450, 11200, 11300, 12850, 3050, 60.0, 0, 0, 0, 1, 40, 1, 80, true, 70, 1.25, 1.1, 1.2, 1.3, 0, false, 0, 0, 3.2, 0, CrewType.All, true, false, AnimationType.MclarenF1, 1.1)); 
            carInfo.Add(new Cars("Mercedes W12", true, true, false, false, -1, false, false, false, -1, -1, -1, false, false, "Mercedes W12", "Mercedes W12", 11800, 11600, 11700, 11800, 11800, 11800, 11900, 12960,4000, 54.5, 54.1, 0, 0, 1, 5, 1, 40, true, 50, 1.25, 1.1, 1.2, 1.3, 0, false,0, 0, 3.2, 0, CrewType.All, true, false, AnimationType.MclarenF1, 1.05));
            carInfo.Add(new Cars("Mercedes AMG GT3", false, false, true, false, -1, true, true, true, 12, 3, 1, false, false, "Mercedes AMG GT3", "GT3", 7350, 7130, 6780, 6570, 6530, 0, 0,7470, 1284, 37.5, 36.7, 36.8, 48.0, 1, 20, 1, 70, false, 50, 1, 0.9, 1.1, 0.7, 2.7, false, 0, 0, 6.5, 1.5, CrewType.SingleTyre, false, true, AnimationType.AMGGT3, 0.25)); 
            carInfo.Add(new Cars("Ferrari 488 GT3 Evo 2020", false, false, false, true, 1, true, true, false, 1, 1, 12, false, false, "Ferrari 488 GT3" , "GT3", 7300, 7450, 7200, 7200, 7150, 0, 0, 7650, 1962, 33.2, 32.9, 32.9, 36.5, 1, 50, 1, 100, false, 0, 1, 0.9, 1.1, 0.7, 2.7, false, 0, 0, 6.5, 1.5, CrewType.SingleTyre, false, true, AnimationType.Ferrari488, 0.25));
            carInfo.Add(new Cars("Volkswagen Beetle GRC Lite", false, false, false, false,-1, false, false, false, -1, -1, -1, false, false, "Default", "Rally", 7500, 7500, 7500, 7500, 7500, 0, 0, 8190, 1918, 50, 50, 50, 50, 50, 50, 50, 50, false, 0, 0, 0, 0, 0, 0, false, 0, 0, 0, 0, CrewType.SingleTyre, true, true, AnimationType.Analog, 1)); //not finished 
            carInfo.Add(new Cars("Porsche 911 GT3 Cup (992)", false, false, false, false, -1, false, false, false, -1, -1, -1, false, false, "Single", "BiasOnly", 8560, 8560, 8560, 8560, 8560, 0, 0, 8740, 1570, 40.5, 40.0, 40.5, 47.0, 1, 10, 1, 15, false, 0, 0.9, 0.8, 1.2, 0.9, 3, false, 0, 0, 6.5, 1.5, CrewType.SingleTyre, false, true, AnimationType.Porsche, 0.3));
            carInfo.Add(new Cars("Dallara IR18", true, false, false, false, -1, false, false, false, -1, 1, 8, false, true, "Indycar", "Indycar", 11900, 11900, 11930, 11960, 11960, 0, 0, 11995, 2999, 51.5, 0, 0, 0, 1, 70, 1, 80, true, 80, 0, 0, 0, 0, 9.2, true, 7.1, 0, 6.2, 0, CrewType.All, true, false, AnimationType.Indycar, 0.7)); 
            carInfo.Add(new Cars("Formula Renault 3.5", false, true, false, false, -1, false, false, false, -1, -1, -1, false, false, "FR3.5", "FR3.5", 8940, 8940, 8940, 8940, 8940, 0, 0, 8950, 2499, 44.5, 0, 0, 0, 1, 30, 1, 90, true, 63, 1.1, 1.1, 1, 1.1, 8.1, false, 0, 0, 6.2, 0, CrewType.All, true, false, AnimationType.FormulaRenault, 0.4));
            carInfo.Add(new Cars("Mazda MX-5 Cup", false, false, false, false, -1, true, false, false, -1, -1, -1, false, false, "Single", "Default", 7200, 7380, 7420, 7420, 0, 0, 0, 7350, 885, 45.5, 0, 40, 48, 1, 40, 1, 40, false, 0, 0.8, 0.8, 1, 0.5, 0.6, false, 0, 0, 10.3, 0, CrewType.LeftRight, true, true, AnimationType.MX5, 0.15));
            carInfo.Add(new Cars("Formula Vee", false, false, false, false, -1, false, false, false, -1, -1, -1, false, false, "Single", "BiasOnly", 6500, 6500, 6500, 0, 0, 0, 0, 7330, 1171, 58.0, 57.5, 20, 65, 1, 90, 1, 90, false, 0, 0.65, 0.8, 1, 0.5, 0.56, false, 0, 0, 15, 0, CrewType.FrontRear, true, true, AnimationType.Vee, 0.1));
            carInfo.Add(new Cars("Skip Barber Formula 2000", false, false, false, false, -1, false, false, false, -1, -1, -1, false, false, "Single", "Default", 6190, 6140, 6240, 6270, 0, 0, 0, 6350, 800, 28.5, 0, 27.0, 31.0, 1, 80, 1, 80, false, 0, 0.85, 0.85, 1, 0.7, 0.58, false, 0, 0, 9.0, 1.4, CrewType.FrontRear, true, false, AnimationType.Analog, 0.1));
            carInfo.Add(new Cars("Audi R8 LMS", false, false, false, true, 12, true, true, false, 12, -1, -1, false, false, "Audi R8 GT3", "GT3", 7950, 8080, 8120, 8050, 8000, 0, 0, 8480, 1090, 39.8, 39.5, 39.8, 45.0, 1, 100, 1, 100, false, 0, 1, 0.9, 1.1, 0.7, 2.7, false, 0, 0, 6.5, 1.5, CrewType.SingleTyre, false, true, AnimationType.AudiR8, 0.25));
            carInfo.Add(new Cars("Lamborghini Huracan GT3 EVO", false, false, false, true, 12, true, true, false, 12, 1, 12, false, false, "Lamborghini Huracan GT3", "GT3", 8250, 8200, 8220, 8220, 8240, 0, 0, 8480, 1090, 41.8,41.5,41.6,45, 1, 60, 1, 70, false, 0, 1, 0.9, 1.1, 0.7, 2.7, false, 0, 0, 6.5, 1.5, CrewType.SingleTyre, false, true, AnimationType.LamboGT3, 0.25));
            carInfo.Add(new Cars("Porsche 911 GT3.R", true, false, false, true, 0, true, true, false, 0, 4, 0, false, false, "Porsche GT3R", "Porsche GT3R", 9250, 9250, 9250, 9250, 9250, 0, 0, 9435, 1846, 65.0, 64.5, 64.5, 65.5, 1, 90, 1, 90, false, 0, 1, 0.9, 1.1, 0.7, 2.7, false, 0, 0, 6.5, 1.5, CrewType.SingleTyre, false, true, AnimationType.PorscheGT3R, 0.25));
            carInfo.Add(new Cars("Audi 90 Quattro GTO", false, false, false, false, -1, false, false, false, -1, -1, -1, false, false, "Single", "Default", 7650, 7650, 7650, 7650, 0, 0, 0, 7670, 1300, 0, 0, 0, 0, 0, 0, 0, 0, false, 0, 0, 0, 0, 0, 0, false, 0, 0, 10, 1, CrewType.SingleTyre, true, true, AnimationType.Porsche, 0.15));
            carInfo.Add(new Cars("Supercars Ford Mustang GT", false, false, false, false, -1, false, false, false, -1, -1, -1, false, false, "Single", "Supercar", 7470, 7470, 7470, 7480, 7480, 0, 0, 7490, 1205, 29.5, 29.0, 29.6, 33.0, 1, 50, 1, 100, true, 0,1.15, 0.7,0.7, 0.9, 2.36, false, 0, 0, 6.7, 0.9, CrewType.All, true, true, AnimationType.Supercar, 0.35));
            carInfo.Add(new Cars("Supercars Holden ZB Commodore", false, false, false, false, -1, false, false, false, -1, -1, -1, false, false, "Single", "Supercar", 7470, 7470, 7470, 7480, 7480, 0, 0, 7490, 1205, 29.5, 29.0, 29.6, 33.0, 1, 50, 1, 100, true, 0, 1.15, 0.7, 0.7, 0.9, 2.36, false, 0, 0, 6.7, 0.9, CrewType.All, true, true, AnimationType.Supercar, 0.35));
            carInfo.Add(new Cars("iRacing Formula IR-04", false, false, false, false, -1, false, false, false, -1, -1, -1, false, false, "Single", "BiasOnly", 7150, 7100, 7090, 7090, 7090, 0, 0, 7270, 1200, 33.8, 24.5, 25, 37, 1, 100, 1, 100, false, 0, 0.9, 1, 1, 0.9, 6.25, false, 0, 0, 6.11, 0, CrewType.All, true, false, AnimationType.F4, 0.35));


            // Declare a property available in the property list
            pluginManager.AddProperty("DDUstartLED", this.GetType(), Settings.DDUstartLED);
            pluginManager.AddProperty("SW1startLED", this.GetType(), Settings.SW1startLED);
            pluginManager.AddProperty("DDUEnabled", this.GetType(), Settings.DDUEnabled);
            pluginManager.AddProperty("SW1Enabled", this.GetType(), Settings.SW1Enabled);
            pluginManager.AddProperty("DashLEDEnabled", this.GetType(), Settings.DashLEDEnabled);
            pluginManager.AddProperty("ShowMapEnabled", this.GetType(), Settings.ShowMapEnabled);
            pluginManager.AddProperty("ShowBrakeThrottleGaugesEnabled", this.GetType(), Settings.ShowBrakeThrottleGaugesEnabled);
            pluginManager.AddProperty("DashType", this.GetType(), Settings.DashType);
            pluginManager.AddProperty("LapInfoScreen", this.GetType(), Settings.LapInfoScreen);
            pluginManager.AddProperty("ShiftTimingAssist", this.GetType(), Settings.ShiftTimingAssist);
            pluginManager.AddProperty("ShiftWarning", this.GetType(), Settings.ShiftWarning);
            pluginManager.AddProperty("ARBswapped", this.GetType(), Settings.SupercarSwapPosition);
            pluginManager.AddProperty("ARBstiffForward", this.GetType(), Settings.SupercarARBDirection);
            pluginManager.AddProperty("SmallFuelIncrement", this.GetType(), Settings.SmallFuelIncrement);
            pluginManager.AddProperty("LargeFuelIncrement", this.GetType(), Settings.LargeFuelIncrement);
            pluginManager.AddProperty("CoupleInCarToPit", this.GetType(), Settings.CoupleInCarToPit);

            pluginManager.AddProperty("Idle", this.GetType(), true);
            pluginManager.AddProperty("SmoothGear", this.GetType(), "");
            pluginManager.AddProperty("TrackEntry", this.GetType(), false);
            pluginManager.AddProperty("LastGearMaxRPM", this.GetType(), 0);
            pluginManager.AddProperty("LastGear", this.GetType(), 0);
            pluginManager.AddProperty("OvertakeMode", this.GetType(), false);

            pluginManager.AddProperty("StopWatch", this.GetType(), TimeSpan.FromSeconds(0));
            pluginManager.AddProperty("StopWatchSplit", this.GetType(), TimeSpan.FromSeconds(0));

            pluginManager.AddProperty("AccelerationTo100KPH", this.GetType(), 0);
            pluginManager.AddProperty("AccelerationTo200KPH", this.GetType(), 0);
            pluginManager.AddProperty("BrakeCurveValues", this.GetType(), "");
            pluginManager.AddProperty("BrakeCurvePeak", this.GetType(),0);
            pluginManager.AddProperty("BrakeCurveAUC", this.GetType(), 0);
            pluginManager.AddProperty("ThrottleCurveValues", this.GetType(), "");
            pluginManager.AddProperty("ThrottleAgro", this.GetType(), 0);

            pluginManager.AddProperty("ERSTarget", this.GetType(), 0); 
            pluginManager.AddProperty("ERSCharges", this.GetType(), 0);
            pluginManager.AddProperty("TCActive", this.GetType(), false);
            pluginManager.AddProperty("TCToggle", this.GetType(), false);
            pluginManager.AddProperty("ABSToggle", this.GetType(), false);
            pluginManager.AddProperty("HasTC", this.GetType(), false);
            pluginManager.AddProperty("HasABS", this.GetType(), false);
            pluginManager.AddProperty("HasDRS", this.GetType(), false);
            pluginManager.AddProperty("DRSState", this.GetType(), "");
            pluginManager.AddProperty("HasAntiStall", this.GetType(), false);
            pluginManager.AddProperty("HasOvertake", this.GetType(), false);
            pluginManager.AddProperty("MapHigh", this.GetType(), mapHigh);
            pluginManager.AddProperty("MapLow", this.GetType(), mapLow);
            pluginManager.AddProperty("P2PCount", this.GetType(), -1);
            pluginManager.AddProperty("P2PStatus", this.GetType(), false);
            pluginManager.AddProperty("DRSCount", this.GetType(), -1);

            pluginManager.AddProperty("SlipLF", this.GetType(), 0);
            pluginManager.AddProperty("SlipRF", this.GetType(), 0);
            pluginManager.AddProperty("SlipLR", this.GetType(), 0);
            pluginManager.AddProperty("SlipRR", this.GetType(), 0);


            pluginManager.AddProperty("AnimationType", this.GetType(), 1);
            pluginManager.AddProperty("ShiftLightRPM", this.GetType(), 0);
            pluginManager.AddProperty("ReactionTime", this.GetType(), 0);

            pluginManager.AddProperty("Position", this.GetType(), 0);
            pluginManager.AddProperty("HotLapPosition", this.GetType(), 0);
            pluginManager.AddProperty("RaceFinished", this.GetType(), false);
            pluginManager.AddProperty("SoF", this.GetType(), 0);
            pluginManager.AddProperty("IRchange", this.GetType(), 0);
            pluginManager.AddProperty("MyClassColor", this.GetType(), "");


            pluginManager.AddProperty("OptimalShiftGear1", this.GetType(), 0);
            pluginManager.AddProperty("OptimalShiftGear2", this.GetType(), 0);
            pluginManager.AddProperty("OptimalShiftGear3", this.GetType(), 0);
            pluginManager.AddProperty("OptimalShiftGear4", this.GetType(), 0);
            pluginManager.AddProperty("OptimalShiftGear5", this.GetType(), 0);
            pluginManager.AddProperty("OptimalShiftGear6", this.GetType(), 0);
            pluginManager.AddProperty("OptimalShiftGear7", this.GetType(), 0);
            pluginManager.AddProperty("OptimalShiftCurrentGear", this.GetType(), 0);
            pluginManager.AddProperty("OptimalShiftLastGear", this.GetType(), 0);

            pluginManager.AddProperty("TrueRevLimiter", this.GetType(), 0);
            pluginManager.AddProperty("IdleRPM", this.GetType(), 0);

            pluginManager.AddProperty("CenterDashType", this.GetType(), "");
            pluginManager.AddProperty("MenuType", this.GetType(), "");

            pluginManager.AddProperty("LaunchBitePoint", this.GetType(), 0);
            pluginManager.AddProperty("LaunchSpin", this.GetType(), 0);
            pluginManager.AddProperty("LaunchIdealRangeStart", this.GetType(), 0);
            pluginManager.AddProperty("LaunchIdealRangeStop", this.GetType(), 0);
            pluginManager.AddProperty("LaunchGearRelease", this.GetType(), 0);
            pluginManager.AddProperty("LaunchGearReleased", this.GetType(), 0);
            pluginManager.AddProperty("LaunchTimeRelease", this.GetType(), 0);
            pluginManager.AddProperty("LaunchTimeReleased", this.GetType(), 0);
            pluginManager.AddProperty("HighPower", this.GetType(), false);
            pluginManager.AddProperty("LaunchThrottle", this.GetType(), 0);


            pluginManager.AddProperty("ApproximateCalculations", this.GetType(), false);
            pluginManager.AddProperty("LapsRemaining", this.GetType(), 0);
            pluginManager.AddProperty("LapBalance", this.GetType(), 0);

            pluginManager.AddProperty("LapStatus", this.GetType(), 0);

            pluginManager.AddProperty("StintTimer", this.GetType(), new TimeSpan (0));
            pluginManager.AddProperty("StintTotalTime", this.GetType(), new TimeSpan (0));
            pluginManager.AddProperty("StintTotalHotlaps", this.GetType(), 0);
            pluginManager.AddProperty("StintCurrentHotlap", this.GetType(), 0);
            pluginManager.AddProperty("StintValidLaps", this.GetType(), 0);
            pluginManager.AddProperty("StintInvalidLaps", this.GetType(), 0);

            pluginManager.AddProperty("Pace", this.GetType(), new TimeSpan(0));

            pluginManager.AddProperty("PitBoxPosition", this.GetType(), 1);
            pluginManager.AddProperty("PitBoxApproach", this.GetType(), false);
            pluginManager.AddProperty("PitEntry", this.GetType(), false);
            pluginManager.AddProperty("PitSpeeding", this.GetType(), false);

            pluginManager.AddProperty("SessionBestLap", this.GetType(), new TimeSpan(0));

            pluginManager.AddProperty("HotlapLivePosition", this.GetType(), 0);

            pluginManager.AddProperty("QualyWarmUpLap", this.GetType(), false);
            pluginManager.AddProperty("QualyLap1Status", this.GetType(), 0);
            pluginManager.AddProperty("QualyLap2Status", this.GetType(), 0);
            pluginManager.AddProperty("QualyLap1Time", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("QualyLap2Time", this.GetType(), new TimeSpan(0));

            pluginManager.AddProperty("Lap01Time", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("Lap02Time", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("Lap03Time", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("Lap04Time", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("Lap05Time", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("Lap06Time", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("Lap07Time", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("Lap08Time", this.GetType(), new TimeSpan(0));

            pluginManager.AddProperty("Lap01Status", this.GetType(), 0);
            pluginManager.AddProperty("Lap02Status", this.GetType(), 0);
            pluginManager.AddProperty("Lap03Status", this.GetType(), 0);
            pluginManager.AddProperty("Lap04Status", this.GetType(), 0);
            pluginManager.AddProperty("Lap05Status", this.GetType(), 0);
            pluginManager.AddProperty("Lap06Status", this.GetType(), 0);
            pluginManager.AddProperty("Lap07Status", this.GetType(), 0);
            pluginManager.AddProperty("Lap08Status", this.GetType(), 0);

            pluginManager.AddProperty("Lap01Delta", this.GetType(), 0);
            pluginManager.AddProperty("Lap02Delta", this.GetType(), 0);
            pluginManager.AddProperty("Lap03Delta", this.GetType(), 0);
            pluginManager.AddProperty("Lap04Delta", this.GetType(), 0);
            pluginManager.AddProperty("Lap05Delta", this.GetType(), 0);
            pluginManager.AddProperty("Lap06Delta", this.GetType(), 0);
            pluginManager.AddProperty("Lap07Delta", this.GetType(), 0);
            pluginManager.AddProperty("Lap08Delta", this.GetType(), 0);

            pluginManager.AddProperty("CurrentSector", this.GetType(), 0);
            pluginManager.AddProperty("CurrentSector1Time", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("CurrentSector2Time", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("CurrentSector3Time", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("CurrentSector1Delta", this.GetType(), 0);
            pluginManager.AddProperty("CurrentSector2Delta", this.GetType(), 0);
            pluginManager.AddProperty("CurrentSector3Delta", this.GetType(), 0);
            pluginManager.AddProperty("CurrentSector1Status", this.GetType(), 0);
            pluginManager.AddProperty("CurrentSector2Status", this.GetType(), 0);
            pluginManager.AddProperty("CurrentSector3Status", this.GetType(), 0);
            pluginManager.AddProperty("SessionBestSector1", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("SessionBestSector2", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("SessionBestSector3", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("Sector1Pace", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("Sector2Pace", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("Sector3Pace", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("Sector1Score", this.GetType(), 0);
            pluginManager.AddProperty("Sector2Score", this.GetType(), 0);
            pluginManager.AddProperty("Sector3Score", this.GetType(), 0);

            pluginManager.AddProperty("OptimalLapTime", this.GetType(), new TimeSpan(0));

            pluginManager.AddProperty("Lap01Sector1Time", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("Lap01Sector2Time", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("Lap01Sector3Time", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("Lap01Sector1Delta", this.GetType(), 0);
            pluginManager.AddProperty("Lap01Sector2Delta", this.GetType(), 0);
            pluginManager.AddProperty("Lap01Sector3Delta", this.GetType(), 0);
            pluginManager.AddProperty("Lap01Sector1Status", this.GetType(), 0);
            pluginManager.AddProperty("Lap01Sector2Status", this.GetType(), 0);
            pluginManager.AddProperty("Lap01Sector3Status", this.GetType(), 0);
            pluginManager.AddProperty("Lap01FuelTargetDelta", this.GetType(), 0);

            pluginManager.AddProperty("Lap02Sector1Time", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("Lap02Sector2Time", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("Lap02Sector3Time", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("Lap02Sector1Delta", this.GetType(), 0);
            pluginManager.AddProperty("Lap02Sector2Delta", this.GetType(), 0);
            pluginManager.AddProperty("Lap02Sector3Delta", this.GetType(), 0);
            pluginManager.AddProperty("Lap02Sector1Status", this.GetType(), 0);
            pluginManager.AddProperty("Lap02Sector2Status", this.GetType(), 0);
            pluginManager.AddProperty("Lap02Sector3Status", this.GetType(), 0);
            pluginManager.AddProperty("Lap02FuelTargetDelta", this.GetType(), 0);

            pluginManager.AddProperty("Lap03Sector1Time", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("Lap03Sector2Time", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("Lap03Sector3Time", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("Lap03Sector1Delta", this.GetType(), 0);
            pluginManager.AddProperty("Lap03Sector2Delta", this.GetType(), 0);
            pluginManager.AddProperty("Lap03Sector3Delta", this.GetType(), 0);
            pluginManager.AddProperty("Lap03Sector1Status", this.GetType(), 0);
            pluginManager.AddProperty("Lap03Sector2Status", this.GetType(), 0);
            pluginManager.AddProperty("Lap03Sector3Status", this.GetType(), 0);
            pluginManager.AddProperty("Lap03FuelTargetDelta", this.GetType(), 0);

            pluginManager.AddProperty("Lap04Sector1Time", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("Lap04Sector2Time", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("Lap04Sector3Time", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("Lap04Sector1Delta", this.GetType(), 0);
            pluginManager.AddProperty("Lap04Sector2Delta", this.GetType(), 0);
            pluginManager.AddProperty("Lap04Sector3Delta", this.GetType(), 0);
            pluginManager.AddProperty("Lap04Sector1Status", this.GetType(), 0);
            pluginManager.AddProperty("Lap04Sector2Status", this.GetType(), 0);
            pluginManager.AddProperty("Lap04Sector3Status", this.GetType(), 0);
            pluginManager.AddProperty("Lap04FuelTargetDelta", this.GetType(), 0);

            pluginManager.AddProperty("Lap05Sector1Time", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("Lap05Sector2Time", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("Lap05Sector3Time", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("Lap05Sector1Delta", this.GetType(), 0);
            pluginManager.AddProperty("Lap05Sector2Delta", this.GetType(), 0);
            pluginManager.AddProperty("Lap05Sector3Delta", this.GetType(), 0);
            pluginManager.AddProperty("Lap05Sector1Status", this.GetType(), 0);
            pluginManager.AddProperty("Lap05Sector2Status", this.GetType(), 0);
            pluginManager.AddProperty("Lap05Sector3Status", this.GetType(), 0);
            pluginManager.AddProperty("Lap05FuelTargetDelta", this.GetType(), 0);

            pluginManager.AddProperty("Lap06Sector1Time", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("Lap06Sector2Time", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("Lap06Sector3Time", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("Lap06Sector1Delta", this.GetType(), 0);
            pluginManager.AddProperty("Lap06Sector2Delta", this.GetType(), 0);
            pluginManager.AddProperty("Lap06Sector3Delta", this.GetType(), 0);
            pluginManager.AddProperty("Lap06Sector1Status", this.GetType(), 0);
            pluginManager.AddProperty("Lap06Sector2Status", this.GetType(), 0);
            pluginManager.AddProperty("Lap06Sector3Status", this.GetType(), 0);
            pluginManager.AddProperty("Lap06FuelTargetDelta", this.GetType(), 0);

            pluginManager.AddProperty("Lap07Sector1Time", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("Lap07Sector2Time", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("Lap07Sector3Time", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("Lap07Sector1Delta", this.GetType(), 0);
            pluginManager.AddProperty("Lap07Sector2Delta", this.GetType(), 0);
            pluginManager.AddProperty("Lap07Sector3Delta", this.GetType(), 0);
            pluginManager.AddProperty("Lap07Sector1Status", this.GetType(), 0);
            pluginManager.AddProperty("Lap07Sector2Status", this.GetType(), 0);
            pluginManager.AddProperty("Lap07Sector3Status", this.GetType(), 0);
            pluginManager.AddProperty("Lap07FuelTargetDelta", this.GetType(), 0);

            pluginManager.AddProperty("Lap08Sector1Time", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("Lap08Sector2Time", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("Lap08Sector3Time", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("Lap08Sector1Delta", this.GetType(), 0);
            pluginManager.AddProperty("Lap08Sector2Delta", this.GetType(), 0);
            pluginManager.AddProperty("Lap08Sector3Delta", this.GetType(), 0);
            pluginManager.AddProperty("Lap08Sector1Status", this.GetType(), 0);
            pluginManager.AddProperty("Lap08Sector2Status", this.GetType(), 0);
            pluginManager.AddProperty("Lap08Sector3Status", this.GetType(), 0);
            pluginManager.AddProperty("Lap08FuelTargetDelta", this.GetType(), 0);

            pluginManager.AddProperty("LapRecord", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("DeltaLastLap", this.GetType(), 0);
            pluginManager.AddProperty("DeltaSessionBest", this.GetType(), 0);
            pluginManager.AddProperty("DeltaLapRecord", this.GetType(), 0);
            pluginManager.AddProperty("DeltaLastLapChange", this.GetType(), "");
            pluginManager.AddProperty("DeltaSessionBestChange", this.GetType(), "");
            pluginManager.AddProperty("DeltaLapRecordChange", this.GetType(), "");

            pluginManager.AddProperty("P1Gap", this.GetType(), 0);
            pluginManager.AddProperty("P1Name", this.GetType(), "");
            pluginManager.AddProperty("P1Pace", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("P1Finished", this.GetType(), false);
            pluginManager.AddProperty("P1LapBalance", this.GetType(), 0);

            pluginManager.AddProperty("ClassP1Gap", this.GetType(), 0);
            pluginManager.AddProperty("ClassP1Name", this.GetType(), "");
            pluginManager.AddProperty("ClassP1Pace", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("ClassP1RealGap", this.GetType(), 0);

            pluginManager.AddProperty("LuckyDogGap", this.GetType(), 0);
            pluginManager.AddProperty("LuckyDogRealGap", this.GetType(), 0);
            pluginManager.AddProperty("LuckyDogName", this.GetType(), "");
            pluginManager.AddProperty("LuckyDogPositionsAhead", this.GetType(), 0);

            pluginManager.AddProperty("AheadName", this.GetType(), "");
            pluginManager.AddProperty("AheadGap", this.GetType(), 0);
            pluginManager.AddProperty("AheadPace", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("AheadBestLap", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("AheadIsConnected", this.GetType(), false);
            pluginManager.AddProperty("AheadIsInPit", this.GetType(), false);
            pluginManager.AddProperty("AheadSlowLap", this.GetType(), false);
            pluginManager.AddProperty("AheadPrognosis", this.GetType(), 0);
            pluginManager.AddProperty("AheadLapsToOvertake", this.GetType(), 0);
            pluginManager.AddProperty("AheadLapsSincePit", this.GetType(), -1);
            pluginManager.AddProperty("AheadP2PStatus", this.GetType(), false);
            pluginManager.AddProperty("AheadP2PCount", this.GetType(), -1);
            pluginManager.AddProperty("AheadRealGap", this.GetType(), 0);

            pluginManager.AddProperty("BehindName", this.GetType(), "");
            pluginManager.AddProperty("BehindGap", this.GetType(), 0);
            pluginManager.AddProperty("BehindPace", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("BehindBestLap", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("BehindIsConnected", this.GetType(), false);
            pluginManager.AddProperty("BehindIsInPit", this.GetType(), false);
            pluginManager.AddProperty("BehindSlowLap", this.GetType(), false);
            pluginManager.AddProperty("BehindPrognosis", this.GetType(), 0);
            pluginManager.AddProperty("BehindLapsToOvertake", this.GetType(), 0);
            pluginManager.AddProperty("BehindLapsSincePit", this.GetType(), -1);
            pluginManager.AddProperty("BehindP2PStatus", this.GetType(), false);
            pluginManager.AddProperty("BehindP2PCount", this.GetType(), -1);
            pluginManager.AddProperty("BehindRealGap", this.GetType(), 0);

            pluginManager.AddProperty("LeftCarGap", this.GetType(), 0);
            pluginManager.AddProperty("LeftCarName", this.GetType(), "");
            pluginManager.AddProperty("RightCarGap", this.GetType(), 0);
            pluginManager.AddProperty("RightCarName", this.GetType(), "");

            pluginManager.AddProperty("CarAhead01Gap", this.GetType(),  0);
            pluginManager.AddProperty("CarAhead01RaceGap", this.GetType(), 0);
            pluginManager.AddProperty("CarAhead01BestLap", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("CarAhead01Name", this.GetType(), "");
            pluginManager.AddProperty("CarAhead01Position", this.GetType(), 0);
            pluginManager.AddProperty("CarAhead01IRating", this.GetType(), 0);
            pluginManager.AddProperty("CarAhead01Licence", this.GetType(), "");
            pluginManager.AddProperty("CarAhead01IsAhead", this.GetType(), false);
            pluginManager.AddProperty("CarAhead01IsClassLeader", this.GetType(), false);
            pluginManager.AddProperty("CarAhead01IsInPit", this.GetType(), false);
            pluginManager.AddProperty("CarAhead01ClassColor", this.GetType(), "");
            pluginManager.AddProperty("CarAhead01ClassDifference", this.GetType(), 0);
            pluginManager.AddProperty("CarAhead01JokerLaps", this.GetType(), 0);
            pluginManager.AddProperty("CarAhead01LapsSincePit", this.GetType(), -1);
            pluginManager.AddProperty("CarAhead01P2PCount" , this.GetType(), -1);
            pluginManager.AddProperty("CarAhead01P2PStatus", this.GetType(), false);
            pluginManager.AddProperty("CarAhead01RealGap", this.GetType(), 0);
            pluginManager.AddProperty("CarAhead01RealRelative", this.GetType(), 0);

            pluginManager.AddProperty("CarAhead02Gap", this.GetType(), 0);
            pluginManager.AddProperty("CarAhead02RaceGap", this.GetType(), 0);
            pluginManager.AddProperty("CarAhead02BestLap", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("CarAhead02Name", this.GetType(), "");
            pluginManager.AddProperty("CarAhead02Position", this.GetType(), 0);
            pluginManager.AddProperty("CarAhead02IRating", this.GetType(), 0);
            pluginManager.AddProperty("CarAhead02Licence", this.GetType(), "");
            pluginManager.AddProperty("CarAhead02IsAhead", this.GetType(), false);
            pluginManager.AddProperty("CarAhead02IsClassLeader", this.GetType(), false);
            pluginManager.AddProperty("CarAhead02IsInPit", this.GetType(), false);
            pluginManager.AddProperty("CarAhead02ClassColor", this.GetType(), "");
            pluginManager.AddProperty("CarAhead02ClassDifference", this.GetType(), 0);
            pluginManager.AddProperty("CarAhead02JokerLaps", this.GetType(), 0);
            pluginManager.AddProperty("CarAhead02LapsSincePit", this.GetType(), -1);
            pluginManager.AddProperty("CarAhead02P2PCount", this.GetType(), -1);
            pluginManager.AddProperty("CarAhead02P2PStatus", this.GetType(), false);
            pluginManager.AddProperty("CarAhead02RealGap", this.GetType(), 0);
            pluginManager.AddProperty("CarAhead02RealRelative", this.GetType(), 0);

            pluginManager.AddProperty("CarAhead03Gap", this.GetType(), 0);
            pluginManager.AddProperty("CarAhead03RaceGap", this.GetType(), 0);
            pluginManager.AddProperty("CarAhead03BestLap", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("CarAhead03Name", this.GetType(), "");
            pluginManager.AddProperty("CarAhead03Position", this.GetType(), 0);
            pluginManager.AddProperty("CarAhead03IRating", this.GetType(), 0);
            pluginManager.AddProperty("CarAhead03Licence", this.GetType(), "");
            pluginManager.AddProperty("CarAhead03IsAhead", this.GetType(), false);
            pluginManager.AddProperty("CarAhead03IsClassLeader", this.GetType(), false);
            pluginManager.AddProperty("CarAhead03IsInPit", this.GetType(), false);
            pluginManager.AddProperty("CarAhead03ClassColor", this.GetType(), "");
            pluginManager.AddProperty("CarAhead03ClassDifference", this.GetType(), 0);
            pluginManager.AddProperty("CarAhead03JokerLaps", this.GetType(), 0);
            pluginManager.AddProperty("CarAhead03LapsSincePit", this.GetType(), -1);
            pluginManager.AddProperty("CarAhead03P2PCount", this.GetType(), -1);
            pluginManager.AddProperty("CarAhead03P2PStatus", this.GetType(), false);
            pluginManager.AddProperty("CarAhead03RealGap", this.GetType(), 0);
            pluginManager.AddProperty("CarAhead03RealRelative", this.GetType(), 0);

            pluginManager.AddProperty("CarAhead04Gap", this.GetType(), 0);
            pluginManager.AddProperty("CarAhead04RaceGap", this.GetType(), 0);
            pluginManager.AddProperty("CarAhead04BestLap", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("CarAhead04Name", this.GetType(), "");
            pluginManager.AddProperty("CarAhead04Position", this.GetType(), 0);
            pluginManager.AddProperty("CarAhead04IRating", this.GetType(), 0);
            pluginManager.AddProperty("CarAhead04Licence", this.GetType(), "");
            pluginManager.AddProperty("CarAhead04IsAhead", this.GetType(), false);
            pluginManager.AddProperty("CarAhead04IsClassLeader", this.GetType(), false);
            pluginManager.AddProperty("CarAhead04IsInPit", this.GetType(), false);
            pluginManager.AddProperty("CarAhead04ClassColor", this.GetType(), "");
            pluginManager.AddProperty("CarAhead04ClassDifference", this.GetType(), 0);
            pluginManager.AddProperty("CarAhead04JokerLaps", this.GetType(), 0);
            pluginManager.AddProperty("CarAhead04LapsSincePit", this.GetType(), -1);
            pluginManager.AddProperty("CarAhead04P2PCount", this.GetType(), -1);
            pluginManager.AddProperty("CarAhead04P2PStatus", this.GetType(), false);
            pluginManager.AddProperty("CarAhead04RealGap", this.GetType(), 0);
            pluginManager.AddProperty("CarAhead04RealRelative", this.GetType(), 0);

            pluginManager.AddProperty("CarAhead05Gap", this.GetType(), 0);
            pluginManager.AddProperty("CarAhead05RaceGap", this.GetType(), 0);
            pluginManager.AddProperty("CarAhead05BestLap", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("CarAhead05Name", this.GetType(), "");
            pluginManager.AddProperty("CarAhead05Position", this.GetType(), 0);
            pluginManager.AddProperty("CarAhead05IRating", this.GetType(), 0);
            pluginManager.AddProperty("CarAhead05Licence", this.GetType(), "");
            pluginManager.AddProperty("CarAhead05IsAhead", this.GetType(), false);
            pluginManager.AddProperty("CarAhead05IsClassLeader", this.GetType(), false);
            pluginManager.AddProperty("CarAhead05IsInPit", this.GetType(), false);
            pluginManager.AddProperty("CarAhead05ClassColor", this.GetType(), "");
            pluginManager.AddProperty("CarAhead05ClassDifference", this.GetType(), 0);
            pluginManager.AddProperty("CarAhead05JokerLaps", this.GetType(), 0);
            pluginManager.AddProperty("CarAhead05LapsSincePit", this.GetType(), -1);
            pluginManager.AddProperty("CarAhead05P2PCount", this.GetType(), -1);
            pluginManager.AddProperty("CarAhead05P2PStatus", this.GetType(), false);
            pluginManager.AddProperty("CarAhead05RealGap", this.GetType(), 0);
            pluginManager.AddProperty("CarAhead05RealRelative", this.GetType(), 0);

            pluginManager.AddProperty("CarBehind01Gap", this.GetType(), 0);
            pluginManager.AddProperty("CarBehind01RaceGap", this.GetType(), 0);
            pluginManager.AddProperty("CarBehind01BestLap", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("CarBehind01Name", this.GetType(), "");
            pluginManager.AddProperty("CarBehind01Position", this.GetType(), 0);
            pluginManager.AddProperty("CarBehind01IRating", this.GetType(), 0);
            pluginManager.AddProperty("CarBehind01Licence", this.GetType(), "");
            pluginManager.AddProperty("CarBehind01IsAhead", this.GetType(), false);
            pluginManager.AddProperty("CarBehind01IsClassLeader", this.GetType(), false);
            pluginManager.AddProperty("CarBehind01IsInPit", this.GetType(), false);
            pluginManager.AddProperty("CarBehind01ClassColor", this.GetType(), "");
            pluginManager.AddProperty("CarBehind01ClassDifference", this.GetType(), 0);
            pluginManager.AddProperty("CarBehind01JokerLaps", this.GetType(), 0);
            pluginManager.AddProperty("CarBehind01LapsSincePit", this.GetType(), -1);
            pluginManager.AddProperty("CarBehind01P2PCount", this.GetType(), -1);
            pluginManager.AddProperty("CarBehind01P2PStatus", this.GetType(), false);
            pluginManager.AddProperty("CarBehind01RealGap", this.GetType(), 0);
            pluginManager.AddProperty("CarBehind01RealRelative", this.GetType(), 0);

            pluginManager.AddProperty("CarBehind02Gap", this.GetType(), 0);
            pluginManager.AddProperty("CarBehind02RaceGap", this.GetType(), 0);
            pluginManager.AddProperty("CarBehind02BestLap", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("CarBehind02Name", this.GetType(), "");
            pluginManager.AddProperty("CarBehind02Position", this.GetType(), 0);
            pluginManager.AddProperty("CarBehind02IRating", this.GetType(), 0);
            pluginManager.AddProperty("CarBehind02Licence", this.GetType(), "");
            pluginManager.AddProperty("CarBehind02IsAhead", this.GetType(), false);
            pluginManager.AddProperty("CarBehind02IsClassLeader", this.GetType(), false);
            pluginManager.AddProperty("CarBehind02IsInPit", this.GetType(), false);
            pluginManager.AddProperty("CarBehind02ClassColor", this.GetType(), "");
            pluginManager.AddProperty("CarBehind02ClassDifference", this.GetType(), 0);
            pluginManager.AddProperty("CarBehind02JokerLaps", this.GetType(), 0);
            pluginManager.AddProperty("CarBehind02LapsSincePit", this.GetType(), -1);
            pluginManager.AddProperty("CarBehind02P2PCount", this.GetType(), -1);
            pluginManager.AddProperty("CarBehind02P2PStatus", this.GetType(), false);
            pluginManager.AddProperty("CarBehind02RealGap", this.GetType(), 0);
            pluginManager.AddProperty("CarBehind02RealRelative", this.GetType(), 0);

            pluginManager.AddProperty("CarBehind03Gap", this.GetType(), 0);
            pluginManager.AddProperty("CarBehind03RaceGap", this.GetType(), 0);
            pluginManager.AddProperty("CarBehind03BestLap", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("CarBehind03Name", this.GetType(), "");
            pluginManager.AddProperty("CarBehind03Position", this.GetType(), 0);
            pluginManager.AddProperty("CarBehind03IRating", this.GetType(), 0);
            pluginManager.AddProperty("CarBehind03Licence", this.GetType(), "");
            pluginManager.AddProperty("CarBehind03IsAhead", this.GetType(), false);
            pluginManager.AddProperty("CarBehind03IsClassLeader", this.GetType(), false);
            pluginManager.AddProperty("CarBehind03IsInPit", this.GetType(), false);
            pluginManager.AddProperty("CarBehind03ClassColor", this.GetType(), "");
            pluginManager.AddProperty("CarBehind03ClassDifference", this.GetType(), 0);
            pluginManager.AddProperty("CarBehind03JokerLaps", this.GetType(), 0);
            pluginManager.AddProperty("CarBehind03LapsSincePit", this.GetType(), -1);
            pluginManager.AddProperty("CarBehind03P2PCount", this.GetType(), -1);
            pluginManager.AddProperty("CarBehind03P2PStatus", this.GetType(), false);
            pluginManager.AddProperty("CarBehind03RealGap", this.GetType(), 0);
            pluginManager.AddProperty("CarBehind03RealRelative", this.GetType(), 0);

            pluginManager.AddProperty("CarBehind04Gap", this.GetType(), 0);
            pluginManager.AddProperty("CarBehind04RaceGap", this.GetType(), 0);
            pluginManager.AddProperty("CarBehind04BestLap", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("CarBehind04Name", this.GetType(), "");
            pluginManager.AddProperty("CarBehind04Position", this.GetType(), 0);
            pluginManager.AddProperty("CarBehind04IRating", this.GetType(), 0);
            pluginManager.AddProperty("CarBehind04Licence", this.GetType(), "");
            pluginManager.AddProperty("CarBehind04IsAhead", this.GetType(), false);
            pluginManager.AddProperty("CarBehind04IsClassLeader", this.GetType(), false);
            pluginManager.AddProperty("CarBehind04IsInPit", this.GetType(), false);
            pluginManager.AddProperty("CarBehind04ClassColor", this.GetType(), "");
            pluginManager.AddProperty("CarBehind04ClassDifference", this.GetType(), 0);
            pluginManager.AddProperty("CarBehind04JokerLaps", this.GetType(), 0);
            pluginManager.AddProperty("CarBehind04LapsSincePit", this.GetType(), -1);
            pluginManager.AddProperty("CarBehind04P2PCount", this.GetType(), -1);
            pluginManager.AddProperty("CarBehind04P2PStatus", this.GetType(), false);
            pluginManager.AddProperty("CarBehind04RealGap", this.GetType(), 0);
            pluginManager.AddProperty("CarBehind04RealRelative", this.GetType(), 0);

            pluginManager.AddProperty("CarBehind05Gap", this.GetType(), 0);
            pluginManager.AddProperty("CarBehind05RaceGap", this.GetType(), 0);
            pluginManager.AddProperty("CarBehind05BestLap", this.GetType(), new TimeSpan(0));
            pluginManager.AddProperty("CarBehind05Name", this.GetType(), "");
            pluginManager.AddProperty("CarBehind05Position", this.GetType(), 0);
            pluginManager.AddProperty("CarBehind05IRating", this.GetType(), 0);
            pluginManager.AddProperty("CarBehind05Licence", this.GetType(), "");
            pluginManager.AddProperty("CarBehind05IsAhead", this.GetType(), false);
            pluginManager.AddProperty("CarBehind05IsClassLeader", this.GetType(), false);
            pluginManager.AddProperty("CarBehind05IsInPit", this.GetType(), false);
            pluginManager.AddProperty("CarBehind05ClassColor", this.GetType(), "");
            pluginManager.AddProperty("CarBehind05ClassDifference", this.GetType(), 0);
            pluginManager.AddProperty("CarBehind05JokerLaps", this.GetType(), 0);
            pluginManager.AddProperty("CarBehind05LapsSincePit", this.GetType(), -1);
            pluginManager.AddProperty("CarBehind05P2PCount", this.GetType(), -1);
            pluginManager.AddProperty("CarBehind05P2PStatus", this.GetType(), false);
            pluginManager.AddProperty("CarBehind05RealGap", this.GetType(), 0);
            pluginManager.AddProperty("CarBehind05RealRelative", this.GetType(), 0);

            pluginManager.AddProperty("FuelDelta", this.GetType(), 0);
            pluginManager.AddProperty("FuelPitWindowFirst", this.GetType(), 0);
            pluginManager.AddProperty("FuelPitWindowLast", this.GetType(), 0);
            pluginManager.AddProperty("FuelMinimumFuelFill", this.GetType(), 0);
            pluginManager.AddProperty("FuelMaximumFuelFill", this.GetType(), 0);
            pluginManager.AddProperty("FuelPitStops", this.GetType(), 0);
            pluginManager.AddProperty("FuelConserveToSaveAStop", this.GetType(), 0);
            pluginManager.AddProperty("FuelAlert", this.GetType(), false);

            pluginManager.AddProperty("FuelDeltaLL", this.GetType(), 0);
            pluginManager.AddProperty("FuelPitWindowFirstLL", this.GetType(), 0);
            pluginManager.AddProperty("FuelPitWindowLastLL", this.GetType(), 0);
            pluginManager.AddProperty("FuelMinimumFuelFillLL", this.GetType(), 0);
            pluginManager.AddProperty("FuelMaximumFuelFillLL", this.GetType(), 0);
            pluginManager.AddProperty("FuelPitStopsLL", this.GetType(), 0);
            pluginManager.AddProperty("FuelConserveToSaveAStopLL", this.GetType(), 0);

            pluginManager.AddProperty("FuelSlowestFuelSavePace", this.GetType(), new TimeSpan(0)) ;
            pluginManager.AddProperty("FuelSaveDeltaValue", this.GetType(), 0);
            pluginManager.AddProperty("FuelPerLapOffset", this.GetType(), 0);
            pluginManager.AddProperty("FuelPerLapTarget", this.GetType(), 0);
            pluginManager.AddProperty("FuelPerLapTargetLastLapDelta", this.GetType(), 0);
            pluginManager.AddProperty("FuelTargetDeltaCumulative", this.GetType(), 0);

            pluginManager.AddProperty("TrackType", this.GetType(), 0);
            pluginManager.AddProperty("JokerThisLap", this.GetType(), false);
            pluginManager.AddProperty("JokerCount", this.GetType(), 0);

            pluginManager.AddProperty("MinimumCornerSpeed", this.GetType(), 0);
            pluginManager.AddProperty("StraightLineSpeed", this.GetType(), 0);

            pluginManager.AddProperty("PitToggleLF", this.GetType(), false);
            pluginManager.AddProperty("PitToggleRF", this.GetType(), false);
            pluginManager.AddProperty("PitToggleLR", this.GetType(), false);
            pluginManager.AddProperty("PitToggleRR", this.GetType(), false);
            pluginManager.AddProperty("PitToggleFuel", this.GetType(), false);
            pluginManager.AddProperty("PitToggleWindscreen", this.GetType(), false);
            pluginManager.AddProperty("PitToggleRepair", this.GetType(), false);

            pluginManager.AddProperty("PitServiceFuelTarget", this.GetType(), 0);
            pluginManager.AddProperty("PitServiceLFPSet", this.GetType(), 0);
            pluginManager.AddProperty("PitServiceRFPSet", this.GetType(), 0);
            pluginManager.AddProperty("PitServiceLRPSet", this.GetType(), 0);
            pluginManager.AddProperty("PitServiceRRPSet", this.GetType(), 0);
            pluginManager.AddProperty("PitServiceLFPCold", this.GetType(), 0);
            pluginManager.AddProperty("PitServiceRFPCold", this.GetType(), 0);
            pluginManager.AddProperty("PitServiceLRPCold", this.GetType(), 0);
            pluginManager.AddProperty("PitServiceRRPCold", this.GetType(), 0);

            pluginManager.AddProperty("CurrentFrontWing", this.GetType(), 0);
            pluginManager.AddProperty("CurrentRearWing", this.GetType(), 0);
            pluginManager.AddProperty("CurrentPowersteer", this.GetType(), 0);
            pluginManager.AddProperty("CurrentTape", this.GetType(), 0);

            pluginManager.AddProperty("PitCrewType", this.GetType(), 0);
            pluginManager.AddProperty("PitTimeTires", this.GetType(), 0);
            pluginManager.AddProperty("PitTimeFuel", this.GetType(), 0);
            pluginManager.AddProperty("PitTimeWindscreen", this.GetType(), 0);
            pluginManager.AddProperty("PitTimeAdjustment", this.GetType(), 0);
            pluginManager.AddProperty("PitTimeDriveThrough", this.GetType(), 0);
            pluginManager.AddProperty("PitTimeService", this.GetType(), 0);
            pluginManager.AddProperty("PitTimeTotal", this.GetType(), 0);


            pluginManager.AddProperty("PitExitPosition", this.GetType(), 0);

            pluginManager.AddProperty("PitExitCar1Name", this.GetType(), "");
            pluginManager.AddProperty("PitExitCar1Gap", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar1Position", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar1ClassDifference", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar1IsAhead", this.GetType(), false);
            pluginManager.AddProperty("PitExitCar1IsFaster", this.GetType(), false);

            pluginManager.AddProperty("PitExitCar2Name", this.GetType(), "");
            pluginManager.AddProperty("PitExitCar2Gap", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar2Position", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar2ClassDifference", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar2IsAhead", this.GetType(), false);
            pluginManager.AddProperty("PitExitCar2IsFaster", this.GetType(), false);

            pluginManager.AddProperty("PitExitCar3Name", this.GetType(), "");
            pluginManager.AddProperty("PitExitCar3Gap", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar3Position", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar3ClassDifference", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar3IsAhead", this.GetType(), false);
            pluginManager.AddProperty("PitExitCar3IsFaster", this.GetType(), false);

            pluginManager.AddProperty("PitExitCar4Name", this.GetType(), "");
            pluginManager.AddProperty("PitExitCar4Gap", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar4Position", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar4ClassDifference", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar4IsAhead", this.GetType(), false);
            pluginManager.AddProperty("PitExitCar4IsFaster", this.GetType(), false);

            pluginManager.AddProperty("PitExitCar5Name", this.GetType(), "");
            pluginManager.AddProperty("PitExitCar5Gap", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar5Position", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar5ClassDifference", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar5IsAhead", this.GetType(), false);
            pluginManager.AddProperty("PitExitCar5IsFaster", this.GetType(), false);

            pluginManager.AddProperty("PitExitCar6Name", this.GetType(), "");
            pluginManager.AddProperty("PitExitCar6Gap", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar6Position", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar6ClassDifference", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar6IsAhead", this.GetType(), false);
            pluginManager.AddProperty("PitExitCar6IsFaster", this.GetType(), false);

            pluginManager.AddProperty("PitExitCar7Name", this.GetType(), "");
            pluginManager.AddProperty("PitExitCar7Gap", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar7Position", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar7ClassDifference", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar7IsAhead", this.GetType(), false);
            pluginManager.AddProperty("PitExitCar7IsFaster", this.GetType(), false);

            pluginManager.AddProperty("PitExitCar8Name", this.GetType(), "");
            pluginManager.AddProperty("PitExitCar8Gap", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar8Position", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar8ClassDifference", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar8IsAhead", this.GetType(), false);
            pluginManager.AddProperty("PitExitCar8IsFaster", this.GetType(), false);

            pluginManager.AddProperty("PitExitCar9Name", this.GetType(), "");
            pluginManager.AddProperty("PitExitCar9Gap", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar9Position", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar9ClassDifference", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar9IsAhead", this.GetType(), false);
            pluginManager.AddProperty("PitExitCar9IsFaster", this.GetType(), false);

            pluginManager.AddProperty("PitExitCar10Name", this.GetType(), "");
            pluginManager.AddProperty("PitExitCar10Gap", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar10Position", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar10ClassDifference", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar10IsAhead", this.GetType(), false);
            pluginManager.AddProperty("PitExitCar10IsFaster", this.GetType(), false);

            pluginManager.AddProperty("PitExitCar11Name", this.GetType(), "");
            pluginManager.AddProperty("PitExitCar11Gap", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar11Position", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar11ClassDifference", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar11IsAhead", this.GetType(), false);
            pluginManager.AddProperty("PitExitCar11IsFaster", this.GetType(), false);

            pluginManager.AddProperty("PitExitCar12Name", this.GetType(), "");
            pluginManager.AddProperty("PitExitCar12Gap", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar12Position", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar12ClassDifference", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar12IsAhead", this.GetType(), false);
            pluginManager.AddProperty("PitExitCar12IsFaster", this.GetType(), false);

            pluginManager.AddProperty("PitExitCar13Name", this.GetType(), "");
            pluginManager.AddProperty("PitExitCar13Gap", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar13Position", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar13ClassDifference", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar13IsAhead", this.GetType(), false);
            pluginManager.AddProperty("PitExitCar13IsFaster", this.GetType(), false);

            pluginManager.AddProperty("PitExitCar14Name", this.GetType(), "");
            pluginManager.AddProperty("PitExitCar14Gap", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar14Position", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar14ClassDifference", this.GetType(), 0);
            pluginManager.AddProperty("PitExitCar14IsAhead", this.GetType(), false);
            pluginManager.AddProperty("PitExitCar14IsFaster", this.GetType(), false);

            pluginManager.AddProperty("DDCclutch", this.GetType(), 0);
            pluginManager.AddProperty("DDCbitePoint", this.GetType(), 0);
            pluginManager.AddProperty("DDCbrake", this.GetType(), 0);
            pluginManager.AddProperty("DDCthrottle", this.GetType(), 0);

            pluginManager.AddProperty("DDCR1", this.GetType(), -1);
            pluginManager.AddProperty("DDCR2", this.GetType(), -1);
            pluginManager.AddProperty("DDCR3", this.GetType(), -1);
            pluginManager.AddProperty("DDCR4", this.GetType(), -1);
            pluginManager.AddProperty("DDCR5", this.GetType(), -1);
            pluginManager.AddProperty("DDCR6", this.GetType(), -1);
            pluginManager.AddProperty("DDCR7", this.GetType(), -1);
            pluginManager.AddProperty("DDCR8", this.GetType(), -1);
            pluginManager.AddProperty("DDCR15", this.GetType(), -1);
            pluginManager.AddProperty("DDCDDSMode", this.GetType(), -1);
            pluginManager.AddProperty("DDCDDSEnabled", this.GetType(), false);
            pluginManager.AddProperty("DDCEnabled", this.GetType(), false);
            pluginManager.AddProperty("DDCclutchEnabled", this.GetType(), false);
            pluginManager.AddProperty("DDCclutchMode", this.GetType(), -1);
            pluginManager.AddProperty("DDCbiteSetting", this.GetType(), -1);


            pluginManager.AddProperty("DDCB1", this.GetType(), -1);
            pluginManager.AddProperty("DDCB2", this.GetType(), -1);
            pluginManager.AddProperty("DDCB3", this.GetType(), -1);
            pluginManager.AddProperty("DDCB4", this.GetType(), -1);

            pluginManager.AddProperty("DDCthrottleHoldActive", this.GetType(), -1);
            pluginManager.AddProperty("DDCmagicActive", this.GetType(), -1);
            pluginManager.AddProperty("DDCquickSwitchMode", this.GetType(), -1);
            pluginManager.AddProperty("DDCquickSwitchActive", this.GetType(), -1);
            pluginManager.AddProperty("DDChandbrakeActive", this.GetType(), -1);

            pluginManager.AddProperty("DDCneutralMode", this.GetType(), -1);
            pluginManager.AddProperty("DDCneutralActive", this.GetType(), false);
            pluginManager.AddProperty("DDCPreset", this.GetType(), -1);

            pluginManager.AddProperty("SW1DDSMode", this.GetType(), -1);
            pluginManager.AddProperty("SW1ClutchMode", this.GetType(), -1);
            pluginManager.AddProperty("SW1BiteSetting", this.GetType(), -1);

            pluginManager.AddProperty("SW1QuickSwitchMode", this.GetType(), -1);
            pluginManager.AddProperty("SW1HandbrakeActive", this.GetType(), -1);
            pluginManager.AddProperty("SW1RadioButtonMode", this.GetType(), -1);
            pluginManager.AddProperty("SW1RightRotaryMode", this.GetType(), -1);
            pluginManager.AddProperty("SW1LeftRotaryMode", this.GetType(), -1);
            pluginManager.AddProperty("SW1MagicToggleMode", this.GetType(), -1);
            pluginManager.AddProperty("SW1RightToggleMode", this.GetType(), -1);
            pluginManager.AddProperty("SW1LeftToggleMode", this.GetType(), -1);
            pluginManager.AddProperty("SW1ShifterMode", this.GetType(), -1);
            pluginManager.AddProperty("SW1QuickSwitchActive", this.GetType(), false);
            pluginManager.AddProperty("SW1ThrottleHoldActive", this.GetType(), false);
            pluginManager.AddProperty("SW1MagicToggleActive", this.GetType(), false);
            pluginManager.AddProperty("SW1Preset", this.GetType(), -1);
            pluginManager.AddProperty("SW1NeutralMode", this.GetType(), -1);
            pluginManager.AddProperty("SW1NeutralActive", this.GetType(), false);

            pluginManager.AddProperty("SW1Clutch", this.GetType(), 0);
            pluginManager.AddProperty("SW1BitePoint", this.GetType(), 0);
            pluginManager.AddProperty("SW1Brake", this.GetType(), 0);
            pluginManager.AddProperty("SW1Throttle", this.GetType(), 0);
        }
        
        public void AddProp(string PropertyName, dynamic defaultValue) => PluginManager.AddProperty(PropertyName, GetType(), defaultValue);
        public void SetProp(string PropertyName, dynamic value) => PluginManager.SetPropertyValue(PropertyName, GetType(), value);
        public dynamic GetProp(string PropertyName) => PluginManager.GetPropertyValue("DataCorePlugin.GameRawData." + PropertyName);
        public bool HasProp(string PropertyName) => PluginManager.GetAllPropertiesNames().Contains("DataCorePlugin.GameRawData." + PropertyName);
        public void AddEvent(string EventName) => PluginManager.AddEvent(EventName, GetType());
        public void TriggerEvent(string EventName) => PluginManager.TriggerEvent(EventName, GetType());
        public void AddAction(string ActionName, Action<PluginManager, string> ActionBody) => PluginManager.AddAction(ActionName, GetType(), ActionBody);
    }
}