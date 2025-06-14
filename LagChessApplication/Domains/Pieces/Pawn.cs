using LagChessApplication.Domains.Enums;
using LagChessApplication.Extensions.Pieces;
using LagChessApplication.Interfaces;
using System.Drawing;

namespace LagChessApplication.Domains.Pieces
{
    public class Pawn : PieceBase, IPiece
    {
        public Pawn(Point position, PieceColorEnum color) : base(position, color)
        {
            Type = PieceTypeEnum.Pawn;
            MoveStyle = PieceMoveStyleEnum.OneStraight;
        }

        public bool IsAttack(Point to)
        {
            var moveStyle = (Position, to).ConvertToMoveStyleEnum();

            return moveStyle == PieceMoveStyleEnum.Diagonal;
        }

        private static readonly int[] possibleHorizontalMoves = [0, 1];

        public override bool IsValidMove(Point to)
        {
            var verticalMove = Math.Abs(to.Y - Position.Y);
            var horizontalMove = Math.Abs(to.X - Position.X);

            return verticalMove switch
            {
                1 => possibleHorizontalMoves.Contains(horizontalMove),
                
                2 => _isAtStartingPosition && horizontalMove == 0,
                
                _ => false,
            };
        }

        public bool IsDoubleStepMove(Point to)
        {
            return Math.Abs(to.Y - Position.Y) == 2;
        }
    }
}
