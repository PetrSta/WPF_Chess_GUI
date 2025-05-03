namespace Chess_Logic
{
    // representation of king
    public class King(Colors color) : Piece
    {
        // type of the piece
        public override PieceEnum PieceType => PieceEnum.King;
        // color of the piece
        public override Colors Color { get; } = color;

        // possible move directions     
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

        // helper method that helps us to make sure the rook did not move
        private static bool DidRookMove(Square square, Chessboard chessboard)
        {
            // if square is empty the rook had to move / be captured
            if (chessboard[square] == null)
            {
                return false;
            }

            // we do not need to check the color of the rook, if the rook would be different color it had to move there
            Piece rook = chessboard[square];
            return rook.PieceType == PieceEnum.Rook && !rook.HasMoved;
        }

        // helper method that helps us to make sure all squares between king and rook are empty
        private static bool AreSquaresEmpty(IEnumerable<Square> squares, Chessboard chessboard)
        {
            return squares.All(square => chessboard.IsEmpty(square));
        }

        // check if king can castle king side, ignoring rules for check for now
        private bool CanCastleKingSide(Square startingSquare, Chessboard chessboard)
        {
            // if the king moved, it cannot castle
            if (HasMoved)
            {
                return false;
            }

            // get the square where the rook should be
            Square rookSquare = new Square(startingSquare.Row, 7);
            // all the square between king and rook on the KingSide
            Square[] squaresBetween = new Square[] { new Square(startingSquare.Row, 5), new Square(startingSquare.Row, 6) };

            return DidRookMove(rookSquare, chessboard) && AreSquaresEmpty(squaresBetween, chessboard);
        }

        // check if king can castle queen side, ignoring rules for check for now
        private bool CanCastleQueenSide(Square startingSquare, Chessboard chessboard)
        {
            // if the king moved, it cannot castle
            if (HasMoved)
            {
                return false;
            }

            // get the square where the rook should be
            Square rookSquare = new Square(startingSquare.Row, 0);
            // all the square between king and rook on the QueenSide
            Square[] squaresBetween = new Square[] { new Square(startingSquare.Row, 1), new Square(startingSquare.Row, 2), new Square(startingSquare.Row, 3) };

            return DidRookMove(rookSquare, chessboard) && AreSquaresEmpty(squaresBetween, chessboard);
        }

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

            if (CanCastleKingSide(startingSquare, chessboard))
            {
                yield return new Castle(MoveTypes.CastleKingSide, startingSquare);
            } 

            if (CanCastleQueenSide(startingSquare, chessboard))
            {
                yield return new Castle(MoveTypes.CastleQueenSide, startingSquare);
            }
        }

        // check for moves that put opponents king in check
        public override bool ChecksOpponentsKing(Square startingSquare, Chessboard chessboard)
        {
            return PossibleSquares(startingSquare, chessboard).Any(endingSquare =>
            {
                Piece piece = chessboard[endingSquare];
                return piece != null && piece.PieceType == PieceEnum.King;
            });
        }
    }
}
