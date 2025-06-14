using LagChessApplication.Domains;
using LagChessApplication.Domains.Pieces;
using LagChessApplication.Exceptions;
using LagChessApplication.Extensions.Rules;
using System.Drawing;

namespace LagChessApplication.Extensions.Boards
{
    public static class SimulatedBoardExtension
    {
        public static Board CreateClone(Board board) => board.Clone();

        public static Board SimulatedMovePiece(this Board board, Point from, Point to)
        {
            var simulatedBoard = CreateClone(board);

            var simulatedPiece = simulatedBoard.GetPiece(from);

            if (!simulatedBoard.IsValidMove(simulatedPiece, to))
            {
                var isCastling = simulatedPiece is King king && king.IsCastlingMove(board, to);

                if (!isCastling)
                {
                    throw InvalidMoveException.CreateSimuleted(simulatedPiece, to);
                }
            }

            simulatedBoard.SetPiece(simulatedPiece, to);

            return simulatedBoard;
        }

        public static bool SimulatedMovePiecePutsOwnKingInCheck(this Board board, Point from, Point to)
        {
            var simulatedBoard = board.SimulatedMovePiece(from, to);

            var simulatedPiece = simulatedBoard.GetPiece(to);

            return simulatedBoard.MovePutsOwnKingInCheck(simulatedPiece);
        }
    }
}
