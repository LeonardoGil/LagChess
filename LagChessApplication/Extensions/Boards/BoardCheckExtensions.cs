using LagChessApplication.Domains;
using LagChessApplication.Domains.Enums;
using LagChessApplication.Domains.Pieces;
using LagChessApplication.Extensions.Pieces;
using LagChessApplication.Interfaces;
using System.Drawing;

namespace LagChessApplication.Extensions.Boards
{
    internal static class BoardCheckExtensions
    {
        internal static bool MovePutsOwnKingInCheck(this Board board, IPiece piece)
        {
            var opponentColor = piece.Color == PieceColorEnum.White ? PieceColorEnum.Black : PieceColorEnum.White;

            var king = board.AvailablePieces.First(x => x is King && x.Color == piece.Color);

            var opponentPieces = board.AvailablePieces.Where(x => x.Color == opponentColor);

            return opponentPieces.Any(opponentPiece => opponentPiece.GetPossibleMovesAndAttacks().Any(point => point == king.Position) && board.IsPathClear(opponentPiece, king.Position));
        }

        internal static bool MovePutsOpponentKingInCheck(this Board board, IPiece piece)
        {
            var opponentColor = piece.Color == PieceColorEnum.White ? PieceColorEnum.Black : PieceColorEnum.White;

            var opponentKing = board.AvailablePieces.First(x => x is King && x.Color == opponentColor);

            var pieces = board.AvailablePieces.Where(x => x.Color == piece.Color);

            return pieces.Any(x => x.GetPossibleMovesAndAttacks().Any(point => point == opponentKing.Position) && board.IsPathClear(x, opponentKing.Position));
        }

        internal static bool MovePutsOpponentKingInCheckmate(this Board board, IPiece piece)
        {
            var opponentColor = piece.Color == PieceColorEnum.White ? PieceColorEnum.Black : PieceColorEnum.White;

            var opponentKing = board.AvailablePieces.First(x => x is King && x.Color == opponentColor);

            return !HasAnyLegalMoveToEscapeCheck(board, opponentKing) && !CanAnyPieceCaptureThreat(board, opponentKing) && !CanAnyPieceBlockThreat(board, opponentKing);
        }

        private static bool CanAnyPieceBlockThreat(this Board board, IPiece king)
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
                            if (!board.SimulatedMovePiecePutsOwnKingInCheck(piece.Position, move))
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

        private static bool CanAnyPieceCaptureThreat(this Board board, IPiece king)
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
                    if (!board.SimulatedMovePiecePutsOwnKingInCheck(piece.Position, attacker.Position))
                        return true;
                }
                catch
                {
                    continue;
                }
            }

            return false;
        }

        private static bool HasAnyLegalMoveToEscapeCheck(this Board board, IPiece king)
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
                    return !board.SimulatedMovePiecePutsOwnKingInCheck(king.Position, move);
                }
                catch
                {
                    return false;
                }
            });
        }

        private static List<Point> GetBlockingSquares(Point kingPos, Point attackerPos, IPiece attacker)
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