using LagChessApplication.Domains;
using LagChessApplication.Domains.Chess;
using LagChessApplication.Domains.Enums;
using LagChessApplication.Domains.Pieces;
using LagChessApplication.Extensions.Pieces;
using LagChessApplication.Extensions.Rules;
using LagChessApplication.Interfaces;
using System.Drawing;

namespace LagChessApplication.Extensions.Boards
{
    internal static class BoardMoveExtension
    {
        internal static bool CanPlacePiece(this Board board, IPiece piece, Point to)
        {
            var occupiedPiece = board.GetTryPiece(to);

            return occupiedPiece is null || occupiedPiece.Color != piece.Color;
        }

        internal static bool IsOccupied(this Board board, Point point) => board.GetTryPiece(point) is not null;

        internal static bool IsPathClear(this Board board, IPiece piece, Point to)
        {
            var from = piece.Position;
            var moveStyle = (from, to).ConvertToMoveStyleEnum();

            var directionX = Math.Sign(to.X - from.X);
            var directionY = Math.Sign(to.Y - from.Y);

            switch (moveStyle)
            {
                case PieceMoveStyleEnum.Straight:
                case PieceMoveStyleEnum.Diagonal:
                    var current = new Point(from.X + directionX, from.Y + directionY);

                    while (current != to)
                    {
                        if (board.IsOccupied(current))
                            return false;

                        current = new Point(current.X + directionX, current.Y + directionY);
                    }

                    return true;

                case PieceMoveStyleEnum.LShaped:
                    return true;

                default:
                    throw new NotSupportedException("Unknown movement style");
            }
        }

        internal static void SetPiecePosition(this Board board, Pawn pawn, Point to, ChessMove lastMove)
        {
            var captured = board.GetTryPiece(to);

            if (captured is null && pawn.IsAttack(to) && lastMove.IsDoublePawnAdvance() && to.X == lastMove.To.Point.X)
            {
                captured = board.GetPiece(new Point(to.X, lastMove.To.Point.Y));
            }

            board.TryCapturePieceAt(captured);

            pawn.Move(to);

            if (pawn.ShouldPromotePawn() && board.TryGetPromotionValue(out var promotionValue))
            {
                pawn.PromotePawn(board, promotionValue);
            }
        }

        internal static void SetPiecePosition(this Board board, IPiece piece, Point to)
        {
            var captured = board.GetTryPiece(to);

            board.TryCapturePieceAt(captured);

            piece.Move(to);
        }

        internal static bool IsInBoard(Point position) => IsInBoard(position.X, position.Y);

        internal static bool IsInBoard(int x, int y) => x is >= 1 and <= 8 && y is >= 1 and <= 8;
    }
}
