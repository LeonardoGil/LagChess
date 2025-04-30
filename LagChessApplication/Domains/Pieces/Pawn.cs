using LagChessApplication.Domains.Enums;
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

        public override bool IsValidMove(Point to)
        {
            var move = _isAtStartingPosition ? 2 : 1;

            var horizontal = to.X >= Position.X - 1 && to.X <= Position.X + 1;

            return Color switch
            {
                PieceColorEnum.White => horizontal && (to.Y > Position.Y && to.Y <= Position.Y + move),
                PieceColorEnum.Black => horizontal && (to.Y < Position.Y && to.Y >= Position.Y - move),
                
                _ => throw new NotSupportedException(),
            };
        }
    }
}
