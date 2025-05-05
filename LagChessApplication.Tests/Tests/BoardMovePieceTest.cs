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
            var gameChess = GameChessExtension.Create();

            gameChess.Play(Square.E2, Square.E3);
            gameChess.Play(Square.D7, Square.D6);

            gameChess.Play(Square.G1, Square.H3);
            gameChess.Play(Square.C8, Square.H3);

            gameChess.Play(Square.G2, Square.H3);
            gameChess.Play(Square.E7, Square.E6);

            gameChess.Play(Square.H1, Square.G1);
            gameChess.Play(Square.G8, Square.F6);

            gameChess.Play(Square.D1, Square.H5);
            gameChess.Play(Square.F6, Square.H5);

            gameChess.Play(Square.G1, Square.G4);
            gameChess.Play(Square.B8, Square.C6);

            gameChess.Play(Square.E1, Square.E2);
            gameChess.Play(Square.F7, Square.F6);
        }

        [Fact]
        public void AllPieces_ShouldRejectInvalidMoves_FromInitialPositions()
        {
            var gameChess = GameChessExtension.Create();

            Assert.Throws<MoveInvalidException>(() => gameChess.Play(Square.A2, Square.A1));
            Assert.Throws<MoveInvalidException>(() => gameChess.Play(Square.A1, Square.C3));
            Assert.Throws<MoveInvalidException>(() => gameChess.Play(Square.B1, Square.B5));
            Assert.Throws<MoveInvalidException>(() => gameChess.Play(Square.D1, Square.F1));
            Assert.Throws<MoveInvalidException>(() => gameChess.Play(Square.E1, Square.E3));
        }

        [Fact]
        public void Pawn_ShouldThrowKingInCheckException_WhenExposingKingToCheck()
        {
            var gameChess = GameChessExtension.Create();

            gameChess.Play(Square.C2, Square.C3);
            gameChess.Play(Square.A7, Square.A6);
            gameChess.Play(Square.D1, Square.A4);

            Assert.Throws<KingInCheckException>(() => gameChess.Play(Square.D7, Square.D6));
        }

        [Fact]
        public void Rook_ShouldRevealCheck_WhenPieceMovesOutOfTheWay()
        {
            var gameChess = GameChessExtension.Create();

            gameChess.Play(Square.B1, Square.C3);
            gameChess.Play(Square.A7, Square.A6);

            gameChess.Play(Square.C3, Square.D5);
            gameChess.Play(Square.E7, Square.E6);

            gameChess.Play(Square.D5, Square.C7);

            Assert.Throws<KingInCheckException>(() => gameChess.Play(Square.D8, Square.G5));
        }

        [Fact]
        public void Bishop_ShouldThrowInvalidMoveException_WhenPieceBlocksThePath()
        {
            var gameChess = GameChessExtension.Create();

            gameChess.Play(Square.E2, Square.E3);
            gameChess.Play(Square.D7, Square.D6);

            gameChess.Play(Square.G1, Square.H3);
            gameChess.Play(Square.C8, Square.H3);

            Assert.Throws<MoveInvalidException>(() => gameChess.Play(Square.F1, Square.H3));
        }
    }
}
