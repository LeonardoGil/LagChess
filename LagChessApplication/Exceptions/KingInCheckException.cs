using LagChessApplication.Interfaces;
using System.Drawing;

namespace LagChessApplication.Exceptions
{
    public class KingInCheckException(IPiece piece, Point to) : InvalidMoveException(piece, to)
    {
        public static new KingInCheckException Create(IPiece piece, Point to) => new(piece, to);
    }
}
