using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using ZBank.Entity.BusinessObjects;

namespace ZBank.DataStore
{
    internal class ColorsHelper
    {
        static ColorsHelper() {
            AccentBrushes = new List<AccentBrush>()
            {
                new AccentBrush()
                {
                    AccentColor = GetSolidColorBrush("#FFBA4273"),
                    AccentColorDark1 = GetSolidColorBrush("#FF312027"),
                    AccentColorDark2 = GetSolidColorBrush("#FF41242F"),
                    AccentColorDark3 = GetSolidColorBrush("#FF5A2A3E"),
                    AccentColorLight1 = GetSolidColorBrush("#FF503641"),
                    AccentColorLight2 = GetSolidColorBrush("#FFA43A64"),
                    AccentColorLight3 = GetSolidColorBrush("#FF9C3760"),
                },

                new AccentBrush()
                {
                    AccentColor = GetSolidColorBrush("#FF6A3F73"),
                    AccentColorDark1 = GetSolidColorBrush("#FF251F27"),
                    AccentColorDark2 = GetSolidColorBrush("#FF2D2330"),
                    AccentColorDark3 = GetSolidColorBrush("#FF5A2A3E"),
                    AccentColorLight1 = GetSolidColorBrush("#FF3E3541"),
                    AccentColorLight2 = GetSolidColorBrush("#FF5D3765"),
                    AccentColorLight3 = GetSolidColorBrush("#FF593561"),
                }
            };
        }

        public static IEnumerable<AccentBrush> AccentBrushes { get; set; }  
        public static SolidColorBrush GetSolidColorBrush(string hex)
        {
            hex = hex.Replace("#", string.Empty);
            byte a = (byte)(Convert.ToUInt32(hex.Substring(0, 2), 16));
            byte r = (byte)(Convert.ToUInt32(hex.Substring(2, 2), 16));
            byte g = (byte)(Convert.ToUInt32(hex.Substring(4, 2), 16));
            byte b = (byte)(Convert.ToUInt32(hex.Substring(6, 2), 16));
            SolidColorBrush myBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(a, r, g, b));
            return myBrush;
        }
    };
}
