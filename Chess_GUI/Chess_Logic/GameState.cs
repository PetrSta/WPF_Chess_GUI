﻿namespace Chess_Logic
{
    // representation of game state -> current chessboard, player to move and the game result
    public class GameState
    {
        // the chessboard
        public Chessboard Chessboard { get; }
        // player / color to move a piece
        public Colors PlayerToMove { get; private set; }
        // result of the game
        public GameResult GameResult { get; private set; } = null;
        // counter for 50 move rule
        private int fiftyMoveRuleCounter = 0;
        // string for current state
        private string stateString;
        // dictionary saving all state strings
        private readonly Dictionary<string, int> stateHistory = new Dictionary<string, int>();

        public GameState(Colors player, Chessboard chessboard)
        {
            PlayerToMove = player;
            Chessboard = chessboard;

            stateString = new StringOfState(player, chessboard).ToString();
            stateHistory[stateString] = 1;
        }

        // check if game is over
        public bool GameOver()
        {
            return GameResult != null;
        }

        // helper method for FiftyMoveRule
        private bool FiftyMoveRule()
        {
            // full moves -> both player moving a piece -> division is not needed
            // but it is better representation
            int fullMoves = fiftyMoveRuleCounter / 2;
            return fullMoves == 50;
        }

        // check for threefold repetition
        private bool ThreefoldRepetition()
        {
            return stateHistory[stateString] == 3;
        }

        // check if the game is over
        private void CheckGameOver()
        {
            // if there are no possible moves
            if (!AllLegalMoveForPlayer(PlayerToMove).Any())
            {
                if (Chessboard.PlayersKingInCheck(PlayerToMove))
                {
                    GameResult = GameResult.Win(PlayerToMove.getOpponent());
                }
                else
                {
                    GameResult = GameResult.Draw(GameEndState.Stalemate);
                }
            }
            // if there is insufficent material
            else if (Chessboard.InsufficentMaterial())
            {
                GameResult = GameResult.Draw(GameEndState.InsufficentMaterial);
            } 
            // check for fity move rule
            else if (FiftyMoveRule())
            {
                GameResult = GameResult.Draw(GameEndState.FiftyMoveRule);
            }
            // check for threefold repetition
            else if(ThreefoldRepetition())
            {
                GameResult = GameResult.Draw(GameEndState.ThreefoldRepetition);
            }
        }

        // check if there are legcal moves and if there are get them
        public IEnumerable<Move> LegalMovesForPiece(Square startingSquare)
        {
            if(Chessboard.IsEmpty(startingSquare) || Chessboard[startingSquare].Color != PlayerToMove)
            {
                return Enumerable.Empty<Move>();
            }

            Piece piece = Chessboard[startingSquare];

            IEnumerable<Move> potentialMoves = piece.GetMoves(startingSquare, Chessboard);
            return potentialMoves.Where(move => move.LegalMove(Chessboard));
        }

        // find all legal moves for player
        public IEnumerable<Move> AllLegalMoveForPlayer(Colors color)
        {
            IEnumerable<Move> potentialMoves = Chessboard.SquaresWithPiecesOfColor(color).SelectMany(pos =>
            {
                Piece piece = Chessboard[pos];
                return piece.GetMoves(pos, Chessboard);
            });

            return potentialMoves.Where(move => move.LegalMove(Chessboard));
        }

        // update state string and state history
        private void UpdateStateString()
        {
            // get current state string
            stateString = new StringOfState(PlayerToMove, Chessboard).ToString();

            // if its a new string add it
            if (!stateHistory.ContainsKey(stateString))
            {
                stateHistory[stateString] = 1;
            }
            // otherwise increment
            else
            {
                stateHistory[stateString]++;
            }
        }

        // execute selected piece move
        public void MovePiece(Move selectedMove)
        {
            // en passant is only possible on the next move
            Chessboard.SetEnPassantSquare(PlayerToMove, null);
            // execute the given move
            bool progressesGame = selectedMove.Execute(Chessboard);

            // check if we "progress" the game -> moving pawn or capturing piece
            // if not increment fifty move rule counter
            if (!progressesGame) 
            { 
                fiftyMoveRuleCounter++;
            } 
            // otherwise reset it
            else
            {
                fiftyMoveRuleCounter = 0;
                // previous states are now unreachable
                stateHistory.Clear();
            }

            // change which player is next to move
            PlayerToMove = PlayerToMove.getOpponent();
            // update state string
            UpdateStateString();
            // check for game over states
            CheckGameOver();
        }
    }  
}
