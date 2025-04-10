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
        public void Pawn_Should_Move_With_Success()
        {
            var board = BoardMock.WithOnePawnEach();

            var whitePawn = board.White.Pieces.First(x => x is Pawn);
            var blackPawn = board.Black.Pieces.First(x => x is Pawn);

            var whiteDirection = (Size)new Point(0, 1);
            var blackDirection = (Size)new Point(0, -1);

            board.MovePiece(whitePawn.Position, Point.Add(whitePawn.Position, whiteDirection));
            board.MovePiece(blackPawn.Position, Point.Add(blackPawn.Position, blackDirection));

            board.GetPiece(new Point(1, 2)).Should().Be(whitePawn);
            board.GetPiece(new Point(1, 7)).Should().Be(blackPawn);
        }

        [Fact]
        public void Rook_Should_Move_With_Success()
        {
            var board = BoardMock.WithOneRookEach();

            var whiteRook = board.White.Pieces.First(x => x is Rook);
            var blackRook = board.Black.Pieces.First(x => x is Rook);

            var whiteDirection = (Size)new Point(0, 6);
            var blackDirection = (Size)new Point(7, 0);

            board.MovePiece(whiteRook.Position, Point.Add(whiteRook.Position, whiteDirection));
            board.MovePiece(blackRook.Position, Point.Add(blackRook.Position, blackDirection));

            board.GetPiece(new Point(1, 7)).Should().Be(whiteRook);
            board.GetPiece(new Point(8, 8)).Should().Be(blackRook);
        }

        [Fact]
        public void King_Should_Move_With_Success()
        {
            var board = BoardMock.WithOneKingEach();

            var whiteKing = board.White.Pieces.First(x => x is King);
            var blackKing = board.Black.Pieces.First(x => x is King);

            var whiteDirection = (Size)new Point(0, 1);
            var blackDirection = (Size)new Point(1, 0);

            board.MovePiece(whiteKing.Position, Point.Add(whiteKing.Position, whiteDirection));
            board.MovePiece(blackKing.Position, Point.Add(blackKing.Position, blackDirection));

            board.GetPiece(new Point(1, 2)).Should().Be(whiteKing);
            board.GetPiece(new Point(2, 8)).Should().Be(blackKing);
        }
    }
}
