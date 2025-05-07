using LagChessApplication.Domains.Enums;
using LagChessApplication.Exceptions;
using LagChessApplication.Interfaces;
using System.Drawing;

namespace LagChessApplication.Domains.Chess
{
    public class ChessGame
    {
        public ChessGame(Player white, Player black)
        {
            ArgumentNullException.ThrowIfNull(white);
            ArgumentNullException.ThrowIfNull(black);

            White = white;
            Black = black;

            IPiece[] pieces = [.. White.Pieces, .. Black.Pieces];

            Board = new(pieces);
            History = new();
        }

        public Board Board { get; init; }
        public Player White { get; init; }
        public Player Black { get; init; }

        public ChessHistory History { get; private set; }

        public int Turn { get; private set; } = 1;
        public PieceColorEnum TurnPlayer { get; private set; }

        public void Play(Point from, Point to)
        {
            if (!IsMoveFromCurrentPlayer(from))
                throw InvalidPieceOwnershipException.Create(Board.GetPiece(from), TurnPlayer);

            var move = Board.MovePiece(from, to);

            History.Add(move);

            NextTurn();
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
