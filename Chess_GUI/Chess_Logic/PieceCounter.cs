namespace Chess_Logic
{
    // represents counter for pieces
    public class PieceCounter
    {
        // stores the counter for each piece type for white
        private Dictionary<PieceEnum, int> whitePiecesCount = new();
        // stores the counter for each piece type for black
        private Dictionary<PieceEnum, int> blackPiecesCount = new();
        
        // stores counter for all pieces
        public int totalPiecesCount { get; private set; }

        // constructor
        public PieceCounter()
        {
            foreach (PieceEnum pieceType in Enum.GetValues(typeof(PieceEnum)))
            {
                whitePiecesCount[pieceType] = 0;
                blackPiecesCount[pieceType] = 0;
            }
        }

        // increment counter for piece type of one player
        public void Increment(Colors player, PieceEnum piece)
        {
            if (player == Colors.White)
            {
                whitePiecesCount[piece]++;
                totalPiecesCount++;
            }
            else if (player == Colors.Black)
            {
                blackPiecesCount[piece]++;
                totalPiecesCount++;
            }
        }

        // get counter for piece type of white player
        public int WhitePieceTypeCount(PieceEnum piece)
        {
            return whitePiecesCount[piece];
        }

        // get counter for piece type of black player
        public int BlackPieceTypeCount(PieceEnum piece)
        {
            return blackPiecesCount[piece];
        }
    }
}
