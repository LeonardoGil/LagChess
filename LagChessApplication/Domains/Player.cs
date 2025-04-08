using LagChessApplication.Domains.Pieces;

namespace LagChessApplication.Domains
{
    internal class Player
    {
        public Player()
        {
            Pieces = [ new Pawn() ];
        }

        public string Name { get; set; }

        public IPiece[] Pieces { get; init; }
    }
}
