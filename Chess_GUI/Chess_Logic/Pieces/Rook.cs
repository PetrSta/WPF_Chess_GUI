namespace Chess_Logic
{
    // representation of rook
    public class Rook(Colors color) : Piece
    {
        // variables
        public override PieceEnum PieceType => PieceEnum.Rook;
        public override Colors Color { get; } = color;

        // possible rook move directions
        private static readonly Direction[] directions = new Direction[]
        {
            Direction.Up,
            Direction.Down,
            Direction.Left,
            Direction.Right
        };

        // copy the piece
        public override Piece Copy()
        {
            Rook copy = new Rook(Color);
            copy.HasMoved = HasMoved;

            return copy;
        }

        // possible rook moves
        public override IEnumerable<Move> GetMoves(Square startingSquare, Chessboard chessboard)
        {
            return RechableSquaresInDirection(startingSquare, chessboard, directions).Select(endingSquare => new StandardMove(startingSquare, endingSquare));
        }
    }
}
