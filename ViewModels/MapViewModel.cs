using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UBB_SE_2025_EUROTRUCKERS.Services;

namespace UBB_SE_2025_EUROTRUCKERS.ViewModels
{
    internal class MapViewModel : ViewModelBase
    {
        private readonly IDeliveryService _deliveryService;
        private readonly INavigationService _navigationService;

        public MapViewModel()
        {

        }
    }
}
