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
    public sealed partial class RegisterPage : Page
    {
        private readonly RegisterViewModel viewModel;
        private Window _window;

        public RegisterPage(Window window)
        {
            this.InitializeComponent();
            viewModel = new RegisterViewModel(App.Services.GetRequiredService<IUserService>());
            this.DataContext = viewModel;
            _window = window;
        }

        private async void OnRegisterClicked(object sender, RoutedEventArgs e)
        {
            viewModel.Password = passwordBox.Password;
            viewModel.ConfirmPassword = confirmBox.Password;
            bool success = await viewModel.Register();

            if (success)
            {
                _window.Content = new LoginPage(_window);
            }
            else
            {
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Registration Failed",
                    Content = "Please check your input and try again.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };
                await dialog.ShowAsync();
            }
        }

        private void OnLoginNavigate(object sender, RoutedEventArgs e)
        {
            _window.Content = new LoginPage(_window);
        }
    }
} 