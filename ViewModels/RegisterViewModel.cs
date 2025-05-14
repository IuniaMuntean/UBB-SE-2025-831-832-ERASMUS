using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using UBB_SE_2025_EUROTRUCKERS.Helpers;
using UBB_SE_2025_EUROTRUCKERS.Models;
using UBB_SE_2025_EUROTRUCKERS.Services;
using Microsoft.Extensions.DependencyInjection;

namespace UBB_SE_2025_EUROTRUCKERS.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        private readonly IUserService _userService;
        public event PropertyChangedEventHandler PropertyChanged;

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
            }
        }

        public ICommand RegisterCommand { get; }

        public RegisterViewModel()
        {
            _userService = App.Services.GetRequiredService<IUserService>();
            RegisterCommand = new RelayCommand(async _ => await Register());
        }

        public RegisterViewModel(IUserService userService)
        {
            _userService = userService;
            RegisterCommand = new RelayCommand(async _ => await Register());
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task<bool> Register()
        {
            if (Password != ConfirmPassword)
                return false;

            var user = new User
            {
                Username = Username,
                Password = Password
            };

            return await _userService.RegisterAsync(user);
        }
    }
} 