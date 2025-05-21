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
        private readonly IMapService _mapService;

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
            IMapService mapService)
        {
            _navigationService = navigationService;
            _loggingService = loggingService;
            _orderService = orderService;
            _mapService = mapService;

            Title = "Orders";

            LoadOrdersCommand = new AsyncRelayCommand(LoadOrdersAsync);
            NavigateToDetailsCommand = new RelayCommand<Order>(NavigateToDetails);
            AddOrderCommand = new AsyncRelayCommand(NavigateToAddOrder);
            DeleteOrderCommand = new AsyncRelayCommand<Order>(DeleteOrderAsync);
            UpdateOrderCommand = new AsyncRelayCommand<Order>(NavigateToUpdateOrder);
            NavigateBackCommand = new RelayCommand(NavigateBack);
            SubmitOrderCommand = new AsyncRelayCommand(SubmitOrderAsync);
            CreateDeliveryCommand = new RelayCommand<Order>(NavigateToCreateDelivery);

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
        public IRelayCommand<Order> CreateDeliveryCommand { get; }

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
            _navigationService.NavigateTo<AddOrderViewModel>();
        }

        private async Task NavigateToUpdateOrder(Order order)
        {
            if (order == null) return;
            _navigationService.NavigateToWithParameter<AddOrderViewModel>(order);
        }

        private void NavigateBack()
        {
            _navigationService.NavigateTo<OrderViewModel>();
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
                _navigationService.NavigateTo<OrderViewModel>();
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

        private void NavigateToCreateDelivery(Order order)
        {
            if (order != null)
            {
                _navigationService.NavigateToWithParameter<AddDeliveryViewModel>(order);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
