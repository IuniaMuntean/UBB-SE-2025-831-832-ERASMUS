using Microsoft.UI.Xaml.Controls;
using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using UBB_SE_2025_EUROTRUCKERS.ViewModels;
using UBB_SE_2025_EUROTRUCKERS.Views;
using UBB_SE_2025_EUROTRUCKERS.Services.interfaces;

namespace UBB_SE_2025_EUROTRUCKERS.Services
{
    

    public class NavigationService : INavigationService
    {
        private Frame _contentFrame;
        private readonly IServiceProvider _serviceProvider;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void SetContentFrame(Frame frame)
        {
            _contentFrame = frame ?? throw new ArgumentNullException(nameof(frame));
        }

        public void NavigateTo<TViewModel>() where TViewModel : ViewModelBase
        {
            if (_contentFrame == null)
                throw new InvalidOperationException("ContentFrame has not been set. Call SetContentFrame first.");

            var viewType = GetViewTypeForViewModel(typeof(TViewModel));
            _contentFrame.Navigate(viewType);
        }

        public void NavigateToWithParameter<TViewModel>(object parameter) where TViewModel : ViewModelBase
        {
            if (_contentFrame == null)
                throw new InvalidOperationException("ContentFrame has not been set");

            var viewType = GetViewTypeForViewModel(typeof(TViewModel));

            if (viewType == null)
                throw new InvalidOperationException($"Could not find the corresponding view for {typeof(TViewModel).FullName}");

            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter), "Parameter cannot be null");

            _contentFrame.Navigate(viewType, parameter);
        }

        public bool CanGoBack()
        {
            return _contentFrame?.CanGoBack ?? false;
        }

        public void GoBack()
        {
            if (_contentFrame?.CanGoBack == true)
            {
                _contentFrame.GoBack();
            }
        }

        private Type GetViewTypeForViewModel(Type viewModelType)
        {
            // Get the base namespace by removing the ViewModels part
            var baseNamespace = viewModelType.Namespace.Replace("ViewModels", "");
            
            // Try singular form first
            var singularViewName = $"{baseNamespace}Views.{viewModelType.Name.Replace("ViewModel", "View")}";
            var viewType = Type.GetType(singularViewName + ", " + viewModelType.Assembly.FullName);

            // If not found, try plural form
            if (viewType == null)
            {
                var pluralViewName = $"{baseNamespace}Views.{viewModelType.Name.Replace("ViewModel", "sView")}";
                viewType = Type.GetType(pluralViewName + ", " + viewModelType.Assembly.FullName);
            }

            if (viewType == null)
            {
                var attemptedSingular = $"{baseNamespace}Views.{viewModelType.Name.Replace("ViewModel", "View")}";
                var attemptedPlural = $"{baseNamespace}Views.{viewModelType.Name.Replace("ViewModel", "sView")}";
                throw new ArgumentException($"Could not find the corresponding view for {viewModelType}. Attempted to find {attemptedSingular} and {attemptedPlural}");
            }

            return viewType;
        }
    }
}