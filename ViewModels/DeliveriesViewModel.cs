using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;
using UBB_SE_2025_EUROTRUCKERS.Services;
using UBB_SE_2025_EUROTRUCKERS.Models;
using UBB_SE_2025_EUROTRUCKERS.Services.interfaces;

namespace UBB_SE_2025_EUROTRUCKERS.ViewModels
{
    public partial class DeliveriesViewModel : ViewModelBase
    {
        private readonly IDeliveryService _deliveryService;
        private readonly INavigationService _navigationService;
        private readonly ILoggingService _loggingService;

        [ObservableProperty]
        private ObservableCollection<Delivery> _deliveries = new();

        public DeliveriesViewModel(
            IDeliveryService deliveryService,
            INavigationService navigationService,
            ILoggingService loggingService)
        {
            _deliveryService = deliveryService;
            _navigationService = navigationService;
            _loggingService = loggingService;
            Title = "Deliveries";
            LoadDeliveriesCommand = new AsyncRelayCommand(LoadDeliveriesAsync);
            NavigateToDetailsCommand = new RelayCommand<Delivery>(NavigateToDetails);

            _ = LoadDeliveriesCommand.ExecuteAsync(null);
        }

        public IAsyncRelayCommand LoadDeliveriesCommand { get; }
        public IRelayCommand<Delivery> NavigateToDetailsCommand { get; }

        private async Task LoadDeliveriesAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                var deliveries = await _deliveryService.GetAllDeliveriesAsync();
                Deliveries.Clear();
                foreach (var delivery in deliveries)
                {
                    Deliveries.Add(delivery);
                }
                _loggingService.LogInformation($"Loaded {Deliveries.Count} deliveries");
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error loading deliveries", ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void NavigateToDetails(Delivery? delivery)
        {
            if (delivery != null)
            {
                _navigationService.NavigateToWithParameter<DetailsViewModel>(delivery);
                _loggingService.LogDebug($"Navigating to details view for delivery {delivery.delivery_id}");
            }
        }
    }
}