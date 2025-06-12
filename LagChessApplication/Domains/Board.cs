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
            
        private bool _capturedPiece;
        private Pawn? _anPassantTarget;
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

            return new Board(pieces, _anPassantTarget, onPawnPromotion);
        }

        public IPiece GetPiece(Point from) => AvailablePieces.FirstOrDefault(x => x.Position == from) ?? throw PieceNotFoundException.Create(from);
        public IPiece? GetTryPiece(Point from) => AvailablePieces.FirstOrDefault(x => x.Position == from);

        public ChessMove MovePiece(Square from, Square to) => MovePiece(from.Point, to.Point);

        public ChessMove MovePiece(Point from, Point to)
        {
            var piece = GetPiece(from);

            if (!IsValidMove(piece, to))
                throw InvalidMoveException.Create(piece, to);

            try
            {
                if (BoardExtension.ShouldPromotePawn(piece, to))
                {
                    ArgumentNullException.ThrowIfNull(OnPawnPromotion);
                    _pawnPromotion = OnPawnPromotion.Invoke();
                }

                var simulatedBoard = SimulatedBoardExtension.CreateClone(this).SimulatedMovePiece(from, to);

                var simulatedPiece = simulatedBoard.GetPiece(to);

                if (MovePutsOwnKingInCheck(simulatedBoard, simulatedPiece))
                {
                    throw KingInCheckException.Create(piece, to);
                }

                SetPiecePosition(piece, to);

                var opponentIsCheck = MovePutsOpponentKingInCheck(this, piece);

                var opponentIsCheckmated = opponentIsCheck && MovePutsOpponentKingInCheckmate(this, piece);

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

        private bool CanPlacePiece(IPiece piece, Point to)
        {
            var occupiedPiece = GetTryPiece(to);

            return occupiedPiece is null || occupiedPiece.Color != piece.Color;
        }

        private static bool MovePutsOwnKingInCheck(Board board, IPiece piece)
        {
            var opponentColor = piece.Color == PieceColorEnum.White ? PieceColorEnum.Black : PieceColorEnum.White;

            var king = board.AvailablePieces.First(x => x is King && x.Color == piece.Color);

            var opponentPieces = board.AvailablePieces.Where(x => x.Color == opponentColor);

            return opponentPieces.Any(opponentPiece => opponentPiece.GetPossibleMovesAndAttacks().Any(point => point == king.Position) && board.IsPathClear(opponentPiece, king.Position));
        }

        private bool MovePutsOpponentKingInCheckmate(Board board, IPiece piece)
        {
            var opponentColor = piece.Color == PieceColorEnum.White ? PieceColorEnum.Black : PieceColorEnum.White;

            var opponentKing = board.AvailablePieces.First(x => x is King && x.Color == opponentColor);

            return !HasAnyLegalMoveToEscapeCheck(board, opponentKing) && !CanAnyPieceCaptureThreat(board, opponentKing) && !CanAnyPieceBlockThreat(board, opponentKing);
        }

        private bool CanAnyPieceBlockThreat(Board board, IPiece king)
        {
            var friendlyPieces = board.AvailablePieces.Where(piece => piece.Color == king.Color && piece is not King);

            var opponentAttackers = board.AvailablePieces.Where(p => p.Color != king.Color).Where(p => p.GetPossibleMovesAndAttacks().Contains(king.Position) && board.IsPathClear(p, king.Position));

            foreach (var attacker in opponentAttackers)
            {
                var blockableSquares = GetBlockingSquares(king.Position, attacker.Position, attacker);

                foreach (var piece in friendlyPieces)
                {
                    var possibleMoves = piece.GetPossibleMoves().Where(dest => blockableSquares.Contains(dest));

                    foreach (var move in possibleMoves)
                    {
                        try
                        {
                            var simulatedBoard = SimulatedBoardExtension.CreateClone(board).SimulatedMovePiece(piece.Position, move);

                            var simulatedKing = simulatedBoard.GetPiece(king.Position);

                            if (!MovePutsOwnKingInCheck(simulatedBoard, simulatedKing))
                                return true;
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
            }

            return false;
        }

        private bool CanAnyPieceCaptureThreat(Board board, IPiece king)
        {
            var opponentAttackers = board.AvailablePieces.Where(p => p.Color != king.Color)
                                                         .Where(p => p.GetPossibleMovesAndAttacks().Contains(king.Position) && board.IsPathClear(p, king.Position));

            // Se houver mais de um atacante, não há necessidade de validar
            if (opponentAttackers.Count() != 1)
                return false;

            var attacker = opponentAttackers.First();

            var friendlyPieces = board.AvailablePieces.Where(piece => piece.Color == king.Color && piece is not King);

            foreach (var piece in friendlyPieces)
            {
                var attackMoves = piece.GetPossibleMovesAndAttacks();

                if (!attackMoves.Contains(attacker.Position))
                    continue;

                if (!board.IsPathClear(piece, attacker.Position))
                    continue;

                try
                {
                    var simulatedBoard = SimulatedBoardExtension.CreateClone(board).SimulatedMovePiece(piece.Position, attacker.Position);
                    
                    var simulatedKing = simulatedBoard.GetPiece(king.Position);

                    if (!MovePutsOwnKingInCheck(simulatedBoard, simulatedKing))
                        return true;
                }
                catch
                {
                    continue;
                }
            }

            return false;
        }

        private bool HasAnyLegalMoveToEscapeCheck(Board board, IPiece king)
        {
            var possibleMoves = king.GetPossibleMoves().Where(point =>
            {
                var piece = board.GetTryPiece(point);

                return piece is null || piece.Color != king.Color;
            });

            return possibleMoves.Any(move =>
            {
                try
                {
                    var simulatedBoard = SimulatedBoardExtension.CreateClone(board).SimulatedMovePiece(king.Position, move);

                    var simulatedKing = simulatedBoard.GetPiece(move);

                    return !MovePutsOwnKingInCheck(simulatedBoard, simulatedKing);
                }
                catch
                {
                    return false;
                }
            });
        }

        private static bool MovePutsOpponentKingInCheck(Board board, IPiece piece)
        {
            var opponentColor = piece.Color == PieceColorEnum.White ? PieceColorEnum.Black : PieceColorEnum.White;

            var opponentKing = board.AvailablePieces.First(x => x is King && x.Color == opponentColor);

            var pieces = board.AvailablePieces.Where(x => x.Color == piece.Color);

            return pieces.Any(x => x.GetPossibleMovesAndAttacks().Any(point => point == opponentKing.Position) && board.IsPathClear(x, opponentKing.Position));
        }

        internal bool IsOccupied(Point point) => GetTryPiece(point) is not null;

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

        internal bool IsValidMove(IPiece piece, Point to)
        {
            return piece.IsValidMove(to) && IsPathClear(piece, to) && CanPlacePiece(piece, to) && !(piece is Pawn pawn && PawnMoveExtension.IsMovingInvalid(this, pawn, to, _anPassantTarget));

        }

        private void PromotePawn(Pawn pawn, PieceTypeEnum type)
        {
            ArgumentNullException.ThrowIfNull(pawn);

            var pawnIndex = Array.FindIndex(Pieces, piece => piece.Equals(pawn));

            if (pawnIndex == -1)
                throw new InvalidOperationException("Pawn not found at the given position.");

            Pieces[pawnIndex] = pawn.ConvertTo(type);
        }

        internal void SetPiecePosition(IPiece piece, Point to)
        {
            var occupiedPiece = GetTryPiece(to);

            if (occupiedPiece is not null)
            {
                occupiedPiece.Kill();
                _capturedPiece = true;
            }

            var pawn = piece as Pawn;

            _anPassantTarget = pawn is not null && pawn.IsDoubleStepMove(to) ? pawn : default;

            piece.Move(to);

            if (BoardExtension.ShouldPromotePawn(piece))
            {
                ArgumentNullException.ThrowIfNull(pawn);
                ArgumentNullException.ThrowIfNull(_pawnPromotion);

                PromotePawn(pawn, _pawnPromotion.Value);
            }
        }

        private List<Point> GetBlockingSquares(Point kingPos, Point attackerPos, IPiece attacker)
        {
            var blockingSquares = new List<Point>();

            if (attacker is Knight)
            {
                // Não dá pra bloquear cavalo — só capturar
                blockingSquares.Add(attackerPos);
                return blockingSquares;
            }

            var dx = Math.Sign(attackerPos.X - kingPos.X);
            var dy = Math.Sign(attackerPos.Y - kingPos.Y);

            var x = kingPos.X + dx;
            var y = kingPos.Y + dy;

            while (x != attackerPos.X || y != attackerPos.Y)
            {
                blockingSquares.Add(new Point(x, y));
                x += dx;
                y += dy;
            }

            // A última posição é o próprio atacante (captura)
            blockingSquares.Add(attackerPos);

            return blockingSquares;
        }
    }
}
