using LagChessApplication.Domains;
using LagChessApplication.Domains.Enums;
using LagChessApplication.Exceptions;
using LagChessApplication.Extensions;
using Xunit;

namespace LagChessApplication.Tests.Tests
{
    public class PawnTest
    {
        [Fact]
        public void Pawn_ShouldBePromoted_WhenArrivingAtPromotionRow()
        {
            var gameChess = GameChessExtension.Create();

            gameChess.Board.OnPawnPromotion += () => PieceTypeEnum.Queen;

            gameChess.Play(Square.B2, Square.B3);
            gameChess.Play(Square.A7, Square.A6);

            gameChess.Play(Square.B3, Square.B4);
            gameChess.Play(Square.A6, Square.A5);

            gameChess.Play(Square.B4, Square.B5);
            gameChess.Play(Square.A5, Square.A4);

            gameChess.Play(Square.B5, Square.B6);
            gameChess.Play(Square.A4, Square.A3);

            gameChess.Play(Square.B6, Square.C7);
            gameChess.Play(Square.A8, Square.A4);

            gameChess.Play(Square.C7, Square.B8);
            gameChess.Play(Square.A4, Square.H4);

            gameChess.Play(Square.B8, Square.C8);
            gameChess.Play(Square.H4, Square.H2);
        }

        [Fact]
        public void Pawn_ShouldThrowException_WhenMovingDiagonallyWithoutCapture()
        {
            var gameChess = GameChessExtension.Create();

            gameChess.Play(Square.B2, Square.B3);
            gameChess.Play(Square.A7, Square.A6);

            Assert.Throws<MoveInvalidException>(() => gameChess.Play(Square.B3, Square.C4));
        }

        [Fact]
        public void Pawn_ShouldAllowDiagonalMove_WhenCapturing()
        {
            var gameChess = GameChessExtension.Create();

            gameChess.Play(Square.B2, Square.B3);
            gameChess.Play(Square.A7, Square.A6);

            gameChess.Play(Square.B3, Square.B4);
            gameChess.Play(Square.A6, Square.A5);

            gameChess.Play(Square.B4, Square.A5);
        }
    }
}
