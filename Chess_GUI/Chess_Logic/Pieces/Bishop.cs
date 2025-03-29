namespace Chess_Logic
{
    // representation of bishop
    public class Bishop(Colors color) : Piece
    {
        // type of the piece
        public override PieceEnum PieceType => PieceEnum.Bishop;
        // color of the piece
        public override Colors Color { get; } = color;

        // possible move directions
        private static readonly Direction[] directions = new Direction[]
        {
            Direction.UpLeft,
            Direction.UpRight,
            Direction.DownLeft,
            Direction.DownRight
        };

        // copy the piece
        public override Piece Copy()
        {
            Bishop copy = new Bishop(Color);
            copy.HasMoved = HasMoved;

            return copy;
        }

        // get possible moves
        public override IEnumerable<Move> GetMoves(Square startingSquare, Chessboard chessboard)
        {
            return RechableSquaresInDirection(startingSquare, chessboard, directions).Select(endingSquare => new StandardMove(startingSquare, endingSquare));
        }
    }
}