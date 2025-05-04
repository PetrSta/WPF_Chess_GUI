namespace Chess_Logic
{
    // representation of pawn
    public class Pawn : Piece
    {
        // type of the piece
        public override PieceEnum PieceType => PieceEnum.Pawn;
        // color of the piece
        public override Colors Color { get; }
        // only direction pawn can move, except taking
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

        // check if pawn can push (move forward)
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

        // get possible promotions
        private static IEnumerable<Move> PromotionMoves(Square startingSquare, Square endingSquare)
        {
            yield return new PawnPromotion(startingSquare, endingSquare, PieceEnum.Knight);
            yield return new PawnPromotion(startingSquare, endingSquare, PieceEnum.Bishop);
            yield return new PawnPromotion(startingSquare, endingSquare, PieceEnum.Rook);
            yield return new PawnPromotion(startingSquare, endingSquare, PieceEnum.Queen);
        }

        // possible moves without capture
        private IEnumerable<Move> ForwardMoves(Square startingSquare, Chessboard chessboard)
        {
            // 1 square move
            Square forwardSquare = startingSquare + forward;

            if(CanPush(forwardSquare, chessboard))
            {
                // check if pawn reched promotion square, color check should not be needed since pawns cannot move backwards
                if(forwardSquare.Row == 0 || forwardSquare.Row == 7)
                {
                    foreach (Move promotionMove in PromotionMoves(startingSquare, forwardSquare))
                    { 
                        yield return promotionMove;
                    }
                }
                // otherwise standard pawn move
                else
                {
                    yield return new StandardMove(startingSquare, forwardSquare);
                }

                // if pawn has not moved it can move two squares
                Square firstPawnPush = forwardSquare + forward;

                if(!HasMoved && CanPush(firstPawnPush, chessboard))
                {
                    yield return new FirstPawnMove(startingSquare, firstPawnPush);
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

                // check for en passant capture
                if (endingSquare == chessboard.GetEnPassantSquare(Color.getOpponent()))
                {
                    yield return new EnPassant(startingSquare, endingSquare);
                }
                // check for normal capture
                else if (CanCapture(endingSquare, chessboard))
                {
                    // check if pawn reched promotion square, color check is not needed
                    if (endingSquare.Row == 0 || endingSquare.Row == 7)
                    {
                        foreach (Move promotionMove in PromotionMoves(startingSquare, endingSquare))
                        {
                            yield return promotionMove;
                        }
                    }
                    // otherwise standard pawn capturing move
                    else
                    {
                        yield return new StandardMove(startingSquare, endingSquare);
                    }
                }
            }
        }

        // all possible moves
        public override IEnumerable<Move> GetMoves(Square startingSquare, Chessboard chessboard)
        {
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
