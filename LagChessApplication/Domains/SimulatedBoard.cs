namespace LagChessApplication.Domains
{
    public class SimulatedBoard(Board board)
    {
        public Board Board { get; init; } = board;

        public static SimulatedBoard CreateClone(Board board) => new(board.Clone());
    }
}
