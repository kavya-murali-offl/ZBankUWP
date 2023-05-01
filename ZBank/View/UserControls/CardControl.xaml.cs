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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZBank.View.UserControls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CardControl : Page
    {
        public CardControl()
        {
            this.InitializeComponent();
        }


        public FrameworkElement MyContent
        {
            get { return (FrameworkElement)GetValue(MyContentProperty); }
            set { SetValue(MyContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyContentProperty =
            DependencyProperty.Register("MyContent", typeof(FrameworkElement), typeof(CardControl), new PropertyMetadata(null));


        public string MyTitle
        {
            get { return (string)GetValue(MyTitleProperty); }
            set { SetValue(MyTitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyTitleProperty =
            DependencyProperty.Register("MyTitle", typeof(string), typeof(CardControl), new PropertyMetadata(null));


        public FrameworkElement MyControls
        {
            get { return (FrameworkElement)GetValue(MyControlsProperty); }
            set { SetValue(MyControlsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyControlsProperty =
            DependencyProperty.Register("MyControls", typeof(FrameworkElement), typeof(CardControl), new PropertyMetadata(null));
    }
}
