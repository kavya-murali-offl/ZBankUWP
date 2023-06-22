using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
using ZBank.ViewModel;
using ZBankManagement.Entity.EnumerationTypes;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBank.View.UserControls
{
    public sealed partial class AddEditBeneficiaryView : UserControl, IView
    {
        private AddEditBeneficiaryViewModel ViewModel { get; set; }
        
        public AddEditBeneficiaryView()
        {
            this.InitializeComponent();
            ViewModel = new AddEditBeneficiaryViewModel(this);
        }

    
     
    }
}
