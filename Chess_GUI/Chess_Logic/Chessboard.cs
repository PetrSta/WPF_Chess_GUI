﻿namespace Chess_Logic
{
    public class Chessboard
    {
        // array to represent board
        private readonly Piece[,] pieces = new Piece[8, 8];

        // get piece from array or set piece to array
        public Piece this[int row, int collumn]
        {
            get { return pieces[row, collumn]; }
            set { pieces[row, collumn] = value; }
        }

        // get piece from position or set piece to position
        public Piece this[Square square]
        {
            get { return this[square.Row, square.Column]; }
            set { this[square.Row, square.Column] = value; }
        }

        // setup starting position
        private void StartingPosition()
        {
            // black pieces 8th row
            this[0, 0] = new Rook(Colors.Black);
            this[0, 1] = new Knight(Colors.Black);
            this[0, 2] = new Bishop(Colors.Black);
            this[0, 3] = new Queen(Colors.Black);
            this[0, 4] = new King(Colors.Black);
            this[0, 5] = new Bishop(Colors.Black);
            this[0, 6] = new Knight(Colors.Black);
            this[0, 7] = new Rook(Colors.Black);

            // white pieces 1st row
            this[7, 0] = new Rook(Colors.White);
            this[7, 1] = new Knight(Colors.White);
            this[7, 2] = new Bishop(Colors.White);
            this[7, 3] = new Queen(Colors.White);
            this[7, 4] = new King(Colors.White);
            this[7, 5] = new Bishop(Colors.White);
            this[7, 6] = new Knight(Colors.White);
            this[7, 7] = new Rook(Colors.White);

            // pawns
            for (int column = 0; column < 8; column++)
            {
                this[1, column] = new Pawn(Colors.Black);
                this[6, column] = new Pawn(Colors.White);
            }
        }

        // initialize function for chessboard -> starting position
        public static Chessboard Initialize()
        {
            Chessboard chessboard = new Chessboard();
            chessboard.StartingPosition();
            return chessboard;
        }

        // check if square is in bounds of 8 x 8 chessboard
        public static bool IsInBounds(Square square)
        {
            return square.Row >= 0 && square.Row <= 7 && square.Column >= 0 && square.Column <= 7;
        }

        // check if square on chessboard is empty
        public bool IsEmpty(Square square)
        { 
            return this[square] == null;
        }

        // return all squares with piece on them
        public IEnumerable<Square> SquaresWithPiece()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    Square square = new Square(row, column);

                    if(!IsEmpty(square))
                    {
                        yield return square;
                    }
                }
            }
        }

        // return only squares with piece of given color on them
        public IEnumerable<Square> SquaresWithPiecesOfColor(Colors color)
        {
            return SquaresWithPiece().Where(square => this[square].Color == color);
        }

        // loop through all of openents pieces and check if any can check our king
        public bool PlayersKingInCheck(Colors color)
        {
            return SquaresWithPiecesOfColor(color.getOpponent()).Any(square =>
            {
                Piece piece = this[square];
                return piece != null && piece.ChecksOpponentsKing(square, this);
            });
        }

        // create a copy of the chessboard
        public Chessboard Copy()
        {
            Chessboard chessboardCopy = new Chessboard();

            foreach (Square square in SquaresWithPiece())
            {
                chessboardCopy[square] = this[square].Copy();
            }

            return chessboardCopy;
        }
    }
}
