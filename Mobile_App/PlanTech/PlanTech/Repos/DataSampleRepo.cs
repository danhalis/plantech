using PlanTech.Models;
using System;
using System.Collections.Generic;

/*
 * Team: PlanTech (Team #1)
 * Semester: Winter 2022
 * Course: 420-6A6-AB APPLICATION DEVELOPMENT III
 * Date: 03 May 2022
 */
namespace PlanTech.Repos
{
    /// <summary>
    /// Static class to provide sample data for all hardware components.
    /// </summary>
    public static class DataSampleRepo
    {
        /// <summary>
        /// Creates mock pitch and roll angle data.
        /// </summary>
        /// <returns>
        /// An array containing a list of pitch angle sample data and a list of roll angle sample data.
        /// </returns>
        public static List<DataSample>[] GetAngleDataSamples()
        {
            List<DataSample> pitch = new List<DataSample>();
            List<DataSample> roll = new List<DataSample>();

            // Sample 1
            DateTime timestamp = DateTime.Now;
            pitch.Add(new DataSample() { Value = 1.1f, Timestamp = timestamp });
            roll.Add(new DataSample() { Value = -0.4f, Timestamp = timestamp });

            // Sample 2
            timestamp = DateTime.Now; pitch.Add(new DataSample() { Value = 0.1f, Timestamp = timestamp });
            pitch.Add(new DataSample() { Value = 0.1f, Timestamp = timestamp });
            roll.Add(new DataSample() { Value = 0.2f, Timestamp = timestamp });

            // Sample 3
            timestamp = DateTime.Now;
            pitch.Add(new DataSample() { Value = -0.7f, Timestamp = timestamp });
            roll.Add(new DataSample() { Value = 0.3f, Timestamp = timestamp });

            return new List<DataSample>[] { pitch, roll };
        }

        /// <summary>
        /// Creates mock vibration data.
        /// </summary>
        /// <returns>
        /// A list of vibration sample data.
        /// </returns>
        public static List<DataSample> GetVibrationDataSamples()
        {
            return new List<DataSample>()
            {
                new DataSample() { Value = 0.1f, Timestamp = DateTime.Now},
                new DataSample() { Value = 0.0f, Timestamp = DateTime.Now},
                new DataSample() { Value = 0.2f, Timestamp = DateTime.Now},
            };
        }

        /// <summary>
        /// Creates mock sound data.
        /// </summary>
        /// <returns>
        /// A list of sound sample data.
        /// </returns>
        public static List<DataSample> GetSoundDataSamples()
        {
            return new List<DataSample>()
            {
                new DataSample() { Value = 24.3f, Timestamp = DateTime.Now},
                new DataSample() { Value = 19.9f, Timestamp = DateTime.Now},
                new DataSample() { Value = 15.2f, Timestamp = DateTime.Now},
                new DataSample() { Value = 11.0f, Timestamp = DateTime.Now},
                new DataSample() { Value = 18.6f, Timestamp = DateTime.Now},
                new DataSample() { Value = 17.2f, Timestamp = DateTime.Now},
            };
        }

        /// <summary>
        /// Creates mock luminosity data.
        /// </summary>
        /// <returns>
        /// A list of luminosity sample data.
        /// </returns>
        public static List<DataSample> GetLuminosityDataSamples()
        {
            return new List<DataSample>()
            {
                new DataSample() { Value = 48.8f, Timestamp = DateTime.Now},
                new DataSample() { Value = 47.9f, Timestamp = DateTime.Now},
                new DataSample() { Value = 48.2f, Timestamp = DateTime.Now},
                new DataSample() { Value = 48.0f, Timestamp = DateTime.Now},
            };
        }

        /// <summary>
        /// Creates mock temperature and humidity data.
        /// </summary>
        /// <returns>
        /// An array containing a list of temperature sample data and a list of humidity sangle data.
        /// </returns>
        public static List<DataSample>[] GetTemperatureDataSamples()
        {
            List<DataSample> temperature = new List<DataSample>();
            List<DataSample> humidity = new List<DataSample>();

            // Sample 1
            DateTime timestamp = DateTime.Now;
            temperature.Add(new DataSample() { Value = 24.2f, Timestamp = timestamp });
            humidity.Add(new DataSample() { Value = 19.9f, Timestamp = timestamp });

            // Sample 2
            timestamp = DateTime.Now;
            temperature.Add(new DataSample() { Value = 24.3f, Timestamp = timestamp });
            humidity.Add(new DataSample() { Value = 19.9f, Timestamp = timestamp });

            // Sample 3
            timestamp = DateTime.Now;
            temperature.Add(new DataSample() { Value = 24.1f, Timestamp = timestamp });
            humidity.Add(new DataSample() { Value = 19.8f, Timestamp = timestamp });

            return new List<DataSample>[] { temperature, humidity };
        }

        /// <summary>
        /// Creates mock water level data.
        /// </summary>
        /// <returns>
        /// A list of water level sample data.
        /// </returns>
        public static List<DataSample> GetWaterDataSamples()
        {
            return new List<DataSample>()
            {
                new DataSample() { Value = 0.2f, Timestamp = DateTime.Now},
                new DataSample() { Value = 0.2f, Timestamp = DateTime.Now},
                new DataSample() { Value = 0.2f, Timestamp = DateTime.Now},
                new DataSample() { Value = 0.2f, Timestamp = DateTime.Now},
                new DataSample() { Value = 0.2f, Timestamp = DateTime.Now},
                new DataSample() { Value = 0.2f, Timestamp = DateTime.Now},
                new DataSample() { Value = 0.2f, Timestamp = DateTime.Now},
            };
        }

        /// <summary>
        /// Creates mock moisture data.
        /// </summary>
        /// <returns>
        /// A list of moisture sample data.
        /// </returns>
        public static List<DataSample> GetMoistureDataSamples()
        {
            return new List<DataSample>()
            {
                new DataSample() { Value = 0.4f, Timestamp = DateTime.Now},
                new DataSample() { Value = 0.4f, Timestamp = DateTime.Now},
                new DataSample() { Value = 0.5f, Timestamp = DateTime.Now},
                new DataSample() { Value = 0.5f, Timestamp = DateTime.Now},
                new DataSample() { Value = 0.5f, Timestamp = DateTime.Now},
            };
        }
    }
}
