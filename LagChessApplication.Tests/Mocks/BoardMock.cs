using LagChessApplication.Domains;
using LagChessApplication.Domains.Pieces;
using System.Drawing;

namespace LagChessApplication.Tests.Mocks
{
    public static class BoardMock
    {
        public static Board WithOnePawnEach()
        {
            var white = new Player
            {
                Pieces = [PieceBase.CreatePieceWhite<Pawn>(1, 1)]
            };

            var black = new Player
            {
                Pieces = [PieceBase.CreatePieceBlack<Pawn>(1, 8)]
            };

            return new Board(white, black);
        }
    }
}
