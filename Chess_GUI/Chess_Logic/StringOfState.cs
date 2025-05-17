using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// building string for state on chessboard
namespace Chess_Logic
{
    public class StringOfState
    {
        private readonly StringBuilder stringBuilder = new StringBuilder();

        // function which assigns charactar based on piece type
        private static char CharOfPiece(Piece piece)
        {
            // switch which decides piece char
            char pieceChar = piece.PieceType switch 
            { 
                PieceEnum.Pawn => 'p',
                PieceEnum.Knight => 'n',
                PieceEnum.Bishop => 'b',
                PieceEnum.Rook => 'r',
                PieceEnum.Queen => 'q',
                PieceEnum.King => 'k',
                _ => ' '
            };

            // if piece is white we use uppercase
            if (piece.Color == Colors.White)
            {
                return char.ToUpper(pieceChar);
            }
            // for black pieces lowercase
            return pieceChar;
        }

        // add placement of pieces in a given row
        private void AddDataForRow(Chessboard chessboard, int row)
        {
            // counter for empty spaces
            int emptySquares = 0;

            // loop over columns in a given row
            for (int column = 0; column < 8; column++)
            {
                // check for empty spaces
                if (chessboard[row, column] == null)
                {
                    emptySquares++;
                    continue;
                }

                // if we found a pieces we need to write the number of empty spaces
                if(emptySquares > 0)
                {
                    stringBuilder.Append(emptySquares);
                    emptySquares = 0;
                }

                // write the char for given piece
                stringBuilder.Append(CharOfPiece(chessboard[row, column]));
            }

            // if row ends with empty spaces add them
            if(emptySquares > 0)
            {
                stringBuilder.Append(emptySquares);
            }
        }

        // loop over the whole chessboard
        private void AddPlacementOfPieces(Chessboard chessboard)
        {
            // loop over all rows on chessboard and add them
            for(int row = 0; row < 8; row++)
            {
                if(row != 0)
                {
                    stringBuilder.Append('/');
                }
                AddDataForRow(chessboard, row);
            }
        }

        // add string representing next players move
        private void AddCurrentPlayer(Colors currentPlayer)
        {
            // white
            if(currentPlayer == Colors.White)
            {
                stringBuilder.Append('w');
            } 
            // black
            else
            {
                stringBuilder.Append('b');
            }
        }

        // add string representing castling rights
        private void AddCastlingRights(Chessboard chessboard)
        {
            // checkk all castling rights
            bool whiteCanCastleKingSide = chessboard.KingsideCastlingRights(Colors.White);
            bool whiteCanCastleQueenSide = chessboard.QueensideCastlingRights(Colors.White);
            bool blackCanCastleKingSide = chessboard.KingsideCastlingRights(Colors.Black);
            bool blackCanCastleQueenSide = chessboard.QueensideCastlingRights(Colors.Black);

            // based on previously create bool values append the string
            if (!whiteCanCastleKingSide || whiteCanCastleQueenSide || blackCanCastleKingSide || blackCanCastleQueenSide)
            {
                stringBuilder.Append('-');
                return;
            } 
            else if(whiteCanCastleKingSide)
            {
                stringBuilder.Append('K');
            }
            else if (whiteCanCastleQueenSide)
            {
                stringBuilder.Append('Q');
            }
            else if (blackCanCastleKingSide)
            {
                stringBuilder.Append('k');
            }
            else if (blackCanCastleQueenSide)
            {
                stringBuilder.Append('q');
            }
            return;
        }

        // add en passant square if one is available
        private void AddEnPassantRight(Chessboard chessboard, Colors currentPlayer)
        {
            // if no square exists skip
            if(!chessboard.EnPassantPossible(currentPlayer))
            {
                stringBuilder.Append('-');
                return;
            }

            // if it exists convert number representation to normal chess notation
            Square enPassantSquare = chessboard.GetEnPassantSquare(currentPlayer.getOpponent());
            char file = (char)('a' + enPassantSquare.Column);
            int rank = 8 - enPassantSquare.Row;
            // add the square
            stringBuilder.Append(file);
            stringBuilder.Append(rank);
        }

        // get access to the final string
        public override string ToString()
        {
            return stringBuilder.ToString();
        }

        public StringOfState(Colors currentPlayer, Chessboard chessboard) 
        {
            // add piece placement data
            AddPlacementOfPieces(chessboard);
            // add current player
            stringBuilder.Append(' ');
            AddCurrentPlayer(currentPlayer);
            // add castling rights
            stringBuilder.Append(' ');
            AddCastlingRights(chessboard);
            // add en passant data
            stringBuilder.Append(' ');
            AddEnPassantRight(chessboard, currentPlayer);
        }

    }
}
