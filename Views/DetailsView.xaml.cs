using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml;
using System;
using Microsoft.Extensions.DependencyInjection;
using UBB_SE_2025_EUROTRUCKERS.ViewModels;
using UBB_SE_2025_EUROTRUCKERS.Models;

namespace UBB_SE_2025_EUROTRUCKERS.Views
{
    public sealed partial class DetailsView : Page
    {
        public static readonly DependencyProperty SelectedDeliveryProperty =
            DependencyProperty.Register(
                nameof(SelectedDelivery),
                typeof(Delivery),
                typeof(DetailsView),
                new PropertyMetadata(null, OnSelectedDeliveryChanged));

        public Delivery SelectedDelivery
        {
            get => (Delivery)GetValue(SelectedDeliveryProperty);
            set => SetValue(SelectedDeliveryProperty, value);
        }

        public DetailsViewModel ViewModel { get; }

        public DetailsView()
        {
            this.InitializeComponent();
            ViewModel = App.Services.GetRequiredService<DetailsViewModel>();
            this.DataContext = ViewModel;
        }

        private static void OnSelectedDeliveryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DetailsView view && e.NewValue is Delivery delivery)
            {
                view.ViewModel.SelectedDelivery = delivery;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is Delivery delivery)
            {
                SelectedDelivery = delivery;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}

