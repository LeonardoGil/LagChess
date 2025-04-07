using LagChessApplication.Domains.Enums;
using System.Drawing;

namespace LagChessApplication.Domains.Pieces
{
    internal interface IPiece
    {
        Point? Position { get; }
        bool IsDead { get; }

        PieceColorEnum Color { get; }
        PieceTypeEnum Type { get; }

        void Kill();
        void Move();
    }
}
