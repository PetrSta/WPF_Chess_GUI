using Chess_Logic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Chess_UI
{
    public static class PieceImages
    {
        // relative paths to white piece assets
        private static readonly Dictionary<PieceEnum, ImageSource> whiteSources = new()
        {
            { PieceEnum.Pawn, LoadImage("Assets/White/Pawn.png") },
            { PieceEnum.Bishop, LoadImage("Assets/White/Bishop.png") },
            { PieceEnum.Knight, LoadImage("Assets/White/Knight.png") },
            { PieceEnum.Rook, LoadImage("Assets/White/Rook.png") },
            { PieceEnum.Queen, LoadImage("Assets/White/Queen.png") },
            { PieceEnum.King, LoadImage("Assets/White/King.png") }
        };

        // relative paths to black piece assets
        private static readonly Dictionary<PieceEnum, ImageSource> blackSources = new()
        {
            { PieceEnum.Pawn, LoadImage("Assets/Black/Pawn.png") },
            { PieceEnum.Bishop, LoadImage("Assets/Black/Bishop.png") },
            { PieceEnum.Knight, LoadImage("Assets/Black/Knight.png") },
            { PieceEnum.Rook, LoadImage("Assets/Black/Rook.png") },
            { PieceEnum.Queen, LoadImage("Assets/Black/Queen.png") },
            { PieceEnum.King, LoadImage("Assets/Black/King.png") }
        };

        // load asset
        private static ImageSource LoadImage(string filePath)
        {
            return new BitmapImage(new Uri(filePath, UriKind.Relative));
        }

        // return asset path based on given piece and color
        public static ImageSource GetImage(Chess_Logic.Colors color, PieceEnum piece)
        {
            return color switch
            {
                Chess_Logic.Colors.White => whiteSources[piece],
                Chess_Logic.Colors.Black => blackSources[piece],
                _ => null
            };
        }

        // return asset path based on given piece
        public static ImageSource GetImage(Piece piece)
        {
            if (piece == null)
            {
                return null;
            }
            return GetImage(piece.Color, piece.PieceType);
        }
    }
}
