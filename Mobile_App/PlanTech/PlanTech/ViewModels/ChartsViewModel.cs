using Microcharts;
using PlanTech.Models;
using PlanTech.Repos;
using PlanTech.Views;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
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
    /// ViewModel class for the application charts.
    /// </summary>
    public class ChartsViewModel : ViewModel
    {
        int ENTRY_COUNT = 10;
        public enum ChartOption
        {
            None,
            Angles,
            Vibration,
            Sound,
            Luminosity,
            TempHumi,
            WaterLevel,
            SoilMoisture
        }

        /// <summary>
        /// Default constructor for the ChartsViewModel class.
        /// Initializes all commands belonging to the class.
        /// </summary>
        public ChartsViewModel(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            ComparisonCharts = new string[]
            {
                "Temperature", "Humidity", "Vibration", "Luminosity", "Pitch", "Roll", "Sound", "Water Level", "Soil Moisture" 
            };
            AnglesPage = new Command(ViewAnglesPage);
            VibrationPage = new Command(ViewVibrationPage);
            SoundPage = new Command(ViewSoundPage);
            LuminosityPage = new Command(ViewLuminosityPage);
            TemperaturePage = new Command(ViewTemperaturePage);
            WaterPage = new Command(ViewWaterPage);
            MoisturePage = new Command(ViewMoisturePage);

            Connectivity.ConnectivityChanged += OnConnectivityChanged;
        }
        private MainViewModel MainViewModel { get; set; }

        public bool HasInternetConnection => Connectivity.NetworkAccess == NetworkAccess.Internet || Connectivity.NetworkAccess == NetworkAccess.ConstrainedInternet;
        private void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            OnPropertyChanged(nameof(HasInternetConnection));
        }
        List<DataSample>[] chartsData = new List<DataSample>[2];
        Dictionary<string, float> comparisonChartData;
        public ChartOption SelectedChart { get; set; }
        public string[] ComparisonCharts { get; set; }
        private string selectedComparisonChart { get; set; }
        public string SelectedComparisonChart
        {
            get { return selectedComparisonChart; }
            set
            {
                selectedComparisonChart = value;

                if (value != null)
                    GetComparisonChart();
            }
        }

        /// <summary>
        /// Gets whether the charts are empty or not.
        /// </summary>
        public bool[] IsEmpty
        {
            get { return new bool[] { chartsData[0]?.Count == 0, chartsData[1]?.Count == 0 }; }
        }

        /// <summary>
        /// Stores the title of the page.
        /// </summary>
        private string pageTitle;
        /// <summary>
        /// Gets or sets the title of the page.
        /// </summary>
        public string PageTitle
        {
            get { return pageTitle; }
            set
            {
                if (pageTitle != value)
                {
                    pageTitle = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Stores the headers of the charts.
        /// </summary>
        private string[] headers;
        /// <summary>
        /// Gets or sets the headers of the charts.
        /// </summary>
        public string[] Headers
        {
            get { return headers; }
            set
            {
                if (headers != value)
                {
                    headers = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Stores the charts.
        /// </summary>
        private List<Chart> charts;
        /// <summary>
        /// Gets or sets the charts.
        /// </summary>
        public List<Chart> Charts
        {
            get { return charts; }
            set
            {
                if (charts != value)
                {
                    charts = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or privately sets the command for the angles page.
        /// </summary>
        public ICommand AnglesPage { get; private set; }
        /// <summary>
        /// Populates the ChartsViewModel properties with the relevant angle data and navigates to the MultiChartPage.
        /// </summary>
        private async void ViewAnglesPage()
        {
            // Default chart data
            PageTitle = "Angles";
            Headers = new string[] { "Container Pitch Angles", "Container Roll Angles" };
            SelectedChart = ChartOption.Angles;
            chartsData[0] = await MainViewModel.DataRepo.GetPitchData(MainViewModel.SelectedContainerViewModel.DeviceId);
            chartsData[1] = await MainViewModel.DataRepo.GetRollData(MainViewModel.SelectedContainerViewModel.DeviceId);
            UpdateMultiChart();
            await Navigation.PushAsync(new MultiChartPage());
        }

        /// <summary>
        /// Gets or privately sets the command for the vibration page.
        /// </summary>
        public ICommand VibrationPage { get; private set; }
        /// <summary>
        /// Populates the ChartsViewModel properties with the relevant vibration data and navigates to the SingleChartPage.
        /// </summary>
        private async void ViewVibrationPage()
        {
            PageTitle = "Vibration";
            Headers = new string[] { "Container Vibration Levels" };
            SelectedChart = ChartOption.Vibration;
            chartsData[0] = await MainViewModel.DataRepo.GetVibrationData(MainViewModel.SelectedContainerViewModel.DeviceId);
            UpdateSingleChart();
            await Navigation.PushAsync(new SingleChartPage());
        }

        /// <summary>
        /// Gets or privately sets the command for the sound page.
        /// </summary>
        public ICommand SoundPage { get; private set; }
        /// <summary>
        /// Populates the ChartsViewModel properties with the relevant sound data and navigates to the SingleChartPage.
        /// </summary>
        private async void ViewSoundPage()
        {
            PageTitle = "Sound";
            Headers = new string[] { "Container Sound Levels" };
            SelectedChart = ChartOption.Sound;
            chartsData[0] = await MainViewModel.DataRepo.GetSoundData(MainViewModel.SelectedContainerViewModel.DeviceId);
            UpdateSingleChart();
            await Navigation.PushAsync(new SingleChartPage());
        }

        /// <summary>
        /// Gets or privately sets the command for the luminosity page.
        /// </summary>
        public ICommand LuminosityPage { get; private set; }
        /// <summary>
        /// Populates the ChartsViewModel properties with the relevant luminosity data and navigates to the SingleChartPage.
        /// </summary>
        private async void ViewLuminosityPage()
        {
            PageTitle = "Luminosity";
            Headers = new string[] { "Container Luminosity Levels" };
            SelectedChart = ChartOption.Luminosity;
            chartsData[0] = await MainViewModel.DataRepo.GetLuminosityData(MainViewModel.SelectedContainerViewModel.DeviceId);
            UpdateSingleChart();
            await Navigation.PushAsync(new SingleChartPage());
        }

        /// <summary>
        /// Gets or privately sets the command for the temperature page.
        /// </summary>
        public ICommand TemperaturePage { get; private set; }
        /// <summary>
        /// Populates the ChartsViewModel properties with the relevant temperature and humidity data and navigates to the TemperaturePage.
        /// </summary>
        private async void ViewTemperaturePage()
        {
            PageTitle = "Temperature & Humidity";
            Headers = new string[] { "Container Temperature", "Container Humidity" };
            SelectedChart = ChartOption.TempHumi;
            chartsData[0] = await MainViewModel.DataRepo.GetTemperatureData(MainViewModel.SelectedContainerViewModel.DeviceId);
            chartsData[1] = await MainViewModel.DataRepo.GetHumidityData(MainViewModel.SelectedContainerViewModel.DeviceId);
            UpdateMultiChart();
            await Navigation.PushAsync(new TemperatureHumidityPage());
        }

        /// <summary>
        /// Gets or privately sets the command for the water level page.
        /// </summary>
        public ICommand WaterPage { get; private set; }
        /// <summary>
        /// Populates the ChartsViewModel properties with the relevant water level data and navigates to the SingleChartPage.
        /// </summary>
        private async void ViewWaterPage()
        {
            PageTitle = "Water Level";
            Headers = new string[] { "Container Water Levels" };
            SelectedChart = ChartOption.WaterLevel;
            chartsData[0] = await MainViewModel.DataRepo.GetWaterLevelData(MainViewModel.SelectedContainerViewModel.DeviceId);
            UpdateSingleChart();
            await Navigation.PushAsync(new SingleChartPage());

        }

        /// <summary>
        /// Gets or privately sets the command for the moisture page.
        /// </summary>
        public ICommand MoisturePage { get; private set; }
        /// <summary>
        /// Populates the ChartsViewModel properties with the relevant moisture data and navigates to the SingleChartPage.
        /// </summary>
        private async void ViewMoisturePage()
        {
            PageTitle = "Moisture";
            Headers = new string[] { "Container Moisture Levels" };
            SelectedChart = ChartOption.SoilMoisture;
            chartsData[0] = await MainViewModel.DataRepo.GetSoilMoistureData(MainViewModel.SelectedContainerViewModel.DeviceId);
            UpdateSingleChart();
            await Navigation.PushAsync(new SingleChartPage());
        }

        /// <summary>
        /// Update the chart related properties for a page that features a single chart.
        /// </summary>
        /// <param name="dataPoint">The data point to be added to the chart</param>
        /// <param name="chartData">The data already featured in the chart</param>
        private void UpdateSingleChart()
        {
            List<Chart> charts = new List<Chart>();

            // Limit the chart to the max amount of entries
            if (chartsData[0].Count > ENTRY_COUNT)
                chartsData[0] = chartsData[0].GetRange(chartsData[0].Count - ENTRY_COUNT, ENTRY_COUNT);

            charts.Add(ChartsRepo.GetLineChart(chartsData[0]));
            Charts = charts;
        }

        /// <summary>
        /// Update the chart related properties for a page that features two charts.
        /// </summary>
        /// <param name="dataPoints">The data points to be added to the charts</param>
        /// <param name="chartsData">The data already features in the charts</param>
        private void UpdateMultiChart()
        {
            List<Chart> charts = new List<Chart>();

            // Limit the first chart to the max amount of entries
            if (chartsData[0].Count > ENTRY_COUNT)
                chartsData[0] = chartsData[0].GetRange(chartsData[0].Count - ENTRY_COUNT, ENTRY_COUNT);

            // Limit the second chart to the max amount of entries
            if (chartsData[1].Count > ENTRY_COUNT)
                chartsData[1] = chartsData[1].GetRange(chartsData[1].Count - ENTRY_COUNT, ENTRY_COUNT);

            charts.Add(ChartsRepo.GetLineChart(chartsData[0]));
            charts.Add(ChartsRepo.GetLineChart(chartsData[1], "#cc0000"));
            Charts = charts;
        }

        /// <summary>
        /// Add the new telemetry data of the selected chart to the chart data and then call the appropriate method to update the current chart
        /// </summary>
        /// <param name="telemetryMessage">The telemetry data to add</param>
        /// <param name="timestamp">The timestamp of the telemetry message</param>
        public void UpdateChart(TelemetryMessage telemetryMessage, DateTime timestamp)
        {
            // Add the appropriate data to the chart data
            switch (SelectedChart)
            {
                case ChartOption.Angles:
                    if (telemetryMessage.GeoLocation.Angles["pitch"] != null)
                        chartsData[0].Add(new DataSample() { Value = Convert.ToSingle(telemetryMessage.GeoLocation.Angles["pitch"]), Timestamp = timestamp });

                    if (telemetryMessage.GeoLocation.Angles["roll"] != null)
                        chartsData[1].Add(new DataSample() { Value = Convert.ToSingle(telemetryMessage.GeoLocation.Angles["roll"]), Timestamp = timestamp });

                    UpdateMultiChart();
                    break;
                case ChartOption.Vibration:
                    if (telemetryMessage.GeoLocation.Vibration != null)
                    {
                        chartsData[0].Add(new DataSample() { Value = Convert.ToSingle(telemetryMessage.GeoLocation.Vibration), Timestamp = timestamp });
                        UpdateSingleChart();
                    }
                    break;
                case ChartOption.Sound:
                    if (telemetryMessage.Security["sound"] != null)
                    {
                        chartsData[0].Add(new DataSample() { Value = Convert.ToSingle(telemetryMessage.Security["sound"]), Timestamp = timestamp });
                        UpdateSingleChart();
                    }
                    break;
                case ChartOption.Luminosity:
                    if (telemetryMessage.Security["luminosity"] != null)
                    {
                        chartsData[0].Add(new DataSample() { Value = Convert.ToSingle(telemetryMessage.Security["luminosity"]), Timestamp = timestamp });
                        UpdateSingleChart();
                    }
                    break;
                case ChartOption.TempHumi:
                    if (telemetryMessage.Plant["temperature"] != null)
                        chartsData[0].Add(new DataSample() { Value = Convert.ToSingle(telemetryMessage.Plant["temperature"]), Timestamp = timestamp });

                    if (telemetryMessage.Plant["humidity"] != null)
                        chartsData[1].Add(new DataSample() { Value = Convert.ToSingle(telemetryMessage.Plant["humidity"]), Timestamp = timestamp });

                    UpdateMultiChart();
                    break;
                case ChartOption.WaterLevel:
                    if (telemetryMessage.Plant["water_level"] != null)
                    {
                        chartsData[0].Add(new DataSample() { Value = Convert.ToSingle(telemetryMessage.Plant["water_level"]), Timestamp = timestamp });
                        UpdateSingleChart();
                    }
                    break;
                case ChartOption.SoilMoisture:
                    if (telemetryMessage.Plant["soil_moisture"] != null)
                    {
                        chartsData[0].Add(new DataSample() { Value = Convert.ToSingle(telemetryMessage.Plant["soil_moisture"]), Timestamp = timestamp });
                        UpdateSingleChart();
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Gets the comparison chart for the selected comparison chart.
        /// </summary>
        public async void GetComparisonChart()
        {
            comparisonChartData = await MainViewModel.DataRepo.GetMostRecentSensorData(selectedComparisonChart.ToLower().Replace(" ", "_"));
            Charts = new List<Chart>() { ChartsRepo.GetBarChart(comparisonChartData) };
        }

        /// <summary>
        /// Add the appropriate telemetry data to the comparison chart data and then update the chart
        /// </summary>
        /// <param name="telemetryMessage">The telemetry data to add</param>
        /// <param name="deviceId">The ID of the device that the telemetry data belongs to</param>
        public void UpdateComparisonChart(TelemetryMessage telemetryMessage, string deviceId)
        {
            // Add the appropriate data to the comparison chart data
            switch (SelectedComparisonChart)
            {
                case "Temperature":
                    if (telemetryMessage.Plant["temperature"] != null)
                        comparisonChartData[deviceId] = Convert.ToSingle(telemetryMessage.Plant["temperature"]);
                    break;
                case "Humidity":
                    if (telemetryMessage.Plant["humidity"] != null)
                        comparisonChartData[deviceId] = Convert.ToSingle(telemetryMessage.Plant["humidity"]);
                    break;
                case "Vibration":
                    if (telemetryMessage.GeoLocation.Vibration != null)
                        comparisonChartData[deviceId] = Convert.ToSingle(telemetryMessage.GeoLocation.Vibration);
                    break;
                case "Luminosity":
                    if (telemetryMessage.Security["luminosity"] != null)
                        comparisonChartData[deviceId] = Convert.ToSingle(telemetryMessage.Security["luminosity"]);
                    break;
                case "Pitch":
                    if (telemetryMessage.GeoLocation.Angles["pitch"] != null)
                        comparisonChartData[deviceId] = Convert.ToSingle(telemetryMessage.GeoLocation.Angles["pitch"]);
                    break;
                case "Roll":
                    if (telemetryMessage.GeoLocation.Angles["roll"] != null)
                        comparisonChartData[deviceId] = Convert.ToSingle(telemetryMessage.GeoLocation.Angles["roll"]);
                    break;
                case "Sound":
                    if (telemetryMessage.Security["sound"] != null)
                        comparisonChartData[deviceId] = Convert.ToSingle(telemetryMessage.Security["sound"]);
                    break;
                case "Water Level":
                    if (telemetryMessage.Plant["water_level"] != null)
                        comparisonChartData[deviceId] = Convert.ToSingle(telemetryMessage.Plant["water_level"]);
                    break;
                case "Soil Moisture":
                    if (telemetryMessage.Plant["soil_moisture"] != null)
                        comparisonChartData[deviceId] = Convert.ToSingle(telemetryMessage.Plant["soil_moisture"]);
                    break;
            }

            // Update the chart
            Charts = new List<Chart>() { ChartsRepo.GetBarChart(comparisonChartData) };
        }
    }
}
