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



        public int NarrowScreenBreakPoint
        {
            get { return (int)GetValue(NarrowScreenBreakPointProperty); }
            set { SetValue(NarrowScreenBreakPointProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NarrowScreenBreakPoint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NarrowScreenBreakPointProperty =
            DependencyProperty.Register("NarrowScreenBreakPoint", typeof(int), typeof(OneByTwoGrid), new PropertyMetadata(800));



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


        public string Width2Ratio
        {
            get { return (string)GetValue(Width2RatioProperty); }
            set { SetValue(Width2RatioProperty, value); }
        }

        public static readonly DependencyProperty Width2RatioProperty =
            DependencyProperty.Register("Width2Ratio", typeof(string), typeof(OneByTwoGrid), new PropertyMetadata("*"));


        public string Width1Ratio
        {
            get { return (string)GetValue(Width1RatioProperty); }
            set {
                if (value == "1*") NarrowScreenBreakPoint = 800;
                else if (value == "2*") NarrowScreenBreakPoint = 1000;
                SetValue(Width1RatioProperty, value); }
        }

        public static readonly DependencyProperty Width1RatioProperty =
            DependencyProperty.Register("Width1Ratio", typeof(string), typeof(OneByTwoGrid), new PropertyMetadata("*"));
    }
}
