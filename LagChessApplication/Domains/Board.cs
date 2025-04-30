using LagChessApplication.Domains.Enums;
using LagChessApplication.Domains.Pieces;
using LagChessApplication.Exceptions;
using LagChessApplication.Extensions;
using LagChessApplication.Interfaces;
using System.Drawing;

namespace LagChessApplication.Domains
{
    public class Board : IDeepCloneable<Board>
    {
        #region Constructor
        public Board() { }

        internal Board(Func<PieceTypeEnum>? onPawnPromotion)
        {
            if (onPawnPromotion is not null)
            {
                OnPawnPromotion += onPawnPromotion;
                _pawnPromotion = OnPawnPromotion.Invoke();
            }
        }
        #endregion

        #region Properties
        public event Func<PieceTypeEnum>? OnPawnPromotion;
        private PieceTypeEnum? _pawnPromotion;

        public required Player White { get; set; }
        public required Player Black { get; set; }

        private IPiece[] Pieces { get => [.. White.Pieces, .. Black.Pieces]; }
        public IPiece[] AvailablePieces { get => Pieces.Where(x => !x.IsDead).ToArray(); }
        #endregion

        #region Methods
        public Board Clone() => new(_pawnPromotion.HasValue ? () => { return _pawnPromotion.Value; } : null)
        {
            White = White.Clone(),
            Black = Black.Clone()
        };

        internal IPiece? GetPiece(Point from) => AvailablePieces.FirstOrDefault(x => x.Position == from);

        public void MovePiece(Square from, Square to) => MovePiece(from.Point, to.Point);

        public void MovePiece(Point from, Point to)
        {
            var piece = GetPiece(from) ?? throw new Exception("The position does not contain any piece.");

            if (!piece.IsValidMove(to))
                throw MoveInvalidException.Create(piece, to);

            if (IsPawnMovingDiagonallyInvalid(piece, to))
                throw MoveInvalidException.Create(piece, to);

            if (!IsPathClear(piece, to))
                throw MoveInvalidException.Create(piece, to);

            if (!CanPlacePiece(piece, to))
                throw MoveInvalidException.Create(piece, to);

            try
            {
                if (BoardExtension.ShouldPromotePawn(piece, to))
                {
                    ArgumentNullException.ThrowIfNull(OnPawnPromotion);
                    _pawnPromotion = OnPawnPromotion.Invoke();
                }

                CheckIfMoveResultsInCheck(from, to);

                SetPiecePosition(piece, to);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _pawnPromotion = null;
            }
        }

        private bool CanPlacePiece(IPiece piece, Point to)
        {
            var occupiedPiece = GetPiece(to);

            return occupiedPiece is null || occupiedPiece.Color != piece.Color;
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

        private bool IsOccupied(Point point) => GetPiece(point) is not null;

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

        private bool IsPawnMovingDiagonallyInvalid(IPiece piece, Point to)
        {
            return piece is Pawn pawn && pawn.IsAttack(to) && (!IsOccupied(to) || GetPiece(to)?.Color == pawn.Color);
        }

        private void PromotePawn(Pawn pawn, PieceTypeEnum type)
        {
            ArgumentNullException.ThrowIfNull(pawn);

            var pieces = pawn.Color == PieceColorEnum.White ? White.Pieces : Black.Pieces;

            var pawnIndex = Array.FindIndex(pieces, piece => piece.Equals(pawn));

            if (pawnIndex == -1)
                throw new InvalidOperationException("Pawn not found at the given position.");

            pieces[pawnIndex] = pawn.ConvertTo(type);
        }

        private void SetPiecePosition(IPiece piece, Point to)
        {
            var occupiedPiece = GetPiece(to);

            occupiedPiece?.Kill();

            piece.Move(to);

            if (BoardExtension.ShouldPromotePawn(piece))
            {
                var pawn = piece as Pawn;

                ArgumentNullException.ThrowIfNull(pawn);
                ArgumentNullException.ThrowIfNull(_pawnPromotion);

                PromotePawn(pawn, _pawnPromotion.Value);
            }
        }
        #endregion
    }
}
