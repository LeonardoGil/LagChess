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
        public Board(Player white, Player black) => Instance(white, black);
        
        internal Board(Player white, Player black, Func<PieceTypeEnum>? onPawnPromotion)
        {
            Instance(white, black);

            OnPawnPromotion += onPawnPromotion;
            _pawnPromotion = OnPawnPromotion?.Invoke();
        }

        public Board Clone() => new(White.Clone(), Black.Clone(), _pawnPromotion.HasValue ? () => { return _pawnPromotion.Value; } : null);
        
        private void Instance(Player white, Player black)
        {
            ArgumentNullException.ThrowIfNull(white);
            ArgumentNullException.ThrowIfNull(black);

            White = white;
            Black = black;
        }
        #endregion

        #region Properties
        public event Func<PieceTypeEnum> OnPawnPromotion;
        private PieceTypeEnum? _pawnPromotion;

        public Player White { get; private set; }
        public Player Black { get; private set; }

        private IPiece[] Pieces { get => [.. White.Pieces, .. Black.Pieces]; }
        public IPiece[] AvailablePieces { get => Pieces.Where(x => !x.IsDead).ToArray(); }
        #endregion

        #region Methods
        public IPiece? GetPiece(Point from) => AvailablePieces.FirstOrDefault(x => x.Position == from);

        public void MovePiece(Square from, Square to) => MovePiece(from.Point, to.Point);

        public void MovePiece(Point from, Point to)
        {
            var piece = GetPiece(from) ?? throw new Exception("The position does not contain any piece.");

            if (!piece.IsValidMove(to) || !IsPathClear(piece, to) || !CanPlacePiece(piece, to))
            {
                throw MoveInvalidException.Create(piece, to);
            }

            try
            {
                if (BoardExtension.ShouldPromotePawn(piece, to))
                {
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

            return occupiedPiece is null || (piece is not Pawn || (piece.Position, to).ConvertToMoveStyleEnum() == PieceMoveStyleEnum.Diagonal) && piece.Color != occupiedPiece.Color;
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
