using System.Windows;
using System.Windows.Controls;

namespace Chess_UI
{
    /// <summary>
    /// Interaction logic for PauseMenu.xaml
    /// </summary>
    public partial class PauseMenu : UserControl
    {
        // custom event
        public event Action<MenuOptions> OptionSelected;

        // constructor
        public PauseMenu()
        {
            InitializeComponent();
        }

        // event handlers
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
