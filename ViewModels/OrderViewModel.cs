using CommunityToolkit.Mvvm.ComponentModel;
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
using CommunityToolkit.Mvvm.Input;

namespace UBB_SE_2025_EUROTRUCKERS.ViewModels
{
    public class OrderViewModel : ViewModelBase
    {
        private readonly IOrderService _orderService;
        private readonly INavigationService _navigationService;
        private readonly ILoggingService _loggingService;

        private ObservableCollection<Order> _orders = new();
        public ObservableCollection<Order> Orders
        {
            get => _orders;
            set
            {
                _orders = value;
                OnPropertyChanged();
            }
        }

        public OrderViewModel(
            IOrderService orderService,
            INavigationService navigationService,
            ILoggingService loggingService)
        {
            _navigationService = navigationService;
            _loggingService = loggingService;
            _orderService = orderService;

            Title = "Orders"; // inherited from ViewModelBase

            LoadOrdersCommand = new AsyncRelayCommand(LoadOrdersAsync);
            NavigateToDetailsCommand = new RelayCommand<Order>(NavigateToDetails);
            AddOrderCommand = new AsyncRelayCommand<Order>(AddOrderAsync);
            DeleteOrderCommand = new AsyncRelayCommand<Order>(DeleteOrderAsync);
            UpdateOrderCommand = new AsyncRelayCommand<Order>(UpdateOrderAsync);

            _ = LoadOrdersCommand.ExecuteAsync((object?)null); // Fix CS8625
        }

        public IAsyncRelayCommand LoadOrdersCommand { get; }
        public IRelayCommand<Order> NavigateToDetailsCommand { get; }
        public IAsyncRelayCommand<Order> AddOrderCommand { get; }
        public IAsyncRelayCommand<Order> DeleteOrderCommand { get; }
        public IAsyncRelayCommand<Order> UpdateOrderCommand { get; }

        public async Task LoadOrdersAsync()
        {
            try
            {
                Orders.Clear();
                var orders = await _orderService.GetOrdersAsync();
                foreach (var order in orders)
                    Orders.Add(order);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error loading orders: {ex.Message}");
            }
        }

        public async Task AddOrderAsync(Order? order)
        {
            if (order is null)
            {
                _loggingService.LogWarning("Attempted to add a null order");
                return;
            }

            try
            {
                await _orderService.AddOrderAsync(order);
                Orders.Add(order);
                _loggingService.LogInformation($"Added order {order.OrderId}");
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Failed to add order {order.OrderId}: {ex.Message}", ex);
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
                Orders.Remove(order);
                _loggingService.LogInformation($"Deleted order {order.OrderId}");
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Failed to delete order {order.OrderId}: {ex.Message}", ex);
            }
        }

        public async Task UpdateOrderAsync(Order? order)
        {
            if (order is null)
            {
                _loggingService.LogWarning("Attempted to update a null order");
                return;
            }

            try
            {
                await _orderService.UpdateOrderAsync(order);
                var existingOrder = Orders.FirstOrDefault(o => o.OrderId == order.OrderId);
                if (existingOrder is not null)
                {
                    var index = Orders.IndexOf(existingOrder);
                    Orders[index] = order;
                }
                _loggingService.LogInformation($"Updated order {order.OrderId}");
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Failed to update order {order.OrderId}: {ex.Message}", ex);
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
