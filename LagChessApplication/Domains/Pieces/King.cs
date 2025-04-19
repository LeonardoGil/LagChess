using LagChessApplication.Domains.Enums;
using LagChessApplication.Interfaces;
using System.Drawing;

namespace LagChessApplication.Domains.Pieces
{
    public class King : PieceBase, IPiece
    {
        public King(Point position, PieceColorEnum color) : base(position, color)
        {
            Type = PieceTypeEnum.King;
            MoveStyle = PieceMoveStyleEnum.OneAll;
        }

        public override bool IsValidMove(Point to)
        {
            var possibilities = new int[3] { -1, 0, 1 };

            var vertical = possibilities.Contains(Position.Y - to.Y);
            var horizontal = possibilities.Contains(Position.X - to.X);

            return vertical && horizontal;
        }
    }
}
