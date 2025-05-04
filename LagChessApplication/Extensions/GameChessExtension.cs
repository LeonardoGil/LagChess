using LagChessApplication.Domains;

namespace LagChessApplication.Extensions
{
    public static class GameChessExtension
    {
        public static GameChess Create(string white = "white", string black = "black") =>
            new(PlayerExtension.CreateWhite(white), PlayerExtension.CreateBlack(black));
    }
}
