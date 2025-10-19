using LagChessApplication.Domains.Enums;
using LagChessApplication.Domains.Pieces;
using LagChessApplication.Domains;
using LagChessApplication.Interfaces;
using System.Drawing;

namespace LagChessApplication.Extensions.Rules
{
    internal static class PawnPromotionExtension
    {
        internal static void PromotePawn(this Pawn pawn, Board board, PieceTypeEnum type)
        {
            ArgumentNullException.ThrowIfNull(pawn);

            var pawnIndex = Array.FindIndex(board.Pieces, piece => piece.Equals(pawn));

            if (pawnIndex == -1)
                throw new InvalidOperationException("Pawn not found at the given position.");

            board.Pieces[pawnIndex] = pawn.ConvertTo(type);
        }

        internal static bool ShouldPromotePawn(this IPiece piece) => piece.ShouldPromotePawn(piece.Position);

        internal static bool ShouldPromotePawn(this IPiece piece, Point position) => piece is Pawn && IsAtPromotionRow(position, piece.Color);

        internal static bool IsAtPromotionRow(Point position, PieceColorEnum color) => color == PieceColorEnum.Black && position.Y == 1 ||
                                                                                      color == PieceColorEnum.White && position.Y == 8;
    }
}
