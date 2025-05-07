using LagChessApplication.Domains;
using LagChessApplication.Domains.Enums;
using LagChessApplication.Exceptions;
using LagChessApplication.Extensions;
using Xunit;

namespace LagChessApplication.Tests.Tests
{
    public class ChessGameTest
    {
        [Fact]
        public void TurnManager_ShouldCalculateTurnNumberAndPlayerTurn_WhenGameIsInProgress()
        {
            var chessGame = GameChessExtension.Create();

            // Turn 1
            chessGame.Play(Square.E2, Square.E3);
            chessGame.Play(Square.D7, Square.D6);

            // Turn 2

            Assert.Equal(2, chessGame.Turn);
            Assert.Equal(PieceColorEnum.White, chessGame.TurnPlayer);

            chessGame.Play(Square.G1, Square.H3);
            chessGame.Play(Square.C8, Square.H3);

            // Turn 3

            Assert.Equal(3, chessGame.Turn);

            chessGame.Play(Square.B1, Square.A3);
            
            Assert.Equal(PieceColorEnum.Black, chessGame.TurnPlayer);
            
            chessGame.Play(Square.D6, Square.D5);

            // Turn 4
            Assert.Equal(4, chessGame.Turn);
        }

        [Fact]
        public void TurnManager_ShouldThrow_WhenPlayerMovesPieceOutOfTurn()
        {
            var chessGame = GameChessExtension.Create();

            chessGame.Play(Square.E2, Square.E4);
            chessGame.Play(Square.E7, Square.E5);

            chessGame.Play(Square.B2, Square.B3);

            Assert.Throws<InvalidPieceOwnershipException>(() => chessGame.Play(Square.B3, Square.B4));
        }
    }
}
