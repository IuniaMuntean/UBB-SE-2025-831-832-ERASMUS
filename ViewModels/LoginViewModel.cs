using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using UBB_SE_2025_EUROTRUCKERS.Helpers;
using UBB_SE_2025_EUROTRUCKERS.Models;
using UBB_SE_2025_EUROTRUCKERS.Services;
using Microsoft.Extensions.DependencyInjection;

namespace UBB_SE_2025_EUROTRUCKERS.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
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

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            _userService = App.Services.GetRequiredService<IUserService>();
            LoginCommand = new RelayCommand(async _ => await Login());
        }

        public LoginViewModel(IUserService userService)
        {
            _userService = userService;
            LoginCommand = new RelayCommand(async _ => await Login());
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task<bool> Login()
        {
            var user = new User
            {
                Username = Username,
                Password = Password
            };

            return await _userService.LoginAsync(user);
        }
    }
} 