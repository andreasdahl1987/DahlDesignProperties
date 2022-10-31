﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace User.PluginSdkDemo
{
    /// <summary>
    /// Settings class, make sure it can be correctly serialized using JSON.net
    /// </summary>
    public class DataPluginDemoSettings : INotifyPropertyChanged
    {

        public int ReactionTime { get; set; } = 300;
        public bool DDUEnabled { get; set; } = false;
        public int DDUstartLED { get; set; } = 1;
        public bool SW1Enabled { get; set; } = false;
        public int SW1startLED { get; set; } = 30;

        public bool DashLEDEnabled { get; set; } = false;
        public bool LapInfoScreen { get; set; } = true;
        public bool ShiftWarning { get; set; } = false;
        public bool WheelSlipLEDs { get; set; } = false;

        //When we adjust the small fuel up/down, how much?  In Liters.
        public int SmallPitstopFuelIncrement { get; set; } = 2;


        //When we adjust the small fuel up/down, how much?  In Liters.
        public int LargePitstopFuelIncrement { get; set; } = 2;


        public string AheadPlayerText { get; set; } = "Blue flag";

        public string BehindPlayerText { get; set; } = "Sorry!";

        public string DDC { get; set; } = "Arduino_Leonardo";
        
        public bool DDCEnabled { get; set; } = false;
        
        public bool DDCclutchEnabled { get; set; } = false;

        public bool DDSEnabled { get; set; } = false;

        public double fuelPerLapTarget { get; set; } = 2.50;

        public double fuelOffsetIncrement { get; set; } = 0.01;

        public bool supercarSwapPosition = false;

        public bool supercarARBdirection = false;

        #region Property supporting UI refresh from code
        /*
        private string _FilePath;
        public string FilePath
        {
            get => _FilePath;
            set => SetField(ref _FilePath, value);
        }
        */
        #endregion

        #region Utilities methods to refresh the UI see https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=netframework-4.7.2

        protected void OnPropertyChanged(string propertyName)
          => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion


    }


}