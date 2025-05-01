using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chess_UI
{
    /// <summary>
    /// Interakční logika pro PauseMenu.xaml
    /// </summary>
    public partial class PauseMenu : UserControl
    {
        public event Action<MenuOptions> OptionSelected;

        public PauseMenu()
        {
            InitializeComponent();
        }

        private void ContinueClick(object sender, RoutedEventArgs e)
        {
            OptionSelected?.Invoke(MenuOptions.Continue);
        }

        private void RestartClick(object sender, RoutedEventArgs e)
        {
            OptionSelected?.Invoke(MenuOptions.Restart);
        }
    }
}
