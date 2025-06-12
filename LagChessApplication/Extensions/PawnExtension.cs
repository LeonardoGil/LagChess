using LagChessApplication.Domains;
using LagChessApplication.Domains.Enums;
using LagChessApplication.Domains.Pieces;
using System.Drawing;

namespace LagChessApplication.Extensions
{
    public static class PawnExtension
    {
        public static void PromotePawn(this Pawn pawn, Board board, PieceTypeEnum type)
        {
            ArgumentNullException.ThrowIfNull(pawn);

            var pawnIndex = Array.FindIndex(board.Pieces, piece => piece.Equals(pawn));

            if (pawnIndex == -1)
                throw new InvalidOperationException("Pawn not found at the given position.");

            board.Pieces[pawnIndex] = pawn.ConvertTo(type);
        }

        public static bool IsMovingInvalid(this Pawn pawn, Board board, Point to, Pawn? anPassantTarget = null)
        {
            var isSameColor = board.IsOccupied(to) && board.GetPiece(to).Color == pawn.Color;

            var isInvalidAttack = board.IsOccupied(to) && (!pawn.IsAttack(to) || isSameColor);

            var isInvalidMove = !board.IsOccupied(to) && pawn.IsAttack(to) && (anPassantTarget is null || !anPassantTarget.AnPassantMove(to));

            return isInvalidAttack || isInvalidMove;
        }

        private static bool AnPassantMove(this Pawn pawnTargert, Point to)
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
