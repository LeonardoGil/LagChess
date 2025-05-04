using LagChessApplication.Interfaces;

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


    }
}
