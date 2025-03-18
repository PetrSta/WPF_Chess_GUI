namespace Chess_Logic
{
    public class Direction(int rowDelta, int columnDelta)
    {
        // row or column movement directions
        public readonly static Direction Up = new Direction(-1, 0);
        public readonly static Direction Down = new Direction(+1, 0);
        public readonly static Direction Left = new Direction(0, -1);
        public readonly static Direction Right = new Direction(0, +1);
        // diagonal movement directions
        public readonly static Direction UpLeft = Up + Left;
        public readonly static Direction UpRight = Up + Right;
        public readonly static Direction DownLeft = Down + Left;
        public readonly static Direction DownRight = Down + Right;

        // vector representing movement
        public int RowDelta {  get; } = rowDelta;
        public int ColumnDelta { get; } = columnDelta;

        // overridden operators
        public static Direction operator +(Direction direction1, Direction direction2)
        {
            return new Direction(direction1.RowDelta + direction2.RowDelta, direction1.ColumnDelta + direction2.ColumnDelta);
        }

        public static Direction operator *(int scalar, Direction direction)
        {
            return new Direction(scalar * direction.RowDelta, scalar * direction.ColumnDelta);
        }
    }
}
