using LagChessApplication.Domains.Chess;
using LagChessApplication.Domains.Enums;

namespace LagChessApplication.Extensions
{
    public static class GameChessExtension
    {
        public static ChessGame Create(string player1 = "white", string player2 = "black", Func<PieceTypeEnum>? onPawnPromotion = null)
        {
            var white = PlayerExtension.CreateWhite(player1);
            var black = PlayerExtension.CreateBlack(player2);

            onPawnPromotion ??= () => PieceTypeEnum.Queen;

            return new(white, black, onPawnPromotion);
        }
    }
}
