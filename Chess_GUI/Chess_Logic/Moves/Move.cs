namespace Chess_Logic
{
    // representation of move
    public abstract class Move
    {
        // variables
        public abstract MoveTypes MoveTypes { get; }
        public abstract Square StartingSquare { get; }
        public abstract Square EndingSquare { get; }

        // execute the move
        public abstract void Execute(Chessboard chessboard);

        public virtual bool LegalMove(Chessboard chessboard)
        {
            Colors playerColor = chessboard[StartingSquare].Color;
            Chessboard coppiedChessboard = chessboard.Copy();
            Execute(coppiedChessboard);
            return !coppiedChessboard.PlayersKingInCheck(playerColor);
        }
    }
}
