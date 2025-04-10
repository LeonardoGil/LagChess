using LagChessApplication.Domains.Enums;
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

        public override IPiece Clone()
        {
            return new Pawn(Position ?? new Point(0, 0), Color)
            {
                IsDead = IsDead
            };
        }

        public override bool IsValidMove(Point to)
        {
            var position = Position ?? throw new ArgumentNullException();

            var horizontal = to.X == position.X || (to.X == position.X + 1 || to.X == position.X - 1);
            var vertical = to.Y != position.Y;

            switch (Color)
            {
                case PieceColorEnum.White:
                    vertical = vertical && to.Y == position.Y + 1;
                    break;

                case PieceColorEnum.Black:
                    vertical = vertical && to.Y == position.Y - 1;
                    break;
            }

            return horizontal && vertical;
        }
    }
}
