using LagChessApplication.Domains;
using LagChessApplication.Domains.Enums;
using LagChessApplication.Exceptions;
using LagChessApplication.Extensions;
using Xunit;

namespace LagChessApplication.Tests.Tests.Moves
{
    public class PawnMoveTests
    {
        [Fact]
        public void Pawn_ShouldThrowException_WhenMovingDiagonallyWithoutCapture()
        {
            var chessGame = GameChessExtension.Create();

            chessGame.Play(Square.B2, Square.B3);
            chessGame.Play(Square.A7, Square.A6);

            Assert.Throws<InvalidMoveException>(() => chessGame.Play(Square.B3, Square.C4));
        }

        [Fact]
        public void Pawn_ShouldThrowException_WhenTryingToCaptureForward()
        {
            var chessGame = GameChessExtension.Create();

            chessGame.Play(Square.A2, Square.A4);
            chessGame.Play(Square.A7, Square.A5);

            Assert.Throws<InvalidMoveException>(() => chessGame.Play(Square.A4, Square.A5));
        }

        [Fact]
        public void Pawn_ShouldThrowException_WhenMovingLikeKnightOnFirstMove()
        {
            var chessGame = GameChessExtension.Create();

            Assert.Throws<InvalidMoveException>(() => chessGame.Play(Square.A2, Square.B4));
        }

        [Fact]
        public void Pawn_ShouldAllowDiagonalMove_WhenCapturing()
        {
            var chessGame = GameChessExtension.Create();

            chessGame.Play(Square.B2, Square.B3);
            chessGame.Play(Square.A7, Square.A6);

            chessGame.Play(Square.B3, Square.B4);
            chessGame.Play(Square.A6, Square.A5);

            chessGame.Play(Square.B4, Square.A5);
        }
    }
}
