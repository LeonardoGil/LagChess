using LagChessApplication.Domains;
using LagChessApplication.Exceptions;
using LagChessApplication.Extensions;
using Xunit;

namespace LagChessApplication.Tests.Tests.Rules
{
    public class EnPassantTests
    {
        [Fact]
        public void Pawn_ShouldAllowEnPassant_WhenConditionsAreMet()
        {
            var chessGame = GameChessExtension.Create();

            chessGame.Play(Square.E2, Square.E4);
            chessGame.Play(Square.A7, Square.A6);

            chessGame.Play(Square.E4, Square.E5);
            chessGame.Play(Square.D7, Square.D5);

            chessGame.Play(Square.E5, Square.D6);
        }

        [Fact]
        public void Pawn_ShouldNotAllowDiagonalMove_WhenNotEnPassant()
        {
            var chessGame = GameChessExtension.Create();

            chessGame.Play(Square.E2, Square.E4);
            chessGame.Play(Square.A7, Square.A6);

            chessGame.Play(Square.E4, Square.E5);
            chessGame.Play(Square.D7, Square.D6);

            Assert.Throws<InvalidMoveException>(() => chessGame.Play(Square.E5, Square.F6));
        }

        [Fact]
        public void Pawn_ShouldNotAllowEnPassant_AfterTurnExpires()
        {
            var chessGame = GameChessExtension.Create();

            chessGame.Play(Square.E2, Square.E4);
            chessGame.Play(Square.A7, Square.A6);

            chessGame.Play(Square.E4, Square.E5);
            chessGame.Play(Square.D7, Square.D5);

            chessGame.Play(Square.H2, Square.H3);
            chessGame.Play(Square.A6, Square.A5);

            Assert.Throws<InvalidMoveException>(() => chessGame.Play(Square.E5, Square.D6));
        }
    }
}
