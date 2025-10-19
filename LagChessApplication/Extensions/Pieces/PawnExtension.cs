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
    }
}
