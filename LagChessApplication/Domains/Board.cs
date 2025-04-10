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

        private IPiece[] Pieces { get => [.. White.Pieces, .. Black.Pieces]; }
        public IPiece[] AvailablePieces { get => Pieces.Where(x => !x.IsDead).ToArray(); }

        public IPiece? GetPiece(Point from) => AvailablePieces.FirstOrDefault(x => x.Position == from);

        private bool IsOccupied(Point point) => GetPiece(point) is not null;

        public void MovePiece(Point from, Point to)
        {
            var piece = GetPiece(from) ?? throw new Exception("The position does not contain any piece.");

            if (!piece.IsValidMove(to) || !IsPathClear(piece, to) || !CanPlacePiece(piece, to))
            {
                throw MoveInvalidException.Create(piece, to);
            }

            SetPiecePosition(piece, to);
        }

        private void SetPiecePosition(IPiece piece, Point to)
        {
            var occupiedPiece = GetPiece(to);

            occupiedPiece?.Kill();

            piece.Move(to);
        }

        private bool CanPlacePiece(IPiece piece, Point to)
        {
            var occupiedPiece = GetPiece(to);

            return occupiedPiece is null || (piece is not Pawn || (piece.Position, to).ConvertToMoveStyleEnum() == PieceMoveStyleEnum.Diagonal) && piece.Color != occupiedPiece.Color;
        }

        private bool IsPathClear(IPiece piece, Point to)
        {
            var from = piece.Position;
            var moveStyle = (from, to).ConvertToMoveStyleEnum();

            var directionX = Math.Sign(to.X - from.X);
            var directionY = Math.Sign(to.Y - from.Y);

            switch (moveStyle)
            {
                case PieceMoveStyleEnum.Straight:
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

        public Board Clone()
        {
            return new Board(White.Clone(), Black.Clone());
        }

        public static Board Create(string white = nameof(White), string black = nameof(Black)) => new(Player.CreateWhite(white), Player.CreateBlack(black));
    }
}
