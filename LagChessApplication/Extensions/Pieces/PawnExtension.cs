using LagChessApplication.Domains;
using LagChessApplication.Domains.Chess;
using LagChessApplication.Domains.Enums;
using LagChessApplication.Domains.Pieces;
using LagChessApplication.Extensions.Boards;
using LagChessApplication.Interfaces;
using System.Drawing;

namespace LagChessApplication.Extensions.Pieces
{
    internal static class PawnExtension
    {
        internal static void PromotePawn(this Pawn pawn, Board board, PieceTypeEnum type)
        {
            ArgumentNullException.ThrowIfNull(pawn);

            var pawnIndex = Array.FindIndex(board.Pieces, piece => piece.Equals(pawn));

            if (pawnIndex == -1)
                throw new InvalidOperationException("Pawn not found at the given position.");

            board.Pieces[pawnIndex] = pawn.ConvertTo(type);
        }

        internal static bool IsMovingValid(this Pawn pawn, Board board, Point to, ChessMove lastMove)
        {
            if (pawn.IsAttack(to))
            {
                var target = board.GetTryPiece(to);

                if (target is null)
                {
                    return lastMove.IsDoublePawnAdvance() && to.X == lastMove.To.Point.X;
                }
                else
                {
                    return !pawn.IsSameColor(target);
                }
            }
            else
            {
                return !board.IsOccupied(to);
            }
        }

        internal static bool ShouldPromotePawn(this IPiece piece) => piece.ShouldPromotePawn(piece.Position);

        internal static bool ShouldPromotePawn(this IPiece piece, Point position) => piece is Pawn && IsAtPromotionRow(position, piece.Color);

        private static bool IsAtPromotionRow(Point position, PieceColorEnum color) => color == PieceColorEnum.Black && position.Y == 1 ||
                                                                                      color == PieceColorEnum.White && position.Y == 8;
    }
}
