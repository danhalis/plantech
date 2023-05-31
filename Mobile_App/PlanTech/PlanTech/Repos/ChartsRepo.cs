using System.Collections.Generic;
using Microcharts;
using PlanTech.Models;

/*
 * Team: PlanTech (Team #1)
 * Semester: Winter 2022
 * Course: 420-6A6-AB APPLICATION DEVELOPMENT III
 * Date: 03 May 2022
 */
namespace PlanTech.Repos
{
    /// <summary>
    /// A static class to provide charts.
    /// </summary>
    public static class ChartsRepo
    {
        /// <summary>
        /// Creates a line chart that includes the provided data.
        /// </summary>
        /// <param name="data">
        /// The data to be used for the chart entries.
        /// </param>
        /// <param name="color">
        /// The color to be used for the charts.
        /// </param>
        /// <returns>
        /// A line chart with the provided data.
        /// </returns>
        public static LineChart GetLineChart(List<DataSample> data, string color = "#003399")
        {
            return new LineChart()
            {
                ValueLabelOrientation = Orientation.Horizontal,
                LabelOrientation = Orientation.Horizontal,
                BackgroundColor = SkiaSharp.SKColor.Empty,
                Entries = DataSampleListToChartEntries(data, color),
                AnimationDuration = new System.TimeSpan(0),
                IsAnimated = false,
                LabelTextSize = 20f,
            };
        }

        /// <summary>
        /// Create a bar chart that includes the provided data.
        /// </summary>
        /// <param name="data">
        /// The data to be used for the chart entries.
        /// </param>
        /// <returns>
        /// A bar chart with the provided data.
        /// </returns>
        public static BarChart GetBarChart(Dictionary<string, float> data)
        {
            List<ChartEntry> entries = new List<ChartEntry>();

            foreach (KeyValuePair<string, float> entry in data)
            {
                entries.Add(new ChartEntry(entry.Value)
                {
                    Label = entry.Key,
                    ValueLabel = entry.Value.ToString(),
                    Color = SkiaSharp.SKColor.Parse("#003399"),
                });
            }

            return new BarChart()
            {
                ValueLabelOrientation = Orientation.Horizontal,
                LabelOrientation = Orientation.Horizontal,
                BackgroundColor = SkiaSharp.SKColor.Empty,
                Entries = entries,
                AnimationDuration = new System.TimeSpan(0),
                IsAnimated = false,
                LabelTextSize = 25f,
            };
        }

        /// <summary>
        /// Converts a provided list of data samples to chart entries.
        /// </summary>
        /// <param name="dataSamples">
        /// The data samples to convert.
        /// </param>
        /// <param name="color">
        /// The color to make the chart entries.
        /// </param>
        /// <returns>
        /// A list of chart entries based on the provided data samples.
        /// </returns>
        private static List<ChartEntry> DataSampleListToChartEntries(List<DataSample> dataSamples, string color)
        {
            List<ChartEntry> entries = new List<ChartEntry>();

            foreach (DataSample dataSample in dataSamples)
            {
                entries.Add(new ChartEntry(dataSample.Value)
                {
                    Label = string.Format("{0:t}", dataSample.Timestamp),
                    ValueLabel = dataSample.Value.ToString(),
                    Color = SkiaSharp.SKColor.Parse(color),
                });
            }

            return entries;
        }
    }
}
