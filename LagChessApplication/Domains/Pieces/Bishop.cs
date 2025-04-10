using LagChessApplication.Domains.Enums;
using System.Drawing;

namespace LagChessApplication.Domains.Pieces
{
    public class Bishop : PieceBase, IPiece
    {
        public Bishop(Point position, PieceColorEnum color) : base(position, color)
        {
            MoveStyle = PieceMoveStyleEnum.Diagonal;
            Type = PieceTypeEnum.Bishop;
        }

        public override bool IsValidMove(Point to)
        {
            int dx = Math.Abs(Position.X - to.X);
            int dy = Math.Abs(Position.Y - to.Y);

            return dx == dy;
        }
    }
}
