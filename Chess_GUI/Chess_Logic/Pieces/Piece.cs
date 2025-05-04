namespace Chess_Logic
{
    // abstract representation of piece
    public abstract class Piece
    {
        public abstract PieceEnum PieceType { get; }
        public abstract Colors Color { get; }
        // needed for some move types
        public bool HasMoved { get; set; } = false;

        // copy the piece
        public abstract Piece Copy();

        // get legal moves of piece
        public abstract IEnumerable<Move> GetMoves(Square startingSquare, Chessboard chessboard);

        // check in a given direction
        protected IEnumerable<Square> CheckDirection(Square startingSquare, Chessboard chessboard, Direction direction)
        {
            for(Square square = startingSquare + direction; Chessboard.IsInBounds(square); square += direction)
            {
                // if the square is empty we can move there
                if(chessboard.IsEmpty(square))
                {
                    yield return square;
                    continue;
                }

                // if the square is not empty but the piece there belongs to opponent we can capture there
                Piece piece = chessboard[square];
                if(piece.Color != Color)
                {
                    yield return square;
                }
                yield break;
            }
        }

        // check for possible moves in given directions
        protected IEnumerable<Square> RechableSquaresInDirection(Square startingSquare, Chessboard chessboard, Direction[] directions)
        {
            return directions.SelectMany(direction => CheckDirection(startingSquare, chessboard, direction));
        }

        // check for moves that put opponents king in check
        public virtual bool ChecksOpponentsKing(Square startingSquare, Chessboard chessboard)
        {
            // if any move would be able to move to a square with opponents king return true
            return GetMoves(startingSquare, chessboard).Any(move =>
            {
                Piece piece = chessboard[move.EndingSquare];
                return piece != null && piece.PieceType == PieceEnum.King;
            });

        }
    }
}
