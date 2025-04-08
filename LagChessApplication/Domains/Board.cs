using LagChessApplication.Domains.Enums;
using LagChessApplication.Domains.Pieces;
using LagChessApplication.Exceptions;
using System.Drawing;

namespace LagChessApplication.Domains
{
    public class Board(Player white, Player black)
    {
        public Player White { get; init; } = white;
        public Player Black { get; init; } = black;

        public IPiece[] Pieces { get => [.. White.Pieces, .. Black.Pieces]; }

        public void MovePiece(IPiece piece, Point to)
        {
            if (!piece.IsValidMove(to) || !IsPathClear(piece, to))
            {
                throw MoveInvalidException.Create(piece, to);
            }

            SetPiecePosition(piece, to);
        }

        private void SetPiecePosition(IPiece piece, Point to)
        {
            var isOccupied = IsOccupied(to);

            if (isOccupied)
            {
                var occupiedPiece = Pieces.First(x => x.Position == to);

                if (piece.Color == occupiedPiece.Color || piece is Pawn)
                {
                    throw MoveInvalidException.Create(piece, to);
                }

                occupiedPiece.Kill();
            }

            piece.Move(to);
        }

        private bool IsPathClear(IPiece piece, Point to)
        {
            var from = piece.Position ?? throw new Exception("Piece is dead");
            var moveStyle = (from, to).ConvertToMoveStyleEnum();
            
            var directionX = Math.Sign(to.X - from.X);
            var directionY = Math.Sign(to.Y - from.Y);

            switch (moveStyle)
            {
                case PieceMoveStyleEnum.Linear:
                case PieceMoveStyleEnum.Diagonal:
                    var current = new Point(from.X + directionX, from.Y + directionY);

                    while (current != to)
                    {
                        if (IsOccupied(current))
                            return false;

                        current = new Point(current.X + directionX, current.Y + directionY);
                    }

                    return true;

                case PieceMoveStyleEnum.LShaped:
                    return true;

                default:
                    throw new NotSupportedException("Unknown movement style");
            }
        }

        private bool IsOccupied(Point point)
        {
            return Pieces.Any(p => p.Position == point);
        }
    }
}
