namespace Chess_Logic
{
    // representation of queen
    public class Queen(Colors color) : Piece
    {
        // type of the piece
        public override PieceEnum PieceType => PieceEnum.Queen;
        // color of the piece
        public override Colors Color { get; } = color;

        // possible queen move directions
        private static readonly Direction[] directions = new Direction[]
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
            Queen copy = new Queen(Color);
            copy.HasMoved = HasMoved;

            return copy;
        }

        // possible queen moves
        public override IEnumerable<Move> GetMoves(Square startingSquare, Chessboard chessboard)
        {
            return RechableSquaresInDirection(startingSquare, chessboard, directions).Select(endingSquare => new StandardMove(startingSquare, endingSquare));
        }
    }
}
