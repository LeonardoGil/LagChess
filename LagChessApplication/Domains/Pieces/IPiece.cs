using LagChessApplication.Domains.Enums;
using System.Drawing;

namespace LagChessApplication.Domains.Pieces
{
    internal interface IPiece
    {
        Point Position { get; set; }

        PieceColorEnum Color { get; set; }
        PieceTypeEnum Type { get; set; }

        void Move();
        bool CanMove();
    }
}
