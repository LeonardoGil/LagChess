using LagChessApplication.Domains.Chess;
using LagChessApplication.Domains.Enums;
using LagChessApplication.Domains.Pieces;
using LagChessApplication.Exceptions;
using LagChessApplication.Extensions.Boards;
using LagChessApplication.Extensions.Pieces;
using LagChessApplication.Extensions.Rules;
using LagChessApplication.Interfaces;
using System.Drawing;

namespace LagChessApplication.Domains
{
    public class Board : IDeepCloneable<Board>
    {
        internal Board(IPiece[] pieces, Func<PieceTypeEnum> onPawnPromotion, ChessMove lastMove = default)
        {
            Pieces = pieces;

            _lastMove = lastMove;

            OnPawnPromotion += onPawnPromotion;
        }

        internal event Func<PieceTypeEnum> OnPawnPromotion;

        private bool _capturedPiece;
        private ChessMove _lastMove;
        private PieceTypeEnum? _pawnPromotion;

        public Board Clone()
        {
            var pieces = AvailablePieces.Select(x => x.Clone()).ToArray();

            PieceTypeEnum onPawnPromotion()
            {
                return TryGetPromotionValue(out var promotionValue) ? promotionValue : default;
            }

            return new Board(pieces, onPawnPromotion, _lastMove);
        }

        internal IPiece[] Pieces { get; }

        internal IPiece[] AvailablePieces { get => Pieces.Where(x => !x.IsDead).ToArray(); }

        public IPiece GetPiece(Point from) => AvailablePieces.FirstOrDefault(x => x.Position == from) ?? throw PieceNotFoundException.Create(from);

        public IPiece? GetTryPiece(Point from) => AvailablePieces.FirstOrDefault(x => x.Position == from);

        internal ChessMove MovePiece(Point from, Point to)
        {
            var piece = GetPiece(from);

            ValidateMove(piece, to);

            if (piece.ShouldPromotePawn(to))
            {
                _pawnPromotion = OnPawnPromotion.Invoke();
            }

            if (this.SimulatedMovePiecePutsOwnKingInCheck(from, to))
                throw KingInCheckException.Create(piece, to);

            try
            {
                SetPiece(piece, to);

                var opponentIsCheck = this.MovePutsOpponentKingInCheck(piece);

                var opponentIsCheckmated = opponentIsCheck && this.MovePutsOpponentKingInCheckmate(piece);

                return _lastMove = ChessMove.Create(from, to, piece.Type, opponentIsCheck, opponentIsCheckmated, _capturedPiece, _pawnPromotion);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _pawnPromotion = default;
                _capturedPiece = default;
            }
        }

        internal bool TryGetPromotionValue(out PieceTypeEnum promotionValue)
        {
            promotionValue = _pawnPromotion.GetValueOrDefault();

            return _pawnPromotion.HasValue;
        }

        internal void ValidateMove(IPiece piece, Point to)
        {
            if (!piece.IsValidMove(to))
            {
                var king = piece as King;

                if (king is not null && king.IsCastlingMove(this, to))
                {
                    king.ValidateCastlingMove(this, to);
                }
                else
                    throw InvalidMoveException.Create(piece, to);
            }

            if (!this.IsPathClear(piece, to))
                throw InvalidMoveException.Create(piece, to);

            if (!this.CanPlacePiece(piece, to))
                throw InvalidMoveException.Create(piece, to);

            if (piece is Pawn pawn && !pawn.IsMovingValid(this, to, _lastMove))
                throw InvalidMoveException.Create(piece, to);
        }

        internal void SetPiece(IPiece piece, Point to)
        {
            if (piece.IsCastlingMove(this, to))
            {
                this.SetCastlingPositions(piece, to);
            }
            else
            {
                if (piece is Pawn pawn)
                {
                    this.SetPiecePosition(pawn, to, _lastMove);
                }
                else
                {
                    this.SetPiecePosition(piece, to);
                }
            }
        }

        internal void CapturePieceAt(IPiece target)
        {
            target.Kill();
            _capturedPiece = true;
        }

        internal void TryCapturePieceAt(IPiece? target)
        {
            if (target is null)
            {
                _capturedPiece = false;
                return;
            }

            CapturePieceAt(target);
        }
    }
}