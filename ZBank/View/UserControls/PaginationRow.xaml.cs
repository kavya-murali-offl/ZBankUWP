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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBank.View.UserControls
{
    public sealed partial class PaginationRow : UserControl
    {
        public PaginationRow()
        {
            this.InitializeComponent();
        }

        public int CurrentPage
        {
            get { return (int)GetValue(CurrentPageProperty); }
            set { SetValue(CurrentPageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentPage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentPageProperty =
            DependencyProperty.Register("CurrentPage", typeof(int), typeof(PaginationRow), new PropertyMetadata(0));



        public int TotalPages
        {
            get { return (int)GetValue(TotalPagesProperty); }
            set { SetValue(TotalPagesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TotalPages.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TotalPagesProperty =
            DependencyProperty.Register("TotalPages", typeof(int), typeof(PaginationRow), new PropertyMetadata(0));


        public int RowsPerPage
        {
            get { return (int)GetValue(RowsPerPageProperty); }
            set { SetValue(RowsPerPageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RowsPerPage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowsPerPageProperty =
            DependencyProperty.Register("RowsPerPage", typeof(int), typeof(PaginationRow), new PropertyMetadata(0));


        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
