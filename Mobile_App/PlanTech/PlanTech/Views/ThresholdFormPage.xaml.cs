using PlanTech.ViewModels;
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
    public partial class ThresholdFormPage : ContentPage
    {
        public ThresholdFormPage()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
            var mainViewModel = (Application.Current as App).MainViewModel;
            BackButtonBehavior backButton = new BackButtonBehavior();
            backButton.IsEnabled = false;

            BindingContext = mainViewModel;
        }

        protected override bool OnBackButtonPressed()
        {
            backBehavior.Command.Execute(null);
            return true;
        }
    }
}