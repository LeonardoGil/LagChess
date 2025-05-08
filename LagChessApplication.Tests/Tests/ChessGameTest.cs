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
        public void ChessHistory_ShouldFillHistoryAccurately_WhenGameIsInProgress()
        {
            var chessGame = GameChessExtension.Create();

            chessGame.Board.OnPawnPromotion += () => PieceTypeEnum.Queen;

            chessGame.Play(Square.B2, Square.B4);

            var actualMove = chessGame.History.Get().First();

            Assert.Equal(PieceTypeEnum.Pawn, actualMove.Piece);
            
            Assert.False(actualMove.CapturedPiece);
            
            Assert.Null(actualMove.PawnPromotion);

            Assert.Equal("b4", actualMove.Notation);

            chessGame.Play(Square.C7, Square.C5);

            chessGame.Play(Square.B4, Square.C5); 

            chessGame.Play(Square.B7, Square.B6);

            chessGame.Play(Square.C5, Square.B6); 

            chessGame.Play(Square.C8, Square.A6);

            chessGame.Play(Square.B6, Square.B7);

            chessGame.Play(Square.D7, Square.D6);

            chessGame.Play(Square.B7, Square.A8);

            actualMove = chessGame.History.Get().First();

            Assert.Equal(PieceTypeEnum.Pawn, actualMove.Piece);

            Assert.Equal(PieceTypeEnum.Queen, actualMove.PawnPromotion);

            Assert.True(actualMove.CapturedPiece);

            Assert.Equal("bxa8=Q", actualMove.Notation);
        }

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
