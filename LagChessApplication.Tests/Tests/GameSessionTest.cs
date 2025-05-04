using LagChessApplication.Domains;
using LagChessApplication.Domains.Enums;
using LagChessApplication.Exceptions;
using LagChessApplication.Extensions;
using Xunit;

namespace LagChessApplication.Tests.Tests
{
    public class GameSessionTest
    {
        [Fact]
        public void TurnManager_ShouldCalculateTurnNumberAndPlayerTurn_WhenGameIsInProgress()
        {
            var gameChess = GameChessExtension.Create();

            // Turn 1
            gameChess.Play(Square.E2, Square.E3);
            gameChess.Play(Square.D7, Square.D6);

            // Turn 2

            Assert.Equal(gameChess.Turn, 2);
            Assert.Equal(gameChess.TurnPlayer, PieceColorEnum.White);

            gameChess.Play(Square.G1, Square.H3);
            gameChess.Play(Square.C8, Square.H3);

            // Turn 3

            Assert.Equal(gameChess.Turn, 3);

            gameChess.Play(Square.B1, Square.A3);
            
            Assert.Equal(gameChess.TurnPlayer, PieceColorEnum.Black);
            
            gameChess.Play(Square.D6, Square.D5);

            // Turn 4
            Assert.Equal(gameChess.Turn, 4);
        }

        [Fact]
        public void TurnManager_ShouldThrow_WhenPlayerMovesPieceOutOfTurn()
        {
            var gameChess = GameChessExtension.Create();

            gameChess.Play(Square.E2, Square.E4);
            gameChess.Play(Square.E7, Square.E5);

            gameChess.Play(Square.B2, Square.B3);

            Assert.Throws<InvalidPieceOwnershipException>(() => gameChess.Play(Square.B3, Square.B4));
        }
    }
}
