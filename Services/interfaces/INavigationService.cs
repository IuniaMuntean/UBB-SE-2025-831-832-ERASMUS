using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UBB_SE_2025_EUROTRUCKERS.ViewModels;

namespace UBB_SE_2025_EUROTRUCKERS.Services.interfaces
{
    public interface INavigationService
    {
        void SetContentFrame(Frame frame);
        void NavigateTo<TViewModel>() where TViewModel : ViewModelBase;
        void NavigateToWithParameter<TViewModel>(object parameter) where TViewModel : ViewModelBase;
        void GoBack();
        bool CanGoBack();
    }
}
