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
    public partial class ContainerListPage : ContentPage
    {
        public ContainerListPage()
        {
            InitializeComponent();
            var mainViewModel = (Application.Current as App).MainViewModel;
            mainViewModel.Navigation = Navigation;
            BindingContext = mainViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            MainViewModel viewModel = BindingContext as MainViewModel;
            viewModel.Navigation = Navigation;

            collectionView.SelectedItem = null;
        }
    }
}