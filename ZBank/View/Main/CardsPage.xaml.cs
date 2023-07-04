using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZBank.AppEvents;
using ZBank.Config;
using ZBank.Entities;
using ZBank.View.Modals;
using ZBank.View.UserControls;
using ZBank.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZBank.View.Main
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CardsPage : Page, IView
    {
        private CardsViewModel ViewModel { get; set; }   

        public CardsPage()
        {
            this.InitializeComponent();
            ViewModel = new CardsViewModel(this);
            LimitSlider.IsEnabled = false;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.OnPageLoaded();
            ViewNotifier.Instance.LimitUpdated += OnUpdatedLimit;
        }

        private void OnUpdatedLimit(bool isUpdated, Card updatedCard)
        {
            LimitSlider.IsEnabled = false;
            if (isUpdated && ViewModel.DataModel.OnViewCard.CardNumber == updatedCard.CardNumber)
            {
                LimitSlider.Value = double.Parse(updatedCard.TransactionLimit.ToString());
            }
            else
            {
                LimitSlider.Value = double.Parse(ViewModel.DataModel.OnViewCard.TransactionLimit.ToString());
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.OnPageUnLoaded();
            ViewNotifier.Instance.LimitUpdated -= OnUpdatedLimit;
        }

        private void ViewEyeButton_Click(object sender, RoutedEventArgs e)
        {
            HideEyeButton.Visibility = Visibility.Visible;
            ViewEyeButton.Visibility = Visibility.Collapsed;
        }

        private void HideEyeButton_Click(object sender, RoutedEventArgs e)
        {
            ViewEyeButton.Visibility = Visibility.Visible;
            HideEyeButton.Visibility = Visibility.Collapsed;
        }

        private async void ResetPinButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = this.XamlRoot;
            dialog.RequestedTheme = ThemeSelector.Theme;
            dialog.Title = "Reset Pin";
            dialog.Content = new ResetPinContent(dialog, ViewModel.DataModel.OnViewCard, ViewModel.ResetPinCommand);
            await dialog.ShowAsync();
        }

       
        private async void AddCardButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = this.XamlRoot;
            dialog.RequestedTheme = ThemeSelector.Theme;
            dialog.Title = "New Credit Card";
            dialog.Content = new AddCardView(dialog);
            await dialog.ShowAsync();
        }

        private void ChangeLimitButton_Click(object sender, RoutedEventArgs e)
        {
            LimitSlider.IsEnabled = true;
        }

        private void UpdateLimitSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if(ViewModel.DataModel.OnViewCard.SpendingLimit != decimal.Parse(UpdatedLimit.ToString()))
            {
                ViewModel.UpdateTransactionLimit(LimitSlider.Value);
            }
        }

        private void CancelUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            LimitSlider.Value = double.Parse(ViewModel.DataModel.OnViewCard.SpendingLimit.ToString());
            LimitSlider.IsEnabled = false;  
        }

        private double UpdatedLimit { get; set; }

        private void LimitSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            UpdatedLimit = e.NewValue;
        }

        private void PayCardButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog contentDialog = new ContentDialog();  

        }
    } 
}

