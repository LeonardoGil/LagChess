using LagChessApplication.Domains.Enums;
using LagChessApplication.Domains.Pieces;
using LagChessApplication.Exceptions;
using LagChessApplication.Interfaces;
using System.Drawing;

namespace LagChessApplication.Domains
{
    public class Board(Player white, Player black) : IDeepCloneable<Board>
    {
        public Player White { get; init; } = white;
        public Player Black { get; init; } = black;

        public IPiece[] Pieces { get => [.. White.Pieces, .. Black.Pieces]; }

        public IPiece GetPiece(Point from) => Pieces.FirstOrDefault(x => x.Position == from);

        public void MovePiece(Point from, Point to)
        {
            var piece = GetPiece(from);

            if (!piece.IsValidMove(to) || !IsPathClear(piece, to) || !CanPlacePiece(piece, to))
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

                occupiedPiece.Kill();
            }

            piece.Move(to);
        }

        private bool CanPlacePiece(IPiece piece, Point to)
        {
            if (IsOccupied(to))
            {
                var occupiedPiece = Pieces.First(x => x.Position == to);

                return piece is not Pawn && piece.Color != occupiedPiece.Color;
            }

            return true;
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

        public Board Clone()
        {
            return new Board(White.Clone(), Black.Clone());
        }
    }
}
