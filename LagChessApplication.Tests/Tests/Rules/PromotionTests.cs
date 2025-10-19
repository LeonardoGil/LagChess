using LagChessApplication.Domains;
using LagChessApplication.Domains.Enums;
using LagChessApplication.Extensions;
using Xunit;

namespace LagChessApplication.Tests.Tests.Rules
{
    public class PromotionTests
    {
        [Fact]
        public void Pawn_ShouldBePromoted_WhenCapturingOnLastRank()
        {
            var chessGame = GameChessExtension.Create(onPawnPromotion: () => PieceTypeEnum.Queen);

            chessGame.Play(Square.A2, Square.A4);
            chessGame.Play(Square.H7, Square.H6);
            
            chessGame.Play(Square.A4, Square.A5);
            chessGame.Play(Square.H6, Square.H5);
            
            chessGame.Play(Square.A5, Square.A6);
            chessGame.Play(Square.H5, Square.H4);
            
            chessGame.Play(Square.A6, Square.B7);
            chessGame.Play(Square.H4, Square.H3);

            chessGame.Play(Square.B7, Square.A8);

            var lastMove = chessGame.History.LastMove;

            Assert.NotNull(lastMove.PawnPromotion);
            Assert.Equal(PieceTypeEnum.Queen, lastMove.PawnPromotion);

            var promotedPiece = chessGame.Board.GetPiece(Square.A8);

            Assert.NotNull(promotedPiece);
            Assert.Equal(PieceTypeEnum.Queen, promotedPiece.Type);
        }
    }
}
