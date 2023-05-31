using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static PlanTech.ViewModels.ChartsViewModel;

/*
 * Team: PlanTech (Team #1)
 * Semester: Winter 2022
 * Course: 420-6A6-AB APPLICATION DEVELOPMENT III
 * Date: 03 May 2022
 */
namespace PlanTech.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SingleChartPage : ContentPage
    {
        public SingleChartPage()
        {
            InitializeComponent();
            BindingContext = (Application.Current as App).MainViewModel.ChartsViewModel;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            (Application.Current as App).MainViewModel.ChartsViewModel.SelectedChart = ChartOption.None;
        }
    }
}