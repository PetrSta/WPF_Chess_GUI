using Chess_Logic;
using System.Windows;
using System.Windows.Controls;

namespace Chess_UI
{
    /// <summary>
    /// Interaction logic for GameEndMenu.xaml
    /// </summary>
    public partial class GameEndMenu : UserControl
    {
        public event Action<MenuOptions> OptionSelected;

        // constructor for the game end menu window
        public GameEndMenu(GameState gameState)
        {
            InitializeComponent();

            GameResult gameResult = gameState.GameResult;
            WinnerText.Text = GetWinnerText(gameResult.WinningColor);
            ReasonText.Text = GetReasonText(gameResult.EndState, gameState.PlayerToMove);
        }

        // get text to show based on the winners color
        private static string GetWinnerText(Chess_Logic.Colors winner)
        {
            return winner switch
            {
                Chess_Logic.Colors.White => "White wins",
                Chess_Logic.Colors.Black => "Black wins",
                _ => "Draw"
            };
        }

        // get text to show based on the winners color
        private static string PlayerString(Chess_Logic.Colors player)
        {
            return player switch
            {
                Chess_Logic.Colors.White => "White",
                Chess_Logic.Colors.Black => "Black",
                _ => ""
            };
        }

        // get the reason why the game ended
        private static string GetReasonText(GameEndState gameEndState, Chess_Logic.Colors currentPlayer)
        {
            return gameEndState switch
            {
                GameEndState.Stalemate => "Stalemate " + PlayerString(currentPlayer) + " cannot move",
                GameEndState.Checkmate => "Checkmate " + PlayerString(currentPlayer) + " cannot move",
                GameEndState.FiftyMoveRule => "Fifty moves",
                GameEndState.InsufficentMaterial => "Insufficent material",
                GameEndState.ThreefoldRepetition => "Threefold repetition",
                _ => ""
            };
        }

        // button click handler
        private void RestartClick(object sender, RoutedEventArgs e)
        {
            OptionSelected?.Invoke(MenuOptions.Restart);
        }

        // button click handler
        private void ExitClick(object sender, RoutedEventArgs e)
        {
            OptionSelected?.Invoke(MenuOptions.Exit);
        }
    }
}
