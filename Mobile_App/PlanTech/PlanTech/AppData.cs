using System;
using System.IO;
using System.Reflection;
using DotNetEnv;

namespace PlanTech
{
    public static class AppData
    {
        public static void Load()
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("PlanTech..env"))
            {
                _ = Env.Load(stream);
            }
        }

        public static string EventHubCompatibleName => Environment.GetEnvironmentVariable("EVENT_HUB_COMPATIBLE_NAME"); 
        public static string EventHubConnectionString => Environment.GetEnvironmentVariable("EVENT_HUB_CONNECTION_STRING");
        public static string EventHubCompatibleEndpoint => Environment.GetEnvironmentVariable("EVENT_HUB_COMPATIBLE_ENDPOINT");
        public static string StorageConnectionString => Environment.GetEnvironmentVariable("STORAGE_CONNECTION_STRING");
        public static string BlobContainerName => Environment.GetEnvironmentVariable("BLOB_CONTAINER_NAME");

        public static class DeviceTwinPropertyKeys
        {
            public static string Fan => "fan";
            public static string Buzzer => "buzzer";
            public static string DoorLock => "door_lock";
            public static string LED => "led";
        }

        public static class DeviceTwinPropertyValues
        {
            public static string FanIsOn => "on";
            public static string FanIsOff => "off";
            public static string BuzzerIsOn => "on";
            public static string BuzzerIsOff => "off";
            public static string DoorIsLocked => "locked";
            public static string DoorIsUnlocked => "unlocked";
            public static string LedIsOn => "on";
            public static string LedIsOff => "off";
        }
        
        public static string AlarmingStateNotificationTitle = "Alarming !!!";
        public static string WarningStateNotificationTitle = "Warning !!!";
    }
}
