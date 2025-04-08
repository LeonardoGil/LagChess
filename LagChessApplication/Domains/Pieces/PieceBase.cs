using LagChessApplication.Domains.Enums;
using System.Drawing;

namespace LagChessApplication.Domains.Pieces
{
    public abstract class PieceBase(Point position, PieceColorEnum color) : IPiece
    {
        public Point? Position { get; private set; } = position;
        public bool IsDead { get; private set; }

        public PieceColorEnum Color { get; init; } = color;
        public PieceTypeEnum Type { get; init; }
        public PieceMoveStyleEnum MoveStyle { get; init; }

        public void Kill()
        {
            Position = null;
            IsDead = true;
        }

        public void Move(Point to) => Position = to;

        public abstract bool CanMove(Point to);
    }
}
