using LagChessApplication.Domains.Enums;
using System.Drawing;

namespace LagChessApplication.Interfaces
{
    public interface IPiece : IDeepCloneable<IPiece>
    {
        Point Position { get; }
        bool IsDead { get; }

        PieceColorEnum Color { get; }
        PieceTypeEnum Type { get; }
        PieceMoveStyleEnum MoveStyle { get; }

        void Kill();
        void Move(Point to);
        bool IsValidMove(Point to);
    }
}
