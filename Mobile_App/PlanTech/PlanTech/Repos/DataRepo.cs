using PlanTech.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PlanTech.Repos
{
    /// <summary>
    /// Represents a repository of <see cref="Container"/> objects that provides database operations.
    /// </summary>
    public class DataRepo
    {
        int DATA_LIMIT = 1000;
        SQLiteAsyncConnection conn;

        public string Message { get; private set; }
        private const string CONTAINER_SAVE_FILE_FORMAT = "{0}_{1}";

        public DataRepo(string dbPath)
        {
            conn = new SQLiteAsyncConnection(dbPath);
            conn.CreateTableAsync(typeof(Container)).Wait();
            conn.CreateTableAsync(typeof(DataSample)).Wait();

            // Comment out after running once.
            //conn.InsertAllAsync(Load()).Wait();
        }

        /// <summary>
        /// Loads sample <see cref="Container"/> objects into the database.
        /// </summary>
        /// <returns>A <see cref="List{T}"/> of <see cref="Container"/> objects.</returns>
        public static IEnumerable<Container> Load()
        {
            const int DEFAULT_CAPACITY = 3;
            List<Container> containers = new List<Container>(DEFAULT_CAPACITY);
            const int DEFAULT_WATER_HIGH = 500;
            const int DEFAULT_WATER_Low = 100;
            
            const int DEFAULT_SOILMOISTURE_HIGH = 200;
            const int DEFAULT_SOILMOISTURE_LOW = 50;
            
            const float DEFAULT_VIBRATION_HIGH = 2000;
            const float DEFAULT_VIBRATION_LOW = 1000;
            
            const int DEFAULT_NOISE_HIGH = 20;
            const int DEFAULT_NOISE_LOW = 5;

            const int DEFAULT_LUMINOSITY_HIGH = 70;
            const int DEFAULT_LUMINOSITY_LOW = 10;

            const float DEFAULT_TEMP_HIGH = 24;
            const float DEFAULT_TEMP_LOW = 20;

            const float DEFAULT_HUMI_HIGH = 35;
            const float DEFAULT_HUMI_LOW = 25;

            const float DEFAULT_PITCH_HIGH = 1;
            const float DEFAULT_PITCH_LOW = -1;
            
            const float DEFAULT_ROLL_HIGH = 1;
            const float DEFAULT_ROLL_LOW = -1;

            containers.Add(new Container
            {
                Name = "Big Container 1",
                DeviceId = "device_1",
                IsRented = true,
                WaterLevelHigh = DEFAULT_WATER_HIGH,
                WaterLevelLow = DEFAULT_WATER_Low,
                SoilMoistureHigh = DEFAULT_SOILMOISTURE_HIGH,
                SoilMoistureLow = DEFAULT_SOILMOISTURE_LOW,
                VibrationHigh = DEFAULT_VIBRATION_HIGH,
                VibrationLow = DEFAULT_VIBRATION_LOW,
                NoiseHigh = DEFAULT_NOISE_HIGH,
                NoiseLow = DEFAULT_NOISE_LOW,
                LuminosityHigh = DEFAULT_LUMINOSITY_HIGH,
                LuminosityLow = DEFAULT_LUMINOSITY_LOW,
                TemperatureHigh = DEFAULT_TEMP_HIGH,
                TemperatureLow = DEFAULT_TEMP_LOW,
                HumidityHigh = DEFAULT_HUMI_HIGH,
                HumidityLow = DEFAULT_HUMI_LOW,
                PitchAngleHigh = DEFAULT_PITCH_HIGH,
                PitchAngleLow = DEFAULT_PITCH_LOW,
                RollAngleHigh = DEFAULT_ROLL_HIGH,
                RollAngleLow = DEFAULT_ROLL_LOW
            }) ;
            containers.Add(new Container
            {
                Name = "Small Container 2",
                DeviceId = "device_2",
                WaterLevelHigh = DEFAULT_WATER_HIGH,
                WaterLevelLow = DEFAULT_WATER_Low,
                SoilMoistureHigh = DEFAULT_SOILMOISTURE_HIGH,
                SoilMoistureLow = DEFAULT_SOILMOISTURE_LOW,
                VibrationHigh = DEFAULT_VIBRATION_HIGH,
                VibrationLow = DEFAULT_VIBRATION_LOW,
                NoiseHigh = DEFAULT_NOISE_HIGH,
                NoiseLow = DEFAULT_NOISE_LOW,
                LuminosityHigh = DEFAULT_LUMINOSITY_HIGH,
                LuminosityLow = DEFAULT_LUMINOSITY_LOW,
                TemperatureHigh = DEFAULT_TEMP_HIGH,
                TemperatureLow = DEFAULT_TEMP_LOW,
                HumidityHigh = DEFAULT_HUMI_HIGH,
                HumidityLow = DEFAULT_HUMI_LOW,
                PitchAngleHigh = DEFAULT_PITCH_HIGH,
                PitchAngleLow = DEFAULT_PITCH_LOW,
                RollAngleHigh = DEFAULT_ROLL_HIGH,
                RollAngleLow = DEFAULT_ROLL_LOW

            });
            containers.Add(new Container
            {
                Name = "Medium Container 3",
                DeviceId = "device_3",
                WaterLevelHigh = DEFAULT_WATER_HIGH,
                WaterLevelLow = DEFAULT_WATER_Low,
                SoilMoistureHigh = DEFAULT_SOILMOISTURE_HIGH,
                SoilMoistureLow = DEFAULT_SOILMOISTURE_LOW,
                VibrationHigh = DEFAULT_VIBRATION_HIGH,
                VibrationLow = DEFAULT_VIBRATION_LOW,
                NoiseHigh = DEFAULT_NOISE_HIGH,
                NoiseLow = DEFAULT_NOISE_LOW,
                LuminosityHigh = DEFAULT_LUMINOSITY_HIGH,
                LuminosityLow = DEFAULT_LUMINOSITY_LOW,
                TemperatureHigh = DEFAULT_TEMP_HIGH,
                TemperatureLow = DEFAULT_TEMP_LOW,
                HumidityHigh = DEFAULT_HUMI_HIGH,
                HumidityLow = DEFAULT_HUMI_LOW,
                PitchAngleHigh = DEFAULT_PITCH_HIGH,
                PitchAngleLow = DEFAULT_PITCH_LOW,
                RollAngleHigh = DEFAULT_ROLL_HIGH,
                RollAngleLow = DEFAULT_ROLL_LOW
            });

            return containers;
        }

        /// <summary>
        /// Converts a <see cref="TelemetryMessage"/> object to a <see cref="Container"/> object.
        /// </summary>
        /// <param name="telemetryMessage"></param>
        /// <returns>The <see cref="Container"/> object if the given telemetry message is not null, null otherwise</returns>
        public Container ConvertToContainerFrom(TelemetryMessage telemetryMessage)
        {
            if (telemetryMessage == null)
                return null;

            Container container = new Container();

            if (telemetryMessage.GeoLocation.Address != null)
                container.Location = telemetryMessage.GeoLocation.Address;

            if (telemetryMessage.GeoLocation.Angles["pitch"] != null)
                container.PitchAngle = Convert.ToSingle(telemetryMessage.GeoLocation.Angles["pitch"]);

            if (telemetryMessage.GeoLocation.Angles["roll"] != null)
                container.RollAngle = Convert.ToSingle(telemetryMessage.GeoLocation.Angles["roll"]);

            if (telemetryMessage.GeoLocation.Vibration != null)
                container.Vibration = Convert.ToSingle(telemetryMessage.GeoLocation.Vibration);

            if (telemetryMessage.Security["luminosity"] != null)
                container.LuminosityLevel = Convert.ToInt32(telemetryMessage.Security["luminosity"]);

            if (telemetryMessage.Security["sound"] != null)
                container.NoiseLevel = Convert.ToInt32(telemetryMessage.Security["sound"]);

            if (telemetryMessage.Security["door_is_closed"] != null)
                container.DoorIsClosed = Convert.ToBoolean(telemetryMessage.Security["door_is_closed"]);

            if (telemetryMessage.Plant["temperature"] != null)
                container.Temperature = Convert.ToSingle(telemetryMessage.Plant["temperature"]);

            if (telemetryMessage.Plant["humidity"] != null)
                container.Humidity = Convert.ToSingle(telemetryMessage.Plant["humidity"]);

            if (telemetryMessage.Plant["water_level"] != null)
                container.WaterLevel = Convert.ToInt32(telemetryMessage.Plant["water_level"]);

            if (telemetryMessage.Plant["soil_moisture"] != null)
                container.SoilMoisture = Convert.ToInt32(telemetryMessage.Plant["soil_moisture"]);

            return container;
        }

        /// <summary>
        /// Save all container sensor data that is included in the provided telemetry message.
        /// </summary>
        /// <param name="telemetryMessage">The telemetry message</param>
        /// <param name="timestamp">The timestamp of the telemtry message</param>
        /// <param name="containerId">The ID of the container that the sensor data was retrieved from</param>
        public void SaveContainerSensorData(TelemetryMessage telemetryMessage, DateTime timestamp, string containerId)
        {
            if (telemetryMessage.GeoLocation.Angles["pitch"] != null)
                SaveSensorData(new DataSample() { Sensor = "pitch", Value = Convert.ToSingle(telemetryMessage.GeoLocation.Angles["pitch"]), Timestamp = timestamp, ContainerId = containerId });

            if (telemetryMessage.GeoLocation.Angles["roll"] != null)
                SaveSensorData(new DataSample() { Sensor = "roll", Value = Convert.ToSingle(telemetryMessage.GeoLocation.Angles["roll"]), Timestamp = timestamp, ContainerId = containerId });

            if (telemetryMessage.GeoLocation.Vibration != null)
                SaveSensorData(new DataSample() { Sensor = "vibration", Value = Convert.ToSingle(telemetryMessage.GeoLocation.Vibration), Timestamp = timestamp, ContainerId = containerId });

            if (telemetryMessage.Security["luminosity"] != null)
                SaveSensorData(new DataSample() { Sensor = "luminosity", Value = Convert.ToSingle(telemetryMessage.Security["luminosity"]), Timestamp = timestamp, ContainerId = containerId });

            if (telemetryMessage.Security["sound"] != null)
                SaveSensorData(new DataSample() { Sensor = "sound", Value = Convert.ToSingle(telemetryMessage.Security["sound"]), Timestamp = timestamp, ContainerId = containerId });

            if (telemetryMessage.Plant["temperature"] != null)
                SaveSensorData(new DataSample() { Sensor = "temperature", Value = Convert.ToSingle(telemetryMessage.Plant["temperature"]), Timestamp = timestamp, ContainerId = containerId });

            if (telemetryMessage.Plant["humidity"] != null)
                SaveSensorData(new DataSample() { Sensor = "humidity", Value = Convert.ToSingle(telemetryMessage.Plant["humidity"]), Timestamp = timestamp, ContainerId = containerId });

            if (telemetryMessage.Plant["water_level"] != null)
                SaveSensorData(new DataSample() { Sensor = "water_level", Value = Convert.ToSingle(telemetryMessage.Plant["water_level"]), Timestamp = timestamp, ContainerId = containerId });

            if (telemetryMessage.Plant["soil_moisture"] != null)
                SaveSensorData(new DataSample() { Sensor = "soil_moisture", Value = Convert.ToSingle(telemetryMessage.Plant["soil_moisture"]), Timestamp = timestamp, ContainerId = containerId });
        }

        /// <summary>
        /// Retrieves a <see cref="Container"/> object by its device id.
        /// </summary>
        /// <param name="deviceId">The device id on Azure IoT Hub</param>
        /// <returns>The <see cref="Container"/> object if it was found with the given device id, null otherwise</returns>
        public async Task<Container> GetContainerByDeviceId(string deviceId)
        {
            try
            {
                Container container = await conn.Table<Container>().FirstAsync(c => c.DeviceId == deviceId);
                return container;
            }
            catch (InvalidOperationException)
            {
                Message = "No containers was found with the given device id.";
                return null;
            }
        }

        /// <summary>
        /// Retrieves all <see cref="Container"/> objects.
        /// </summary>
        /// <returns>The <see cref="List{T}"/> of <see cref="Container"/> objects</returns>
        public async Task<List<Container>> GetContainers()
        {
            return await conn.Table<Container>().ToListAsync();
        }

        /// <summary>
        /// Updates a list of <see cref="Container"/> objects.
        /// </summary>
        /// <param name="containers">The list of <see cref="Container"/> objects</param>
        public async Task UpdateContainers(IEnumerable<Container> containers)
        {
            Message = "Updating all containers...";
            _ = await conn.UpdateAllAsync(containers);
        }
        
        /// <summary>
        /// Serializes given object to JSON string and saves it to json file.
        /// </summary>
        /// <param name="obj">The object to save</param>
        /// <param name="fileName">The file name. '.json' extension should not be included</param>
        /// <returns>The saved json file path</returns>
        public string SaveObjectToFile(object obj, string fileName)
        {
            fileName = fileName.Trim();
            fileName = fileName + ".json";
            string savePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), fileName);
            
            string jsonString = JsonConvert.SerializeObject(obj);
            jsonString = JToken.Parse(jsonString).ToString(Formatting.Indented);

            File.WriteAllText(savePath, jsonString);

            return savePath;
        }


        /// <summary>
        /// Convert given container to object with necessary container info properties according to given user role
        /// </summary>
        /// <param name="container">Container to convert</param>
        /// <param name="user">UserRole according to which conversion should happen.</param>
        /// <returns></returns>
        private object ConvertContainerToUserContainerInfo(Container container, UserRoles user)
        {
            object obj;

            switch (user)
            {
                case UserRoles.FarmTechnician:
                    obj = new
                    {
                        User = "Farm Technician",
                        container.Name,
                        container.Temperature,
                        container.Humidity,
                        container.WaterLevel,
                        container.SoilMoisture,
                        container.FanIsOn,
                        container.LightsAreOn,
                        container.Location
                    };
                    break;

                default:
                    obj = new
                    {
                        User = "Fleet Manager",
                        container.Name,
                        container.IsRented,
                        container.BuzzerIsOn,
                        container.DoorIsClosed,
                        container.DoorIsLocked,
                        container.Location,
                        container.LuminosityLevel,
                        container.PitchAngle,
                        container.RollAngle,
                        container.Vibration,
                        container.NoiseLevel,
                        container.MotionIsDetected,
                    };
                    break;
            }

            return obj;
        }


        /// <summary>
        /// Serializes given object to JSON string and saves it to json file.
        /// </summary>
        /// <param name="container">The container to save</param>
        /// <param name="fileName">The json file name</param>
        /// <param name="user">The user role for which to save the container info</param>
        /// <returns>The saved json file path</returns>
        public string SaveContainerToFile(Container container, UserRoles user)
        {
            object obj = ConvertContainerToUserContainerInfo(container, user);
            return SaveObjectToFile(obj, string.Format(CONTAINER_SAVE_FILE_FORMAT, container.Name.Trim(), user.ToString()));
        }

        /// <summary>
        /// Gets the container info in json string info for the appropriate user role.
        /// </summary>
        /// <param name="container">The container info to use</param>
        /// <param name="user">The user role for which to get the container info</param>
        /// <returns>The container info in json format</returns>
        public string GetContainerInfo(Container container, UserRoles user)
        {
            object obj = ConvertContainerToUserContainerInfo(container, user);
            string jsonInfo = JsonConvert.SerializeObject(obj);

            jsonInfo = JToken.Parse(jsonInfo).ToString(Formatting.Indented);

            return jsonInfo;
        }
        /// <summary>
        /// Saves a data sample.
        /// </summary>
        /// <param name="data">The data sample to be saved</param>
        /// <returns></returns>
        public void SaveSensorData(DataSample data)
        {
            Message = "Saving data sample...";
            conn.InsertAsync(data);
        }

        /// <summary>
        /// Get all container sensor data samples.
        /// </summary>
        /// <returns>A list of all stored sensor data samples</returns>
        public async Task<List<DataSample>> GetContainerSensorData(string containerId)
        {
            return await conn.Table<DataSample>().Where(data => data.ContainerId == containerId).ToListAsync();
        }

        /// <summary>
        /// Get all container pitch data samples.
        /// </summary>
        /// <returns>A list of pitch data samples</returns>
        public async Task<List<DataSample>> GetPitchData(string containerId)
        {
            return await conn.Table<DataSample>().Where(data => data.Sensor == "pitch" && data.ContainerId == containerId).ToListAsync();
        }

        /// <summary>
        /// Get all container roll data samples.
        /// </summary>
        /// <returns>A list of roll data samples</returns>
        public async Task<List<DataSample>> GetRollData(string containerId)
        {
            return await conn.Table<DataSample>().Where(data => data.Sensor == "roll" && data.ContainerId == containerId).ToListAsync();
        }

        /// <summary>
        /// Get all container vibration data samples.
        /// </summary>
        /// <returns>A list of vibration data samples</returns>
        public async Task<List<DataSample>> GetVibrationData(string containerId)
        {
            return await conn.Table<DataSample>().Where(data => data.Sensor == "vibration" && data.ContainerId == containerId).ToListAsync();
        }

        /// <summary>
        /// Get all container luminosity data samples.
        /// </summary>
        /// <returns>A list of luminosity data samples</returns>
        public async Task<List<DataSample>> GetLuminosityData(string containerId)
        {
            return await conn.Table<DataSample>().Where(data => data.Sensor == "luminosity" && data.ContainerId == containerId).ToListAsync();
        }

        /// <summary>
        /// Get all container sound data samples.
        /// </summary>
        /// <returns>A list of sound data samples</returns>
        public async Task<List<DataSample>> GetSoundData(string containerId)
        {
            return await conn.Table<DataSample>().Where(data => data.Sensor == "sound" && data.ContainerId == containerId).ToListAsync();
        }

        /// <summary>
        /// Get all container temperature data samples.
        /// </summary>
        /// <returns>A list of temperature samples</returns>
        public async Task<List<DataSample>> GetTemperatureData(string containerId)
        {
            return await conn.Table<DataSample>().Where(data => data.Sensor == "temperature" && data.ContainerId == containerId).ToListAsync();
        }

        /// <summary>
        /// Get all container humidity data samples.
        /// </summary>
        /// <returns>A list of humidity samples</returns>
        public async Task<List<DataSample>> GetHumidityData(string containerId)
        {
            return await conn.Table<DataSample>().Where(data => data.Sensor == "humidity" && data.ContainerId == containerId).ToListAsync();
        }

        /// <summary>
        /// Get all container water level data samples.
        /// </summary>
        /// <returns>A list of water level samples</returns>
        public async Task<List<DataSample>> GetWaterLevelData(string containerId)
        {
            return await conn.Table<DataSample>().Where(data => data.Sensor == "water_level" && data.ContainerId == containerId).ToListAsync();
        }

        /// <summary>
        /// Get all container soil moisture data samples.
        /// </summary>
        /// <returns>A list of soil moisture samples</returns>
        public async Task<List<DataSample>> GetSoilMoistureData(string containerId)
        {
            return await conn.Table<DataSample>().Where(data => data.Sensor == "soil_moisture" && data.ContainerId == containerId).ToListAsync();
        }

        /// <summary>
        /// Get the most recent data of the specified sensor for each container.
        /// </summary>
        /// <returns>A dictionary of the temperature values by container IDs</returns>
        public async Task<Dictionary<string, float>> GetMostRecentSensorData(string sensor)
        {
            List<DataSample> allData = await conn.Table<DataSample>().ToListAsync();
            List<string> containerIds = allData.Select(data => data.ContainerId).Distinct().ToList();
            Dictionary<string, float> containerSensorData = new Dictionary<string, float>();

            foreach (string id in containerIds)
            {
                containerSensorData.Add(id, allData.Where(data => data.ContainerId == id && data.Sensor == sensor).Select(data => data.Value).FirstOrDefault());
            }

            return containerSensorData;
        }

        /// <summary>
        /// Limits the data that is saved in the database.
        /// </summary>
        /// <returns></returns>
        public async Task LimitSensorDataTable()
        {
            List<DataSample> tableData = await conn.Table<DataSample>().ToListAsync();

            if (tableData.Count <= DATA_LIMIT)
                return;

            List<DataSample> dataToKeep = tableData.Take(DATA_LIMIT).ToList();

            foreach (DataSample data in tableData)
            {
                if (!dataToKeep.Contains(data))
                    await conn.DeleteAsync(data);
            }
        }
    }
}