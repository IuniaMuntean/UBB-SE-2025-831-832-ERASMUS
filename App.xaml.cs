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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace UBB_SE_2025_EUROTRUCKERS
{
    public partial class App : Application
    {
        public IHost Host { get; private set; }
        public static IServiceProvider Services { get; private set; }

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
            services.AddLogging(configure => configure.AddDebug());

            // 4. Repositories
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // 5. ViewModels
            services.AddTransient<MainViewModel>();
            services.AddTransient<DeliveriesViewModel>();
            services.AddTransient<DetailsViewModel>();

            // 6. Views
            services.AddTransient<MainWindow>();
            services.AddTransient<DeliveriesView>();
            services.AddTransient<DetailsView>();

            // 7. Additional configuration
            services.AddLogging(configure => configure.AddDebug());
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);

            try
            {
                // Inicializar la base de datos
                using var scope = Services.CreateScope();
                var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
                await initializer.InitializeDatabaseAsync();

                // Mostrar la ventana principal
                var mainWindow = Services.GetRequiredService<MainWindow>();
                mainWindow.Activate();

                // Inicializar servicios que necesitan la ventana
                var dialogService = Services.GetRequiredService<IDialogService>();
                if (dialogService is DialogService concreteDialogService)
                {
                    // Esperar a que la ventana esté lista
                    await Task.Delay(300); // Pequeña espera para asegurar la inicialización
                    concreteDialogService.Initialize(mainWindow);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error durante el inicio: {ex}");

                try
                {
                    var dialogService = Services.GetRequiredService<IDialogService>();
                    await dialogService.ShowErrorDialogAsync(
                        "Error de inicio",
                        $"No se pudo iniciar la aplicación. Error: {ex.Message}");
                }
                catch (Exception dialogEx)
                {
                    Debug.WriteLine($"Error al mostrar diálogo de error: {dialogEx}");
                    // Fallback absoluto
                    System.Diagnostics.Debug.WriteLine($"CRITICAL ERROR: {ex}");
                }

                Current.Exit();
            }
        }
    }
}
