using LagChessApplication.Domains.Pieces;
using LagChessApplication.Extensions;
using LagChessApplication.Interfaces;

namespace LagChessApplication.Domains
{
    public class Player : IDeepCloneable<Player>
    {
        public required string Name { get; set; }

        public required IPiece[] Pieces { get; init; }

        public IPiece[] AvailablePieces { get => Pieces.Where(x => !x.IsDead).ToArray(); }

        public Player Clone()
        {
            return new Player
            {
                Name = Name,
                Pieces = AvailablePieces.Select(x => x.Clone()).ToArray()
            };
        }
    }
}
