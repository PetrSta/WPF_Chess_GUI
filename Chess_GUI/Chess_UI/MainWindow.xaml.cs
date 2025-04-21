using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using Chess_Logic;
using Rectangle = System.Windows.Shapes.Rectangle;
using Color = System.Windows.Media.Color;

namespace Chess_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // source for assets
        private readonly Image[,] pieceImages = new Image[8, 8];
        // hightlight array
        private readonly Rectangle[,] highlights = new Rectangle[8, 8];
        // move dictionary
        private readonly Dictionary<Square, Move> possibleMovesCache = new Dictionary<Square, Move>();
        // the gamestate
        private GameState gameState;
        // user selected square
        private Square selectedSquare = null;
        // user highlighted squares
        private readonly List<Square> highlightSquareCache = new List<Square>();

        // draw piece assets based on chessboard
        private void DrawBoard(Chessboard chessboard)
        {
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    Piece piece = chessboard[row, column];
                    pieceImages[row, column].Source = PieceImages.GetImage(piece);
                }
            }
        }

        // initialize chessboard
        private void InitializeChessboard()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    Image image = new Image();
                    pieceImages[row, column] = image;
                    PieceGrid.Children.Add(image);

                    Rectangle highlight = new Rectangle();
                    highlights[row, column] = highlight;
                    MovesHighlightGrid.Children.Add(highlight);
                }
            }
        }

        // main function
        public MainWindow()
        {
            InitializeComponent();
            InitializeChessboard();

            gameState = new GameState(Chess_Logic.Colors.White, Chessboard.Initialize());
            DrawBoard(gameState.Chessboard);
        }

        // store possible moves 
        // TODO, in case of pawn promotion we are trying to add moves for earch square -> try to rework this logic 
        private void StoreMoves(IEnumerable<Move> moves)
        {
            possibleMovesCache.Clear();
            foreach (Move move in moves)
            {
                possibleMovesCache.TryAdd(move.EndingSquare, move);
            }
        }

        // show possible moves highlight
        private void ShowHighlights()
        {
            // color for the highlights
            Color color = Color.FromArgb(135, 125, 255, 75);

            // for every possible move square -> use the color as fill
            foreach (Square endingSquare in possibleMovesCache.Keys) 
            {
                highlights[endingSquare.Row, endingSquare.Column].Fill = new SolidColorBrush(color);
            }
        }

        // hide possible moves highlight
        private void HideHighlights()
        {
            // hide the green highlights
            foreach (Square endingSquare in possibleMovesCache.Keys)
            {
                highlights[endingSquare.Row, endingSquare.Column].Fill = Brushes.Transparent;
            }
        }

        // hide player highlighted moves
        private void HideUserHighlights()
        {
            // remove red highlight from squares
            foreach (Square userHightlightedSquare in highlightSquareCache)
            {
                highlights[userHightlightedSquare.Row, userHightlightedSquare.Column].Fill = Brushes.Transparent;
            }
            // clear the cache
            highlightSquareCache.Clear();
        }

        // get clicked square of the piece grid
        private Square ToSquarePosition(Point point)
        {
            double squareSize = PieceGrid.ActualHeight / 8;

            int row = (int)(point.Y / squareSize);
            int column = (int)(point.X / squareSize);

            return new Square(row, column);
        }

        // check if square has piece and if so show possible moves
        private void SelectPiece(Square square)
        {
            IEnumerable<Move> legalMoves = gameState.LegalMovesForPiece(square);

            // check if any moves for the piece are legal
            if (legalMoves.Any())
            {
                selectedSquare = square;
                StoreMoves(legalMoves);
                HideUserHighlights();
                ShowHighlights();
            }
        }


        // move the piece
        private void HandleMove(Move move)
        {
            gameState.MovePiece(move);
            DrawBoard(gameState.Chessboard);

            if(gameState.GameOver())
            {
                ShowGameEndMenu();
            }
        }

        // handle pawn promotion move type
        private void HandlePromotion(Square startingSquare, Square endingSquare)
        {
            // visually move the pawn
            pieceImages[endingSquare.Row, endingSquare.Column].Source = PieceImages.GetImage(gameState.PlayerToMove, PieceEnum.Pawn);
            pieceImages[startingSquare.Row, startingSquare.Column].Source = null;

            // crate the promotion menu
            PromotionMenu promotionMenu = new PromotionMenu(gameState.PlayerToMove);
            MenuContainer.Content = promotionMenu;

            // based on which piece was selected -> execute coresponding promotion move
            promotionMenu.SelectedPiece += Type =>
            {
                MenuContainer.Content = null;
                Move promotionMove = new PawnPromotion(startingSquare, endingSquare, Type);
                HandleMove(promotionMove);
            };
        }

        // function to handle user highlighting a square
        private void HandleHighlithgtInput(Square square)
        {
            // if piece is selected desele ct it and hide highlights of possible moves
            if (selectedSquare != null)
            {
                selectedSquare = null;
                HideHighlights();
                possibleMovesCache.Clear();
            }

            // if the square is already highlighted remove the highlight
            if (highlightSquareCache.Contains(square))
            {
                highlights[square.Row, square.Column].Fill = Brushes.Transparent;
                highlightSquareCache.Remove(square);
            }
            // else highlight the square
            else
            {
                highlightSquareCache.Add(square);
                Color color = Color.FromArgb(135, 255, 55, 55);
                highlights[square.Row, square.Column].Fill = new SolidColorBrush(color);
            }
        }

        // handling of player move input
        private void HandleMoveInput(Square square)
        {
            // we want to hide hightlights of currently selected piece, eihter if player moves his piece -||- or if player selects invalid square -||-
            HideHighlights();
            // we are either selecting new piece or executing a move -> selected square should be null to avoid problems
            selectedSquare = null;
            // if player clicked on another one of his pieces - select that one
            if (gameState.Chessboard[square] != null && gameState.Chessboard[square].Color == gameState.PlayerToMove)
            {
                SelectPiece(square);
            }
            // othervise check if the square represents possible move and handle it
            else
            {
                // try to get move for the square
                if (possibleMovesCache.TryGetValue(square, out Move move))
                {
                    // if the move is a pawn promotion move
                    if (move.MoveTypes == MoveTypes.Promotion)
                    {
                        HandlePromotion(move.StartingSquare, move.EndingSquare);
                    }
                    // if it is normal move
                    else
                    {
                        HandleMove(move);
                    }
                }
                // othervise clear the highlights -> clicked square which has no move for the selected piece
                else
                {
                    possibleMovesCache.Clear();
                }
            }
        }

        // check if the point is in piece grid -> helper function for mouse down handling
        private bool PieceGridOutOfBounds(Point point)
        {
            // ignore edges of the board -> point coords can be negative or go out of bounds for piece grid,
            // they are related to top left corner of the grid -> this can result in "out of bounds" scenario
            double pieceGridSize = PieceGrid.ActualHeight;
            // check for left hand side and top side of the chessboard
            if (point.X < 0 || point.Y < 0)
            {
                return true;
            }
            // check for right hand size and bottom of the chessboard
            if (point.X > pieceGridSize || point.Y > pieceGridSize)
            {
                return true;
            }

            return false;
        }

        // event handling - mouseLeftDown   
        private void BoardGrid_LeftMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (IsGameEndMenuOnScreen())
            {
                return;
            }

            // get point in PieceGrid whe MouseButtonEvent happened
            Point point = e.GetPosition(PieceGrid);

            // check for out of bounds
            if (PieceGridOutOfBounds(point))
            {
                return;
            }

            // clicked square of grid
            Square clickedSquare = ToSquarePosition(point);

            // if no piece is selected - try to select it
            if (selectedSquare == null)
            {
                SelectPiece(clickedSquare);
            }
            // othervise try moving selected piece
            else
            {
                HandleMoveInput(clickedSquare);
            }
        }

        // event handling - mouseRightDown - highlighting
        private void BoardGrid_RightMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (IsGameEndMenuOnScreen())
            {
                return;
            }

            // get point in PieceGrid whe MouseButtonEvent happened
            Point point = e.GetPosition(PieceGrid);

            // check for out of bounds
            if (PieceGridOutOfBounds(point))
            {
                return;
            }

            // clicked square of grid
            Square squareToHiglight = ToSquarePosition(point);

            HandleHighlithgtInput(squareToHiglight);
        }

        // check if game end menu is on screen right now
        private bool IsGameEndMenuOnScreen()
        {
            return MenuContainer.Content != null;
        }

        // restart the state of the game
        private void RestartGame()
        {
            HideHighlights();
            HideUserHighlights();
            possibleMovesCache.Clear();

            gameState = new GameState(Chess_Logic.Colors.White, Chessboard.Initialize());
            DrawBoard(gameState.Chessboard);
        }

        // if the game ended we call this to show game end menu and handle the buttons
        private void ShowGameEndMenu()
        {
            GameEndMenu gameEndMenu = new GameEndMenu(gameState);
            MenuContainer.Content = gameEndMenu;

            // if player wants to restart the game, do so
            gameEndMenu.OptionSelected += menuOption =>
            {
                if (menuOption == MenuOptions.Restart)
                {
                    MenuContainer.Content = null;
                    RestartGame();
                }
                else
                {
                    Application.Current.Shutdown();
                }
            };
        }
    }
}