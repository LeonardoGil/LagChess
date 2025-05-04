using System.Drawing;

namespace LagChessApplication.Exceptions
{
    public class PieceNotFoundException(Point position,
                                      string? message = null,
                                      Exception? inner = null) : Exception(message ?? $"No piece found at position {position}.", inner)
    {
        public static PieceNotFoundException Create(Point position) => new(position);
    }
}
