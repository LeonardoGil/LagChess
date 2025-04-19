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

        public override bool IsValidMove(Point to)
        {
            var horizontal = to.X == Position.X || (to.X == Position.X + 1 || to.X == Position.X - 1);
            var vertical = to.Y != Position.Y;

            switch (Color)
            {
                case PieceColorEnum.White:
                    vertical = vertical && to.Y == Position.Y + 1;
                    break;

                case PieceColorEnum.Black:
                    vertical = vertical && to.Y == Position.Y - 1;
                    break;
            }

            return horizontal && vertical;
        }
    }
}
