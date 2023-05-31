using System;
using System.Collections.Generic;
using System.Text;

namespace PlanTech.ViewModels
{
    /// <summary>
    /// Represents a <see cref="ViewModel"/> for communication with <see cref="Views.ThresholdFormPage"/>.
    /// </summary>
    public class DashboardSettingViewModel : ViewModel
    {

        private ContainerViewModel _settingSelectedContainerViewModel;

        /// <summary>
        /// Represents the selected container view model with setting changes.
        /// </summary>
        public ContainerViewModel SettingSelectedContainerViewModel
        {
            get => _settingSelectedContainerViewModel;
            set
            {
                if (_settingSelectedContainerViewModel == value)
                {
                    return;
                }

                _settingSelectedContainerViewModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Whether or not the settings are valid
        /// </summary>
        public bool IsValid
        {
            get
            {
                return IsWaterLowValid && IsWaterHighValid && IsSoilMoistureLowValid && IsSoilMoistureHighValid && IsVibrationLowValid && IsVibrationHighValid && IsNoiseLowValid && IsNoiseHighValid && IsLuminosityLowValid && IsLuminoistyHighValid && IsTempLowValid && IsTempHighValid && IsHumiLowValid && IsHumiHighValid && IsPitchAngleLowValid && IsPitchAngleHighValid && IsRollAngleLowValid && IsRollAngleHighValid;
            }
        }

        private bool _isWaterLowValid;
        private bool _isWaterHighValid;
        private bool _isSoilMoistureLowValid;
        private bool _isSoilMoistureHighValid;
        private bool _isVibrationLowValid;
        private bool _isVibrationHighValid;
        private bool _isNoiseLowValid;
        private bool _isNoiseHighValid;
        private bool _isLuminosityLowValid;
        private bool _isLuminoistyHighValid;

        /// <summary>
        /// Determine wheter the low water level threshold is valid
        /// </summary>
        public bool IsWaterLowValid 
        {
            get
            {
                return _isWaterLowValid;
            }
            set
            {
                if (_isWaterLowValid == value)
                    return;

                _isWaterLowValid = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        /// <summary>
        /// Determine wheter the high water level threshold is valid
        /// </summary>
        public bool IsWaterHighValid
        {
            get
            {
                return _isWaterHighValid;
            }
            set
            {
                if (_isWaterHighValid == value)
                    return;

                _isWaterHighValid = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        /// <summary>
        /// Determine wheter the low soil moisture level threshold is valid
        /// </summary>
        public bool IsSoilMoistureLowValid
        {
            get
            {
                return _isSoilMoistureLowValid;
            }
            set
            {
                if (_isSoilMoistureLowValid == value)
                    return;

                _isSoilMoistureLowValid = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        /// <summary>
        /// Determine wheter the high soil moisture level threshold is valid
        /// </summary>
        public bool IsSoilMoistureHighValid
        {
            get
            {
                return _isSoilMoistureHighValid;
            }
            set
            {
                if (_isSoilMoistureHighValid == value)
                    return;

                _isSoilMoistureHighValid = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        /// <summary>
        /// Determine wheter the low vibration level threshold is valid
        /// </summary>
        public bool IsVibrationLowValid
        {
            get
            {
                return _isVibrationLowValid;
            }
            set
            {
                if (_isVibrationLowValid == value)
                    return;

                _isVibrationLowValid = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        /// <summary>
        /// Determine wheter the high vibration level threshold is valid
        /// </summary>
        public bool IsVibrationHighValid
        {
            get
            {
                return _isVibrationHighValid;
            }
            set
            {
                if (_isVibrationHighValid == value)
                    return;

                _isVibrationHighValid = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        /// <summary>
        /// Determine wheter the low noise level threshold is valid
        /// </summary>
        public bool IsNoiseLowValid
        {
            get
            {
                return _isNoiseLowValid;
            }
            set
            {
                if (_isNoiseLowValid == value)
                    return;

                _isNoiseLowValid = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        /// <summary>
        /// Determine wheter the high noise level threshold is valid
        /// </summary>
        public bool IsNoiseHighValid
        {
            get
            {
                return _isNoiseHighValid;
            }
            set
            {
                if (_isNoiseHighValid == value)
                    return;

                _isNoiseHighValid = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        /// <summary>
        /// Determine wheter the low luminosity level threshold is valid
        /// </summary>
        public bool IsLuminosityLowValid
        {
            get
            {
                return _isLuminosityLowValid;
            }
            set
            {
                if (_isLuminosityLowValid == value)
                    return;

                _isLuminosityLowValid = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        /// <summary>
        /// Determine wheter the high luminosity level threshold is valid
        /// </summary>
        public bool IsLuminoistyHighValid
        {
            get
            {
                return _isLuminoistyHighValid;
            }
            set
            {
                if (_isLuminoistyHighValid == value)
                    return;

                _isLuminoistyHighValid = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        private bool _isTempHighValid;
        public bool IsTempHighValid
        {
            get
            {
                return _isTempHighValid;
            }
            set
            {
                if (_isTempHighValid == value)
                    return;

                _isTempHighValid = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        private bool _isTempLowValid;
        public bool IsTempLowValid
        {
            get
            {
                return _isTempLowValid;
            }
            set
            {
                if (_isTempLowValid == value)
                    return;

                _isTempLowValid = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        private bool _isHumiLowValid;
        public bool IsHumiLowValid
        {
            get
            {
                return _isHumiLowValid;
            }
            set
            {
                if (_isHumiLowValid == value)
                    return;

                _isHumiLowValid = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        private bool _isHumiHighValid;
        public bool IsHumiHighValid
        {
            get
            {
                return _isHumiHighValid;
            }
            set
            {
                if (_isHumiHighValid == value)
                    return;

                _isHumiHighValid = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        private bool _isPitchAngleHighValid;
        public bool IsPitchAngleHighValid
        {
            get
            {
                return _isPitchAngleHighValid;
            }
            set
            {
                if (_isPitchAngleHighValid == value)
                    return;

                _isPitchAngleHighValid = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        private bool _isPitchAngleLowValid;
        public bool IsPitchAngleLowValid
        {
            get
            {
                return _isPitchAngleLowValid;
            }
            set
            {
                if (_isPitchAngleLowValid == value)
                    return;

                _isPitchAngleLowValid = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        private bool _isRollAngleLowValid;
        public bool IsRollAngleLowValid
        {
            get
            {
                return _isRollAngleLowValid;
            }
            set
            {
                if (_isRollAngleLowValid == value)
                    return;

                _isRollAngleLowValid = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        private bool _isRollAngleHighValid;
        public bool IsRollAngleHighValid
        {
            get
            {
                return _isRollAngleHighValid;
            }
            set
            {
                if (_isRollAngleHighValid == value)
                    return;

                _isRollAngleHighValid = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        /// <summary>
        /// Determine wheter the threshold settings changes of SettingSelectedContainerViewModel are equal to the given ContainerViewModel.
        /// </summary>
        /// <param name="containerViewModel">The ContainerViewModel to compare to</param>
        /// <returns>True if settings are equal, False otherwise.</returns>
        public bool AreSettingsEqual(ContainerViewModel containerViewModel)
        {
            return SettingSelectedContainerViewModel.WaterLevelHigh == containerViewModel.WaterLevelHigh && SettingSelectedContainerViewModel.WaterLevelLow == containerViewModel.WaterLevelLow && SettingSelectedContainerViewModel.SoilMoistureHigh == containerViewModel.SoilMoistureHigh && SettingSelectedContainerViewModel.SoilMoistureLow == containerViewModel.SoilMoistureLow && SettingSelectedContainerViewModel.VibrationHigh == containerViewModel.VibrationHigh && SettingSelectedContainerViewModel.VibrationLow == containerViewModel.VibrationLow && SettingSelectedContainerViewModel.NoiseHigh == containerViewModel.NoiseHigh && SettingSelectedContainerViewModel.NoiseLow == containerViewModel.NoiseLow && SettingSelectedContainerViewModel.LuminosityHigh == containerViewModel.LuminosityHigh && SettingSelectedContainerViewModel.LuminosityLow == containerViewModel.LuminosityLow && SettingSelectedContainerViewModel.TemperatureHigh == containerViewModel.TemperatureHigh && SettingSelectedContainerViewModel.TemperatureLow == containerViewModel.TemperatureLow && SettingSelectedContainerViewModel.HumidityHigh == containerViewModel.HumidityHigh && SettingSelectedContainerViewModel.HumidityLow == containerViewModel.HumidityLow && SettingSelectedContainerViewModel.PitchAngleHigh == containerViewModel.PitchAngleHigh && SettingSelectedContainerViewModel.PitchAngleLow == containerViewModel.PitchAngleLow && SettingSelectedContainerViewModel.RollAngleHigh == containerViewModel.RollAngleHigh && SettingSelectedContainerViewModel.RollAngleLow == containerViewModel.RollAngleLow;
        }


    }
}
