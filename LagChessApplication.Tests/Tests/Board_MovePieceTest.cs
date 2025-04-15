using LagChessApplication.Domains;
using LagChessApplication.Exceptions;
using System.Drawing;
using Xunit;

namespace LagChessApplication.Tests.Tests
{
    public class Board_MovePieceTest
    {
        [Fact]
        public void AllPieces_ShouldMoveCorrectly_FromInitialPositions()
        {
            var board = Board.Create();

            board.MovePiece(new Point(5, 2), new Point(5, 3)); //e3
            board.MovePiece(new Point(4, 7), new Point(4, 6)); //d6

            board.MovePiece(new Point(7, 1), new Point(8, 3)); //h3
            board.MovePiece(new Point(3, 8), new Point(8, 3)); //xh3

            board.MovePiece(new Point(7, 2), new Point(8, 3)); //gxh3
            board.MovePiece(new Point(5, 7), new Point(5, 6)); //e6

            board.MovePiece(new Point(8, 1), new Point(7, 1)); //g1
            board.MovePiece(new Point(7, 8), new Point(6, 6)); //e6

            board.MovePiece(new Point(4, 1), new Point(8, 5)); //h5
            board.MovePiece(new Point(6, 6), new Point(8, 5)); //xh5

            board.MovePiece(new Point(7, 1), new Point(7, 4)); //g4
            board.MovePiece(new Point(2, 8), new Point(3, 6)); //c6


            board.MovePiece(new Point(5, 1), new Point(5, 2)); //e2
            board.MovePiece(new Point(6, 7), new Point(6, 6)); //f6
        }

        [Fact]
        public void AllPieces_ShouldRejectInvalidMoves_FromInitialPositions()
        {
            var board = Board.Create();

            var invalidMoves = new (Point from, Point to)[5]
            {
                (new Point(1, 2), new Point(1, 1)), // Pawn Return
                (new Point(1, 1), new Point(3, 3)), // Rook Diagonal
                (new Point(2, 1), new Point(2, 5)), // Knight Straight
                (new Point(4, 1), new Point(6, 1)), // Queen occupied
                (new Point(5, 1), new Point(5, 3))  // King move 2
            };

            foreach (var (from, to) in invalidMoves)
            {
                Assert.Throws<MoveInvalidException>(() => board.MovePiece(from, to));
            }
        }

        [Fact]
        public void Pawn_ShouldThrowKingInCheckException_WhenExposingKingToCheck()
        {
            var board = Board.Create();

            var moves = new (Point from, Point to)[]
            {
                (new Point(3, 2), new Point(3, 3)),
                (new Point(1, 7), new Point(1, 6)),
                (new Point(4, 1), new Point(1, 4)),
            };

            foreach (var (from, to) in moves)
            {
                board.MovePiece(from, to);
            }

            Assert.Throws<KingInCheckException>(() => board.MovePiece(new Point(4, 7), new Point(4, 6)));
        }

        [Fact]
        public void Rook_ShouldRevealCheck_WhenPieceMovesOutOfTheWay()
        {
            var board = Board.Create();

            var moves = new (Point from, Point to)[]
            {
                (new Point(2, 1), new Point(3, 3)),
                (new Point(1, 7), new Point(1, 6)),

                (new Point(3, 3), new Point(4, 5)),
                (new Point(5, 7), new Point(5, 6)),

                (new Point(4, 5), new Point(3, 7)),
            };

            foreach (var (from, to) in moves)
                board.MovePiece(from, to);

            Assert.Throws<KingInCheckException>(() => board.MovePiece(new Point(4, 8), new Point(7, 5))); 
        }

        [Fact]
        public void Bishop_ShouldThrowInvalidMoveException_WhenPieceBlocksThePath()
        {
            var board = Board.Create();

            board.MovePiece(new Point(5, 2), new Point(5, 3)); //e3
            board.MovePiece(new Point(4, 7), new Point(4, 6)); //d6

            board.MovePiece(new Point(7, 1), new Point(8, 3)); //h3
            board.MovePiece(new Point(3, 8), new Point(8, 3)); //xh3

            Assert.Throws<MoveInvalidException>(() =>
            {
                board.MovePiece(new Point(6, 1), new Point(8, 3)); //gxh3
            });
        }
    }
}
