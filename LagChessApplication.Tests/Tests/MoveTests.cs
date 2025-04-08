using FluentAssertions;
using LagChessApplication.Domains.Pieces;
using LagChessApplication.Tests.Mocks;
using System.Drawing;
using Xunit;

namespace LagChessApplication.Tests.Tests
{
    public class MoveTests
    {
        [Fact]
        public void Pawn_Should_Move_Front()
        {
            var board = BoardMock.WithOnePawnEach();

            var whitePawn = board.White.Pieces.First(x => x is Pawn);
            var blackPawn = board.Black.Pieces.First(x => x is Pawn);

            var whiteDirection = (Size)new Point(0, 1);
            var blackDirection = (Size)new Point(0, -1);

            board.MovePiece(whitePawn.Position.Value, Point.Add(whitePawn.Position.Value, whiteDirection));
            board.MovePiece(blackPawn.Position.Value, Point.Add(blackPawn.Position.Value, blackDirection));

            board.GetPiece(new Point(1, 2)).Should().Be(whitePawn);
            board.GetPiece(new Point(1, 7)).Should().Be(blackPawn);
        }
    }
}
