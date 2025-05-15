using System;
using Microsoft.UI.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using LaunchActivatedEventArgs = Microsoft.UI.Xaml.LaunchActivatedEventArgs;
using UBB_SE_2025_EUROTRUCKERS.ViewModels;
using UBB_SE_2025_EUROTRUCKERS.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Threading.Tasks;
using UBB_SE_2025_EUROTRUCKERS.Services;
using UBB_SE_2025_EUROTRUCKERS.Data;
using UBB_SE_2025_EUROTRUCKERS.Services.interfaces;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace UBB_SE_2025_EUROTRUCKERS
{
    public partial class App : Application
    {
        public IHost Host { get; private set; }
        public static IServiceProvider Services { get; private set; }
        public static Window MainWindow { get; private set; }

        public App()
        {
            this.InitializeComponent();

            Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
                .ConfigureServices(ConfigureServices)
                .Build();

            Services = Host.Services;
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // 1. Configuration of Entity Framework Core with PostgreSQL
            services.AddDbContext<TransportDbContext>(options =>
            {
                options.UseNpgsql("Host=localhost;Database=transport_dev;Username=postgres;Password=postgres");

                // Aditional settings (for development)
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            });

            // 2. DB initilization
            services.AddTransient<DatabaseInitializer>();

            // 3. App services
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddTransient<IDeliveryService, DeliveryService>();
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<ILoggingService, LoggingService>();
            services.AddTransient<IUserService, UserService>();
            services.AddLogging(configure => configure.AddDebug());

            // 4. Repositories
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // 5. ViewModels
            services.AddTransient<MainViewModel>();
            services.AddTransient<DeliveriesViewModel>();
            services.AddTransient<DetailsViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<RegisterViewModel>();

            // 6. Views
            services.AddTransient<MainWindow>();
            services.AddTransient<DeliveriesView>();
            services.AddTransient<DetailsView>();
            services.AddTransient<LoginPage>();
            services.AddTransient<RegisterPage>();

            // 7. Additional configuration
            services.AddLogging(configure => configure.AddDebug());
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);

            try
            {
                // Initialize database
                using var scope = Services.CreateScope();
                var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
                await initializer.InitializeDatabaseAsync();

                MainWindow = new MainWindow();             // create the MainWindow
                App.MainWindow = MainWindow;               // ✅ this line is essential!
                MainWindow.Content = new LoginPage(MainWindow); // ✅ pass window reference!
                MainWindow.Activate();

                // Initialize services that need the window
                var dialogService = Services.GetRequiredService<IDialogService>();
                if (dialogService is DialogService concreteDialogService)
                {
                    await Task.Delay(300); // Small delay to ensure initialization
                    concreteDialogService.Initialize(MainWindow);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during startup: {ex}");

                try
                {
                    var dialogService = Services.GetRequiredService<IDialogService>();
                    await dialogService.ShowErrorDialogAsync(
                        "Startup Error",
                        $"Could not start the application. Error: {ex.Message}");
                }
                catch (Exception dialogEx)
                {
                    Debug.WriteLine($"Error showing error dialog: {dialogEx}");
                    // Absolute fallback
                    System.Diagnostics.Debug.WriteLine($"CRITICAL ERROR: {ex}");
                }

                Current.Exit();
            }
        }
    }
}
