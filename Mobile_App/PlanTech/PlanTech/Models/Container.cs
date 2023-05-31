/*
 * Team: PlanTech (Team #1)
 * Semester: Winter 2022
 * Course: 420-6A6-AB APPLICATION DEVELOPMENT III
 * Date: 03 May 2022
 */

using SQLite;
namespace PlanTech.Models
{
    /// <summary>
    /// Represents a farm container.
    /// </summary>
    public class Container
    {
        /// <summary>
        /// The Id of the container.
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        /// <summary>
        /// The name of the container.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Indicates whether the container is rented.
        /// </summary>
        public bool IsRented { get; set; } = false;


        //[Unique]
        public string DeviceId { get; set; }


        /// <summary>
        /// The temperature inside the container.
        /// </summary>
        public float? Temperature { get; set; } = null;

        /// <summary>
        /// The humidity inside the container.
        /// </summary>
        public float? Humidity { get; set; } = null;


        /// <summary>
        /// The water level for the plants inside the container.
        /// </summary>
        public int? WaterLevel { get; set; } = null;

        /// <summary>
        /// The low threshold for water level
        /// </summary>
        public int WaterLevelLow { get; set; }

        /// <summary>
        /// The high threshold for water level
        /// </summary>
        public int WaterLevelHigh { get; set; }

        /// <summary>
        /// The moisture of the soil inside the container.
        /// </summary>
        public int? SoilMoisture { get; set; } = null;

        /// <summary>
        /// The low threshold for Soil Moisture
        /// </summary>
        public int SoilMoistureLow { get; set; }

        /// <summary>
        /// The high threshold for Soil Moisture
        /// </summary>
        public int SoilMoistureHigh { get; set; }


        /// <summary>
        /// Indicates whether the fan inside the container is on.
        /// </summary>
        public bool FanIsOn { get; set; } = false;


        /// <summary>
        /// Indicates whether the lights inside the container are on.
        /// </summary>
        public bool LightsAreOn { get; set; } = false;



        /// <summary>
        /// The location of the container.
        /// </summary>
        public string Location { get; set; } = null;


        /// <summary>
        /// The pitch angle of the container.
        /// </summary>
        public float? PitchAngle { get; set; } = null;


        /// <summary>
        /// The roll angle of the container.
        /// </summary>
        public float? RollAngle { get; set; } = null;


        /// <summary>
        /// The vibration level of the container.
        /// </summary>
        public float? Vibration { get; set; } = null;

        /// <summary>
        /// The low threshold for vibration
        /// </summary>
        public float VibrationLow { get; set; }

        /// <summary>
        /// The high threshold for vibration
        /// </summary>
        public float VibrationHigh { get; set; }


        /// <summary>
        /// The noise level inside the container.
        /// </summary>
        public int? NoiseLevel { get; set; } = null;

        /// <summary>
        /// The low threshold for noise level
        /// </summary>
        public int NoiseLow { get; set; }

        /// <summary>
        /// The high threshold for noise level
        /// </summary>
        public int NoiseHigh { get; set; }

        /// <summary>
        /// The luminosity level inside the container.
        /// </summary>
        public int? LuminosityLevel { get; set; } = null;

        /// <summary>
        /// The low threshold for luminosity level
        /// </summary>
        public int LuminosityLow { get; set; }

        /// <summary>
        /// The high threshold for luminosity level
        /// </summary>
        public int LuminosityHigh { get; set; }

        /// <summary>
        /// Indicates whether motion is detected inside the container.
        /// </summary>
        public bool? MotionIsDetected { get; set; } = false;


        /// <summary>
        /// Indicates whether the container's door is closed.
        /// </summary>
        public bool? DoorIsClosed { get; set; } = true;

        /// <summary>
        /// Indicates whether the container's door is locked.
        /// </summary>
        public bool? DoorIsLocked { get; set; } = true;



        /// <summary>
        /// Indicates whether the container's buzzer is on.
        /// </summary>
        public bool BuzzerIsOn { get; set; } = false;

        public float TemperatureHigh {get;set;}
        public float TemperatureLow {get;set;}
        public float HumidityHigh {get;set;}
        public float HumidityLow {get;set;}
        public float PitchAngleHigh {get;set;}
        public float PitchAngleLow {get;set;}
        public float RollAngleHigh {get;set;}
        public float RollAngleLow {get;set;}

        public Container Clone()
        {
            Container container = new Container();

            container.BuzzerIsOn = this.BuzzerIsOn;
            container.DeviceId = this.DeviceId;
            container.DoorIsClosed = this.DoorIsClosed;
            container.DoorIsLocked = this.DoorIsLocked;
            container.FanIsOn = this.FanIsOn;
            container.Humidity = this.Humidity;
            container.Id = this.Id;
            container.IsRented = this.IsRented;
            container.LightsAreOn = this.LightsAreOn;
            container.Location = this.Location;
            container.LuminosityHigh = this.LuminosityHigh;
            container.LuminosityLevel = this.LuminosityLevel;
            container.LuminosityLow = this.LuminosityLow;
            container.MotionIsDetected = this.MotionIsDetected;
            container.Name = this.Name;
            container.NoiseHigh = this.NoiseHigh;
            container.NoiseLevel = this.NoiseLevel;
            container.NoiseLow = this.NoiseLow;
            container.PitchAngle = this.PitchAngle;
            container.RollAngle = this.RollAngle;
            container.SoilMoisture = this.SoilMoisture;
            container.SoilMoistureHigh = this.SoilMoistureHigh;
            container.SoilMoistureLow = this.SoilMoistureLow;
            container.Temperature = this.Temperature;
            container.Vibration = this.Vibration;
            container.VibrationHigh = this.VibrationHigh;
            container.VibrationLow = this.VibrationLow;
            container.WaterLevel = this.WaterLevel;
            container.WaterLevelHigh = this.WaterLevelHigh;
            container.WaterLevelLow = this.WaterLevelLow;
            container.TemperatureHigh = this.TemperatureHigh;
            container.TemperatureLow = this.TemperatureLow;
            container.HumidityHigh = this.HumidityHigh;
            container.HumidityLow = this.HumidityLow;
            container.PitchAngleHigh = this.PitchAngleHigh;
            container.PitchAngleLow = this.PitchAngleLow;
            container.RollAngleHigh = this.RollAngleHigh;
            container.RollAngleLow = this.RollAngleLow;

            return container;
        }
    }
}
