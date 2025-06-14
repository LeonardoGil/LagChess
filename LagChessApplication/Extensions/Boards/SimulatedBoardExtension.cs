using LagChessApplication.Domains;
using LagChessApplication.Exceptions;
using LagChessApplication.Interfaces;
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
                throw InvalidMoveException.CreateSimuleted(simulatedPiece, to);

            simulatedBoard.SetPiecePosition(simulatedPiece, to);

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
