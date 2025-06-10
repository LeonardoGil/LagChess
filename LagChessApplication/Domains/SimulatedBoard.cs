using LagChessApplication.Exceptions;
using System.Drawing;

namespace LagChessApplication.Domains
{
    public static class SimulatedBoard
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
