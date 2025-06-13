using LagChessApplication.Domains.Chess;

namespace LagChessApplication.Extensions
{
    public static class ChessGameExtension
    {
        public static ChessGame Create(string white = "white", string black = "black") =>
            new(PlayerExtension.CreateWhite(white), PlayerExtension.CreateBlack(black));
    }
}
