using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UBB_SE_2025_EUROTRUCKERS.Models;
using UBB_SE_2025_EUROTRUCKERS.Services;

namespace UBB_SE_2025_EUROTRUCKERS.ViewModels
{
    public partial class DetailsViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private Delivery? _selectedDelivery;

        public DetailsViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            Title = "Delivery Details";
        }

        partial void OnSelectedDeliveryChanged(Delivery? value)
        {
            // Handle any logic when SelectedDelivery changes
            if (value != null)
            {
                // You can add any additional logic here when the delivery is set
            }
        }
    }
}
