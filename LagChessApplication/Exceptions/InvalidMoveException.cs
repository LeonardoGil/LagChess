using LagChessApplication.Domains;
using LagChessApplication.Interfaces;
using System.Drawing;

namespace LagChessApplication.Exceptions
{
    public class InvalidMoveException(IPiece piece, 
                                      Point to, 
                                      string? message = null, 
                                      Exception? inner = null) : Exception(message ?? $"Invalid move for piece {piece.Type} to position {(Square)to}.", inner)
    {
        public IPiece Piece { get; init; } = piece;
        public Point To { get; init; } = to;

        public static InvalidMoveException Create(IPiece piece, Point to) => new (piece, to);
    }
}
