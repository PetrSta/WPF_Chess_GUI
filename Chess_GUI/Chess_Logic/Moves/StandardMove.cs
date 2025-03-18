namespace Chess_Logic
{
    // standrd move type
    internal class StandardMove(Square startingSquare, Square endingSquare) : Move
    {
        // variables
        public override MoveTypes MoveTypes => MoveTypes.Standard;
        public override Square StartingSquare { get; } = startingSquare;
        public override Square EndingSquare { get; } = endingSquare;

        // execute standard move
        public override void Execute(Chessboard chessboard)
        {
            Piece piece = chessboard[StartingSquare];
            chessboard[EndingSquare] = piece;
            chessboard[StartingSquare] = null;
            piece.HasMoved = true;
        }
    }
}
