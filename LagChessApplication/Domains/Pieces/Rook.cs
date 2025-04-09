using LagChessApplication.Domains.Enums;
using System.Drawing;

namespace LagChessApplication.Domains.Pieces
{
    public class Rook : PieceBase, IPiece
    {
        public Rook(Point position, PieceColorEnum color) : base(position, color)
        {
            Type = PieceTypeEnum.Rook;
            MoveStyle = PieceMoveStyleEnum.Linear;
        }

        public override IPiece Clone()
        {
            return new Rook(Position ?? new Point(0, 0), Color)
            {
                IsDead = IsDead
            };
        }

        public override bool IsValidMove(Point to)
        {
            var position = Position ?? throw new ArgumentNullException();

            var horizontal = position.X != to.X && position.Y == to.Y;
            var vertical = position.X == to.X && position.Y != to.Y;

            return horizontal || vertical;
        }
    }
}
