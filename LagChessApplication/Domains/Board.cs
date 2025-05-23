using LagChessApplication.Domains.Chess;
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
        public Board(IPiece[] pieces, Func<PieceTypeEnum>? onPawnPromotion = null)
        {
            Pieces = pieces;

            if (onPawnPromotion is not null)
            {
                OnPawnPromotion += onPawnPromotion;
                _pawnPromotion = OnPawnPromotion.Invoke();
            }
        }

        public event Func<PieceTypeEnum>? OnPawnPromotion;

        private bool _capturedPiece;
        private PieceTypeEnum? _pawnPromotion;

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

            return new Board(pieces, onPawnPromotion);
        }

        public IPiece GetPiece(Point from) => AvailablePieces.FirstOrDefault(x => x.Position == from) ?? throw PieceNotFoundException.Create(from);
        public IPiece? GetTryPiece(Point from) => AvailablePieces.FirstOrDefault(x => x.Position == from);

        public ChessMove MovePiece(Square from, Square to) => MovePiece(from.Point, to.Point);

        public ChessMove MovePiece(Point from, Point to)
        {
            var piece = GetPiece(from);

            if (!piece.IsValidMove(to))
                throw InvalidMoveException.Create(piece, to);

            if (IsPawnMovingDiagonallyInvalid(piece, to))
                throw InvalidMoveException.Create(piece, to);

            if (!IsPathClear(piece, to))
                throw InvalidMoveException.Create(piece, to);

            if (!CanPlacePiece(piece, to))
                throw InvalidMoveException.Create(piece, to);

            try
            {
                if (BoardExtension.ShouldPromotePawn(piece, to))
                {
                    ArgumentNullException.ThrowIfNull(OnPawnPromotion);
                    _pawnPromotion = OnPawnPromotion.Invoke();
                }

                var simulatedBoard = SimulatedBoard.CreateClone(this);

                var simulatedPiece = simulatedBoard.GetPiece(from);

                simulatedBoard.SetPiecePosition(simulatedPiece, to);

                if (MovePutsOwnKingInCheck(simulatedBoard, simulatedPiece))
                {
                    throw KingInCheckException.Create(piece, to);
                }

                SetPiecePosition(piece, to);

                var opponentKingIsCheck = MovePutsOpponentKingInCheck(this, piece);

                if (opponentKingIsCheck)
                {
                    // TO DO
                    MovePutsOpponentKingInCheckmate(simulatedBoard, simulatedPiece);
                }

                return ChessMove.Create(from, to, piece.Type, opponentKingIsCheck, _capturedPiece, _pawnPromotion);
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

        private bool CanPlacePiece(IPiece piece, Point to)
        {
            var occupiedPiece = GetTryPiece(to);

            return occupiedPiece is null || occupiedPiece.Color != piece.Color;
        }

        private bool MovePutsOwnKingInCheck(Board board, IPiece piece)
        {
            var opponentColor = piece.Color == PieceColorEnum.White ? PieceColorEnum.Black : PieceColorEnum.White;

            var king = AvailablePieces.First(x => x is King && x.Color == piece.Color);

            var opponentPieces = AvailablePieces.Where(x => x.Color == opponentColor);

            return opponentPieces.Any(opponentPiece => opponentPiece.GetPossibleMovesAndAttacks().Any(point => point == king.Position) && board.IsPathClear(opponentPiece, king.Position));
        }

        private bool MovePutsOpponentKingInCheckmate(Board board, IPiece piece)
        {
            var opponentColor = piece.Color == PieceColorEnum.White ? PieceColorEnum.Black : PieceColorEnum.White;

            var opponentKing = board.AvailablePieces.First(x => x is King && x.Color == opponentColor);

            if (HasAnyLegalMoveToEscapeCheck(board, opponentKing) || CanAnyPieceCaptureThreat(board, opponentKing))
                return false;

            //TO DO
            return default; 
        }

        private bool CanAnyPieceCaptureThreat(Board board, IPiece king)
        {
            var opponentPieces = board.AvailablePieces.Where(x => x.Color != king.Color);

            var opponentPiecesAttack = opponentPieces.Select(piece => new
            {
                Attacks = piece.GetPossibleMovesAndAttacks(),
                Piece = piece
            })
            .Where(obj => obj.Attacks.Any(point => point == king.Position) && board.IsPathClear(obj.Piece, king.Position));

            var pieces = board.AvailablePieces.Where(x => x.Color == king.Color);

            var piecesAttack = pieces.Select(piece => new
            {
                Attacks = piece.GetPossibleMovesAndAttacks(),
                Piece = piece
            });

            return opponentPiecesAttack.All(opponentPieceAttack =>
            {
                var possibleMovesAttack = piecesAttack.Where(pieceAttack => pieceAttack.Attacks.Any(point => point == opponentPieceAttack.Piece.Position) && board.IsPathClear(pieceAttack.Piece, pieceAttack.Piece.Position));

                if (!possibleMovesAttack.Any())
                    return false;

                return possibleMovesAttack.Any(pieceAttack =>
                {
                    var simulatedBoard = SimulatedBoard.CreateClone(board);

                    var simulatedPieceAttack = simulatedBoard.GetPiece(pieceAttack.Piece.Position);

                    simulatedBoard.SetPiecePosition(simulatedPieceAttack, opponentPieceAttack.Piece.Position);

                    return MovePutsOwnKingInCheck(simulatedBoard, pieceAttack.Piece);
                });
            });
        }

        private bool HasAnyLegalMoveToEscapeCheck(Board board, IPiece king)
        {
            var possibleMoves = king.GetPossibleMoves().Where(point => !IsOccupied(point)); // Incluir peças que o Rei para atacar

            return possibleMoves.Any(move =>
            {
                var simulatedBoard = SimulatedBoard.CreateClone(board);

                var simulatedKing = simulatedBoard.GetPiece(king.Position);

                simulatedBoard.SetPiecePosition(simulatedKing, move);

                return MovePutsOwnKingInCheck(simulatedBoard, simulatedKing);
            });
        }

        private bool MovePutsOpponentKingInCheck(Board board, IPiece piece)
        {
            var opponentColor = piece.Color == PieceColorEnum.White ? PieceColorEnum.Black : PieceColorEnum.White;

            var opponentKing = AvailablePieces.First(x => x is King && x.Color == opponentColor);

            var pieces = AvailablePieces.Where(x => x.Color == piece.Color);

            return pieces.Any(x => x.GetPossibleMovesAndAttacks().Any(point => point == opponentKing.Position) && board.IsPathClear(x, opponentKing.Position));
        }

        private bool IsOccupied(Point point) => GetTryPiece(point) is not null;

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
            return piece is Pawn pawn && pawn.IsAttack(to) && (!IsOccupied(to) || GetPiece(to).Color == pawn.Color);
        }

        private void PromotePawn(Pawn pawn, PieceTypeEnum type)
        {
            ArgumentNullException.ThrowIfNull(pawn);

            var pawnIndex = Array.FindIndex(Pieces, piece => piece.Equals(pawn));

            if (pawnIndex == -1)
                throw new InvalidOperationException("Pawn not found at the given position.");

            Pieces[pawnIndex] = pawn.ConvertTo(type);
        }

        private void SetPiecePosition(IPiece piece, Point to)
        {
            var occupiedPiece = GetTryPiece(to);

            if (occupiedPiece is not null)
            {
                occupiedPiece.Kill();
                _capturedPiece = true;
            }

            piece.Move(to);

            if (BoardExtension.ShouldPromotePawn(piece))
            {
                var pawn = piece as Pawn;

                ArgumentNullException.ThrowIfNull(pawn);
                ArgumentNullException.ThrowIfNull(_pawnPromotion);

                PromotePawn(pawn, _pawnPromotion.Value);
            }
        }
    }
}
