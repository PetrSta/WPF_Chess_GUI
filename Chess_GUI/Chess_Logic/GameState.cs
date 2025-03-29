namespace Chess_Logic
{
    // representation of game state -> current chessboard, player to move and the game result
    public class GameState(Colors color, Chessboard chessboard)
    {
        // the chessboard
        public Chessboard Chessboard { get; } = chessboard;
        // player / color to move a piece
        public Colors ColorToMove { get; private set; } = color;
        // result of the game
        public GameResult GameResult { get; private set; } = null;

        // check if there are legcal moves and if there are get them
        public IEnumerable<Move> LegalMovesForPiece(Square startingSquare)
        {
            if(Chessboard.IsEmpty(startingSquare) || Chessboard[startingSquare].Color != ColorToMove)
            {
                return Enumerable.Empty<Move>();
            }

            Piece piece = Chessboard[startingSquare];

            IEnumerable<Move> potentialMoves = piece.GetMoves(startingSquare, Chessboard);
            return potentialMoves.Where(move => move.LegalMove(Chessboard));
        }

        // execute selected piece move
        public void MovePiece(Move selectedMove)
        {
            selectedMove.Execute(Chessboard);
            ColorToMove = ColorToMove.getOpponent();
            CheckGameOver();
        }

        // find all legal moves for player
        public IEnumerable<Move> AllLegalMoveForPlayer(Colors color)
        {
            IEnumerable<Move> potentialMoves = Chessboard.SquaresWithPiecesOfColor(color).SelectMany(pos =>
            {
                Piece piece = Chessboard[pos];
                return piece.GetMoves(pos, Chessboard);
            });

            return potentialMoves.Where(move => move.LegalMove(Chessboard));
        }

        // check if the game is over
        private void CheckGameOver()
        {
            if (!AllLegalMoveForPlayer(ColorToMove).Any())
            {
                if (Chessboard.PlayersKingInCheck(ColorToMove))
                {
                    GameResult = GameResult.Win(ColorToMove.getOpponent());
                }
                else
                {
                    GameResult = GameResult.Draw(GameEndState.Stalemate);
                }
            }
        }

        // check if game is over
        public bool GameOver()
        {
            return GameResult != null;
        }
    }  
}
