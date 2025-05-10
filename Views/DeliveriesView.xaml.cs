using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using UBB_SE_2025_EUROTRUCKERS.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using UBB_SE_2025_EUROTRUCKERS.Models;

namespace UBB_SE_2025_EUROTRUCKERS.Views
{
    public sealed partial class DeliveriesView : Page
    {
        public DeliveriesViewModel ViewModel { get; }

        public DeliveriesView()
        {
            this.InitializeComponent();
            ViewModel = App.Services.GetRequiredService<DeliveriesViewModel>();
            this.DataContext = ViewModel;
        }

        private void DeliveryCard_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is Delivery delivery)
            {
                ViewModel.NavigateToDetailsCommand.Execute(delivery);
            }
        }
    }
}