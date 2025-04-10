namespace LagChessApplication.Domains
{
    public class SimulatedBoard()
    {
        public static Board CreateClone(Board board) => board.Clone();
    }
}
