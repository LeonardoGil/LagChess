using LagChessApplication.Domains;
using LagChessApplication.Domains.Pieces;
using LagChessApplication.Exceptions;
using LagChessApplication.Extensions.Boards;
using LagChessApplication.Interfaces;
using System.Drawing;

namespace LagChessApplication.Extensions.Rules
{
    internal static class CastlingMoveExtension
    {
        private static int[] possibleCastlingPositionsY = [1, 8];
        private static int[] possibleCastlingPositionsX = [3, 7];

        internal static void SetCastlingPositions(this Board board, IPiece piece, Point to)
        {
            var king = piece as King ?? throw new ArgumentException("Piece must be a King.");

            var rookPosition = GetCastlingRookPosition(king, to);

            var rookTargetPosition = GetCastlingRookTargetPosition(king, to);

            var rook = board.GetTryCastlingRook(king, to) ?? throw PieceNotFoundException.Create(rookPosition);

            king.Move(to);

            rook.Move(rookTargetPosition);
        }

        internal static bool IsCastlingMove(this IPiece piece, Board board, Point to)
        {
            return piece is King king
                && possibleCastlingPositionsX.Contains(to.X)
                && possibleCastlingPositionsY.Contains(to.Y)
                && GetTryCastlingRook(board, king, to) is not null;
        }

        internal static void ValidateCastlingMove(this King king, Board board, Point to)
        {
            if (!IsCastlingMove(king, board, to))
                return;

            if (!king.StartPosition)
                throw InvalidCastlingException.KingHasMoved(king, to);
                    
            if (board.MovePutsOwnKingInCheck(king))
                throw InvalidCastlingException.KingIsInCheck(king, to);

            if (!board.IsPathClear(king, to))
                throw InvalidCastlingException.PathIsNotClear(king, to);

            var rook = board.GetTryCastlingRook(king, to);

            if (rook is null || !rook.StartPosition)
                throw InvalidCastlingException.RookHasMoved(king, to);

            var simulatedBoard = board.Clone();

            var toX = king.Position.X;

            do
            {
                var fromSimulated = new Point(toX, king.Position.Y);

                toX += king.IsKingSideCastling(to) ? 1 : -1;

                var toSimulated = new Point(toX, king.Position.Y);

                simulatedBoard = simulatedBoard.SimulatedMovePiece(fromSimulated, toSimulated);

                var simulatedPiece = simulatedBoard.GetPiece(toSimulated);

                if (simulatedBoard.MovePutsOwnKingInCheck(simulatedPiece))
                    throw InvalidCastlingException.KingPassesThroughCheck(king, to);
            }
            while (toX != to.X);
        }

        private static bool IsKingSideCastling(this King king, Point to) => to.X > king.Position.X;

        private static Point GetCastlingRookTargetPosition(King king, Point to) => new Point(king.IsKingSideCastling(to) ? 6 : 4, king.Position.Y);

        private static Point GetCastlingRookPosition(King king, Point to) => new Point(king.IsKingSideCastling(to) ? 8 : 1, king.Position.Y);

        private static Rook? GetTryCastlingRook(this Board board, King king, Point to)
        {
            var rookPosition = GetCastlingRookPosition(king, to);

            return board.GetTryPiece(rookPosition) as Rook;
        }
    }
}
