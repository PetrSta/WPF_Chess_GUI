namespace Chess_Logic
{
    // Colors of Pieces / Players / Result
    public enum Colors
    {
        None, 
        White, 
        Black
    }

    public static class PlayerFunctions
    {
        // returns opposite color of given color (player color) -> opponent
        public static Colors getOpponent(this Colors player)
        {
            return player switch
            {
                Colors.White => Colors.Black,
                Colors.Black => Colors.White,
                _ => Colors.None,
            };
        }
    }
}
