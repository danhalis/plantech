using Microsoft.Azure.Devices.Shared;
using PlanTech.Models;
using System;
using System.Windows.Input;
using Xamarin.Forms;

/*
 * Team: PlanTech (Team #1)
 * Semester: Winter 2022
 * Course: 420-6A6-AB APPLICATION DEVELOPMENT III
 * Date: 03 May 2022
 */
namespace PlanTech.ViewModels
{
    /// <summary>
    /// Represents a signal level
    /// </summary>
    public enum Signal
    {
        None,
        Low,
        Medium,
        High
    }

    /// <summary>
    /// Represents a <see cref="ViewModel"/> for communication between a <see cref="Models.Container"/> and a <see cref="ContentPage"/>.
    /// </summary>
    public class ContainerViewModel : ViewModel
    {
        private bool _isActivated;

        /// <summary>
        /// Initializes a new instance of <see cref="ContainerViewModel"/> class.
        /// </summary>
        /// <param name="container"></param>
        public ContainerViewModel(Container container)
        {
            Container = container;
            DoorLockToggledCommand = new Command<bool>(UpdateDoorLockStateAction);
            LightToggledCommand = new Command<bool>(UpdateLightStateAction);
            FanToggledCommand = new Command<bool>(UpdateFanStateAction);
            BuzzerToggledCommand = new Command<bool>(UpdateBuzzerStateAction);
        }

        public Container Container { get; private set; }
        public Twin DeviceTwin { get; set; }

        /// <summary>
        /// The id of the container.
        /// </summary>
        public int Id => Container.Id;

        /// <summary>
        /// The id of the tracking device inside the container.
        /// </summary>
        public string DeviceId { get => Container.DeviceId; private set { Container.DeviceId = value; } }

