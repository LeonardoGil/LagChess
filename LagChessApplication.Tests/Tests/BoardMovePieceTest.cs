using LagChessApplication.Domains;
using LagChessApplication.Exceptions;
using LagChessApplication.Extensions;
using System.Drawing;
using Xunit;

namespace LagChessApplication.Tests.Tests
{
    public class BoardMovePieceTest
    {
        [Fact]
        public void AllPieces_ShouldMoveCorrectly_FromInitialPositions()
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
        public void AllPieces_ShouldRejectInvalidMoves_FromInitialPositions()
        {
            var chessGame = GameChessExtension.Create();

            Assert.Throws<InvalidMoveException>(() => chessGame.Play(Square.A2, Square.A1));
            Assert.Throws<InvalidMoveException>(() => chessGame.Play(Square.A1, Square.C3));
            Assert.Throws<InvalidMoveException>(() => chessGame.Play(Square.B1, Square.B5));
            Assert.Throws<InvalidMoveException>(() => chessGame.Play(Square.D1, Square.F1));
            Assert.Throws<InvalidMoveException>(() => chessGame.Play(Square.E1, Square.E3));
        }

        [Fact]
        public void Pawn_ShouldThrowKingInCheckException_WhenExposingKingToCheck()
        {
            var chessGame = GameChessExtension.Create();

            chessGame.Play(Square.C2, Square.C3);
            chessGame.Play(Square.A7, Square.A6);
            chessGame.Play(Square.D1, Square.A4);

            Assert.Throws<KingInCheckException>(() => chessGame.Play(Square.D7, Square.D6));
        }

        [Fact]
        public void Rook_ShouldRevealCheck_WhenPieceMovesOutOfTheWay()
        {
            var chessGame = GameChessExtension.Create();

            chessGame.Play(Square.B1, Square.C3);
            chessGame.Play(Square.A7, Square.A6);

            chessGame.Play(Square.C3, Square.D5);
            chessGame.Play(Square.E7, Square.E6);

            chessGame.Play(Square.D5, Square.C7);

            Assert.Throws<KingInCheckException>(() => chessGame.Play(Square.D8, Square.G5));
        }

        [Fact]
        public void Bishop_ShouldThrowInvalidMoveException_WhenPieceBlocksThePath()
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
