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

        public static Player CreateWhite(string name)
        {
            return new Player
            {
                Name = name,
                Pieces =
                [
                    PieceExtension.CreatePieceWhite<Rook>(1, 1),
                    PieceExtension.CreatePieceWhite<Knight>(2, 1),
                    PieceExtension.CreatePieceWhite<Bishop>(3, 1),
                    PieceExtension.CreatePieceWhite<Queen>(4, 1),
                    PieceExtension.CreatePieceWhite<King>(5, 1),
                    PieceExtension.CreatePieceWhite<Bishop>(6, 1),
                    PieceExtension.CreatePieceWhite<Knight>(7, 1),
                    PieceExtension.CreatePieceWhite<Rook>(8, 1),

                    PieceExtension.CreatePieceWhite<Pawn>(1, 2),
                    PieceExtension.CreatePieceWhite<Pawn>(2, 2),
                    PieceExtension.CreatePieceWhite<Pawn>(3, 2),
                    PieceExtension.CreatePieceWhite<Pawn>(4, 2),
                    PieceExtension.CreatePieceWhite<Pawn>(5, 2),
                    PieceExtension.CreatePieceWhite<Pawn>(6, 2),
                    PieceExtension.CreatePieceWhite<Pawn>(7, 2),
                    PieceExtension.CreatePieceWhite<Pawn>(8, 2)
                ]
            };
        }
        public static Player CreateBlack(string name)
        {
            return new Player
            {
                Name = name,
                Pieces =
                [
                    PieceExtension.CreatePieceBlack<Rook>(1, 8),
                    PieceExtension.CreatePieceBlack<Knight>(2, 8),
                    PieceExtension.CreatePieceBlack<Bishop>(3, 8),
                    PieceExtension.CreatePieceBlack<Queen>(4, 8),
                    PieceExtension.CreatePieceBlack<King>(5, 8),
                    PieceExtension.CreatePieceBlack<Bishop>(6, 8),
                    PieceExtension.CreatePieceBlack<Knight>(7, 8),
                    PieceExtension.CreatePieceBlack<Rook>(8, 8),

                    PieceExtension.CreatePieceBlack<Pawn>(1, 7),
                    PieceExtension.CreatePieceBlack<Pawn>(2, 7),
                    PieceExtension.CreatePieceBlack<Pawn>(3, 7),
                    PieceExtension.CreatePieceBlack<Pawn>(4, 7),
                    PieceExtension.CreatePieceBlack<Pawn>(5, 7),
                    PieceExtension.CreatePieceBlack<Pawn>(6, 7),
                    PieceExtension.CreatePieceBlack<Pawn>(7, 7),
                    PieceExtension.CreatePieceBlack<Pawn>(8, 7)
                ]
            };
        }
    }
}
