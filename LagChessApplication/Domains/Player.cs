using LagChessApplication.Domains.Enums;
using LagChessApplication.Domains.Pieces;
using LagChessApplication.Interfaces;

namespace LagChessApplication.Domains
{
    public class Player : IDeepCloneable<Player>
    {
        public string Name { get; set; }

        public IPiece[] Pieces { get; init; }

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
                    PieceBase.CreatePieceWhite<Rook>(1, 1),
                    PieceBase.CreatePieceWhite<Knight>(2, 1),
                    PieceBase.CreatePieceWhite<Bishop>(3, 1),
                    PieceBase.CreatePieceWhite<Queen>(4, 1),
                    PieceBase.CreatePieceWhite<King>(5, 1),
                    PieceBase.CreatePieceWhite<Bishop>(6, 1),
                    PieceBase.CreatePieceWhite<Knight>(7, 1),
                    PieceBase.CreatePieceWhite<Rook>(8, 1),

                    PieceBase.CreatePieceWhite<Pawn>(1, 2),
                    PieceBase.CreatePieceWhite<Pawn>(2, 2),
                    PieceBase.CreatePieceWhite<Pawn>(3, 2),
                    PieceBase.CreatePieceWhite<Pawn>(4, 2),
                    PieceBase.CreatePieceWhite<Pawn>(5, 2),
                    PieceBase.CreatePieceWhite<Pawn>(6, 2),
                    PieceBase.CreatePieceWhite<Pawn>(7, 2),
                    PieceBase.CreatePieceWhite<Pawn>(8, 2)
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
                    PieceBase.CreatePieceBlack<Rook>(1, 8),
                    PieceBase.CreatePieceBlack<Knight>(2, 8),
                    PieceBase.CreatePieceBlack<Bishop>(3, 8),
                    PieceBase.CreatePieceBlack<Queen>(4, 8),
                    PieceBase.CreatePieceBlack<King>(5, 8),
                    PieceBase.CreatePieceBlack<Bishop>(6, 8),
                    PieceBase.CreatePieceBlack<Knight>(7, 8),
                    PieceBase.CreatePieceBlack<Rook>(8, 8),

                    PieceBase.CreatePieceBlack<Pawn>(1, 7),
                    PieceBase.CreatePieceBlack<Pawn>(2, 7),
                    PieceBase.CreatePieceBlack<Pawn>(3, 7),
                    PieceBase.CreatePieceBlack<Pawn>(4, 7),
                    PieceBase.CreatePieceBlack<Pawn>(5, 7),
                    PieceBase.CreatePieceBlack<Pawn>(6, 7),
                    PieceBase.CreatePieceBlack<Pawn>(7, 7),
                    PieceBase.CreatePieceBlack<Pawn>(8, 7)
                ]
            };
        }
    }
}
