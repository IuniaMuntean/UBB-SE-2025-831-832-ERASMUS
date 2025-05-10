using CommunityToolkit.Mvvm.ComponentModel;

namespace UBB_SE_2025_EUROTRUCKERS.ViewModels
{

    public partial class ViewModelBase : ObservableObject
    {
        [ObservableProperty]
        private string _title = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool _isBusy;

        public bool IsNotBusy => !_isBusy;
    }
}
