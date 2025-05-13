using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Windows.Input;
using UBB_SE_2025_EUROTRUCKERS.Services;
using UBB_SE_2025_EUROTRUCKERS.Views;

namespace UBB_SE_2025_EUROTRUCKERS.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private string _title = "Transport Management";

        public ICommand NavigateToDeliveriesCommand { get; }
        public ICommand LogOutCommand { get; }

        public ICommand NavigateToMapViewCommand { get; }

        public ICommand NavigateToResourcesCommand { get; }

        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            NavigateToDeliveriesCommand = new RelayCommand(() =>
            {
                _navigationService.NavigateTo<DeliveriesViewModel>();
            });

            LogOutCommand = new RelayCommand(() =>
            {
                Application.Current.Exit();
            });

            NavigateToMapViewCommand = new RelayCommand(() => {
                _navigationService.NavigateTo<MapViewModel>();
            });

            NavigateToResourcesCommand = new RelayCommand(() =>
            {
                _navigationService.NavigateTo<ResourcesViewModel>();
            });
        }

        public void SetContentFrame(Frame frame)
        {
            _navigationService.SetContentFrame(frame);
        }
    }
}
