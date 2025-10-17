using LagChessApplication.Domains.Chess;
using LagChessApplication.Domains.Enums;

namespace LagChessApplication.Extensions
{
    internal static class ChessMoveExtension
    {
        internal static bool IsDoublePawnAdvance(this ChessMove lastMove)
        {
            return lastMove.Piece == PieceTypeEnum.Pawn && Math.Abs(lastMove.From.Point.Y - lastMove.To.Point.Y) == 2;
        }
    }
}
