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
    public partial class ComparisonPage : ContentPage
    {
        public ComparisonPage()
        {
            InitializeComponent();
            (Application.Current as App).MainViewModel.ChartsViewModel.SelectedComparisonChart = (Application.Current as App).MainViewModel.ChartsViewModel.ComparisonCharts[0];
            BindingContext = (Application.Current as App).MainViewModel.ChartsViewModel;
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            (Application.Current as App).MainViewModel.ChartsViewModel.SelectedComparisonChart = null;
        }
    }
}