using LagChessApplication.Domains.Enums;
using System.Drawing;

namespace LagChessApplication.Extensions.Pieces
{
    internal static class PieceMoveStyleExtension
    {
        internal static PieceMoveStyleEnum ConvertToMoveStyleEnum(this (Point from, Point to) direction)
        {
            if (IsLinear(direction.from, direction.to))
                return PieceMoveStyleEnum.Straight;

            if (IsDiagonal(direction.from, direction.to))
                return PieceMoveStyleEnum.Diagonal;

            if (IsLShaped(direction.from, direction.to))
                return PieceMoveStyleEnum.LShaped;

            throw new FormatException("Unknown movement style");
        }

        internal static bool IsLinear(Point from, Point to)
        {
            return from.X == to.X && from.Y != to.Y || from.X != to.X && from.Y == to.Y;
        }

        internal static bool IsDiagonal(Point from, Point to)
        {
            return Math.Abs(from.X - to.X) == Math.Abs(from.Y - to.Y);
        }

        internal static bool IsLShaped(Point from, Point to)
        {
            int dx = Math.Abs(from.X - to.X);
            int dy = Math.Abs(from.Y - to.Y);

            return (dx == 2 && dy == 1) || (dx == 1 && dy == 2);
        }
    }
}
