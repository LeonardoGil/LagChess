using LagChessApplication.Domains.Pieces;
using System.Drawing;

namespace LagChessApplication.Exceptions
{
    public class MoveInvalidException(IPiece piece, Point to) : Exception
    {
        public IPiece Piece { get; init; } = piece;
        public Point To { get; init; } = to;

        public static MoveInvalidException Create(IPiece piece, Point to)
        {
            return new MoveInvalidException(piece, to);
        }
    }
}
