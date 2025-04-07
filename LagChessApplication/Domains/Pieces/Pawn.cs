using LagChessApplication.Domains.Enums;
using System.Drawing;

namespace LagChessApplication.Domains.Pieces
{
    internal class Pawn : IPiece
    {
        public Point? Position { get; private set; }
        public bool IsDead { get; private set; }
        
        public PieceColorEnum Color { get; init; }
        public PieceTypeEnum Type => PieceTypeEnum.Pawn;

        public bool CanMove()
        {
            throw new NotImplementedException();
        }

        public void Move()
        {
            throw new NotImplementedException();
        }

        public void Kill()
        {
            throw new NotImplementedException();
        }
    }
}
