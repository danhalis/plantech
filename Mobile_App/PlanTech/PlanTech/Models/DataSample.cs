using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

/*
 * Team: PlanTech (Team #1)
 * Semester: Winter 2022
 * Course: 420-6A6-AB APPLICATION DEVELOPMENT III
 * Date: 03 May 2022
 */
namespace PlanTech.Models
{
    /// <summary>
    /// Class to represent a single data sample that would be retrieved by a sensor.
    /// </summary>
    public class DataSample
    {
        /// <summary>
        /// The Id of the data sample.
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the data sample sensor name
        /// </summary>
        public string Sensor { get; set; }

        /// <summary>
        /// Gets or sets the data sample value
        /// </summary>
        public float Value { get; set; }

        /// <summary>
        /// Gets or sets the data sample Timestamp
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the container ID that the data sample was retrieved from
        /// </summary>
        public string ContainerId { get; set; }
    }
}
