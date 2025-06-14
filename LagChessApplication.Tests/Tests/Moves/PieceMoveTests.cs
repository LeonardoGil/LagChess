using LagChessApplication.Domains;
using LagChessApplication.Exceptions;
using LagChessApplication.Extensions;
using Xunit;

namespace LagChessApplication.Tests.Tests.Moves
{
    public class PieceMoveTests
    {
        [Fact]
        public void Game_ShouldMoveCorrectly_FromInitialPositions()
        {
            var chessGame = GameChessExtension.Create();

            chessGame.Play(Square.E2, Square.E3);
            chessGame.Play(Square.D7, Square.D6);

            chessGame.Play(Square.G1, Square.H3);
            chessGame.Play(Square.C8, Square.H3);

            chessGame.Play(Square.G2, Square.H3);
            chessGame.Play(Square.E7, Square.E6);

            chessGame.Play(Square.H1, Square.G1);
            chessGame.Play(Square.G8, Square.F6);

            chessGame.Play(Square.D1, Square.H5);
            chessGame.Play(Square.F6, Square.H5);

            chessGame.Play(Square.G1, Square.G4);
            chessGame.Play(Square.B8, Square.C6);

            chessGame.Play(Square.E1, Square.E2);
            chessGame.Play(Square.F7, Square.F6);
        }

        [Fact]
        public void Game_ShouldRejectInvalidMoves_FromInitialPositions()
        {
            var chessGame = GameChessExtension.Create();

            Assert.Throws<InvalidMoveException>(() => chessGame.Play(Square.A2, Square.A1));
            Assert.Throws<InvalidMoveException>(() => chessGame.Play(Square.A1, Square.C3));
            Assert.Throws<InvalidMoveException>(() => chessGame.Play(Square.B1, Square.B5));
            Assert.Throws<InvalidMoveException>(() => chessGame.Play(Square.D1, Square.F1));
            Assert.Throws<InvalidMoveException>(() => chessGame.Play(Square.E1, Square.E3));
        }

        [Fact]
        public void Game_ShouldThrowInvalidMoveException_WhenPathIsBlocked()
        {
            var chessGame = GameChessExtension.Create();

            chessGame.Play(Square.E2, Square.E3);
            chessGame.Play(Square.D7, Square.D6);

            chessGame.Play(Square.G1, Square.H3);
            chessGame.Play(Square.C8, Square.H3);

            Assert.Throws<InvalidMoveException>(() => chessGame.Play(Square.F1, Square.H3));
        }
    }
}
