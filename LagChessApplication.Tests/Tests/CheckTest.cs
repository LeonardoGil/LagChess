using LagChessApplication.Domains;
using LagChessApplication.Exceptions;
using LagChessApplication.Extensions;
using Xunit;

namespace LagChessApplication.Tests.Tests
{
    public class CheckTest
    {
        [Fact]
        public void Check_ShouldThrowKingInCheckException_WhenExposingKingToCheck()
        {
            var chessGame = ChessGameExtension.Create();

            chessGame.Play(Square.C2, Square.C3);
            chessGame.Play(Square.A7, Square.A6);
            chessGame.Play(Square.D1, Square.A4);

            Assert.Throws<KingInCheckException>(() => chessGame.Play(Square.D7, Square.D6));
        }

        [Fact]
        public void Check_ShouldThrowKingInCheckException_WhenTryingToMovePieceWhileKingIsInCheck()
        {
            var chessGame = ChessGameExtension.Create();

            chessGame.Play(Square.B1, Square.C3);
            chessGame.Play(Square.A7, Square.A6);

            chessGame.Play(Square.C3, Square.D5);
            chessGame.Play(Square.E7, Square.E6);

            chessGame.Play(Square.D5, Square.C7);

            Assert.Throws<KingInCheckException>(() => chessGame.Play(Square.D8, Square.G5));
        }

        [Fact]
        public void Check_ShouldReturnTrue_WhenOpponentKingIsInCheck()
        {
            var chessGame = ChessGameExtension.Create();

            chessGame.Play(Square.F2, Square.F3);
            chessGame.Play(Square.E7, Square.E5);

            chessGame.Play(Square.G2, Square.G4);
            chessGame.Play(Square.D8, Square.H4);

            var move = chessGame.History.LastMove;

            Assert.True(move.OpponentKingInCheck);
        }

        [Fact]
        public void Check_ShouldAllowKingToEscapeCheck_WhenKingIsThreatened()
        {
            var chessGame = GameChessExtension.Create();

            chessGame.Play(Square.E2, Square.E3);
            chessGame.Play(Square.D7, Square.D5);

            chessGame.Play(Square.D1, Square.H5);
            chessGame.Play(Square.C8, Square.D7);

            chessGame.Play(Square.H5, Square.F7);
            chessGame.Play(Square.E8, Square.F7);
        }
    }
}
