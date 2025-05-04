namespace Chess_Logic
{
    // respresent end state of game result
    public class GameResult
    {
        // color of winning pieces
        public Colors WinningColor { get; }
        // reason why the game ended
        public GameEndState EndState { get; }

        // constructor
        public GameResult(Colors winningColor, GameEndState endState)
        {
            WinningColor = winningColor;
            EndState = endState;
        }

        // quick method for the game ending in a win for either piece color
        public static GameResult Win(Colors winningColor)
        {
            return new GameResult(winningColor, GameEndState.Checkmate);
        }

        // quick method for the game ending in a draw -> winning piece == Colors.None
        public static GameResult Draw(GameEndState endState)
        {
            return new GameResult(Colors.None, endState);
        }
    }
}
