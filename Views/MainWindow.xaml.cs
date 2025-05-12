using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using UBB_SE_2025_EUROTRUCKERS.Services;
using UBB_SE_2025_EUROTRUCKERS.ViewModels;

namespace UBB_SE_2025_EUROTRUCKERS.Views
{
    public sealed partial class MainWindow : Window
    {
        public MainViewModel ViewModel { get; }

        public MainWindow()
        {
            this.InitializeComponent();
            ViewModel = App.Services.GetRequiredService<MainViewModel>();
            var navigationService = App.Services.GetRequiredService<INavigationService>();
            navigationService.SetContentFrame(MainContent);

        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.InvokedItemContainer is NavigationViewItem item)
            {
                switch (item.Tag.ToString())
                {
                    case "deliveries":
                        ViewModel.NavigateToDeliveriesCommand.Execute(null);
                        break;
                    case "logout":
                        ViewModel.LogOutCommand.Execute(null);
                        break;
                    case "mappage":
                        {
                            FullMapView mp = new FullMapView();
                            mp.Activate();
                            //ViewModel.NavigateToMapViewCommand.Execute(null);
                            break;
                        }
                }
            }
        }
    }
}