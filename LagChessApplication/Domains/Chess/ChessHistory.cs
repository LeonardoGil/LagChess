namespace LagChessApplication.Domains.Chess
{
    public class ChessHistory
    {
        private readonly Stack<ChessMove> _moves = new();

        public ChessMove[] Get() => [.. _moves];
        public ChessMove LastMove => _moves.First();

        public void Add(ChessMove move) => _moves.Push(move);
        public void Undo() => _moves.Pop();
    }
}
