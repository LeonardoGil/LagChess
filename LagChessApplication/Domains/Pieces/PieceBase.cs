using LagChessApplication.Domains.Enums;
using System.Drawing;

namespace LagChessApplication.Domains.Pieces
{
    internal abstract class PieceBase : IPiece
    {
        public Point? Position { get; private set; }
        public bool IsDead { get; private set; }

        public PieceColorEnum Color { get; init; }
        public PieceTypeEnum Type { get; init; }

        public void Kill()
        {
            Position = null;
            IsDead = true;
        }

        public abstract void Move();
    }
}