        /// <summary>
        /// The name of the <see cref="Models.Container"/>
        /// </summary>
        public string Name
        {
            get => Container.Name;
            set
            {
                if (Container.Name == value)
                {
                    return;
                }

                Container.Name = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Indicates whether the security is activated.
        /// </summary>
        public bool SecurityIsActivated
        {
            get => _isActivated;
            private set
            {
                _isActivated = value;

                if (_isActivated == false && BuzzerIsOn)
                {
                    BuzzerIsOn = false;
                }
            }
        }

        /// The vibration level of the <see cref="Models.Container"/>
        /// </summary>
        public float? Vibration
        {
            get => Container.Vibration;
            set
            {
                if (Container.Vibration == value)
                {
                    return;
                }

                Container.Vibration = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(VibrationSignal));
            }
        }

        /// <summary>
        /// The motion of the <see cref="Models.Container"/>
        /// </summary>
        public bool? MotionIsDetected
        {
            get => Container.MotionIsDetected;
            set
            {
                if (Container.MotionIsDetected == value)
                {
                    return;
                }

                Container.MotionIsDetected = value;
                OnPropertyChanged();
            }
        }

        /// Activates the security.
        /// </summary>
        public void ActivateSecurity()
        {
            SecurityIsActivated = true;
        }
        /// <summary>
        /// Deactivates the security.
        /// </summary>
        public void DeactivateSecurity()
        {
            SecurityIsActivated = false;
        }

        /// <summary>
        /// Indicates whether the <see cref="Models.Container"/> is rented.
        /// </summary>
        public bool IsRented
        {
            get => Container.IsRented;
            set
            {
                if (Container.IsRented == value)
                {
                    return;
                }

                Container.IsRented = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Indicates whether the <see cref="Models.Container"/> is in alarming state.
        /// </summary>
        public bool IsAlarming
        {
            get
            {
                if (PitchAngle < PitchAngleLow || PitchAngle > PitchAngleHigh)
                    return true;
                if (RollAngle < RollAngleLow || RollAngle > RollAngleHigh)
                    return true;
                if (Vibration < VibrationLow || Vibration > VibrationHigh)
                    return true;
                if (NoiseLevel < NoiseLow || NoiseLevel > NoiseHigh)
                    return true;
                if (LuminosityLevel < LuminosityLow || LuminosityLevel > LuminosityHigh)
                    return true;

                return false;
            }
        }
        /// <summary>
        /// Indicates whether the <see cref="Models.Container"/> is in warning state.
        /// </summary>
        public bool IsWarning
        {
            get
            {
                if (Temperature < TemperatureLow || Temperature > TemperatureHigh)
                    return true;
                if (Humidity < HumidityLow || Humidity > HumidityHigh)
                    return true;
                if (WaterLevel < WaterLevelLow || WaterLevel > WaterLevelHigh)
                    return true;
                if (SoilMoisture < SoilMoistureLow || SoilMoisture > SoilMoistureHigh)
                    return true;

                return false;
            }
        }

        /// <summary>
        /// The temperature inside the container.
        /// </summary>
        public float? Temperature
        {

            get
            {
                return Container.Temperature;
            }
            set
            {
                if (Container.Temperature == value)
                    return;

                Container.Temperature = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The humidity inside the container.
        /// </summary>
        public float? Humidity
        {

            get
            {
                return Container.Humidity;
            }
            set
            {
                if (Container.Humidity == value)
                    return;

                Container.Humidity = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// The water level for the plants inside the container.
        /// </summary>
        public int? WaterLevel
        {

            get
            {
                return Container.WaterLevel;
            }
            set
            {
                if (Container.WaterLevel == value)
                    return;

                Container.WaterLevel = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(WaterLevelSignal));
            }
        }


        /// <summary>
        /// The moisture of the soil inside the container.
        /// </summary>
        public int? SoilMoisture
        {

            get
            {
                return Container.SoilMoisture;
            }
            set
            {
                if (Container.SoilMoisture == value)
                    return;

                Container.SoilMoisture = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SoilMoistureSignal));
            }
        }


        /// <summary>
        /// Indicates whether the fan inside the container is on.
        /// </summary>
        public bool FanIsOn
        {

            get
            {
                return Container.FanIsOn;
            }
            set
            {
                if (Container.FanIsOn == value)
                    return;

                FanToggledCommand.Execute(value);
                OnPropertyChanged();
            }
        }

        private ICommand FanToggledCommand { get; set; }

        private async void UpdateFanStateAction(bool value)
        {
            IsUpdatingFanState = true;
            if (value)
            {
                await (Application.Current as App).MainViewModel.UpdateDesiredProperty(AppData.DeviceTwinPropertyKeys.Fan, AppData.DeviceTwinPropertyValues.FanIsOn);

                Container.FanIsOn = true;
                OnPropertyChanged(nameof(FanIsOn));
            }
            else
            {
                await (Application.Current as App).MainViewModel.UpdateDesiredProperty(AppData.DeviceTwinPropertyKeys.Fan, AppData.DeviceTwinPropertyValues.FanIsOff);

                Container.FanIsOn = false;
                OnPropertyChanged(nameof(FanIsOn));
            }
        }

        private bool _isUpdatingFanState;
        public bool IsUpdatingFanState
        {
            get => _isUpdatingFanState;
            set
            {
                if (_isUpdatingFanState == value)
                    return;

                _isUpdatingFanState = value;

                OnPropertyChanged();
            }
        }


        /// <summary>
        /// Indicates whether the lights inside the container are on.
        /// </summary>
        public bool LightsAreOn
        {
            get
            {
                return Container.LightsAreOn;
            }
            set
            {
                if (Container.LightsAreOn == value)
                    return;

                LightToggledCommand.Execute(value);
                OnPropertyChanged();
            }
        }

        private ICommand LightToggledCommand { get; set; }

        private async void UpdateLightStateAction(bool value)
        {
            IsUpdatingLightState = true;
            if (value)
            {
                await (Application.Current as App).MainViewModel.UpdateDesiredProperty(AppData.DeviceTwinPropertyKeys.LED, AppData.DeviceTwinPropertyValues.LedIsOn);

                Container.LightsAreOn = true;
                OnPropertyChanged(nameof(LightsAreOn));
            }
            else
            {
                await (Application.Current as App).MainViewModel.UpdateDesiredProperty(AppData.DeviceTwinPropertyKeys.LED, AppData.DeviceTwinPropertyValues.LedIsOff);

                Container.LightsAreOn = false;
                OnPropertyChanged(nameof(LightsAreOn));
            }
        }

        private bool _isUpdatingLightState;
        public bool IsUpdatingLightState
        {
            get => _isUpdatingLightState;
            set
            {
                if (_isUpdatingLightState == value)
                    return;

                _isUpdatingLightState = value;

                OnPropertyChanged();
            }
        }



        /// <summary>
        /// The location of the container.
        /// </summary>
        public string Location
        {

            get
            {
                return Container.Location;
            }
            set
            {
                if (Container.Location == value)
                    return;

                Container.Location = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// The pitch angle of the container.
        /// </summary>
        public float? PitchAngle
        {

            get
            {
                return Container.PitchAngle;
            }
            set
            {
                if (Container.PitchAngle == value)
                    return;

                Container.PitchAngle = value;

                OnPropertyChanged(nameof(IsAlarming));
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// The roll angle of the container.
        /// </summary>
        public float? RollAngle
        {

            get
            {
                return Container.RollAngle;
            }
            set
            {
                if (Container.RollAngle == value)
                    return;

                Container.RollAngle = value;
                OnPropertyChanged(nameof(IsAlarming));
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// The noise level inside the container.
        /// </summary>
        public int? NoiseLevel
        {

            get
            {
                return Container.NoiseLevel;
            }
            set
            {
                if (Container.NoiseLevel == value)
                    return;

                Container.NoiseLevel = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(NoiseSignal));
            }
        }

        /// <summary>
        /// The luminosity level inside the container.
        /// </summary>
        public int? LuminosityLevel
        {

            get
            {
                return Container.LuminosityLevel;
            }
            set
            {
                if (Container.LuminosityLevel == value)
                    return;

                Container.LuminosityLevel = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(LuminositySignal));
            }
        }

        /// <summary>
        /// Indicates whether the container's door is closed.
        /// </summary>
        public bool? DoorIsClosed
        {

            get
            {
                return Container.DoorIsClosed;
            }
            set
            {
                if (Container.DoorIsClosed == value)
                    return;

                Container.DoorIsClosed = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Indicates whether the container's door is locked.
        /// </summary>
        public bool? DoorIsLocked
        {

            get
            {
                return Container.DoorIsLocked;
            }
            set
            {
                if (Container.DoorIsLocked == value)
                    return;

                DoorLockToggledCommand.Execute(value);
            }
        }

        public bool IsUpdatingSecurity
        {
            get
            {
                return IsUpdatingDoorLockState || IsUpdatingBuzzerState;

            }
        } 

        private bool _isUpdatingDoorLockState;
        public bool IsUpdatingDoorLockState
        {
            get => _isUpdatingDoorLockState;
            set
            {
                if (_isUpdatingDoorLockState == value)
                    return;

                _isUpdatingDoorLockState = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(IsUpdatingSecurity));
            }
        }

        private async void UpdateDoorLockStateAction(bool value)
        {
            IsUpdatingDoorLockState = true;
            if (value)
            {
                await (Application.Current as App).MainViewModel.UpdateDesiredProperty(AppData.DeviceTwinPropertyKeys.DoorLock, AppData.DeviceTwinPropertyValues.DoorIsLocked);

                Container.DoorIsLocked = true;
                OnPropertyChanged(nameof(DoorIsLocked));
            }
            else
            {
                await (Application.Current as App).MainViewModel.UpdateDesiredProperty(AppData.DeviceTwinPropertyKeys.DoorLock, AppData.DeviceTwinPropertyValues.DoorIsUnlocked);

                Container.DoorIsLocked = false;
                OnPropertyChanged(nameof(DoorIsLocked));
            }
        }

        private ICommand DoorLockToggledCommand { get; set; }

        /// <summary>
        /// Locks the container's door.
        /// </summary>
        public void LockDoor()
        {
            DoorIsLocked = true;
        }
        /// <summary>
        /// Unlocks the container's door.
        /// </summary>
        public void UnlockDoor()
        {
            DoorIsLocked = false;
        }

        /// <summary>
        /// Indicates whether the container's buzzer is on.
        /// </summary>
        public bool BuzzerIsOn
        {

            get
            {
                return Container.BuzzerIsOn;
            }
            set
            {
                if (Container.BuzzerIsOn == value)
                    return;

                BuzzerToggledCommand.Execute(value);
                OnPropertyChanged();
            }
        }

        private async void UpdateBuzzerStateAction(bool value)
        {
            IsUpdatingBuzzerState = true;
            if (value)
            {
                await (Application.Current as App).MainViewModel.UpdateDesiredProperty(AppData.DeviceTwinPropertyKeys.Buzzer, AppData.DeviceTwinPropertyValues.BuzzerIsOn);

                Container.BuzzerIsOn = true;
                OnPropertyChanged(nameof(BuzzerIsOn));
            }
            else
            {
                await (Application.Current as App).MainViewModel.UpdateDesiredProperty(AppData.DeviceTwinPropertyKeys.Buzzer, AppData.DeviceTwinPropertyValues.BuzzerIsOff);

                Container.BuzzerIsOn = false;
                OnPropertyChanged(nameof(BuzzerIsOn));
            }
        }

        private ICommand BuzzerToggledCommand { get; set; }

        private bool _isUpdatingBuzzerState;
        public bool IsUpdatingBuzzerState
        {
            get => _isUpdatingBuzzerState;
            set
            {
                if (_isUpdatingBuzzerState == value)
                    return;

                _isUpdatingBuzzerState = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(IsUpdatingSecurity));
            }
        }

        /// <summary>
        /// Turns on the container's buzzer.
        /// </summary>
        public void TurnBuzzerOn()
        {
            BuzzerIsOn = true;
        }
        /// <summary>
        /// Turns off the container's buzzer.
        /// </summary>
        public void TurnBuzzerOff()
        {
            BuzzerIsOn = false;
        }

        /// <summary>
        /// The low threshold for water level
        /// </summary>
        public int WaterLevelLow
        {

            get
            {
                return Container.WaterLevelLow;
            }
            set
            {
                if (Container.WaterLevelLow == value)
                    return;

                Container.WaterLevelLow = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(WaterLevelSignal));
            }
        }

        /// <summary>
        /// The high threshold for water level
        /// </summary>
        public int WaterLevelHigh
        {

            get
            {
                return Container.WaterLevelHigh;
            }
            set
            {
                if (Container.WaterLevelHigh == value)
                    return;

                Container.WaterLevelHigh = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(WaterLevelSignal));
            }
        }


        /// <summary>
        /// The low threshold for Soil Moisture
        /// </summary>
        public int SoilMoistureLow
        {

            get
            {
                return Container.SoilMoistureLow;
            }
            set
            {
                if (Container.SoilMoistureLow == value)
                    return;

                Container.SoilMoistureLow = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SoilMoistureSignal));
            }
        }

        /// <summary>
        /// The high threshold for Soil Moisture
        /// </summary>
        public int SoilMoistureHigh
        {

            get
            {
                return Container.SoilMoistureHigh;
            }
            set
            {
                if (Container.SoilMoistureHigh == value)
                    return;

                Container.SoilMoistureHigh = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SoilMoistureSignal));
            }
        }

        /// <summary>
        /// The low threshold for vibration
        /// </summary>
        public float VibrationLow
        {

            get
            {
                return Container.VibrationLow;
            }
            set
            {
                if (Container.VibrationLow == value)
                    return;

                Container.VibrationLow = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(VibrationSignal));
            }
        }

        /// <summary>
        /// The high threshold for vibration
        /// </summary>
        public float VibrationHigh
        {

            get
            {
                return Container.VibrationHigh;
            }
            set
            {
                if (Container.VibrationHigh == value)
                    return;

                Container.VibrationHigh = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(VibrationSignal));
            }
        }

