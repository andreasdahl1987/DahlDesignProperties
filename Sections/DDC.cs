using SimHub.Plugins;
using System;

namespace DahlDesign.Plugin.Categories
{
    public class DDC
    {
        private readonly DahlDesign Base;
        public bool controllerEnabled;

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
        public int button8Mode = 0;
        int button9Mode = 0;
        int button10Mode = 0;
        int button11Mode = 0;
        int button15Mode = 0;
        int button16Mode = 0;

        public DDC(DahlDesign dahlDesign)
        {
            Base = dahlDesign;
         
            Base.AddProp("DDCclutch",  0);
            Base.AddProp("DDCbitePoint",  0);
            Base.AddProp("DDCbrake",  0);
            Base.AddProp("DDCthrottle",  0);

            Base.AddProp("DDCR1",  -1);
            Base.AddProp("DDCR2",  -1);
            Base.AddProp("DDCR3",  -1);
            Base.AddProp("DDCR4",  -1);
            Base.AddProp("DDCR5",  -1);
            Base.AddProp("DDCR6",  -1);
            Base.AddProp("DDCR7",  -1);
            Base.AddProp("DDCR8",  -1);
            Base.AddProp("DDCR15",  -1);
            Base.AddProp("DDCDDSMode",  -1);
            Base.AttachDelegate("DDCDDSEnabled",  () => Base.Settings.DDSEnabled);
            Base.AddProp("DDCEnabled",  false);
            Base.AttachDelegate("DDCclutchEnabled",  () => Base.Settings.DDCclutchEnabled);
            Base.AddProp("DDCclutchMode",  -1);
            Base.AddProp("DDCbiteSetting",  -1);


            Base.AddProp("DDCB1",  -1);
            Base.AddProp("DDCB2",  -1);
            Base.AddProp("DDCB3",  -1);
            Base.AddProp("DDCB4",  -1);

            Base.AddProp("DDCthrottleHoldActive",  -1);
            Base.AddProp("DDCmagicActive",  -1);
            Base.AddProp("DDCquickSwitchMode",  -1);
            Base.AddProp("DDCquickSwitchActive",  -1);
            Base.AddProp("DDChandbrakeActive",  -1);

            Base.AddProp("DDCneutralMode",  -1);
            Base.AddProp("DDCneutralActive",  false);
            Base.AddProp("DDCPreset",  -1);

            Base.AddProp("SW1DDSMode",  -1);
            Base.AddProp("SW1ClutchMode",  -1);
            Base.AddProp("SW1BiteSetting",  -1);

            Base.AddProp("SW1QuickSwitchMode",  -1);
            Base.AddProp("SW1HandbrakeActive",  -1);
            Base.AddProp("SW1RadioButtonMode",  -1);
            Base.AddProp("SW1RightRotaryMode",  -1);
            Base.AddProp("SW1LeftRotaryMode",  -1);
            Base.AddProp("SW1MagicToggleMode",  -1);
            Base.AddProp("SW1RightToggleMode",  -1);
            Base.AddProp("SW1LeftToggleMode",  -1);
            Base.AddProp("SW1ShifterMode",  -1);
            Base.AddProp("SW1QuickSwitchActive",  false);
            Base.AddProp("SW1ThrottleHoldActive",  false);
            Base.AddProp("SW1MagicToggleActive",  false);
            Base.AddProp("SW1Preset",  -1);
            Base.AddProp("SW1NeutralMode", -1);
            Base.AddProp("SW1NeutralActive",  false);

            Base.AddProp("SW1Clutch",  0);
            Base.AddProp("SW1BitePoint",  0);
            Base.AddProp("SW1Brake",  0);
            Base.AddProp("SW1Throttle",  0);

            Base.AttachDelegate("DDUstartLED", () => Base.Settings.DDUstartLED);
            Base.AttachDelegate("SW1startLED", () => Base.Settings.SW1startLED);
            Base.AttachDelegate("DDUEnabled", () => Base.Settings.DDUEnabled);
        }

