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
    public partial class AddOrderViewModel : ViewModelBase
    {
        private readonly IOrderService _orderService;
        private readonly INavigationService _navigationService;
        private readonly ILoggingService _loggingService;
        private readonly IMapService _mapService;

        private ObservableCollection<City> _availableCities = new();
        public ObservableCollection<City> AvailableCities
        {
            get => _availableCities;
            set => SetProperty(ref _availableCities, value);
        }

        private Order _newOrder = new();
        public Order NewOrder
        {
            get => _newOrder;
            set => SetProperty(ref _newOrder, value);
        }

        private bool _isEditMode;
        public bool IsEditMode
        {
            get => _isEditMode;
            set => SetProperty(ref _isEditMode, value);
        }

        public string PageTitle => IsEditMode ? "Edit Order" : "Add New Order";
        public string SubmitButtonText => IsEditMode ? "Update Order" : "Create Order";

        public AddOrderViewModel(
            IOrderService orderService,
            INavigationService navigationService,
            ILoggingService loggingService,
            IMapService mapService)
        {
            _orderService = orderService;
            _navigationService = navigationService;
            _loggingService = loggingService;
            _mapService = mapService;

            Title = PageTitle;
            SubmitOrderCommand = new AsyncRelayCommand(SubmitOrderAsync);
            NavigateBackCommand = new RelayCommand(NavigateBack);
        }

        public IAsyncRelayCommand SubmitOrderCommand { get; }
        public IRelayCommand NavigateBackCommand { get; }

        public async Task InitializeAsync()
        {
            await LoadCitiesAsync();
        }

        private async Task LoadCitiesAsync()
        {
            try
            {
                var cities = await _mapService.GetCitiesAsync();
                AvailableCities.Clear();
                foreach (var city in cities)
                {
                    AvailableCities.Add(city);
                }
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error loading cities: {ex.Message}");
            }
        }

        public void InitializeForEdit(Order order)
        {
            IsEditMode = true;
            NewOrder = new Order
            {
                OrderId = order.OrderId,
                ClientName = order.ClientName,
                CargoType = order.CargoType,
                CargoWeight = order.CargoWeight,
                SourceCity = order.SourceCity,
                DestinationCity = order.DestinationCity
            };
            Title = PageTitle;
        }

        private async Task SubmitOrderAsync()
        {
            if (NewOrder.SourceCity == null || NewOrder.DestinationCity == null)
            {
                _loggingService.LogWarning("Cannot submit order: missing city data");
                return;
            }

            try
            {
                if (IsEditMode)
                {
                    await _orderService.UpdateOrderAsync(NewOrder);
                    _loggingService.LogInformation($"Updated order {NewOrder.OrderId}");
                }
                else
                {
                    await _orderService.AddOrderAsync(NewOrder);
                    _loggingService.LogInformation($"Added new order for client {NewOrder.ClientName}");
                }
                _navigationService.NavigateTo<OrderViewModel>();
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Failed to {(IsEditMode ? "update" : "add")} order: {ex.Message}", ex);
            }
        }

        private void NavigateBack()
        {
            _navigationService.NavigateTo<OrderViewModel>();
        }
    }
} 