using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanTech
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FleetmanagerDashboardPage : ContentPage
    {
        public FleetmanagerDashboardPage()
        {
            InitializeComponent();
            var mainViewModel = (Application.Current as App).MainViewModel;
            mainViewModel.Navigation = Navigation;
            BindingContext = mainViewModel;

            // Override toolbar's back button behavior
            OnBackButtonPressedCommand = new Command(() =>
            {
                App app = Application.Current as App;
                app.MainViewModel.SelectedContainerViewModel = null;
                app.MainViewModel.Navigation.PopAsync();
            });
        }

        private ICommand _onBackButtonPressedCommand;
        /// <summary>
        /// Command for toolbar's back button.
        /// </summary>
        public ICommand OnBackButtonPressedCommand
        {
            get => _onBackButtonPressedCommand;
            private set
            {
                _onBackButtonPressedCommand = value;
                OnPropertyChanged();
            }
        }

        protected override bool OnBackButtonPressed()
        {
            (Application.Current as App).MainViewModel.SelectedContainerViewModel = null;
            return base.OnBackButtonPressed();
        }
    }
}