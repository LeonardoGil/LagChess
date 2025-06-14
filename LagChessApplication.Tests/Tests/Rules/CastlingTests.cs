using LagChessApplication.Domains;
using LagChessApplication.Extensions;
using Xunit;

namespace LagChessApplication.Tests.Tests.Rules
{
    public class CastlingTests
    {
        [Fact]
        public void White_ShouldCastleKingSide_Correctly()
        {
            var chessGame = GameChessExtension.Create();

            chessGame.Play(Square.E2, Square.E3);
            chessGame.Play(Square.E7, Square.E6);

            chessGame.Play(Square.G1, Square.F3);
            chessGame.Play(Square.D7, Square.D6);

            chessGame.Play(Square.F1, Square.E2);
            chessGame.Play(Square.A7, Square.A6);

            chessGame.Play(Square.E1, Square.G1);
        }

        [Fact]
        public void White_ShouldCastleQueenSide_Correctly()
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
        }
    }
}
