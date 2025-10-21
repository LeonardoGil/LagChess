using LagChessApplication.Domains;
using LagChessApplication.Domains.Enums;
using LagChessApplication.Extensions;
using Xunit;

namespace LagChessApplication.Tests.Tests.Rules
{
    public class StalemateTests
    {
        [Fact]
        public void ChessGame_ShouldDetectStalemate_WhenForcedStalemateOccurs()
        {
            var chessGame = ChessGameExtension.Create();

            chessGame.Play(Square.E2, Square.E3);
            chessGame.Play(Square.A7, Square.A5);

            chessGame.Play(Square.D1, Square.H5);
            chessGame.Play(Square.A8, Square.A6);

            chessGame.Play(Square.H5, Square.A5);
            chessGame.Play(Square.H7, Square.H5);

            chessGame.Play(Square.H2, Square.H4);
            chessGame.Play(Square.A6, Square.H6);

            chessGame.Play(Square.A5, Square.C7);
            chessGame.Play(Square.F7, Square.F6);

            chessGame.Play(Square.C7, Square.D7);
            chessGame.Play(Square.E8, Square.F7);

            chessGame.Play(Square.D7, Square.B7);
            chessGame.Play(Square.D8, Square.D3);

            chessGame.Play(Square.B7, Square.B8);
            chessGame.Play(Square.D3, Square.H7);

            chessGame.Play(Square.B8, Square.C8);
            chessGame.Play(Square.F7, Square.G6);

            chessGame.Play(Square.C8, Square.E6);

            Assert.Equal(GameStatusEnum.Stalemate, chessGame.GameStatus);
        }
    }
}
