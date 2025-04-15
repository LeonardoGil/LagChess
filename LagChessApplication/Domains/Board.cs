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

            CheckIfMoveResultsInCheck(from, to);

            SetPiecePosition(piece, to);
        }

        private void CheckIfMoveResultsInCheck(Point from, Point to)
        {
            var simulatedBoard = SimulatedBoard.CreateClone(this);

            var piece = simulatedBoard.GetPiece(from) ?? throw new Exception($"No piece found at position {from}.");

            simulatedBoard.SetPiecePosition(piece, to);

            var opponentColor = piece.Color == PieceColorEnum.White ? PieceColorEnum.Black : PieceColorEnum.White;

            var king = Pieces.First(x => x is King && x.Color == piece.Color);

            foreach (var opponentPiece in Pieces.Where(x => x.Color == opponentColor && !x.IsDead))
            {
                var possibleMoves = opponentPiece.GetPossibleMoves(opponentPiece.MoveStyle).Any(point => point == king.Position);

                if (possibleMoves && simulatedBoard.IsPathClear(opponentPiece, king.Position))
                {
                    throw KingInCheckException.Create(piece, to);
                }

                if (opponentPiece is Pawn)
                {
                    var direction = piece.Color == PieceColorEnum.White ? 1 : -1;

                    if ((king.Position.X == opponentPiece.Position.X - 1 || king.Position.X == opponentPiece.Position.X + 1) && king.Position.Y == opponentPiece.Position.Y + direction)
                    {
                        throw KingInCheckException.Create(piece, to);
                    }
                }
            }
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

        public static bool IsInBoard(Point position) => IsInBoard(position.X, position.Y);
        public static bool IsInBoard(int x, int y) => x is >= 1 and <= 8 && y is >= 1 and <= 8;
    }
}
