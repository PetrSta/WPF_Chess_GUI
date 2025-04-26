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
        public override MoveTypes MoveType => MoveTypes.Promotion;
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
        public override void Execute(Chessboard chessboard)
        {
            Piece pawn = chessboard[StartingSquare];
            chessboard[StartingSquare] = null;

            Piece promotionPiece = CreateNewPieceType(pawn.Color);
            promotionPiece.HasMoved = true;
            chessboard[EndingSquare] = promotionPiece;
        }
    }
}
