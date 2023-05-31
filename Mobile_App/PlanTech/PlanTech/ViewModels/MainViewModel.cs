using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Shared;
using PlanTech.Models;
using PlanTech.Repos;
using PlanTech.Views;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using static PlanTech.ViewModels.ChartsViewModel;

/*
 * Team: PlanTech (Team #1)
 * Semester: Winter 2022
 * Course: 420-6A6-AB APPLICATION DEVELOPMENT III
 * Date: 03 May 2022
 */
namespace PlanTech.ViewModels
{
    /// <summary>
    /// Represents the main <see cref="ViewModel"/> of the app.
    /// </summary>
    public class MainViewModel : ViewModel
    {
        /// <summary>
        /// Initializes an instance of <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel(DataRepo dataRepo)
        {
            DataRepo = dataRepo;
            ChartsViewModel = new ChartsViewModel(this);
            ChartsViewModel.Navigation = Navigation;
            
            // Setup DashboardSettingViewModel
            DashboardSettingViewModel = new DashboardSettingViewModel();
            DashboardSettingViewModel.Navigation = Navigation;

            // Setup Commands
            OnContainerSelectedCommand = new Command<ContainerViewModel>(OnContainerSelected);
            ThresholdSettingCommand = new Command(ThresholdSetting);
            SaveThresholdCommand = new Command(SaveThreshold);
            DashboardSettingBackCommand = new Command(DashboardSettingBack);
            ShareContainerCommand = new Command<Container>(ShareContainer);

            GetContainers();

            Connectivity.ConnectivityChanged += OnConnectivityChanged;

            // Set up Event Hub connection
            var storageClient = new BlobContainerClient(
                AppData.StorageConnectionString,
                AppData.BlobContainerName);

            EventProcessorClient = new EventProcessorClient(
                storageClient,
                EventHubConsumerClient.DefaultConsumerGroupName,
                AppData.EventHubCompatibleEndpoint,
                AppData.EventHubCompatibleName);

            EventProcessorClient.ProcessEventAsync += ProcessEventHandler;
            EventProcessorClient.ProcessErrorAsync += ProcessErrorHandler;

            RegistryManager = RegistryManager.CreateFromConnectionString(AppData.EventHubConnectionString);

            NotificationCenter.Current.NotificationTapped += OnNotificationTapped;
        }

        public bool HasInternetConnection => Connectivity.NetworkAccess == NetworkAccess.Internet || Connectivity.NetworkAccess == NetworkAccess.ConstrainedInternet;
        private void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            OnPropertyChanged(nameof(HasInternetConnection));

            if (HasInternetConnection)
            {
                _ = EventProcessorClient.StartProcessingAsync();
            }
        }

        public EventProcessorClient EventProcessorClient { get; private set; }
        public RegistryManager RegistryManager { get; private set; }

        private async Task<bool> CheckDuplicateContainerStateNotification(string notificationTitle, int containerId)
        {
            string containerIdString = containerId.ToString();
            IList<NotificationRequest> pendingNotifications = await NotificationCenter.Current.GetPendingNotificationList();
            IList<NotificationRequest> deliveredNotifications = await NotificationCenter.Current.GetDeliveredNotificationList();

            NotificationRequest duplicatePendingNotification = pendingNotifications.FirstOrDefault(n =>
                n.Title == notificationTitle &&
                n.ReturningData == containerIdString);

            NotificationRequest duplicateDeliveredNotification = deliveredNotifications.FirstOrDefault(n =>
                n.Title == notificationTitle &&
                n.ReturningData == containerIdString);

            return duplicatePendingNotification != null || duplicateDeliveredNotification != null;
        }

        private async Task ProcessEventHandler(ProcessEventArgs eventArgs)
        {
            string message = Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray()).Replace("None", "null").Replace("False", "false").Replace("True", "true");

            // Write the body of the event to the console window
            Console.WriteLine("\tReceived event: {0}", message);

            // Update checkpoint in the blob storage so that the app receives only new events the next time it's run
            await eventArgs.UpdateCheckpointAsync(eventArgs.CancellationToken);

            // Get device id
            string deviceId = (string)eventArgs.Data.Properties["device_id"];

