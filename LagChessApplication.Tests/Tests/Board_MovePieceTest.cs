using LagChessApplication.Domains;
using LagChessApplication.Exceptions;
using LagChessApplication.Extensions;
using System.Drawing;
using Xunit;

namespace LagChessApplication.Tests.Tests
{
    public class Board_MovePieceTest
    {
        [Fact]
        public void AllPieces_ShouldMoveCorrectly_FromInitialPositions()
        {
            var gameChess = GameChessExtension.Create();
            var board = gameChess.Board;

            board.MovePiece(Square.E2, Square.E3);
            board.MovePiece(Square.D7, Square.D6);

            board.MovePiece(Square.G1, Square.H3);
            board.MovePiece(Square.C8, Square.H3);

            board.MovePiece(Square.G2, Square.H3);
            board.MovePiece(Square.E7, Square.E6);

            board.MovePiece(Square.H1, Square.G1);
            board.MovePiece(Square.G8, Square.F6);

            board.MovePiece(Square.D1, Square.H5);
            board.MovePiece(Square.F6, Square.H5);

            board.MovePiece(Square.G1, Square.G4);
            board.MovePiece(Square.B8, Square.C6);

            board.MovePiece(Square.E1, Square.E2);
            board.MovePiece(Square.F7, Square.F6);
        }

        [Fact]
        public void AllPieces_ShouldRejectInvalidMoves_FromInitialPositions()
        {
            var gameChess = GameChessExtension.Create();
            var board = gameChess.Board;

            var invalidMoves = new (Square from, Square to)[5]
            {
                (Square.A2, Square.A1), // Pawn Return
                (Square.A1, Square.C3), // Rook Diagonal
                (Square.B1, Square.B5), // Knight Straight
                (Square.D1, Square.F1), // Queen occupied
                (Square.E1, Square.E3)  // King move 2
            };

            foreach (var (from, to) in invalidMoves)
            {
                Assert.Throws<MoveInvalidException>(() => board.MovePiece(from, to));
            }
        }

        [Fact]
        public void Pawn_ShouldThrowKingInCheckException_WhenExposingKingToCheck()
        {
            var gameChess = GameChessExtension.Create();
            var board = gameChess.Board;

            var moves = new (Square from, Square to)[]
            {
                (Square.C2, Square.C3),
                (Square.A7, Square.A6),
                (Square.D1, Square.A4),
            };

            foreach (var (from, to) in moves)
            {
                board.MovePiece(from, to);
            }

            Assert.Throws<KingInCheckException>(() => board.MovePiece(Square.D7, Square.D6));
        }

        [Fact]
        public void Rook_ShouldRevealCheck_WhenPieceMovesOutOfTheWay()
        {
            var gameChess = GameChessExtension.Create();
            var board = gameChess.Board;

            var moves = new (Square from, Square to)[]
            {
                (Square.B1, Square.C3), 
                (Square.A7, Square.A6), 

                (Square.C3, Square.D5), 
                (Square.E7, Square.E6), 

                (Square.D5, Square.C7), 
            };

            foreach (var (from, to) in moves)
                board.MovePiece(from, to);

            Assert.Throws<KingInCheckException>(() => board.MovePiece(Square.D8, Square.G5));

        }

        [Fact]
        public void Bishop_ShouldThrowInvalidMoveException_WhenPieceBlocksThePath()
        {
            var gameChess = GameChessExtension.Create();
            var board = gameChess.Board;

            board.MovePiece(Square.E2, Square.E3);       
            board.MovePiece(Square.D7, Square.D6);       

            board.MovePiece(Square.G1, Square.H3);       
            board.MovePiece(Square.C8, Square.H3);       

            Assert.Throws<MoveInvalidException>(() =>
            {
                board.MovePiece(Square.F1, Square.H3);
            });
        }
    }
}
