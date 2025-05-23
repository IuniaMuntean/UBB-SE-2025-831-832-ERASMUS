using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using UBB_SE_2025_EUROTRUCKERS.Models;
using UBB_SE_2025_EUROTRUCKERS.Services;
using UBB_SE_2025_EUROTRUCKERS.Services.interfaces;

namespace UBB_SE_2025_EUROTRUCKERS.ViewModels
{
    public partial class AddDeliveryViewModel : ViewModelBase
    {
        private readonly IDeliveryService _deliveryService;
        private readonly INavigationService _navigationService;
        private readonly ILoggingService _loggingService;
        private readonly IOrderService _orderService;
        private readonly ITruckService _truckService;
        private readonly IDriverService _driverService;

        private ObservableCollection<Driver> _availableDrivers = new();
        public ObservableCollection<Driver> AvailableDrivers
        {
            get => _availableDrivers;
            set => SetProperty(ref _availableDrivers, value);
        }

        private ObservableCollection<Truck> _availableTrucks = new();
        public ObservableCollection<Truck> AvailableTrucks
        {
            get => _availableTrucks;
            set => SetProperty(ref _availableTrucks, value);
        }

        private Order _selectedOrder;
        public Order SelectedOrder
        {
            get => _selectedOrder;
            set => SetProperty(ref _selectedOrder, value);
        }

        private Delivery _newDelivery = new();
        public Delivery NewDelivery
        {
            get => _newDelivery;
            set => SetProperty(ref _newDelivery, value);
        }

        public string PageTitle => "Create New Delivery";

        public AddDeliveryViewModel(
            IDeliveryService deliveryService,
            INavigationService navigationService,
            ILoggingService loggingService,
            IOrderService orderService,
            ITruckService truckService,
            IDriverService driverService)
        {
            _deliveryService = deliveryService;
            _navigationService = navigationService;
            _loggingService = loggingService;
            _orderService = orderService;
            _truckService = truckService;
            _driverService = driverService;

            Title = PageTitle;
            SubmitDeliveryCommand = new AsyncRelayCommand(SubmitDeliveryAsync);
            NavigateBackCommand = new RelayCommand(NavigateBack);
        }

        public IAsyncRelayCommand SubmitDeliveryCommand { get; }
        public IRelayCommand NavigateBackCommand { get; }

        public async Task InitializeAsync()
        {
            await LoadDriversAsync();
            await LoadTrucksAsync();
        }

        private async Task LoadDriversAsync()
        {
            try
            {
                var drivers = await _driverService.GetAllAsync();
                AvailableDrivers.Clear();
                foreach (var driver in drivers)
                {
                    AvailableDrivers.Add(driver);
                }
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error loading drivers: {ex.Message}");
            }
        }

        private async Task LoadTrucksAsync()
        {
            try
            {
                var trucks = await _truckService.GetAllAsync();
                AvailableTrucks.Clear();
                foreach (var truck in trucks)
                {
                    AvailableTrucks.Add(truck);
                }
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error loading trucks: {ex.Message}");
            }
        }

        public void InitializeWithOrder(Order order)
        {
            SelectedOrder = order;
            var now = DateTime.Now;
            NewDelivery = new Delivery
            {
                Order = order,
                Status = "IN_PROGRESS",
                DepartureTime = new DateTime(now.Year, now.Month, now.Day, 8, 0, 0)  // Today at 8 AM
            };
        }

        private async Task SubmitDeliveryAsync()
        {
            if (NewDelivery.Driver == null)
            {
                _loggingService.LogWarning("Cannot submit delivery: missing driver");
                return;
            }

            if (NewDelivery.Truck == null)
            {
                _loggingService.LogWarning("Cannot submit delivery: missing truck");
                return;
            }

            try
            {
                // Convert local time to UTC before saving
                NewDelivery.DepartureTime = DateTime.SpecifyKind(NewDelivery.DepartureTime, DateTimeKind.Local).ToUniversalTime();
                
                await _deliveryService.CreateDeliveryAsync(NewDelivery);
                _loggingService.LogInformation($"Created new delivery for order {NewDelivery.Order.OrderId}");
                _navigationService.NavigateTo<DeliveriesViewModel>();
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Failed to create delivery: {ex.Message}", ex);
            }
        }

        private void NavigateBack()
        {
            _navigationService.NavigateTo<DeliveriesViewModel>();
        }
    }
} 