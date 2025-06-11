using LagChessApplication.Domains;
using LagChessApplication.Exceptions;
using System.Drawing;

namespace LagChessApplication.Extensions
{
    public static class SimulatedBoardExtension
    {
        public static Board CreateClone(Board board) => board.Clone();

        public static Board SimulatedMovePiece(this Board simulatedBoard, Point from, Point to)
        {
            var simulatedPiece = simulatedBoard.GetPiece(from);

            if (!simulatedBoard.IsValidMove(simulatedPiece, to))
                throw InvalidMoveException.CreateSimuleted(simulatedPiece, to);

            simulatedBoard.SetPiecePosition(simulatedPiece, to);

            return simulatedBoard;
        }
    }
}
