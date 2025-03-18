namespace Chess_Logic
{
    // representation of king
    public class King(Colors color) : Piece
    {
        // variables
        public override PieceEnum PieceType => PieceEnum.King;
        public override Colors Color { get; } = color;

        private static Direction[] directions = new Direction[]
        {
            Direction.Up,
            Direction.Down,
            Direction.Left,
            Direction.Right,
            Direction.UpLeft,
            Direction.UpRight,
            Direction.DownLeft,
            Direction.DownRight
        };

        // copy the piece
        public override Piece Copy()
        {
            King copy = new King(Color);
            copy.HasMoved = HasMoved;

            return copy;
        }

        // get list of possible squares
        private IEnumerable<Square> PossibleSquares(Square startingSquare, Chessboard chessboard)
        {
            foreach (Direction direction in directions)
            {
                Square endingSquare = startingSquare + direction;

                if (!Chessboard.IsInBounds(endingSquare))
                {
                    continue;
                }
                // TODO needs to check if the piece is not protected
                if (chessboard.IsEmpty(endingSquare) || chessboard[endingSquare].Color != Color)
                {
                    yield return endingSquare;
                }
            }
        }

        // get valid moves
        public override IEnumerable<Move> GetMoves(Square startingSquare, Chessboard chessboard)
        {
            foreach (Square endingSquare in PossibleSquares(startingSquare, chessboard)) 
            { 
                yield return new StandardMove(startingSquare, endingSquare);
            }
        }
    }
}
