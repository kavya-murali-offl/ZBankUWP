using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using ZBank.AppEvents.AppEventArgs;
using ZBank.AppEvents;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.View.UserControls;
using ZBank.ViewModel;
using ZBank.View.Main;
using ZBank.View.Modals;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZBank.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DashboardPage : Page, IView
    {
        public int OnViewCardIndex { get; set; } = 0;   

        public DashboardViewModel ViewModel { get; set; }

        private ContentDialog Dialog { get; set; }

        public string EnteredAmount { get; set; }  
        
        public string EnteredDescription { get; set; }   

        public Account SelectedAccount { get; set; }   

        public Beneficiary SelectedBeneficiary { get; set; }
        
        public ModeOfPayment SelectedPaymentMode { get; set; }   

        public DashboardPage()
        {
            this.InitializeComponent();
            ViewModel = new DashboardViewModel(this); }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ViewNotifier.Instance.CardInserted += OnCardInserted;
            ViewModel.OnLoaded();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewNotifier.Instance.CardInserted -= OnCardInserted;
            ViewModel.OnUnLoaded();
        }


        private void Transactions_ViewMoreButton_Click(object sender, RoutedEventArgs e)
        {
            FrameContentChangedArgs args = new FrameContentChangedArgs()
            {
                PageType = typeof(TransactionsPage),
                Params = null
            };

            ViewNotifier.Instance.OnFrameContentChanged(args);
        }

        private void ManageCardButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ManageCard();
        }

        private void OnCardInserted(bool arg1, Card arg2)
        {
            Dialog?.Hide();
        }

        private async void NewCardButton_Click(object sender, RoutedEventArgs e)
        {
            CustomContentDialog contentDialog = new CustomContentDialog();
            contentDialog.DialogContent = new AddCardView(contentDialog.Dialog);
            contentDialog.Dialog.Title = "New Credit Card";
            Dialog = contentDialog.Dialog;
            await contentDialog.OpenDialog();
        }

        private async void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FadeStoryBoard.Begin();
            await Task.Delay(500);
            FadeStoryBoard.Stop();
        }
    }
}

public enum DirectionButton
{
    NEXT,
    PREVIOUS
}
