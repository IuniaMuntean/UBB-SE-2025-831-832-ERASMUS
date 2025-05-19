using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UBB_SE_2025_EUROTRUCKERS.Models;
using UBB_SE_2025_EUROTRUCKERS.Services;
using UBB_SE_2025_EUROTRUCKERS.Services.interfaces;

namespace UBB_SE_2025_EUROTRUCKERS.ViewModels
{
    public class ResourcesViewModel : ViewModelBase
    {
        private readonly IResourceServices _resourceServices;
        private readonly INavigationService _navigationService;
        private readonly ILoggingService _loggingService;

        public ResourcesViewModel(
            IResourceServices resourceService,
            INavigationService navigationService,
            ILoggingService loggingService)
        {
            _resourceServices = resourceService;
            _navigationService = navigationService;
            _loggingService = loggingService;
            Title = "Resources";
        }
    }
}