        public void DataUpdate()
        {
            //Base.Settings.DDSEnabled = false;
            //Base.Settings.DDCclutchEnabled = false; 

            controllerEnabled = Base.Settings.DDCEnabled;

            var controllerSearch = Base.GetProp("JoystickPlugin." + Base.Settings.DDC + "_Z");
            if (controllerSearch == null)
            {
                controllerEnabled = false;
            }

            if (Base.Settings.SW1Enabled)
            {
                int encoderField = Convert.ToInt32(Base.GetProp("JoystickPlugin." + Base.Settings.DDC + "_Z")); //Encoder field
                encoder1Mode = (encoderField & 1);
                encoder2Mode = (encoderField & 2) >> 1;
                encoder3Mode = (encoderField & 4) >> 2;

                string bitField = Convert.ToString(encoderField, 2).PadLeft(16, '0');

                encoder5Mode = (encoderField & 768) >> 8;
                encoder6Mode = (encoderField & 3072) >> 10;
                encoder7Mode = (encoderField & 12288) >> 12;
                encoder8Mode = (encoderField & 16384) >> 14;

                Base.SetProp("SW1HandbrakeActive", encoder1Mode);
                Base.SetProp("SW1QuickSwitchMode", encoder2Mode);

                Base.SetProp("SW1DDSMode",encoder5Mode);
                Base.SetProp("SW1ClutchMode",  encoder7Mode);
                Base.SetProp("SW1BiteSetting",  encoder6Mode);
                Base.SetProp("SW1QuickSwitchActive",  encoder8Mode);


                int buttonField = Convert.ToInt32(Base.GetProp("JoystickPlugin." + Base.Settings.DDC + "_Y")); //Buttonfield
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

                double clutchValue = Convert.ToInt32(Base.GetProp("JoystickPlugin." + Base.Settings.DDC + "_X")) / 655.35;
                double bitePointValue = Convert.ToInt32(Base.GetProp("JoystickPlugin." + Base.Settings.DDC + "_RY")) / 655.35;
                double brakeValue = Convert.ToInt32(Base.GetProp("JoystickPlugin." + Base.Settings.DDC + "_RZ")) / 655.35;
                double throttleValue = Convert.ToInt32(Base.GetProp("JoystickPlugin." + Base.Settings.DDC + "_Slider0")) / 655.35;

                Base.SetProp("SW1RadioButtonMode",  button1Mode);
                Base.SetProp("SW1RightRotaryMode",  button2Mode);
                Base.SetProp("SW1LeftRotaryMode",  button3Mode);
                Base.SetProp("SW1MagicToggleMode",  button4Mode);
                Base.SetProp("SW1RightToggleMode",  button5Mode);
                Base.SetProp("SW1LeftToggleMode",  button6Mode);
                Base.SetProp("SW1ShifterMode",  button7Mode);
                Base.SetProp("SW1NeutralActive",  button8Mode);
                Base.SetProp("SW1ThrottleHoldActive",  button9Mode);
                Base.SetProp("SW1MagicToggleActive",  button10Mode);
                Base.SetProp("SW1Preset",  button11Mode + 1);
                Base.SetProp("SW1NeutralMode",  button15Mode);

                Base.SetProp("SW1Clutch",  Math.Round(clutchValue, 1));
                Base.SetProp("SW1BitePoint",  Math.Round(bitePointValue, 1));
                Base.SetProp("SW1Brake",  Math.Round(brakeValue, 1));
                Base.SetProp("SW1Throttle",  Math.Round(throttleValue, 1));
            }

            else if (controllerEnabled)
            {
                int encoderField = Convert.ToInt32(Base.GetProp("JoystickPlugin." + Base.Settings.DDC + "_Z")); //Encoder field
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


                Base.SetProp("DDCR1",  encoder1Mode);
                Base.SetProp("DDCR2",  encoder2Mode);
                Base.SetProp("DDCR3",  encoder3Mode);
                Base.SetProp("DDCR4",  encoder4Mode);
                Base.SetProp("DDCR5",  encoder5Mode);
                Base.SetProp("DDCR6",  encoder6Mode);
                Base.SetProp("DDCR7",  encoder7Mode);
                Base.SetProp("DDCR8",  encoder8Mode);
                Base.SetProp("DDCR15",  encoder15Mode);


                Base.SetProp("DDCDDSMode",  DDSmode);
                Base.SetProp("DDCclutchMode",  dualClutchesMode);
                Base.SetProp("DDCbiteSetting",  bitePointMode);

                int buttonField = Convert.ToInt32(Base.GetProp("JoystickPlugin." + Base.Settings.DDC + "_Y")); //Buttonfield
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

                double clutchValue = Convert.ToInt32(Base.GetProp("JoystickPlugin." + Base.Settings.DDC + "_X")) / 655.35;
                double bitePointValue = Convert.ToInt32(Base.GetProp("JoystickPlugin." + Base.Settings.DDC + "_RY")) / 655.35;
                double brakeValue = Convert.ToInt32(Base.GetProp("JoystickPlugin." + Base.Settings.DDC + "_RZ")) / 655.35;
                double throttleValue = Convert.ToInt32(Base.GetProp("JoystickPlugin." + Base.Settings.DDC + "_Slider0")) / 655.35;

                Base.SetProp("DDCB1",  button1Mode);
                Base.SetProp("DDCB2",  button2Mode);
                Base.SetProp("DDCB3",  button3Mode);
                Base.SetProp("DDCB4",  button4Mode);

                Base.SetProp("DDCthrottleHoldActive",  button6Mode);
                Base.SetProp("DDCmagicActive",  button7Mode);
                Base.SetProp("DDCquickSwitchMode",  button8Mode);
                Base.SetProp("DDCquickSwitchActive",  button9Mode);
                Base.SetProp("DDChandbrakeActive",  button10Mode);
                Base.SetProp("DDCPreset",  button11Mode + 1);
                Base.SetProp("DDCneutralMode",  button15Mode);
                Base.SetProp("DDCneutralActive",  button5Mode);

                Base.SetProp("DDCclutch",  Math.Round(clutchValue, 1));
                Base.SetProp("DDCbitePoint",  Math.Round(bitePointValue, 1));
                Base.SetProp("DDCbrake",  Math.Round(brakeValue, 1));
                Base.SetProp("DDCthrottle",  Math.Round(throttleValue, 1));
            }
        }
    }
}