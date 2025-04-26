using LagChessApplication.Domains;
using LagChessApplication.Domains.Enums;
using LagChessApplication.Domains.Pieces;
using LagChessApplication.Interfaces;
using System.Drawing;

namespace LagChessApplication.Extensions
{
    public static class BoardExtension
    {
        public static Board Create(string white = "white", string black = "black") => new(PlayerExtension.CreateWhite(white), PlayerExtension.CreateBlack(black));

        public static bool IsInBoard(Point position) => IsInBoard(position.X, position.Y);
        public static bool IsInBoard(int x, int y) => x is >= 1 and <= 8 && y is >= 1 and <= 8;

        public static bool ShouldPromotePawn(IPiece piece) => piece is Pawn && IsAtPromotionRow(piece.Position, piece.Color);

        private static bool IsAtPromotionRow(Point position, PieceColorEnum color) => color == PieceColorEnum.Black && position.Y == 1 ||
                                                                                      color == PieceColorEnum.White && position.Y == 8;
    }
}
