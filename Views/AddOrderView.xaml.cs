using Microsoft.UI.Xaml.Controls;
using Microsoft.Extensions.DependencyInjection;
using UBB_SE_2025_EUROTRUCKERS.ViewModels;
using System;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using UBB_SE_2025_EUROTRUCKERS.Models;

namespace UBB_SE_2025_EUROTRUCKERS.Views
{
    public sealed partial class AddOrderView : Page
    {
        public AddOrderViewModel ViewModel { get; }
        private Window _window;

        public AddOrderView()
        {
            this.InitializeComponent();
            ViewModel = App.Services.GetRequiredService<AddOrderViewModel>();
            this.DataContext = ViewModel;
            _ = ViewModel.InitializeAsync();
        }

        protected override void OnNavigatedTo(Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is Order order)
            {
                ViewModel.InitializeForEdit(order);
            }
        }

        private async void OnSubmitClicked(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(ViewModel.NewOrder.ClientName))
            {
                await ShowErrorDialog("Please enter a client name");
                return;
            }

            if (string.IsNullOrWhiteSpace(ViewModel.NewOrder.CargoType))
            {
                await ShowErrorDialog("Please enter a cargo type");
                return;
            }

            if (ViewModel.NewOrder.CargoWeight <= 0)
            {
                await ShowErrorDialog("Please enter a valid cargo weight");
                return;
            }

            if (ViewModel.NewOrder.SourceCity == null)
            {
                await ShowErrorDialog("Please select a source city");
                return;
            }

            if (ViewModel.NewOrder.DestinationCity == null)
            {
                await ShowErrorDialog("Please select a destination city");
                return;
            }

            // Submit the order
            await ViewModel.SubmitOrderCommand.ExecuteAsync(null);
        }

        private async void OnBackClicked(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.NavigateBackCommand.Execute(null);
        }

        private async Task ShowErrorDialog(string message)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Validation Error",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            await dialog.ShowAsync();
        }
    }
} 