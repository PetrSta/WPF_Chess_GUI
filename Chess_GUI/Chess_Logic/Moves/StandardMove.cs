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
        public override bool Execute(Chessboard chessboard)
        {
            // get the piece we are moving
            Piece piece = chessboard[StartingSquare];
            // check if the move captures
            bool captures = !chessboard.IsEmpty(EndingSquare);
            // move the piece to the new square
            chessboard[EndingSquare] = piece;
            chessboard[StartingSquare] = null;
            piece.HasMoved = true;

            // if the move captures or moves a pawn it progresses the game
            return captures || piece.PieceType == PieceEnum.Pawn;
        }
    }
}
