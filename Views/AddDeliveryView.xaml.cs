using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Extensions.DependencyInjection;
using UBB_SE_2025_EUROTRUCKERS.Models;
using UBB_SE_2025_EUROTRUCKERS.ViewModels;

namespace UBB_SE_2025_EUROTRUCKERS.Views
{
    public sealed partial class AddDeliveryView : Page
    {
        public AddDeliveryViewModel ViewModel { get; }

        public AddDeliveryView()
        {
            this.InitializeComponent();
            ViewModel = App.Services.GetRequiredService<AddDeliveryViewModel>();
            this.DataContext = ViewModel;
            _ = ViewModel.InitializeAsync();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is Order order)
            {
                ViewModel.InitializeWithOrder(order);
            }
        }
    }
} 