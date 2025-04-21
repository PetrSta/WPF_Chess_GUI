using Chess_Logic;
using System.Windows.Controls;
using System.Windows.Input;

namespace Chess_UI
{
    /// <summary>
    /// Interaction logic for PromotionMenu.xaml
    /// </summary>
    public partial class PromotionMenu : UserControl
    {
        public event Action<PieceEnum> SelectedPiece;

        // constructor
        public PromotionMenu(Colors playerColor)
        {
            InitializeComponent();
            // change images based on player color
            Knight.Source = PieceImages.GetImage(playerColor, PieceEnum.Knight);
            Bishop.Source = PieceImages.GetImage(playerColor, PieceEnum.Bishop);
            Rook.Source = PieceImages.GetImage(playerColor, PieceEnum.Rook);
            Queen.Source = PieceImages.GetImage(playerColor, PieceEnum.Queen);
        }

        // event handler for each piece
        private void Knight_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectedPiece?.Invoke(PieceEnum.Knight);
        }

        private void Bishop_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectedPiece?.Invoke(PieceEnum.Bishop);
        }

        private void Rook_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectedPiece?.Invoke(PieceEnum.Rook);
        }

        private void Queen_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectedPiece?.Invoke(PieceEnum.Queen);
        }
    }
}
