using LagChessApplication.Domains.Enums;
using System.Drawing;

namespace LagChessApplication.Domains.Pieces
{
    public class King : PieceBase
    {
        public King(Point position, PieceColorEnum color) : base(position, color)
        {
            Type = PieceTypeEnum.King;
            MoveStyle = PieceMoveStyleEnum.All;
        }

        public override IPiece Clone()
        {
            return new King(Position ?? new Point(0, 0), Color)
            {
                IsDead = IsDead
            };
        }

        public override bool IsValidMove(Point to)
        {
            var position = Position ?? throw new ArgumentNullException();

            var possibilities = new int[3] { -1, 0, 1 };

            var vertical = possibilities.Contains(position.Y - to.Y);
            var horizontal = possibilities.Contains(position.X - to.X);

            return vertical && horizontal;
        }
    }
}
