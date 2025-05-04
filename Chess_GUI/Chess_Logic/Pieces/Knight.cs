namespace Chess_Logic
{
    // representation of knight
    public class Knight(Colors color) : Piece
    {
        // type of the piece
        public override PieceEnum PieceType => PieceEnum.Knight;
        // color of the piece
        public override Colors Color { get; } = color;

        // copy the piece
        public override Piece Copy()
        {
            Knight copy = new Knight(Color);
            copy.HasMoved = HasMoved;

            return copy;
        }

        // get potential square to move to -> knight has specific move pattern
        private static IEnumerable<Square> PotentialSquares(Square startingSquare)
        {
            // check for possible moves
            foreach (Direction verticalDirection in new Direction[] { Direction.Up, Direction.Down })
            {
                // for both up and down direction create the knight move pattern
                foreach (Direction horizontalDirection in new Direction[] { Direction.Left, Direction.Right })
                {
                    yield return startingSquare + 2 * verticalDirection + horizontalDirection;
                    yield return startingSquare + verticalDirection + 2 * horizontalDirection;
                }
            }
        }

        // check potential squares if they are valid
        private IEnumerable<Square> PossibleMoves(Square startingSquare, Chessboard chessboard)
        {
            return PotentialSquares(startingSquare).Where(square => Chessboard.IsInBounds(square) && (chessboard.IsEmpty(square) || chessboard[square].Color != Color));
        }

        // get valid moves
        public override IEnumerable<Move> GetMoves(Square startingSquare, Chessboard chessboard)
        {
            return PossibleMoves(startingSquare, chessboard).Select(endingSquare => new StandardMove(startingSquare, endingSquare));
        }
    }
}
