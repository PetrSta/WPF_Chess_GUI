namespace Chess_Logic
{
    // representation of game state -> current chessboard and player to move
    public class GameState(Colors color, Chessboard chessboard)
    {
        public Chessboard Chessboard { get; } = chessboard;
        public Colors ColorToMove { get; private set; } = color;

        // check if there are legcal moves and if there are get them
        public IEnumerable<Move> LegalMovesForPiece(Square startingSquare)
        {
            if(Chessboard.IsEmpty(startingSquare) || Chessboard[startingSquare].Color != ColorToMove)
            {
                return Enumerable.Empty<Move>();
            }

            Piece piece = Chessboard[startingSquare];
            return piece.GetMoves(startingSquare, Chessboard);
        }

        // execute selected piece move
        public void MovePiece(Move selectedMove)
        {
            selectedMove.Execute(Chessboard);
            ColorToMove = ColorToMove.getOpponent();
        }
    }  
}
