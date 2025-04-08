using LagChessApplication.Domains.Enums;
using System.Drawing;

namespace LagChessApplication.Domains.Pieces
{
    internal class Pawn : PieceBase, IPiece
    {
        public override bool CanMove(Point to)
        {
            var position = Position ?? throw new ArgumentNullException();

            var horizontal = to.X == position.X || (to.X == position.X + 1 || to.X == position.X - 1);
            var vertical = to.Y != position.Y;

            switch (Color)
            {
                case PieceColorEnum.White:
                    vertical = vertical && to.Y == to.Y + 1;
                    break;

                case PieceColorEnum.Black:
                    vertical = vertical && to.Y == to.Y - 1;
                    break;
            }

            return horizontal && vertical;
        }
    }
}
