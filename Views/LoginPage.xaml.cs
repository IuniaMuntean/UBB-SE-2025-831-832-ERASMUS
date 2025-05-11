using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using UBB_SE_2025_EUROTRUCKERS.ViewModels;
using UBB_SE_2025_EUROTRUCKERS.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace UBB_SE_2025_EUROTRUCKERS.Views
{
    public sealed partial class LoginPage : Page
    {
        private readonly LoginViewModel viewModel;
        private Window _window;

        public LoginPage(Window window)
        {
            this.InitializeComponent();
            viewModel = new LoginViewModel(App.Services.GetRequiredService<IUserService>());
            this.DataContext = viewModel;
            _window = window;
        }

        private async void OnLoginClicked(object sender, RoutedEventArgs e)
        {
            viewModel.Password = passwordBox.Password;
            bool success = await viewModel.Login();

            if (success)
            {
                var newMainWindow = new MainWindow();
                newMainWindow.Activate();
                _window.Close(); // optional: closes the login window
            }

            else
            {
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Login Failed",
                    Content = "Invalid username or password.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot 
                };
                await dialog.ShowAsync();
            }
        }

        private void OnRegisterNavigate(object sender, RoutedEventArgs e)
        {
            _window.Content = new RegisterPage(_window);
        }
    }
} 