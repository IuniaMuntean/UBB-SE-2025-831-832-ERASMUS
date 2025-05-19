using Microsoft.UI.Xaml.Controls;
using Microsoft.Extensions.DependencyInjection;
using UBB_SE_2025_EUROTRUCKERS.ViewModels;

namespace UBB_SE_2025_EUROTRUCKERS.Views
{
    public sealed partial class OrdersView : Page
    {
        public OrderViewModel ViewModel { get; }

        public OrdersView()
        {
            this.InitializeComponent();
            ViewModel = App.Services.GetRequiredService<OrderViewModel>();
            this.DataContext = ViewModel;
        }
    }
} 