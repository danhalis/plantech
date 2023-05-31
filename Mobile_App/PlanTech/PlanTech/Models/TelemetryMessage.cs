using Newtonsoft.Json;
using System.Collections.Generic;

namespace PlanTech.Models
{
    public class TelemetryMessage
    {
        public class GeoLocationTelemetryMessage
        {
            [JsonProperty("address")]
            public string Address { get; set; }

            [JsonProperty("angles")]
            public Dictionary<string, object> Angles { get; set; }

            [JsonProperty("vibration")]
            public object Vibration { get; set; }
        }

        [JsonProperty("geo_location")]
        public GeoLocationTelemetryMessage GeoLocation { get; set; }

        [JsonProperty("security")]
        public Dictionary<string, object> Security { get; set; }

        [JsonProperty("plant")]
        public Dictionary<string, object> Plant { get; set; }

        /// <summary>
        /// Converts a json object to a <see cref="TelemetryMessage"/> object.
        /// </summary>
        /// <param name="json">The json object that represents a <see cref="TelemetryMessage"/> object</param>
        /// <returns>The <see cref="TelemetryMessage"/> object</returns>
        public static TelemetryMessage FromJson(string json)
        {
            return JsonConvert.DeserializeObject<TelemetryMessage>(json);
        }
    }
}
