using LagChessApplication.Domains;
using LagChessApplication.Extensions;
using Xunit;

namespace LagChessApplication.Tests.Tests.Rules
{
    public class CheckMateTests
    {
        [Fact]
        public void ChessGame_ShouldDetectCheckmate_WhenFoolsMateOccurs()
        {
            var chessGame = GameChessExtension.Create();

            chessGame.Play(Square.F2, Square.F3);
            chessGame.Play(Square.E7, Square.E5);

            chessGame.Play(Square.G2, Square.G4);
            chessGame.Play(Square.D8, Square.H4);

            var actualMove = chessGame.History.Get().First();

            Assert.True(actualMove.OpponentKingInCheckMate);
        }

        [Fact]
        public void ChessGame_ShouldDetectCheckmate_WhenScholarsMateOccurs()
        {
            var chessGame = GameChessExtension.Create();

            chessGame.Play(Square.E2, Square.E4);
            chessGame.Play(Square.E7, Square.E5);
            chessGame.Play(Square.F1, Square.C4);
            chessGame.Play(Square.B8, Square.C6);
            chessGame.Play(Square.D1, Square.H5);
            chessGame.Play(Square.G8, Square.F6);
            chessGame.Play(Square.H5, Square.F7);

            var actualMove = chessGame.History.Get().First();

            Assert.True(actualMove.OpponentKingInCheckMate);
        }
    }
}
