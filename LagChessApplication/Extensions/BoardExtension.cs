using LagChessApplication.Domains;
using System.Drawing;

namespace LagChessApplication.Extensions
{
    public static class BoardExtension
    {
        public static Board Create(string white = "white", string black = "black") => new(PlayerExtension.CreateWhite(white), PlayerExtension.CreateBlack(black));

        public static bool IsInBoard(Point position) => IsInBoard(position.X, position.Y);
        public static bool IsInBoard(int x, int y) => x is >= 1 and <= 8 && y is >= 1 and <= 8;
    }
}
