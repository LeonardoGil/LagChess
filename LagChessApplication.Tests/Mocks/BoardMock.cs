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

        public static Board WithOneRookEach()
        {
            var white = new Player
            {
                Pieces = [PieceBase.CreatePieceWhite<Rook>(1, 1)]
            };

            var black = new Player
            {
                Pieces = [PieceBase.CreatePieceBlack<Rook>(1, 8)]
            };

            return new Board(white, black);
        }

        public static Board WithOneKingEach()
        {
            var white = new Player
            {
                Pieces = [PieceBase.CreatePieceWhite<King>(1, 1)]
            };

            var black = new Player
            {
                Pieces = [PieceBase.CreatePieceBlack<King>(1, 8)]
            };

            return new Board(white, black);
        }
    }
}
