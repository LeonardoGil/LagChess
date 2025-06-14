using LagChessApplication.Domains;
using LagChessApplication.Domains.Enums;
using LagChessApplication.Domains.Pieces;
using LagChessApplication.Extensions.Pieces;
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

        internal static bool IsValidMove(this Board board, IPiece piece, Point to)
        {
            return piece.IsValidMove(to) && board.IsPathClear(piece, to) && board.CanPlacePiece(piece, to) && !(piece is Pawn pawn && pawn.IsMovingInvalid(board, to, board._anPassantTarget));
        }

        internal static void SetPiecePosition(this Board board, IPiece piece, Point to)
        {
            var occupiedPiece = board.GetTryPiece(to);

            if (occupiedPiece is not null)
            {
                occupiedPiece.Kill();
                board._capturedPiece = true;
            }

            var pawn = piece as Pawn;

            board._anPassantTarget = pawn is not null && pawn.IsDoubleStepMove(to) ? pawn : default;

            piece.Move(to);

            if (pawn is not null && piece.ShouldPromotePawn() && board._pawnPromotion.HasValue)
            {
                pawn.PromotePawn(board, board._pawnPromotion.Value);
            }
        }

        internal static bool IsInBoard(Point position) => IsInBoard(position.X, position.Y);
        
        internal static bool IsInBoard(int x, int y) => x is >= 1 and <= 8 && y is >= 1 and <= 8;
    }
}
