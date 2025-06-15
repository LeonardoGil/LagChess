using LagChessApplication.Domains;
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

            simulatedBoard.ValidateMove(simulatedPiece, to);

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
