using LagChessApplication.Domains;
using LagChessApplication.Domains.Enums;
using LagChessApplication.Domains.Pieces;
using System.Drawing;

namespace LagChessApplication.Extensions
{
    public static class PawnMoveExtension
    {
        public static bool IsMovingInvalid(this Board board, Pawn pawn, Point to, Pawn? anPassantTarget = null)
        {
            var isSameColor = board.IsOccupied(to) && board.GetPiece(to).Color == pawn.Color;

            var isInvalidAttack = board.IsOccupied(to) && (!pawn.IsAttack(to) || isSameColor);

            var isInvalidMove = !board.IsOccupied(to) && pawn.IsAttack(to) && (anPassantTarget is null || !AnPassantMove(to, anPassantTarget));

            return isInvalidAttack || isInvalidMove;
        }

        private static bool AnPassantMove(Point to, Pawn pawnTargert)
        {
            var positionY = pawnTargert.Position.Y - to.Y;

            var moveValid = Math.Abs(positionY) == 1 && pawnTargert.Position.X == to.X;

            return pawnTargert.Color switch
            {
                PieceColorEnum.White => moveValid && positionY > 0,
                
                PieceColorEnum.Black => moveValid && positionY < 0,
                
                _ => throw new NotSupportedException(),
            };
        }
    }
}
