namespace LagChessApplication.Domains.Chess
{
    public class ChessHistory
    {
        private readonly Stack<ChessMove> _moves = new();

        public void Add(ChessMove move) => _moves.Push(move);
        public void Undo() => _moves.Pop();
    }
}
