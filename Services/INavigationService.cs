using Microsoft.UI.Xaml.Controls;
using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using UBB_SE_2025_EUROTRUCKERS.ViewModels;
using UBB_SE_2025_EUROTRUCKERS.Views;

namespace UBB_SE_2025_EUROTRUCKERS.Services
{
    public interface INavigationService
    {
        void SetContentFrame(Frame frame);
        void NavigateTo<TViewModel>() where TViewModel : ViewModelBase;
        void NavigateToWithParameter<TViewModel>(object parameter) where TViewModel : ViewModelBase;
        void GoBack();
        bool CanGoBack();
    }

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
                throw new InvalidOperationException("ContentFrame no ha sido establecido. Llama a SetContentFrame primero.");

            var viewType = GetViewTypeForViewModel(typeof(TViewModel));
            _contentFrame.Navigate(viewType);
        }

        public void NavigateToWithParameter<TViewModel>(object parameter) where TViewModel : ViewModelBase
        {
            if (_contentFrame == null)
                throw new InvalidOperationException("ContentFrame no ha sido establecido");

            var viewType = GetViewTypeForViewModel(typeof(TViewModel));

            if (viewType == null)
                throw new InvalidOperationException($"No se encontró la vista correspondiente para {typeof(TViewModel).FullName}");

            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter), "El parámetro no puede ser nulo");

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
            var viewName = viewModelType.FullName.Replace("ViewModel", "View");
            var viewAssemblyName = viewModelType.Assembly.FullName;
            var viewTypeName = $"{viewName}, {viewAssemblyName}";
            var viewType = Type.GetType(viewTypeName);

            if (viewType == null)
                throw new ArgumentException($"No se encontró la vista correspondiente para {viewModelType}");

            return viewType;
        }
    }
}