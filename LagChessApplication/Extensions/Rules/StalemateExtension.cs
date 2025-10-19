using LagChessApplication.Domains;
using LagChessApplication.Domains.Enums;
using LagChessApplication.Extensions.Boards;
using LagChessApplication.Extensions.Pieces;
using LagChessApplication.Interfaces;

namespace LagChessApplication.Extensions.Rules
{
    internal static class StalemateExtension
    {
        internal static bool IsStalemate(this Board board, IPiece piece)
        {
            var opponentColor = piece.Color == PieceColorEnum.White ? PieceColorEnum.Black : PieceColorEnum.White;

            var opponentPieces = board.AvailablePieces.Where(x => x.Color == opponentColor);

            foreach (var opponentPiece in opponentPieces)
            {
                var possibleMoves = opponentPiece.GetPossibleMovesAndAttacks();
                
                foreach (var move in possibleMoves)
                {
                    try
                    {
                        if (board.SimulatedMovePiecePutsOwnKingInCheck(opponentPiece.Position, move))
                            continue;

                        return false;
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }

            return true;
        }
    }
}
