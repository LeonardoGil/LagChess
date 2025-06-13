using LagChessApplication.Domains.Enums;
using LagChessForm.Resources;

namespace LagChessForm.Extensions
{
    internal class ResourceExtensions
    {
        internal static Bitmap GetPieceImage(PieceTypeEnum type, PieceColorEnum color) => type switch
        {
            PieceTypeEnum.Pawn => color == PieceColorEnum.White ? PieceResource.WhitePawn : PieceResource.BlackPawn,
            PieceTypeEnum.Rook => color == PieceColorEnum.White ? PieceResource.WhiteRook : PieceResource.BlackRook,
            PieceTypeEnum.Knight => color == PieceColorEnum.White ? PieceResource.WhiteKnight : PieceResource.BlackKnight,
            PieceTypeEnum.Bishop => color == PieceColorEnum.White ? PieceResource.WhiteBishop : PieceResource.BlackBishop,
            PieceTypeEnum.Queen => color == PieceColorEnum.White ? PieceResource.WhiteQueen : PieceResource.BlackQueen,
            PieceTypeEnum.King => color == PieceColorEnum.White ? PieceResource.WhiteKing : PieceResource.BlackKing,

            _ => throw new NotImplementedException(),
        };
    }
}
