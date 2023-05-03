using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel.Channels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZBank.Entities.BusinessObjects;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBank.View.UserControls
{
    public sealed partial class AmountInfoCardUserControl : UserControl
    {
        public AmountInfoCardUserControl()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty PrimaryTextHeadingProperty =
             DependencyProperty.Register("PrimaryTextHeading", typeof(string), typeof(AmountInfoCardUserControl), null);

        public string PrimaryTextHeading
        {
            get { return (string)GetValue(PrimaryTextHeadingProperty); }
            set { SetValue(PrimaryTextHeadingProperty, value); }
        }

        public static readonly DependencyProperty PrimaryTextContentProperty =
             DependencyProperty.Register("PrimaryTextContent", typeof(string), typeof(AmountInfoCardUserControl), null);

        public string PrimaryTextContent
        {
            get { return (string)GetValue(PrimaryTextContentProperty); }
            set { SetValue(PrimaryTextContentProperty, value); }
        }


        public static readonly DependencyProperty SecondaryTextHeading1Property =
             DependencyProperty.Register("SecondaryTextHeading1", typeof(string), typeof(AmountInfoCardUserControl), null);

        public string SecondaryTextHeading1
        {
            get { return (string)GetValue(SecondaryTextHeading1Property); }
            set { SetValue(SecondaryTextHeading1Property, value); }
        }


        public static readonly DependencyProperty SecondaryTextHeading2Property =
             DependencyProperty.Register("SecondaryTextHeading2", typeof(string), typeof(AmountInfoCardUserControl), null);

        public string SecondaryTextHeading2
        {
            get { return (string)GetValue(SecondaryTextHeading2Property); }
            set { SetValue(SecondaryTextHeading2Property, value); }
        }

        public static readonly DependencyProperty SecondaryContent1Property =
             DependencyProperty.Register("SecondaryContent1", typeof(string), typeof(AmountInfoCardUserControl), null);

        public string SecondaryContent1
        {
            get { return (string)GetValue(SecondaryContent1Property); }
            set { SetValue(SecondaryContent1Property, value); }
        }


        public static readonly DependencyProperty SecondaryContent2Property =
             DependencyProperty.Register("SecondaryContent2", typeof(string), typeof(AmountInfoCardUserControl), null);

        public string SecondaryContent2
        {
            get { return (string)GetValue(SecondaryContent2Property); }
            set { SetValue(SecondaryContent2Property, value); }

        }
    }

    }