            // Deserialize message
            TelemetryMessage telemetryMessage = TelemetryMessage.FromJson(message);

            // Get message timestamp
            DateTime timestamp = eventArgs.Data.EnqueuedTime.DateTime.ToLocalTime();

            var mainViewModel = (Application.Current as App).MainViewModel;
            ContainerViewModel containerViewModel = mainViewModel.Containers.FirstOrDefault(c => c.DeviceId == deviceId);

            // if the tracking device is within one of the containers
            if (containerViewModel != null)
            {
                // Convert telemetry message to a Container object
                Container container = mainViewModel.DataRepo.ConvertToContainerFrom(telemetryMessage);

                // Map data received from telemetry to the view model
                containerViewModel.MapData(container);

                // Get device twin
                Twin deviceTwin = await RegistryManager.GetTwinAsync(deviceId);

                // Map data received from device twin to view model
                containerViewModel.MapData(deviceTwin.Properties.Reported);

                // if the container is in alarming state and its dashboard is not being displayed
                if (containerViewModel.IsAlarming && mainViewModel.SelectedContainerViewModel != containerViewModel)
                {
                    // if there wasn't a notification about the alarming state of this container
                    if (!await CheckDuplicateContainerStateNotification(AppData.AlarmingStateNotificationTitle, containerViewModel.Id))
                    {
                        // Send notifiction

                        NotificationRequest notification = new NotificationRequest
                        {
                            BadgeNumber = 1,
                            Title = AppData.AlarmingStateNotificationTitle,
                            Description = $"{containerViewModel.Name} is in alarming state. Click to see the issue.",
                            NotificationId = (int)DateTimeOffset.Now.ToUnixTimeSeconds(),
                            Schedule = new NotificationRequestSchedule
                            {
                                NotifyTime = DateTime.Now
                            },
                            Android = new Plugin.LocalNotification.AndroidOption.AndroidOptions
                            {
                                Priority = Plugin.LocalNotification.AndroidOption.AndroidNotificationPriority.Max
                            },
                            ReturningData = containerViewModel.Id.ToString()
                        };

                        _ = NotificationCenter.Current.Show(notification);
                    }
                }

                // if the container is in warning state and its dashboard is not being displayed
                if (containerViewModel.IsWarning && mainViewModel.SelectedContainerViewModel != containerViewModel)
                {
                    // if there wasn't a notification about the warning state of this container
                    if (!await CheckDuplicateContainerStateNotification(AppData.WarningStateNotificationTitle, containerViewModel.Id))
                    {
                        // Send notifiction

                        NotificationRequest notification = new NotificationRequest
                        {
                            BadgeNumber = 1,
                            Title = AppData.WarningStateNotificationTitle,
                            Description = $"{containerViewModel.Name} is in warning state. Click to see the issue.",
                            NotificationId = (int)DateTimeOffset.Now.ToUnixTimeSeconds(),
                            Schedule = new NotificationRequestSchedule
                            {
                                NotifyTime = DateTime.Now
                            },
                            Android = new Plugin.LocalNotification.AndroidOption.AndroidOptions
                            {
                                Priority = Plugin.LocalNotification.AndroidOption.AndroidNotificationPriority.Max
                            },
                            ReturningData = containerViewModel.Id.ToString()
                        };

                        _ = NotificationCenter.Current.Show(notification);
                    }
                }

                // Save the sensor data in the telemetry message
                mainViewModel.DataRepo.SaveContainerSensorData(telemetryMessage, timestamp, deviceId);


                // If container is selected container a chart page is showing, update chart
                if (mainViewModel.SelectedContainerViewModel != null && mainViewModel.SelectedContainerViewModel.DeviceId == deviceId && mainViewModel.ChartsViewModel.SelectedChart != ChartOption.None)
                    _ = Task.Run(() => mainViewModel.ChartsViewModel.UpdateChart(telemetryMessage, timestamp));

                // Update the data in the comparison chart if one is selected
                if (mainViewModel.ChartsViewModel.SelectedComparisonChart != null)
                    mainViewModel.ChartsViewModel.UpdateComparisonChart(telemetryMessage, deviceId);
            }
        }

        private static Task ProcessErrorHandler(ProcessErrorEventArgs eventArgs)
        {
            // Write details about the error to the console window
            Console.WriteLine($"\tPartition '{ eventArgs.PartitionId}': an unhandled exception was encountered. This was not expected to happen.");
            Console.WriteLine(eventArgs.Exception.Message);
            return Task.CompletedTask;
        }

        private async void OnNotificationTapped(Plugin.LocalNotification.EventArgs.NotificationEventArgs e)
        {
            // Remove this notification from the delivered notifications list
            _ = NotificationCenter.Current.Clear(new int[] { e.Request.NotificationId });

            App app = Application.Current as App;

            // if the main page is role selection page
            if (app.MainPage is RoleSelectionPage)
            {
                // Go to containers list
                app.AppShell.CurrentItem = app.AppShell.Items[0];
                app.MainPage = app.AppShell;

                var targetContainer = Containers.FirstOrDefault(c => c.Id == Convert.ToInt32(e.Request.ReturningData));
                
                if (targetContainer == null)
                    return;

                // Go to target container's dashboard
                OnContainerSelected(targetContainer);
            }
            else if (app.MainPage is AppShell)
            {
                AppShell mainPage = app.MainPage as AppShell;

                // if current displayed page is containers list
                if (mainPage.CurrentItem == mainPage.Items[0])
                {
                    // if the app is displaying a container dashboard
                    if (SelectedContainerViewModel != null)
                    {
                        // Pop out of dashboard
                        _ = await Navigation.PopAsync();
                    }
                }
                // if current displayed page is not containers list
                else
                {
                    // Go to containers list
                    mainPage.CurrentItem = mainPage.Items[0];
                }
                
                var targetContainer = Containers.FirstOrDefault(c => c.Id == Convert.ToInt32(e.Request.ReturningData));

                if (targetContainer == null)
                    return;

                // Go to target container's dashboard
                OnContainerSelected(targetContainer);
            }
        }

        public ChartsViewModel ChartsViewModel { get; set; }

        private UserRoles _currentUserRole;
        /// <summary>
        /// The current user's role.
        /// </summary>
        public UserRoles CurrentUserRole
        {
            get => _currentUserRole;
            set
            {
                if (_currentUserRole == value)
                {
                    return;
                }

                _currentUserRole = value;
                OnPropertyChanged();
            }
        }

        public DataRepo DataRepo { get; set; }

        /// <summary>
        /// Gets containers
        /// </summary>
        private async void GetContainers()
        {
            IEnumerable<ContainerViewModel> containerViewModels = (await DataRepo.GetContainers()).Select(container => new ContainerViewModel(container));
            Containers = new ObservableCollection<ContainerViewModel>(containerViewModels);
        }

        private ObservableCollection<ContainerViewModel> _containers;
        /// <summary>
        /// The containers.
        /// </summary>
        public ObservableCollection<ContainerViewModel> Containers
        {
            get => _containers;
            set
            {
                if (_containers == value)
                {
                    return;
                }

                _containers = value;
                OnPropertyChanged();
            }
        }

        private ContainerViewModel _selectedContainerViewModel;
        /// <summary>
        /// Get and set selected container
        /// </summary>
        public ContainerViewModel SelectedContainerViewModel
        {
            get => _selectedContainerViewModel;
            set
            {
                if (_selectedContainerViewModel == value)
                {
                    return;
                }

                _selectedContainerViewModel = value;
                OnPropertyChanged();
            }
        }

        private DashboardSettingViewModel _dashboardSettingViewModel;

        /// <summary>
        /// Get and set dashboard setting view model
        /// </summary>
        public DashboardSettingViewModel DashboardSettingViewModel
        {
            get => _dashboardSettingViewModel;
            set
            {
                if (_dashboardSettingViewModel == value)
                {
                    return;
                }

                _dashboardSettingViewModel = value;
                OnPropertyChanged();
            }
        }

        private ICommand _onContainerSelectedCommand;
        public ICommand OnContainerSelectedCommand
        {
            get => _onContainerSelectedCommand;
            private set
            {
                _onContainerSelectedCommand = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Navigates to appropriate dashboard page according to user role
        /// </summary>
        private async void OnContainerSelected(ContainerViewModel selectedContainer)
        {
            if (selectedContainer != null)
            {
                SelectedContainerViewModel = selectedContainer;

                if (HasInternetConnection)
                {
                    SelectedContainerViewModel.DeviceTwin = await RegistryManager.GetTwinAsync(selectedContainer.DeviceId);

                    // Map data from reported properties
                    selectedContainer.MapData(SelectedContainerViewModel.DeviceTwin.Properties.Reported);
                }

                if (CurrentUserRole == UserRoles.FleetManager)
                {
                    _ = Navigation.PushAsync(new FleetmanagerDashboardPage());
                }
                else
                {
                    _ = Navigation.PushAsync(new FarmDashboardPage());
                }
            }
        }

        public async Task UpdateDesiredProperty(string deviceTwinKey, dynamic value)
        {
            Twin deviceTwin = await RegistryManager.GetTwinAsync(SelectedContainerViewModel.DeviceId); ;
            deviceTwin.Properties.Desired[deviceTwinKey] = value;

            SelectedContainerViewModel.DeviceTwin = await RegistryManager.UpdateTwinAsync(SelectedContainerViewModel.DeviceId, deviceTwin, deviceTwin.ETag);
        }

        public ICommand ThresholdSettingCommand { get;  private set; }

        /// <summary>
        /// Navigate to dashboard setting page
        /// </summary>
        public void ThresholdSetting()
        {
            DashboardSettingViewModel.SettingSelectedContainerViewModel = SelectedContainerViewModel.Clone();
            Navigation.PushAsync(new ThresholdFormPage());
        }

        public ICommand SaveThresholdCommand { get; private set; }

        /// <summary>
        /// Save changed threshold settings in dashboard setting page and navigate back
        /// </summary>
        public async void SaveThreshold()
        {
            if (!DashboardSettingViewModel.IsValid)
            {
                await App.Current.MainPage.DisplayAlert("Invalid changes", "Please fix invalid settings before saving", "Ok");
                return;
            }

            SelectedContainerViewModel.SetProperties(DashboardSettingViewModel.SettingSelectedContainerViewModel);
            _ = await Navigation.PopAsync();
        }

        public ICommand DashboardSettingBackCommand { get; set; }

        /// <summary>
        /// Navigate back from dashboard setting page
        /// </summary>
        public async void DashboardSettingBack()
        {

            // Ask user to save unsaved changes if any changes are made
            if (!DashboardSettingViewModel.AreSettingsEqual(SelectedContainerViewModel))
            {
                const string ACCEPT_STRING = "Yes";
                const string CANCEL_STRING = "No";
                bool saveChanges = await App.Current.MainPage.DisplayAlert("Unsaved Changes", "Would you like to save your settings before continuing ?", ACCEPT_STRING, CANCEL_STRING);

                if (saveChanges)
                {
                    SaveThreshold();
                    return;
                }
            }
            
            _ = await Navigation.PopAsync();
        }

        public ICommand ShareContainerCommand { get; private set; }
        private async void ShareContainer(Container container)
        {
            const int SHARE_FILE_INDEX = 0;
            const int SHARE_TEXT_INDEX = 1;
            string[] actions = { "Share file", "Share text" };

            string chosenAction = await App.Current.MainPage.DisplayActionSheet("Share type", "Cancel", null, actions);
            if(chosenAction == actions[SHARE_FILE_INDEX])
            {
                string jsonFilePath = DataRepo.SaveContainerToFile(container, CurrentUserRole);

                ShareFile file = new ShareFile(jsonFilePath);
                ShareFileRequest shareRequest = new ShareFileRequest("Container Info", file);
                await Share.RequestAsync(shareRequest);
            }
            else if(chosenAction == actions[SHARE_TEXT_INDEX])
            {
                string jsonInfo = DataRepo.GetContainerInfo(container, CurrentUserRole);
                ShareTextRequest shareRequest = new ShareTextRequest(jsonInfo, "Dashboard Data");
                await Share.RequestAsync(shareRequest);
            }

        }

        public override INavigation Navigation 
        {
            get
            {
                if (base.Navigation == null)
                    Navigation = (Application.Current as App).MainPage?.Navigation;

                return base.Navigation;
            }
            set
            {
                base.Navigation = value;
                ChartsViewModel.Navigation = value;
            }
        }
    }
}