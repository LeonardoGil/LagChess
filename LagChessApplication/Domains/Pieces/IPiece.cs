using LagChessApplication.Domains.Enums;
using System.Drawing;

namespace LagChessApplication.Domains.Pieces
{
    public interface IPiece
    {
        Point? Position { get; }
        bool IsDead { get; }

        PieceColorEnum Color { get; }
        PieceTypeEnum Type { get; }
        PieceMoveStyleEnum MoveStyle { get; }

        void Kill();
        void Move(Point to);
        bool CanMove(Point to);
    }
}
