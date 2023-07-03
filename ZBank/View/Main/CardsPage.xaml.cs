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
using ZBank.Config;
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
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.OnPageLoaded();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.OnPageUnLoaded();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

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

        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if(ViewModel?.DataModel?.OnViewCard != null)
            {
                ViewModel.DataModel.OnViewCard.SpendingLimit = decimal.Parse(LimitSlider.Value.ToString());
            }
        }

        private void AddCardButton_Click(object sender, RoutedEventArgs e)
        {

        }
    } 
}

