using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// representing en passant move
namespace Chess_Logic
{
    public class EnPassant : Move
    {
        // variables
        public override MoveTypes MoveType => MoveTypes.EnPassant;
        public override Square StartingSquare { get; }
        public override Square EndingSquare { get; }

        private readonly Square capturedPawnSquare;

        // constructor
        public EnPassant(Square startingSquare, Square endingSquare)
        {
            StartingSquare = startingSquare;
            EndingSquare = endingSquare;
            capturedPawnSquare = new Square(startingSquare.Row, endingSquare.Column);
        }

        // override the standard move execution
        public override void Execute(Chessboard chessboard)
        {
            // move our pawn
            new StandardMove(StartingSquare, EndingSquare).Execute(chessboard);
            // capture opponents pawn
            chessboard[capturedPawnSquare] = null;
        }
    }
}