        /// <summary>
        /// The low threshold for noise level
        /// </summary>
        public int NoiseLow
        {

            get
            {
                return Container.NoiseLow;
            }
            set
            {
                if (Container.NoiseLow == value)
                    return;

                Container.NoiseLow = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(NoiseSignal));
            }
        }

        /// <summary>
        /// The high threshold for noise level
        /// </summary>
        public int NoiseHigh
        {

            get
            {
                return Container.NoiseHigh;
            }
            set
            {
                if (Container.NoiseHigh == value)
                    return;

                Container.NoiseHigh = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(NoiseSignal));
            }
        }


        /// <summary>
        /// The low threshold for luminosity level
        /// </summary>
        public int LuminosityLow
        {

            get
            {
                return Container.LuminosityLow;
            }
            set
            {
                if (Container.LuminosityLow == value)
                    return;

                Container.LuminosityLow = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(LuminositySignal));
            }
        }

        /// <summary>
        /// The high threshold for luminosity level
        /// </summary>
        public int LuminosityHigh
        {

            get
            {
                return Container.LuminosityHigh;
            }
            set
            {
                if (Container.LuminosityHigh == value)
                    return;

                Container.LuminosityHigh = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(LuminositySignal));
            }
        }

        public float TemperatureHigh
        {
            get
            {
                return Container.TemperatureHigh;
            }
            set
            {
                if (Container.TemperatureHigh == value)
                    return;

                Container.TemperatureHigh = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsWarning));
            }
        }

        public float TemperatureLow
        {
            get
            {
                return Container.TemperatureLow;
            }
            set
            {
                if (Container.TemperatureLow == value)
                    return;

                Container.TemperatureLow = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsWarning));
            }
        }

        public float HumidityHigh
        {
            get
            {
                return Container.HumidityHigh;
            }
            set
            {
                if (Container.HumidityHigh == value)
                    return;

                Container.HumidityHigh = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsWarning));
            }
        }
        public float HumidityLow
        {
            get
            {
                return Container.HumidityLow;
            }
            set
            {
                if (Container.HumidityLow == value)
                    return;

                Container.HumidityLow = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsWarning));
            }
        }

        public float PitchAngleHigh
        {
            get
            {
                return Container.PitchAngleHigh;
            }
            set
            {
                if (Container.PitchAngleHigh == value)
                    return;

                Container.PitchAngleHigh = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsAlarming));
            }
        }

        public float PitchAngleLow
        {
            get
            {
                return Container.PitchAngleLow;
            }
            set
            {
                if (Container.PitchAngleLow == value)
                    return;

                Container.PitchAngleLow = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsAlarming));
            }
        }

        public float RollAngleHigh
        {
            get
            {
                return Container.RollAngleHigh;
            }
            set
            {
                if (Container.RollAngleHigh == value)
                    return;

                Container.RollAngleHigh = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsAlarming));
            }
        }

        public float RollAngleLow
        {
            get
            {
                return Container.RollAngleLow;
            }
            set
            {
                if (Container.RollAngleLow == value)
                    return;

                Container.RollAngleLow = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsAlarming));
            }
        }


        /// <summary>
        /// Determine the signal level for water level
        /// </summary>
        public Signal WaterLevelSignal
        {
            get
            {
                if (WaterLevel <= WaterLevelLow)
                    return Signal.Low;

                if (WaterLevel >= WaterLevelHigh)
                    return Signal.High;

                if (WaterLevel == null)
                    return Signal.None;

                return Signal.Medium;
            }
        }

        /// <summary>
        /// Determine the signal level for soil mooisture level
        /// </summary>
        public Signal SoilMoistureSignal
        {
            get
            {
                if (SoilMoisture <= SoilMoistureLow)
                    return Signal.Low;

                if (SoilMoisture >= SoilMoistureHigh)
                    return Signal.High;

                if (SoilMoisture == null)
                    return Signal.None;

                return Signal.Medium;
            }
        }

        /// <summary>
        /// Determine the signal level for vibration level
        /// </summary>
        public Signal VibrationSignal
        {
            get
            {
                if (Vibration <= VibrationLow)
                    return Signal.Low;

                if (Vibration >= VibrationHigh)
                    return Signal.High;

                if (Vibration == null)
                    return Signal.None;

                return Signal.Medium;
            }
        }

        /// <summary>
        /// Determine the signal level for noise level
        /// </summary>
        public Signal NoiseSignal
        {
            get
            {
                if (NoiseLevel <= NoiseLow)
                    return Signal.Low;

                if (NoiseLevel >= NoiseHigh)
                    return Signal.High;

                if (NoiseLevel == null)
                    return Signal.None;

                return Signal.Medium;
            }
        }

        /// <summary>
        /// Determine the signal level for Luminosity level
        /// </summary>
        public Signal LuminositySignal
        {
            get
            {
                if (LuminosityLevel <= LuminosityLow)
                    return Signal.Low;

                if (LuminosityLevel >= LuminosityHigh)
                    return Signal.High;

                if (LuminosityLevel == null)
                    return Signal.None;

                return Signal.Medium;
            }
        }


        /// <summary>
        /// Maps data from the given <see cref="Models.Container"/> object to this <see cref="ContainerViewModel"/>.<br/><br/>
        /// If one of the properties of the given <see cref="Models.Container"/> object is null, the corresponding property of this <see cref="ContainerViewModel"/> remains unchanged with whatever value it currently has.
        /// </summary>
        /// <param name="container">The <see cref="Models.Container"/> object.</param>
        public void MapData(Container container)
        {
            if (container.Location != null)
                Location = container.Location;

            if (container.PitchAngle != null)
                PitchAngle = container.PitchAngle;

            if (container.RollAngle != null)
                RollAngle = container.RollAngle;

            if (container.Vibration != null)
                Vibration = container.Vibration;

            if (container.LuminosityLevel != null)
                LuminosityLevel = container.LuminosityLevel;

            if (container.NoiseLevel != null)
                NoiseLevel = container.NoiseLevel;

            if (container.DoorIsClosed != null)
                DoorIsClosed = container.DoorIsClosed;

            if (container.Temperature != null)
                Temperature = container.Temperature;

            if (container.Humidity != null)
                Humidity = container.Humidity;

            if (container.WaterLevel != null)
                WaterLevel = container.WaterLevel;

            if (container.SoilMoisture != null)
                SoilMoisture = container.SoilMoisture;
        }

        public void MapData(TwinCollection twinCollection)
        {
            if (twinCollection[AppData.DeviceTwinPropertyKeys.Buzzer] == AppData.DeviceTwinPropertyValues.BuzzerIsOn)
            {
                Container.BuzzerIsOn = true;
                IsUpdatingBuzzerState = false;
                OnPropertyChanged(nameof(BuzzerIsOn));
            }
            else if (twinCollection[AppData.DeviceTwinPropertyKeys.Buzzer] == AppData.DeviceTwinPropertyValues.BuzzerIsOff)
            {
                Container.BuzzerIsOn = false;
                IsUpdatingBuzzerState = false;
                OnPropertyChanged(nameof(BuzzerIsOn));
            }

            if (twinCollection[AppData.DeviceTwinPropertyKeys.Fan] == AppData.DeviceTwinPropertyValues.FanIsOn)
            {
                Container.FanIsOn = true;
                IsUpdatingFanState = false;
                OnPropertyChanged(nameof(FanIsOn));
            }
            else if (twinCollection[AppData.DeviceTwinPropertyKeys.Fan] == AppData.DeviceTwinPropertyValues.FanIsOff)
            {
                Container.FanIsOn = false;
                IsUpdatingFanState = false;
                OnPropertyChanged(nameof(FanIsOn));
            }

            if (twinCollection[AppData.DeviceTwinPropertyKeys.DoorLock] == AppData.DeviceTwinPropertyValues.DoorIsLocked)
            {
                Container.DoorIsLocked = true;
                IsUpdatingDoorLockState = false;
                OnPropertyChanged(nameof(DoorIsLocked));
            }
            else if (twinCollection[AppData.DeviceTwinPropertyKeys.DoorLock] == AppData.DeviceTwinPropertyValues.DoorIsUnlocked)
            {
                Container.DoorIsLocked = false;
                IsUpdatingDoorLockState = false;
                OnPropertyChanged(nameof(DoorIsLocked));
            }

            if (twinCollection[AppData.DeviceTwinPropertyKeys.LED] == AppData.DeviceTwinPropertyValues.LedIsOn)
            {
                Container.LightsAreOn = true;
                IsUpdatingLightState = false;
                OnPropertyChanged(nameof(LightsAreOn));
            }
            else if (twinCollection[AppData.DeviceTwinPropertyKeys.LED] == AppData.DeviceTwinPropertyValues.LedIsOff)
            {
                Container.LightsAreOn = false;
                IsUpdatingLightState = false;
                OnPropertyChanged(nameof(LightsAreOn));
            }
        }

        public void SetProperties(ContainerViewModel container)
        {
            this.BuzzerIsOn = container.BuzzerIsOn;
            this.DeviceId = container.DeviceId;
            this.DoorIsClosed = container.DoorIsClosed;
            this.DoorIsLocked = container.DoorIsLocked;
            this.FanIsOn = container.FanIsOn;
            this.Humidity = container.Humidity;
            this.IsRented = container.IsRented;
            this.LightsAreOn = container.LightsAreOn;
            this.Location = container.Location;
            this.LuminosityHigh = container.LuminosityHigh;
            this.LuminosityLevel = container.LuminosityLevel;
            this.LuminosityLow = container.LuminosityLow;
            this.MotionIsDetected = container.MotionIsDetected;
            this.Name = container.Name;
            this.NoiseHigh = container.NoiseHigh;
            this.NoiseLevel = container.NoiseLevel;
            this.NoiseLow = container.NoiseLow;
            this.PitchAngle = container.PitchAngle;
            this.RollAngle = container.RollAngle;
            this.SoilMoisture = container.SoilMoisture;
            this.SoilMoistureHigh = container.SoilMoistureHigh;
            this.SoilMoistureLow = container.SoilMoistureLow;
            this.Temperature = container.Temperature;
            this.Vibration = container.Vibration;
            this.VibrationHigh = container.VibrationHigh;
            this.VibrationLow = container.VibrationLow;
            this.WaterLevel = container.WaterLevel;
            this.WaterLevelHigh = container.WaterLevelHigh;
            this.WaterLevelLow = container.WaterLevelLow;
            this.TemperatureHigh = container.TemperatureHigh;
            this.TemperatureLow = container.TemperatureLow;
            this.HumidityHigh = container.HumidityHigh;
            this.HumidityLow = container.HumidityLow;
            this.PitchAngleHigh = container.PitchAngleHigh;
            this.PitchAngleLow = container.PitchAngleLow;
            this.RollAngleHigh = container.RollAngleHigh;
            this.RollAngleLow = container.RollAngleLow;
        }

        public ContainerViewModel Clone()
        {
            return new ContainerViewModel(this.Container.Clone());
        }
    }
}