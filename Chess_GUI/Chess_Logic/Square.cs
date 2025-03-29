namespace Chess_Logic
{
    public class Square(int row, int column)
    {
        // [0,0] -> top left corner
        public int Row { get; } = row;
        public int Column { get; } = column;

        // get color of the square
        public Colors SquareColor()
        {
            int Sum = Row + Column;

            if(Sum % 2 == 0)
            {
                return Colors.White;
            } 
            else
            {
                return Colors.Black;
            }
        }

        // generated code -> hash¨code, overridden operators
        public override bool Equals(object obj)
        {
            return obj is Square square &&
                   Row == square.Row &&
                   Column == square.Column;
        }

        // overridon hash code
        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Column); 
        }

        // overriden operators
        public static bool operator ==(Square left, Square right)
        {
            return EqualityComparer<Square>.Default.Equals(left, right);
        }

        public static bool operator !=(Square left, Square right)
        {
            return !(left == right);
        }

        public static Square operator +(Square square, Direction direction)
        {
            return new Square(square.Row + direction.RowDelta, square.Column + direction.ColumnDelta);
        }
    }
}
