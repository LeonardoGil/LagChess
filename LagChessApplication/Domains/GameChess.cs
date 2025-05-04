using LagChessApplication.Domains.Enums;
using LagChessApplication.Exceptions;
using LagChessApplication.Interfaces;
using System.Drawing;

namespace LagChessApplication.Domains
{
    public class GameChess
    {
        public GameChess(Player white, Player black)
        {
            ArgumentNullException.ThrowIfNull(white);
            ArgumentNullException.ThrowIfNull(black);

            White = white;
            Black = black;

            IPiece[] pieces = [..White.Pieces, ..Black.Pieces];
            
            Board = new(pieces);
        }

        public Board Board { get; init; }
        public Player White { get; init; }
        public Player Black { get; init; }

        public int Turn { get; private set; } = 1;
        public PieceColorEnum TurnPlayer { get; private set; }

        public void Play(Point from, Point to)
        {
            if (!IsMoveFromCurrentPlayer(from))
                throw InvalidPieceOwnershipException.Create(Board.GetPiece(from), TurnPlayer);

            Board.MovePiece(from, to);

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
