namespace Chess_Logic
{
    // representation of move
    public abstract class Move
    {
        // variables
        public abstract MoveTypes MoveTypes { get; }
        public abstract Square StartingSquare { get; }
        public abstract Square EndingSquare { get; }

        // execute the move
        public abstract void Execute(Chessboard chessboard);
    }
}
