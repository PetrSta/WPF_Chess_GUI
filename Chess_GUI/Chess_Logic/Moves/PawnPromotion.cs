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
        public override MoveTypes MoveTypes => MoveTypes.Promotion;
        public override Square StartingSquare { get; }
        public override Square EndingSquare { get; }

        private readonly PieceEnum newPieceType;

        // constructor
        public PawnPromotion(Square startingSquare, Square endingSquare, PieceEnum newPieceType)
        {
            StartingSquare = startingSquare;
            EndingSquare = endingSquare;
            this.newPieceType = newPieceType;
        }

        // 
        private Piece CreateNewPieceType(Colors playerColor)
        {
            return newPieceType switch
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
            chessboard[StartingSquare] = promotionPiece;
        }
    }
}
