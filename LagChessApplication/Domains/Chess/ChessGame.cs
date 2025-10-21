using LagChessApplication.Domains.Enums;
using LagChessApplication.Exceptions;
using LagChessApplication.Extensions.Rules;
using LagChessApplication.Interfaces;
using System.Drawing;

namespace LagChessApplication.Domains.Chess
{
    public class ChessGame
    {
        public ChessGame(Player white, Player black, Func<PieceTypeEnum> onPawnPromotion)
        {
            ArgumentNullException.ThrowIfNull(white);
            ArgumentNullException.ThrowIfNull(black);
            ArgumentNullException.ThrowIfNull(onPawnPromotion);

            White = white;
            Black = black;

            IPiece[] pieces = [.. White.Pieces, .. Black.Pieces];

            Board = new(pieces, onPawnPromotion);
            History = new();
        }

        public Player White { get; init; }
        public Player Black { get; init; }
        public Player? Winner { get; private set; }

        public Board Board { get; init; }
        public ChessHistory History { get; private set; }

        public int Turn { get; private set; } = 1;
        public PieceColorEnum TurnPlayer { get; private set; }
        public GameStatusEnum GameStatus { get; private set; }

        public ChessMove Play(Point from, Point to)
        {
            if (GameStatus != GameStatusEnum.InProgress)
                throw new InvalidOperationException("The game has already ended.");

            if (!IsMoveFromCurrentPlayer(from))
                throw InvalidPieceOwnershipException.Create(Board.GetPiece(from), TurnPlayer);

            var move = Board.MovePiece(from, to);

            History.Add(move);

            EvaluateGameState();

            if (GameStatus == GameStatusEnum.InProgress)
                NextTurn();

            return move;
        }

        private void EvaluateGameState()
        {
            var opponentColor = TurnPlayer == PieceColorEnum.White ? PieceColorEnum.Black : PieceColorEnum.White;
            var lastMove = History.LastMove;

            if (lastMove.OpponentKingInCheckMate)
            {
                GameStatus = GameStatusEnum.Checkmate;
                Winner = TurnPlayer == PieceColorEnum.White ? White : Black;
                return;
            }

            if (Board.IsStalemate(opponentColor))
            {
                GameStatus = GameStatusEnum.Stalemate;
                Winner = null;
                return;
            }
        }

        private void NextTurn()
        {
            if (TurnPlayer == PieceColorEnum.White)
            {
                TurnPlayer = PieceColorEnum.Black;
            }
            else
            {
                TurnPlayer = PieceColorEnum.White;
                Turn++;
            }
        }

        private bool IsMoveFromCurrentPlayer(Point from) => Board.GetPiece(from).Color == TurnPlayer;
    }
}
