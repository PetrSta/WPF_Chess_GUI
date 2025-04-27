// representing both kingside and queenside castle
namespace Chess_Logic
{
    internal class Castle : Move
    {
        // variables for the king starting and ending square
        public override Square StartingSquare { get; }
        public override Square EndingSquare { get; }
        // variables of the move
        public override MoveTypes MoveType { get; }

        private readonly Direction castlingDirection;
        private readonly Square rookStartingSquare;
        private readonly Square rookEndingSquare;

        // constructor
        public Castle(MoveTypes moveType, Square kingSquare)
        {
            // we can set these variables always in the same way
            MoveType = moveType;
            this.StartingSquare = kingSquare;

            // these variables are set based on the type of castling -> kingside/queenside
            if (moveType == MoveTypes.CastleKingSide) 
            {
                castlingDirection = Direction.Right;
                EndingSquare = new Square(kingSquare.Row, 6);
                rookStartingSquare = new Square(kingSquare.Row, 7);
                rookEndingSquare = new Square(kingSquare.Row, 5);
            } 
            else if(moveType == MoveTypes.CastleQueenSide)
            {
                castlingDirection = Direction.Left;
                EndingSquare = new Square(kingSquare.Row, 2);
                rookStartingSquare = new Square(kingSquare.Row, 0);
                rookEndingSquare = new Square(kingSquare.Row, 3);
            }
        }

        // we can execute castling as two separate moves -> one for king and one for the rook
        public override void Execute(Chessboard chessboard)
        {
            new StandardMove(StartingSquare, EndingSquare).Execute(chessboard);
            new StandardMove(rookStartingSquare, rookEndingSquare).Execute(chessboard);
        }

        // since castling is more complex move we need to create its own method to check if its legal,
        // this is where we control check rules for castling
        public override bool LegalMove(Chessboard chessboard)
        {
            Colors player = chessboard[StartingSquare].Color;

            // if player is under check -> no castling is allowed
            if(chessboard.PlayersKingInCheck(player))
            {
                return false;
            }

            // check if we would castle into / through check
            Chessboard chessboardCopy = chessboard.Copy();
            Square kingsPositionInCopy = StartingSquare;

            for(int  i = 0; i < 2; i++)
            {
                new StandardMove(kingsPositionInCopy, kingsPositionInCopy + castlingDirection).Execute(chessboardCopy);
                kingsPositionInCopy += castlingDirection;

                if(chessboardCopy.PlayersKingInCheck(player))
                {
                    return false;
                }
            }

            // otherwise castling is legal
            return true;
        }
    }
}
