using LagChessApplication.Domains.Enums;
using LagChessApplication.Domains.Pieces;
using LagChessApplication.Exceptions;
using System.Drawing;

namespace LagChessApplication.Domains
{
    internal class Board(Player white, Player black)
    {
        public Player White { get; init; } = white;
        public Player Black { get; init; } = black;

        public IPiece[] Pieces { get => [.. White.Pieces, .. Black.Pieces]; }

        public void MovePiece(IPiece piece, Point to)
        {
            if (piece.CanMove(to))
                throw MoveInvalidException.Create(piece, to);

            if (IsPathClear(piece, to))
                throw new NotImplementedException();
        }

        private bool IsPathClear(IPiece piece, Point to)
        {
            var moveStyle = (piece.Position.Value, to).ConvertToMoveStyleEnum();

            throw new NotImplementedException();
        }
    }
}
