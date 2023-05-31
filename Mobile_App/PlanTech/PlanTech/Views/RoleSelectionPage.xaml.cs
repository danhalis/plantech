using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanTech.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RoleSelectionPage : ContentPage
    {
        public RoleSelectionPage()
        {
            InitializeComponent();
        }

        private void OnFleetManagerButtonClicked(object sender, EventArgs e)
        {
            App app = Application.Current as App;
            app.CurrentUserRole = UserRoles.FleetManager;
            app.AppShell.CurrentItem = app.AppShell.Items[0];
            app.MainPage = app.AppShell;
        }

        private void OnFarmTechinicianButtonClicked(object sender, EventArgs e)
        {
            App app = Application.Current as App;
            app.CurrentUserRole = UserRoles.FarmTechnician;
            app.AppShell.CurrentItem = app.AppShell.Items[0];
            app.MainPage = app.AppShell;
        }
    }
}