using PlanTech.Models;
using PlanTech.Repos;
using PlanTech.ViewModels;
using PlanTech.Views;
using System.Linq;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Collections.Generic;
using Plugin.LocalNotification;
using System;

namespace PlanTech
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Load app data
            AppData.Load();

            DBPath = Path.Combine(FileSystem.AppDataDirectory, "plantech.db3");
            DataRepo = new DataRepo(DBPath);

            MainViewModel = new MainViewModel(DataRepo);

            MainPage = new RoleSelectionPage();
        }

        private AppShell _appShell;
        public AppShell AppShell
        {
            get
            {
                if (_appShell == null)
                    _appShell = new AppShell();

                return _appShell;
            }
        }

        public static string DBPath { get; private set; }
        public static DataRepo DataRepo { get; private set; }

        public MainViewModel MainViewModel { get; set; }

        public UserRoles CurrentUserRole
        {
            get => MainViewModel.CurrentUserRole;
            set => MainViewModel.CurrentUserRole = value;
        }

        protected override void OnStart()
        {
            _ = NotificationCenter.Current.CancelAll();
            _ = NotificationCenter.Current.ClearAll();

            if (MainViewModel.HasInternetConnection)
                _ = MainViewModel.EventProcessorClient.StartProcessingAsync();
        }

        protected override async void OnSleep()
        {
            if (MainViewModel.HasInternetConnection)
                _ = MainViewModel.EventProcessorClient.StopProcessingAsync();

            // Save current data of all containers in local database before entering sleep mode
            List<ContainerViewModel> containerViews = MainViewModel.Containers.ToList();
            List<Container> containers = new List<Container>();

            containerViews.ForEach(cv => containers.Add(cv.Container));

            await DataRepo.UpdateContainers(containers);
            await DataRepo.LimitSensorDataTable();
        }

        protected override void OnResume()
        {
            if (MainViewModel.HasInternetConnection)
                _ = MainViewModel.EventProcessorClient.StartProcessingAsync();
        }
    }

    /// <summary>
    /// User roles.
    /// </summary>
    public enum UserRoles
    {
        FleetManager,
        FarmTechnician
    }
}
