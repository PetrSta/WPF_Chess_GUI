namespace Chess_Logic
{
    // standrd move type
    internal class StandardMove(Square startingSquare, Square endingSquare) : Move
    {
        // move type
        public override MoveTypes MoveType => MoveTypes.Standard;
        // move starting squeare
        public override Square StartingSquare { get; } = startingSquare;
        // move ending square
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
