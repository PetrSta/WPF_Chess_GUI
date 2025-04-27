// represention first pawn move (the only move pawn can move 2 squares)
namespace Chess_Logic
{
    public class FirstPawnMove : Move
    {
        // variables
        public override MoveTypes MoveType => MoveTypes.FirstPawnMove;
        public override Square StartingSquare { get; }
        public override Square EndingSquare { get; }

        private readonly Square enPassantSquare;

        // constructor
        public FirstPawnMove(Square startingSquare, Square endingSquare)
        {
            StartingSquare = startingSquare;
            EndingSquare = endingSquare;
            // we use startingSquare + endingSquare / 2 because it works for both colors
            enPassantSquare = new Square((startingSquare.Row + endingSquare.Row) / 2, startingSquare.Column);
        }

        // override the standard move execution
        public override void Execute(Chessboard chessboard)
        {
            // set the square for potential enPassantMove
            Colors player = chessboard[StartingSquare].Color;
            chessboard.SetEnPassantSquare(player, enPassantSquare);
            // move our pawn
            new StandardMove(StartingSquare, EndingSquare).Execute(chessboard);
        }
    }
}
