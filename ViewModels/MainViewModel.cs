using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Windows.Input;
using UBB_SE_2025_EUROTRUCKERS.Services;
using UBB_SE_2025_EUROTRUCKERS.Views;
using UBB_SE_2025_EUROTRUCKERS.Services.interfaces;

namespace UBB_SE_2025_EUROTRUCKERS.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private string _title = "Transport Management";

        public ICommand LogOutCommand { get; }
        public ICommand NavigateToMapViewCommand { get; }
        public ICommand NavigateToOrdersCommand { get; }
        public ICommand NavigateToDeliveriesCommand { get; }
        public ICommand NavigateToResourcesCommand { get; }

        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            LogOutCommand = new RelayCommand(() =>
            {
                Application.Current.Exit();
            });

            NavigateToMapViewCommand = new RelayCommand(() => {
                _navigationService.NavigateTo<MapViewModel>();
            });

            NavigateToOrdersCommand = new RelayCommand(NavigateToOrders);
            NavigateToDeliveriesCommand = new RelayCommand(NavigateToDeliveries);
            NavigateToResourcesCommand = new RelayCommand(NavigateToResources);
        }

        public void SetContentFrame(Frame frame)
        {
            _navigationService.SetContentFrame(frame);
        }

        private void NavigateToOrders()
        {
            _navigationService.NavigateTo<OrderViewModel>();
        }

        private void NavigateToDeliveries()
        {
            _navigationService.NavigateTo<DeliveriesViewModel>();
        }

        private void NavigateToResources()
        {
            _navigationService.NavigateTo<ResourcesViewModel>();
        }
    }
}
