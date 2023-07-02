using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace ZBank.Entity.BusinessObjects
{
    public class AccentBrush
    {
        public SolidColorBrush AccentColor { get; set; }
        public SolidColorBrush AccentColorDark1 { get; set; }
        public SolidColorBrush AccentColorDark2 { get; set; }
        public SolidColorBrush AccentColorDark3 { get; set; }

        public SolidColorBrush AccentColorLight1 { get; set; }
        public SolidColorBrush AccentColorLight2 { get; set; }
        public SolidColorBrush AccentColorLight3 { get; set; }
    }
}
