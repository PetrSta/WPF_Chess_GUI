using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// representing pawn promotion
namespace Chess_Logic
{
    public class PawnPromotion : Move
    {
        // variables
        public override MoveTypes MoveType => MoveTypes.PawnPromotion;
        public override Square StartingSquare { get; }
        public override Square EndingSquare { get; }
        private readonly PieceEnum NewPieceType;

        // constructor
        public PawnPromotion(Square startingSquare, Square endingSquare, PieceEnum newPieceType)
        {
            StartingSquare = startingSquare;
            EndingSquare = endingSquare;
            this.NewPieceType = newPieceType;
        }

        // helper method to get the piece the pawn is promoted to
        private Piece CreateNewPieceType(Colors playerColor)
        {
            return NewPieceType switch
            {
                PieceEnum.Knight => new Knight(playerColor),
                PieceEnum.Bishop => new Bishop(playerColor),
                PieceEnum.Rook => new Rook(playerColor),
                _ => new Queen(playerColor)
            };
        }

        // transform the pawn into the new piece
        public override bool Execute(Chessboard chessboard)
        {
            // get the pawn and remove it from its current square
            Piece pawn = chessboard[StartingSquare];
            chessboard[StartingSquare] = null;

            // create a piece which pawn will promote to
            Piece promotionPiece = CreateNewPieceType(pawn.Color);
            promotionPiece.HasMoved = true;
            // place it on the promotion square
            chessboard[EndingSquare] = promotionPiece;

            // moving a pawn progresses game
            return true;
        }
    }
}
