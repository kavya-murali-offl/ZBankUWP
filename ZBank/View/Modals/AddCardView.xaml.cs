using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ZBank.Entities;
using ZBank.ViewModel;
using ZBankManagement.Entities.BusinessObjects;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBank.View.Modals
{
    public sealed partial class AddCardView : UserControl, IView
    {

        private AddCardViewModel ViewModel { get; set; }    
        private ContentDialog Dialog { get; set; }
        public AddCardView(ContentDialog dialog)
        {
            this.InitializeComponent();
            ViewModel = new AddCardViewModel(this);
            Dialog = dialog;

        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView item)
            {
                if (item.SelectedIndex >= 0)
                {
                    ViewModel.SelectedCreditCardProvider = ViewModel.CreditCardProviders.ElementAt(item.SelectedIndex);
                    ProviderButton.Content = item.SelectedValue.ToString();
                }
                ProviderButton.Flyout.Hide();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.InsertCard(CardType.CREDIT);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Dialog?.Hide();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewNotifier.Instance.CardInserted += OnCardInserted;

        }

        private void OnCardInserted(bool isInserted, Card insertedCard)
        {
            Dialog?.Hide();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewNotifier.Instance.CardInserted -= OnCardInserted;

        }
    }
}
