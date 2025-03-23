using System.Drawing;

namespace Chess_Logic
{
    // representation of pawn
    public class Pawn : Piece
    {
        // variables
        public override PieceEnum PieceType => PieceEnum.Pawn;
        public override Colors Color { get; }

        private readonly Direction forward;

        // constructor
        public Pawn(Colors color)
        {
            Color = color;

            if(Color == Colors.White) 
            {
                forward = Direction.Up;
            } 
            else if (Color == Colors.Black) 
            {
                forward = Direction.Down;
            }
        }
        
        // copy the pawn
        public override Piece Copy()
        {
            Pawn copy = new Pawn(Color);
            copy.HasMoved = HasMoved;

            return copy;
        }

        // check if pawn can push
        private static bool CanPush(Square square, Chessboard chessboard) 
        { 
            return Chessboard.IsInBounds(square) && chessboard.IsEmpty(square);
        }

        // check if pawn can capture
        private bool CanCapture(Square square, Chessboard chessboard)
        {
            if (!Chessboard.IsInBounds(square) || chessboard.IsEmpty(square))
            {
                return false;
            }

            return chessboard[square].Color != Color;
        }

        // possible moves without capture
        private IEnumerable<Move> ForwardMoves(Square startingSquare, Chessboard chessboard)
        {
            // 1 square move
            Square forwardSquare = startingSquare + forward;

            if(CanPush(forwardSquare, chessboard))
            {
                yield return new StandardMove(startingSquare, forwardSquare);

                // if pawn hasnt moves it can move two squares
                Square firstPawnPush = forwardSquare + forward;
                if(!HasMoved && CanPush(firstPawnPush, chessboard))
                {
                    // TODO change later
                    yield return new StandardMove(startingSquare, firstPawnPush);
                }
            }
        }

        // possible moves with capture
        private IEnumerable<Move> CapturingMoves(Square startingSquare, Chessboard chessboard)
        {
            foreach(Direction direction in new Direction[] { Direction.Left, Direction.Right})
            {
                // check for left and right capturing options
                Square endingSquare = startingSquare + forward + direction;

                if (CanCapture(endingSquare, chessboard))
                {
                    yield return new StandardMove(startingSquare, endingSquare);
                }
            }
        }

        // all possible moves
        public override IEnumerable<Move> GetMoves(Square startingSquare, Chessboard chessboard)
        {
            // TODO pawn promotion, en passant
            return ForwardMoves(startingSquare, chessboard).Concat(CapturingMoves(startingSquare, chessboard));
        }

        // check for moves that put opponents king in check
        public override bool ChecksOpponentsKing(Square startingSquare, Chessboard chessboard)
        {
            return CapturingMoves(startingSquare, chessboard).Any(move =>
            {
                Piece piece = chessboard[move.EndingSquare];
                return piece != null && piece.PieceType == PieceEnum.King;
            });
        }
    }
}
