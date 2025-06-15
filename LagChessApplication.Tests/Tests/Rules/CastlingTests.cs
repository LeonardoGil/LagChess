using LagChessApplication.Domains;
using LagChessApplication.Domains.Enums;
using LagChessApplication.Exceptions;
using LagChessApplication.Extensions;
using System.Drawing;
using Xunit;

namespace LagChessApplication.Tests.Tests.Rules
{
    public class CastlingTests
    {
        [Fact]
        public void Move_ShouldCastleKingSide_Correctly()
        {
            var chessGame = GameChessExtension.Create();

            chessGame.Play(Square.E2, Square.E3);
            chessGame.Play(Square.E7, Square.E6);

            chessGame.Play(Square.G1, Square.F3);
            chessGame.Play(Square.D7, Square.D6);

            chessGame.Play(Square.F1, Square.E2);
            chessGame.Play(Square.A7, Square.A6);

            chessGame.Play(Square.E1, Square.G1);

            Assert.Equal(PieceTypeEnum.King, chessGame.Board.GetPiece(Square.G1).Type);
            Assert.Equal(PieceTypeEnum.Rook, chessGame.Board.GetPiece(Square.F1).Type);
        }

        [Fact]
        public void Move_ShouldCastleQueenSide_Correctly()
        {
            var chessGame = GameChessExtension.Create();

            chessGame.Play(Square.D2, Square.D4);
            chessGame.Play(Square.A7, Square.A6);

            chessGame.Play(Square.C1, Square.E3);
            chessGame.Play(Square.A6, Square.A5);

            chessGame.Play(Square.B1, Square.C3);
            chessGame.Play(Square.A5, Square.A4);

            chessGame.Play(Square.D1, Square.D3);
            chessGame.Play(Square.A4, Square.A3);

            chessGame.Play(Square.E1, Square.C1);

            Assert.Equal(PieceTypeEnum.King, chessGame.Board.GetPiece(Square.C1).Type);
            Assert.Equal(PieceTypeEnum.Rook, chessGame.Board.GetPiece(Square.D1).Type);
        }

        [Fact]
        public void Move_CannotCastleKingSide_IfKingHasMoved()
        {
            var chessGame = GameChessExtension.Create();

            chessGame.Play(Square.E2, Square.E3);
            chessGame.Play(Square.E7, Square.E6);

            chessGame.Play(Square.E1, Square.E2);
            chessGame.Play(Square.D7, Square.D6);

            chessGame.Play(Square.E2, Square.E1);
            chessGame.Play(Square.E6, Square.E5);

            chessGame.Play(Square.F1, Square.E2);
            chessGame.Play(Square.C8, Square.F5);

            chessGame.Play(Square.G1, Square.H3);
            chessGame.Play(Square.D8, Square.H4);

            Assert.Throws<InvalidCastlingException>(() => chessGame.Play(Square.E1, Square.G1));
        }

        [Fact]
        public void Move_CannotCastleQueenSide_IfRookHasMoved()
        {
            var chessGame = GameChessExtension.Create();

            chessGame.Play(Square.A2, Square.A4);
            chessGame.Play(Square.A7, Square.A6);

            chessGame.Play(Square.A1, Square.A3);
            chessGame.Play(Square.D7, Square.D6);

            chessGame.Play(Square.D2, Square.D4);
            chessGame.Play(Square.E7, Square.E5);

            chessGame.Play(Square.D1, Square.D3);
            chessGame.Play(Square.E5, Square.E4);

            chessGame.Play(Square.C1, Square.G5);
            chessGame.Play(Square.E4, Square.D3);

            chessGame.Play(Square.B1, Square.D2);
            chessGame.Play(Square.D8, Square.G5);

            chessGame.Play(Square.A3, Square.A1);
            chessGame.Play(Square.G5, Square.A5);

            Assert.Throws<InvalidCastlingException>(() => chessGame.Play(Square.E1, Square.C1));
        }

        [Fact]
        public void Move_CannotCastle_IfPieceIsBetweenKingAndRook()
        {
            var chessGame = GameChessExtension.Create();

            Assert.Throws<InvalidCastlingException>(() => chessGame.Play(Square.E1, Square.G1));
            Assert.Throws<InvalidCastlingException>(() => chessGame.Play(Square.E1, Square.C1));
        }

        [Fact]
        public void Move_CannotCastle_WhenInCheck()
        {
            var chessGame = GameChessExtension.Create();

            chessGame.Play(Square.G1, Square.H3);
            chessGame.Play(Square.D7, Square.D6);

            chessGame.Play(Square.E2, Square.E3);
            chessGame.Play(Square.C8, Square.H3);

            chessGame.Play(Square.G2, Square.H3);
            chessGame.Play(Square.C7, Square.C6);

            chessGame.Play(Square.D2, Square.D4);
            chessGame.Play(Square.D6, Square.D5);

            chessGame.Play(Square.F1, Square.G2);
            chessGame.Play(Square.D8, Square.A5);

            Assert.Throws<InvalidCastlingException>(() => chessGame.Play(Square.E1, Square.G1));
        }

        [Fact]
        public void Move_CannotCastle_ThroughCheck()
        {
            var chessGame = GameChessExtension.Create();

            chessGame.Play(Square.G1, Square.H3);
            chessGame.Play(Square.D7, Square.D6);

            chessGame.Play(Square.E2, Square.E3);
            chessGame.Play(Square.C8, Square.H3);

            chessGame.Play(Square.G2, Square.H3);
            chessGame.Play(Square.E7, Square.E6);

            chessGame.Play(Square.F1, Square.D3);
            chessGame.Play(Square.D8, Square.G5);

            Assert.Throws<InvalidCastlingException>(() => chessGame.Play(Square.E1, Square.G1));
        }
    }
}
