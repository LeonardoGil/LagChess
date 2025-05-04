using LagChessApplication.Domains.Enums;
using LagChessApplication.Interfaces;
using System.Drawing;

namespace LagChessApplication.Exceptions
{
    public class InvalidPieceOwnershipException(IPiece piece,
                                      PieceColorEnum currentTurnColor,
                                      string? message = null,
                                      Exception? inner = null) : Exception(message ?? $"You cannot move the {piece.Color} piece at {piece.Position} on {currentTurnColor}'s turn.", inner)
    {
        public PieceColorEnum CurrentTurnColor { get; init; } = currentTurnColor;
        public IPiece Piece { get; init; } = piece;

        public static InvalidPieceOwnershipException Create(IPiece piece, PieceColorEnum currentTurnColor) => new(piece, currentTurnColor);
    }
}
