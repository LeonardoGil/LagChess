using LagChessApplication.Domains.Enums;
using LagChessApplication.Interfaces;
using System.Drawing;

namespace LagChessApplication.Domains.Pieces
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
