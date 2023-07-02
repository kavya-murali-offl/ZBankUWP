using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using ZBank.View;
using Windows.ApplicationModel.VoiceCommands;
using ZBankManagement.Entity.BusinessObjects;
using ZBank.Entity.BusinessObjects;
using System.Drawing;
using ZBank.DataStore;

namespace ZBank.ViewModel
{
    internal class SettingsViewModel
    {
        System.Drawing.ColorConverter colorConverter = new System.Drawing.ColorConverter();
        
        private IView View { get; set; }

        public SettingsViewModel(IView view)
        {
            View = view;
            InitializeColors();
        }

        private void InitializeColors()
        {
            AccentColors = new List<SolidColorBrush>(){
                ColorsHelper.GetSolidColorBrush("FF6A3F73"),
                ColorsHelper.GetSolidColorBrush("FF0078D4"),
                ColorsHelper.GetSolidColorBrush("FF3666CD"),
                ColorsHelper.GetSolidColorBrush("FFBA4273"),
                ColorsHelper.GetSolidColorBrush("FF6E6FD8"),
                ColorsHelper.GetSolidColorBrush("FF488FA5"),
                ColorsHelper.GetSolidColorBrush("FF519254"),
                ColorsHelper.GetSolidColorBrush("FF488FA5"),
                ColorsHelper.GetSolidColorBrush("FFCA5010"),
                ColorsHelper.GetSolidColorBrush("FFA4262C"),
                ColorsHelper.GetSolidColorBrush("FF7C93A1"),
            };
        }

        public IEnumerable<SolidColorBrush> AccentColors { get; set; }
    }

}


