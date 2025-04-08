using LagChessApplication.Domains.Pieces;
using LagChessApplication.Interfaces;

namespace LagChessApplication.Domains
{
    public class Player : IDeepCloneable<Player>
    {
        public string Name { get; set; }

        public IPiece[] Pieces { get; init; }

        public Player Clone()
        {
            return new Player
            {
                Name = Name,
                Pieces = Pieces.Select(x => x.Clone()).ToArray()
            };
        }
    }
}
