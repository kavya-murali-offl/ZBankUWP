using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
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
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.Entity.BusinessObjects;
using ZBank.View.UserControls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZBank.View.DataTemplates
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CurrentAccountInfoTemplate : UserControl
    {

        public CurrentAccountInfoTemplate()
        {
            this.InitializeComponent();
        }

        public CurrentAccount SelectedAccount
        {
            get { return (CurrentAccount)GetValue(SelectedAccountProperty); }
            set { SetValue(SelectedAccountProperty, value); }
        }

        public static readonly DependencyProperty SelectedAccountProperty =
            DependencyProperty.Register("SelectedAccount", typeof(CurrentAccount), typeof(CurrentAccountInfoTemplate), new PropertyMetadata(null));
        
        public CardBObj LinkedCard
        {
            get { return (CardBObj)GetValue(LinkedCardProperty); }
            set { SetValue(LinkedCardProperty, value); }
        }

        public static readonly DependencyProperty LinkedCardProperty =
            DependencyProperty.Register("LinkedCard", typeof(CardBObj), typeof(CurrentAccountInfoTemplate), new PropertyMetadata(null));




        public ICommand LinkCardCommand
        {
            get { return (ICommand)GetValue(LinkCardCommandProperty); }
            set { SetValue(LinkCardCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LinkCardCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LinkCardCommandProperty =
            DependencyProperty.Register("LinkCardCommand", typeof(ICommand), typeof(CurrentAccountInfoTemplate), new PropertyMetadata(null));



        public IEnumerable<TransactionBObj> Transactions
        {
            get { return (IEnumerable<TransactionBObj>)GetValue(TransactionsProperty); }
            set { SetValue(TransactionsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Transactions.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TransactionsProperty =
            DependencyProperty.Register("Transactions", typeof(IEnumerable<TransactionBObj>), typeof(CurrentAccountInfoTemplate), new PropertyMetadata(null));

        private async void AddBeneficiaryButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog();
            dialog.RequestedTheme = ThemeService.Theme;
            dialog.Title = "Add Beneficiary";
            dialog.Content = new AddEditBeneficiaryView(dialog);
            await dialog.ShowAsync();
        }
    }
}
