using LagChessApplication.Interfaces;
using System.Drawing;

namespace LagChessApplication.Exceptions
{
    public class InvalidCastlingException : InvalidMoveException
    {
        internal InvalidCastlingException(IPiece piece, Point to, string? message = null, Exception? inner = null) : base(piece, to, message ?? $"Invalid castling move for piece {piece.Type} to position {to}.", inner)
        {
        }

        internal static InvalidCastlingException KingHasMoved(IPiece piece, Point to, string? message = null, Exception? inner = null) => new InvalidCastlingException(piece, to, message ?? "Castling is not allowed: the king has already moved.", inner);

        internal static InvalidCastlingException RookHasMoved(IPiece piece, Point to, string? message = null, Exception? inner = null) => new InvalidCastlingException(piece, to, message ?? "Castling is not allowed: the rook has already moved.", inner);

        internal static InvalidCastlingException PathIsNotClear(IPiece piece, Point to, string? message = null, Exception? inner = null) => new InvalidCastlingException(piece, to, message ?? "Castling is not allowed: there are pieces between the king and the rook.", inner);

        internal static InvalidCastlingException KingIsInCheck(IPiece piece, Point to, string? message = null, Exception? inner = null) => new InvalidCastlingException(piece, to, message ?? "Castling is not allowed: the king is currently in check.", inner);

        internal static InvalidCastlingException KingPassesThroughCheck(IPiece piece, Point to, string? message = null, Exception? inner = null) => new InvalidCastlingException(piece, to, message ?? "Castling is not allowed: the king would pass through a square under attack.", inner);
    }
}
