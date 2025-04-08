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
                Pieces = [Pawn.CreateWhite(new Point(1, 1))]
            };

            var black = new Player
            {
                Pieces = [Pawn.CreateBlack(new Point(1, 8))]
            };

            return new Board(white, black);
        }
    }
}
