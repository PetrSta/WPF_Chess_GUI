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
        private void StoreMoves(IEnumerable<Move> moves)
        {
            possibleMovesCache.Clear();
            foreach (Move move in moves)
            {
                possibleMovesCache.Add(move.EndingSquare, move);
            }
        }

        // show possible moves highlight
        private void ShowHighlights()
        {
            Color color = Color.FromArgb(135, 125, 255, 75);

            foreach (Square endingSquare in possibleMovesCache.Keys) 
            {
                highlights[endingSquare.Row, endingSquare.Column].Fill = new SolidColorBrush(color);
            }
        }

        // hide possible moves highlight
        private void HideHighlights()
        {
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

        // get clicked square
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

        // handling of player input
        private void HandleInput(Square square, bool leftClick)
        {
            // moving piece
            if (leftClick)
            {
                // if no piece is selected - try to select it
                if (selectedSquare == null)
                {
                    SelectPiece(square);
                }
                // if piece is already selected
                else if (selectedSquare != null)
                {
                    // if player selects another one of his pieces - we want to hide hightlights of currently selected, if he moves his piece -||- or if he selects invalid square -||-
                    Square lastSelectedSquare = selectedSquare;
                    selectedSquare = null;
                    HideHighlights();
                    // if player clicked on another one of his pieces - select that one
                    if (gameState.Chessboard[square] != null && gameState.Chessboard[square].Color == gameState.ColorToMove && square != lastSelectedSquare)
                    {
                        SelectPiece(square);
                    }
                    // othervise check if the square represents possible move and handle it
                    else
                    {
                        if (possibleMovesCache.TryGetValue(square, out Move move))
                        {
                            HandleMove(move);
                        }
                        else
                        {
                            possibleMovesCache.Clear();
                        }
                    }
                }
            } 
            // user highlighting square
            else
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
            // clicked square of grid
            Square clickedSquare = ToSquarePosition(point);

            HandleInput(clickedSquare, true);
        }

        // event handling - mouseRightDown - highlighting
        private void BoardGrid_RightMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // get point in PieceGrid whe MouseButtonEvent happened
            Point point = e.GetPosition(PieceGrid);
            // clicked square of grid
            Square squareToHiglight = ToSquarePosition(point);

            HandleInput(squareToHiglight, false);
        }

        // check if game end menu is on screen right now
        private bool IsGameEndMenuOnScreen()
        {
            return GameEndMenuContainer.Content != null;
        }

        private void RestartGame()
        {
            HideHighlights();
            HideUserHighlights();
            possibleMovesCache.Clear();

            gameState = new GameState(Chess_Logic.Colors.White, Chessboard.Initialize());
            DrawBoard(gameState.Chessboard);
        }

        private void ShowGameEndMenu()
        {
            GameEndMenu gameEndMenu = new GameEndMenu(gameState);
            GameEndMenuContainer.Content = gameEndMenu;

            gameEndMenu.OptionSelected += menuOption =>
            {
                if (menuOption == MenuOptions.Restart)
                {
                    GameEndMenuContainer.Content = null;
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