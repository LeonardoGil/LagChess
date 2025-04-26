using LagChessApplication.Domains;
using LagChessApplication.Domains.Enums;
using LagChessApplication.Extensions;
using Xunit;

namespace LagChessApplication.Tests.Tests
{
    public class PawnTest
    {
        [Fact]
        public void Pawn_ShouldBePromoted_WhenArrivingAtPromotionRow()
        {
            var board = BoardExtension.Create();

            board.OnPawnPromotion += () => PieceTypeEnum.Queen;

            board.MovePiece(Square.B2, Square.B3);
            board.MovePiece(Square.A7, Square.A6);

            board.MovePiece(Square.B3, Square.B4);
            board.MovePiece(Square.A6, Square.A5);

            board.MovePiece(Square.B4, Square.B5);
            board.MovePiece(Square.A5, Square.A4);

            board.MovePiece(Square.B5, Square.B6);
            board.MovePiece(Square.A4, Square.A3);

            board.MovePiece(Square.B6, Square.C7);
            board.MovePiece(Square.A8, Square.A4);

            board.MovePiece(Square.C7, Square.B8);
            board.MovePiece(Square.A4, Square.H4);

            board.MovePiece(Square.B8, Square.C8);
            board.MovePiece(Square.H4, Square.H2);
        }
    }
}
