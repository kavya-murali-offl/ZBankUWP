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
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.View;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBank.ViewModel
{
    public sealed partial class AddOrEditAccountViewModel : UserControl
    {
        private IView View { get; set; }

        public AddOrEditAccountViewModel(IView view)
        {
            this.InitializeComponent();
            View = view;
        }

        public SavingsAccount SavingsAccount { get; set; } = new SavingsAccount();
        public CurrentAccount CurrentAccount { get; set; } = new CurrentAccount();
        public TermDepositAccount DepositAccount { get; set; } = new TermDepositAccount();

        public IEnumerable<DropDownItem> TenureList { get; set; } = new List<DropDownItem>()
        {
            new DropDownItem("6 months", 3),
            new DropDownItem("1 year", 12),
            new DropDownItem("2 years", 24),
        };

        public bool IsEdit { get; set; }    

        public Account ContextAccount { get; set; }
    }
}
