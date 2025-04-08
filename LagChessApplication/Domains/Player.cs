using LagChessApplication.Domains.Pieces;

namespace LagChessApplication.Domains
{
    public class Player
    {
        public string Name { get; set; }

        public IPiece[] Pieces { get; init; }
    }
}
