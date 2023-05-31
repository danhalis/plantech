using PlanTech.Models;
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
    /// Represents a repository of <see cref="Container"/> objects.
    /// </summary>
    public class ContainersRepo
    {
        /// <summary>
        /// Retrieves sample <see cref="Container"/> objects.
        /// </summary>
        /// <returns>A <see cref="List{T}"/> of <see cref="Container"/> objects.</returns>
        public static IEnumerable<Container> Load()
        {
            List<Container> list = new List<Container>();

           /* list.Add(new Container()
            {
                Name = "Container #1",
                Location = "Montreal, Canada",
                IsRented = true,
                Temperature = 20,
                Humidity = 40,
                WaterLevel = 6,
                SoilMoisture = 50,
                BuzzerIsOn = true,
            });

            list.Add(new Container()
            {
                Name = "Container #2",
                Location = "New York, USA",
                IsRented = false
            });

            list.Add(new Container()
            {
                Name = "Container #3",
                Location = "Haiphong, Vietnam",
                IsRented = true,
                Temperature = 20,
                Humidity = 40,
                WaterLevel = 6,
                SoilMoisture = 50,
            });*/

            return list;
        }
    }
}
