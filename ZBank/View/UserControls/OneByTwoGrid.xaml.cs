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
    public sealed partial class OneByTwoGrid : UserControl
    {
        public OneByTwoGrid()
        {
            this.InitializeComponent();
        }

        public FrameworkElement Column1Content
        {
            get { return (FrameworkElement)GetValue(Column1ContentProperty); }
            set { SetValue(Column1ContentProperty, value); }
        }

        public static readonly DependencyProperty Column1ContentProperty =
            DependencyProperty.Register("Column1Content", typeof(FrameworkElement), typeof(CardControl), new PropertyMetadata(null));

        public FrameworkElement Column2Content
        {
            get { return (FrameworkElement)GetValue(Column2ContentProperty); }
            set { SetValue(Column2ContentProperty, value); }
        }

        public static readonly DependencyProperty Column2ContentProperty =
            DependencyProperty.Register("Column2Content", typeof(FrameworkElement), typeof(CardControl), new PropertyMetadata(null));

    }
}
