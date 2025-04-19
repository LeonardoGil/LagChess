using LagChessApplication.Domains.Enums;
using LagChessApplication.Interfaces;
using System.Drawing;

namespace LagChessApplication.Domains.Pieces
{
    public class Rook : PieceBase, IPiece
    {
        public Rook(Point position, PieceColorEnum color) : base(position, color)
        {
            Type = PieceTypeEnum.Rook;
            MoveStyle = PieceMoveStyleEnum.Straight;
        }

        public override bool IsValidMove(Point to)
        {
            var horizontal = Position.X != to.X && Position.Y == to.Y;
            var vertical = Position.X == to.X && Position.Y != to.Y;

            return horizontal || vertical;
        }
    }
}
