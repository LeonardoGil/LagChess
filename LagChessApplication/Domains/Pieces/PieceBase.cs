using LagChessApplication.Domains.Enums;
using System.Drawing;

namespace LagChessApplication.Domains.Pieces
{
    public abstract class PieceBase(Point position, PieceColorEnum color) : IPiece
    {
        public Point? Position { get; protected set; } = position;
        public bool IsDead { get; protected set; }

        public PieceColorEnum Color { get; init; } = color;
        public PieceTypeEnum Type { get; init; }
        public PieceMoveStyleEnum MoveStyle { get; init; }

        public void Kill()
        {
            Position = null;
            IsDead = true;
        }

        public void Move(Point to) => Position = to;

        public abstract bool IsValidMove(Point to);

        public abstract IPiece Clone();

        public static T CreatePiece<T>(Point position, PieceColorEnum color) where T : class => (T)Activator.CreateInstance(typeof(T), position, color);
        public static T CreatePieceWhite<T>(int x, int y) where T : class => CreatePiece<T>(new Point(x, y), PieceColorEnum.White);
        public static T CreatePieceBlack<T>(int x, int y) where T : class => CreatePiece<T>(new Point(x, y), PieceColorEnum.Black);
    }

    public static class PieceExtension
    {
        public static Point[] GetPossibleMoves(this IPiece piece, PieceMoveStyleEnum moveStyle)
        {
            var position = piece.Position ?? throw new Exception("Piece is dead");

            switch (moveStyle)
            {
                case PieceMoveStyleEnum.Linear:
                    var linearX = Enumerable.Range(0, 7).Where(x => position.X != x).Select(x => new Point(x, position.Y));
                    var linearY = Enumerable.Range(0, 7).Where(y => position.Y != y).Select(y => new Point(position.X, y));

                    return linearX.Concat(linearY).ToArray();

                case PieceMoveStyleEnum.Diagonal:
                    var diagonal = new List<Point>();

                    for (int dx = -1; dx <= 1; dx += 2)
                    {
                        for (int dy = -1; dy <= 1; dy += 2)
                        {
                            for (int i = 1; i < 8; i++)
                            {
                                var x = position.X + dx * i;
                                var y = position.Y + dy * i;

                                if (x is >= 0 and < 8 && y is >= 0 and < 8)
                                    diagonal.Add(new Point(x, y));
                            }
                        }
                    }
                    return [.. diagonal];

                case PieceMoveStyleEnum.LShaped:
                    var knightMoves = new[]
                    {
                        new Point(position.X + 1, position.Y + 2),
                        new Point(position.X + 1, position.Y - 2),
                        new Point(position.X - 1, position.Y + 2),
                        new Point(position.X - 1, position.Y - 2),
                        new Point(position.X + 2, position.Y + 1),
                        new Point(position.X + 2, position.Y - 1),
                        new Point(position.X - 2, position.Y + 1),
                        new Point(position.X - 2, position.Y - 1)
                    };

                    return knightMoves.Where(p => p.X is >= 0 and < 8 && p.Y is >= 0 and < 8).ToArray();

                case PieceMoveStyleEnum.All:
                    var linearMoves = GetPossibleMoves(piece, PieceMoveStyleEnum.Linear);
                    var diagonalMoves = GetPossibleMoves(piece, PieceMoveStyleEnum.Diagonal);

                    return linearMoves.Union(diagonalMoves).ToArray();

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
