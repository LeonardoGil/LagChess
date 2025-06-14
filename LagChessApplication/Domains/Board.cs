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
        internal Board(IPiece[] pieces, Pawn? anPassantTarget = null, Func<PieceTypeEnum>? onPawnPromotion = null)
        {
            Pieces = pieces;

            _anPassantTarget = anPassantTarget;

            if (onPawnPromotion is not null)
            {
                OnPawnPromotion += onPawnPromotion;
                _pawnPromotion = OnPawnPromotion.Invoke();
            }
        }

        public event Func<PieceTypeEnum>? OnPawnPromotion;

        internal bool _capturedPiece;
        internal Pawn? _anPassantTarget;
        internal PieceTypeEnum? _pawnPromotion;

        public IPiece[] Pieces { get; }

        public IPiece[] AvailablePieces { get => Pieces.Where(x => !x.IsDead).ToArray(); }

        public Board Clone()
        {
            var pieces = AvailablePieces.Select(x => x.Clone()).ToArray();

            var onPawnPromotion = default(Func<PieceTypeEnum>?);

            if (_pawnPromotion.HasValue)
            {
                onPawnPromotion = () => { return _pawnPromotion.Value; };
            }

            return new Board(pieces, _anPassantTarget, onPawnPromotion);
        }

        public IPiece GetPiece(Point from) => AvailablePieces.FirstOrDefault(x => x.Position == from) ?? throw PieceNotFoundException.Create(from);

        public IPiece? GetTryPiece(Point from) => AvailablePieces.FirstOrDefault(x => x.Position == from);

        public ChessMove MovePiece(Square from, Square to) => MovePiece(from.Point, to.Point);

        public ChessMove MovePiece(Point from, Point to)
        {
            var piece = GetPiece(from);

            if (!this.IsValidMove(piece, to))
            {
                var isCastling = piece is King king && king.IsCastlingMoveValid(this, to);

                if (!isCastling)
                {
                    throw InvalidMoveException.Create(piece, to);
                }
            }

            if (piece.ShouldPromotePawn(to))
            {
                ArgumentNullException.ThrowIfNull(OnPawnPromotion);
                _pawnPromotion = OnPawnPromotion.Invoke();
            }

            if (this.SimulatedMovePiecePutsOwnKingInCheck(from, to))
            {
                throw KingInCheckException.Create(piece, to);
            }

            try
            {
                SetPiece(piece, to);

                var opponentIsCheck = this.MovePutsOpponentKingInCheck(piece);

                var opponentIsCheckmated = opponentIsCheck && this.MovePutsOpponentKingInCheckmate(piece);

                return ChessMove.Create(from, to, piece.Type, opponentIsCheck, opponentIsCheckmated, _capturedPiece, _pawnPromotion);
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

        internal void SetPiece(IPiece piece, Point to)
        {
            if (piece.IsCastlingMove(this, to))
            {
                this.SetCastlingPositions(piece, to);
            }
            else
            {
                this.SetPiecePosition(piece, to);
            }
        }
    }
}