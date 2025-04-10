using LagChessApplication.Domains.Enums;
using System.Drawing;

namespace LagChessApplication.Domains.Pieces
{
    public class Knight : PieceBase, IPiece
    {
        public Knight(Point position, PieceColorEnum color) : base(position, color)
        {
            MoveStyle = PieceMoveStyleEnum.LShaped;
            Type = PieceTypeEnum.Knight;
        }

        public override bool IsValidMove(Point to)
        {
            int dx = Math.Abs(Position.X - to.X);
            int dy = Math.Abs(Position.Y - to.Y);

            return (dx == 2 && dy == 1) || (dx == 1 && dy == 2);
        }
    }
}
