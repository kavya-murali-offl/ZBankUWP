using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using ZBank.Entity.BusinessObjects;
using ZBankManagement.AppEvents.AppEventArgs;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBank.View.UserControls
{
    public sealed partial class ResetPinContent : UserControl, INotifyPropertyChanged
    {
        public ResetPinContent()
        {
            this.InitializeComponent();
        }

       public ResetPinContent(ContentDialog dialog, CardBObj card, ICommand submitCommand)
       {
            this.InitializeComponent();
            SelectedCard = card;
            SubmitCommand = submitCommand;
            ContentDialog = dialog;
       }

        private ContentDialog ContentDialog { get; set; }

        public CardBObj SelectedCard
        {
            get { return (CardBObj)GetValue(SelectedCardProperty); }
            set { SetValue(SelectedCardProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedCard.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedCardProperty =
            DependencyProperty.Register("SelectedCard", typeof(CardBObj), typeof(ResetPinContent), new PropertyMetadata(null));

        public ICommand SubmitCommand
        {
            get { return (ICommand)GetValue(SubmitCommandProperty); }
            set { SetValue(SubmitCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SubmitCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SubmitCommandProperty =
            DependencyProperty.Register("SubmitCommand", typeof(ICommand), typeof(ResetPinContent), new PropertyMetadata(null));

        public event PropertyChangedEventHandler PropertyChanged;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateFields())
            {
                SubmitCommand.Execute(new ResetPinArgs()
                {
                    CardNumber = SelectedCard.CardNumber,
                    PinNumber = NewPin
                });
                ContentDialog.Hide();
            }
        }

        private string _errorText { get; set; }  
        public string ErrorText
        {
            get =>  _errorText;
            set
            {
                _errorText = value;
                OnPropertyChanged();
            }
        }


        private bool ValidateFields()
        {
            if(string.IsNullOrEmpty(NewPin) || string.IsNullOrWhiteSpace(NewPin))
            {
                ErrorText = "Pin Number is Required";
            }
            else if(NewPin.Length != 4 || !int.TryParse(NewPin, out int pinNumber))
            {
                ErrorText = "Pin Number should be a number of 4 digits";
            }
           
            if (ErrorText == null || ErrorText.Length == 0) { 
                return true;
            }
            return false;
        }

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string NewPin { get; set; }

        private void PinBox_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            NewPin = sender.Text;
            ErrorText = string.Empty;   
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog.Hide();   
        }
    }
}
