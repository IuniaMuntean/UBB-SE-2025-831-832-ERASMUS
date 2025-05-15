using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using UBB_SE_2025_EUROTRUCKERS.ViewModels;

namespace UBB_SE_2025_EUROTRUCKERS.Views
{
    public sealed partial class ResourcesView : Page
    {
        public ResourcesViewModel ViewModel { get; }
        public ResourcesView()
        {
            this.InitializeComponent();
            ViewModel = App.Services.GetRequiredService<ResourcesViewModel>();
            this.DataContext = ViewModel;
        }
    }
}
