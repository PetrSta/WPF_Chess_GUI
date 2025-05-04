namespace Chess_Logic
{
    // representation of move
    public abstract class Move
    {
        // variables
        public abstract MoveTypes MoveType { get; }
        public abstract Square StartingSquare { get; }
        public abstract Square EndingSquare { get; }

        // execute the move
        public abstract bool Execute(Chessboard chessboard);

        // check if the move is legal
        public virtual bool LegalMove(Chessboard chessboard)
        {
            // we need to know which player is making the move
            Colors playerColor = chessboard[StartingSquare].Color;
            // make a copy of chessboard for testing
            Chessboard coppiedChessboard = chessboard.Copy();
            // try the move
            Execute(coppiedChessboard);
            // if players king would be in check -> move is illegal
            return !coppiedChessboard.PlayersKingInCheck(playerColor);
        }
    }
}
