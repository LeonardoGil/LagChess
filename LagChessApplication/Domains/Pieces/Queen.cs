using LagChessApplication.Domains.Enums;
using System.Drawing;

namespace LagChessApplication.Domains.Pieces
{
    public class Queen : PieceBase
    {
        public Queen(Point position, PieceColorEnum color) : base(position, color)
        {
            MoveStyle = PieceMoveStyleEnum.All;
            Type = PieceTypeEnum.Queen;
        }

        public override bool IsValidMove(Point to)
        {
            int dx = Math.Abs(Position.X - to.X);
            int dy = Math.Abs(Position.Y - to.Y);

            return dx == 0 || dy == 0 || dx == dy;
        }
    }
}
