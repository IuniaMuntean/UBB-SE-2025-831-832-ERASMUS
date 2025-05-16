using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UBB_SE_2025_EUROTRUCKERS.Models;
using UBB_SE_2025_EUROTRUCKERS.Services;
using UBB_SE_2025_EUROTRUCKERS.Services.interfaces;
using UBB_SE_2025_EUROTRUCKERS.Views;

namespace UBB_SE_2025_EUROTRUCKERS.ViewModels
{
    public partial class OrderViewModel : ViewModelBase
    {
        private readonly IOrderService _orderService;
        private readonly INavigationService _navigationService;
        private readonly ILoggingService _loggingService;
        private readonly ICityService _cityService;

        [ObservableProperty]
        private ObservableCollection<Order> _orders = new();

        [ObservableProperty]
        private ObservableCollection<City> _availableCities = new();

        [ObservableProperty]
        private Order _newOrder = new();

        [ObservableProperty]
        private bool _isEditMode;

        public OrderViewModel(
            IOrderService orderService,
            INavigationService navigationService,
            ILoggingService loggingService,
            ICityService cityService)
        {
            _navigationService = navigationService;
            _loggingService = loggingService;
            _orderService = orderService;
            _cityService = cityService;

            Title = "Orders";

            LoadOrdersCommand = new AsyncRelayCommand(LoadOrdersAsync);
            NavigateToDetailsCommand = new RelayCommand<Order>(NavigateToDetails);
            AddOrderCommand = new AsyncRelayCommand(NavigateToAddOrder);
            DeleteOrderCommand = new AsyncRelayCommand<Order>(DeleteOrderAsync);
            UpdateOrderCommand = new AsyncRelayCommand<Order>(NavigateToUpdateOrder);
            NavigateBackCommand = new RelayCommand(NavigateBack);
            SubmitOrderCommand = new AsyncRelayCommand(SubmitOrderAsync);

            _ = LoadOrdersCommand.ExecuteAsync((object?)null);
            _ = LoadCitiesAsync();
        }

        public IAsyncRelayCommand LoadOrdersCommand { get; }
        public IRelayCommand<Order> NavigateToDetailsCommand { get; }
        public IAsyncRelayCommand AddOrderCommand { get; }
        public IRelayCommand<Order> DeleteOrderCommand { get; }
        public IRelayCommand<Order> UpdateOrderCommand { get; }
        public IRelayCommand NavigateBackCommand { get; }
        public IAsyncRelayCommand SubmitOrderCommand { get; }

        private async Task LoadCitiesAsync()
        {
            try
            {
                var cities = await _cityService.GetCitiesAsync();
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

        public async Task LoadOrdersAsync()
        {
            try
            {
                Orders.Clear();
                var orders = await _orderService.GetOrdersAsync();
                foreach (var order in orders)
                {
                    if (order.SourceCity != null && order.DestinationCity != null)
                    {
                        Orders.Add(order);
                    }
                    else
                    {
                        _loggingService.LogWarning($"Order {order.OrderId} has missing city data");
                    }
                }
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error loading orders: {ex.Message}");
            }
        }

        private async Task NavigateToAddOrder()
        {
            IsEditMode = false;
            NewOrder = new Order(); // Reset the form
            _navigationService.NavigateTo<AddOrderView>();
        }

        private async Task NavigateToUpdateOrder(Order order)
        {
            if (order == null) return;
            
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
            _navigationService.NavigateTo<AddOrderView>();
        }

        private void NavigateBack()
        {
            _navigationService.NavigateTo<OrdersView>();
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
                
                await LoadOrdersAsync(); // Refresh the list
                _navigationService.NavigateTo<OrdersView>();
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Failed to {(IsEditMode ? "update" : "add")} order: {ex.Message}", ex);
            }
        }

        public async Task DeleteOrderAsync(Order? order)
        {
            if (order is null)
            {
                _loggingService.LogWarning("Attempted to delete a null order");
                return;
            }

            try
            {
                await _orderService.DeleteOrderAsync(order.OrderId);
                await LoadOrdersAsync(); // Refresh the list
                _loggingService.LogInformation($"Deleted order {order.OrderId}");
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Failed to delete order {order.OrderId}: {ex.Message}", ex);
            }
        }

        private void NavigateToDetails(Order? order)
        {
            if (order == null)
                return;

            _navigationService.NavigateToWithParameter<OrderViewModel>(order);
            _loggingService.LogDebug($"Navigating to details view for order {order.OrderId}");
        }

        public event PropertyChangedEventHandler? PropertyChanged; // Fix CS8618
        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
