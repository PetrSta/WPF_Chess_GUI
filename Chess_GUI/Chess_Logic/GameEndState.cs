namespace Chess_Logic
{
    // enum for reasons why the game can end
    public enum GameEndState
    {
        Checkmate,
        Stalemate,
        FiftyMoveRule,
        InsufficentMaterial,
        ThreefoldRepetition
    }
}
