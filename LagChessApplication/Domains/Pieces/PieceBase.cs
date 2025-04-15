using LagChessApplication.Domains.Enums;
using System.Drawing;

namespace LagChessApplication.Domains.Pieces
{
    public abstract class PieceBase(Point position, PieceColorEnum color) : IPiece
    {
        private Point? _position = position;
        public Point Position
        {
            get
            {
                if (!_position.HasValue)
                    throw new Exception("The piece is dead — it has no position.");

                return _position.Value;
            }
            protected set => _position = value;
        }

        public bool IsDead { get; protected set; }

        public PieceColorEnum Color { get; init; } = color;
        public PieceTypeEnum Type { get; init; }
        public PieceMoveStyleEnum MoveStyle { get; init; }

        public void Kill()
        {
            _position = null;
            IsDead = true;
        }

        public void Move(Point to) => Position = to;

        public abstract bool IsValidMove(Point to);

        public IPiece Clone() => CreatePiece(GetType(), Position, Color);

        public static IPiece CreatePiece(Type type, Point position, PieceColorEnum color) => Activator.CreateInstance(type, position, color) as IPiece;
        public static T CreatePieceWhite<T>(int x, int y) where T : class, IPiece => CreatePiece(typeof(T), new Point(x, y), PieceColorEnum.White) as T;
        public static T CreatePieceBlack<T>(int x, int y) where T : class, IPiece => CreatePiece(typeof(T), new Point(x, y), PieceColorEnum.Black) as T;
    }

    public static class PieceExtension
    {
        public static Point[] GetPossibleMoves(this IPiece piece, PieceMoveStyleEnum moveStyle)
        {
            switch (moveStyle)
            {
                case PieceMoveStyleEnum.OneStraight:
                    
                    var directions = new Size[]
                    {
                            new(0, 1),   
                            new(0, -1),  
                            new(1, 0),   
                            new(-1, 0),  
                    };

                    return directions.Select(x => Point.Add(piece.Position, x))
                                     .Where(position => position.X is >= 0 and < 8 && position.Y is >= 0 and < 8)
                                     .ToArray();

                case PieceMoveStyleEnum.Straight:
                    var linearX = Enumerable.Range(0, 7).Where(x => piece.Position.X != x).Select(x => new Point(x, piece.Position.Y));
                    var linearY = Enumerable.Range(0, 7).Where(y => piece.Position.Y != y).Select(y => new Point(piece.Position.X, y));

                    return linearX.Concat(linearY).ToArray();

                case PieceMoveStyleEnum.Diagonal:
                    var diagonal = new List<Point>();

                    for (int dx = -1; dx <= 1; dx += 2)
                    {
                        for (int dy = -1; dy <= 1; dy += 2)
                        {
                            for (int i = 1; i < 8; i++)
                            {
                                var x = piece.Position.X + dx * i;
                                var y = piece.Position.Y + dy * i;

                                if (x is >= 0 and < 8 && y is >= 0 and < 8)
                                    diagonal.Add(new Point(x, y));
                            }
                        }
                    }
                    return [.. diagonal];

                case PieceMoveStyleEnum.LShaped:
                    var knightMoves = new[]
                    {
                        new Point(piece.Position.X + 1, piece.Position.Y + 2),
                        new Point(piece.Position.X + 1, piece.Position.Y - 2),
                        new Point(piece.Position.X - 1, piece.Position.Y + 2),
                        new Point(piece.Position.X - 1, piece.Position.Y - 2),
                        new Point(piece.Position.X + 2, piece.Position.Y + 1),
                        new Point(piece.Position.X + 2, piece.Position.Y - 1),
                        new Point(piece.Position.X - 2, piece.Position.Y + 1),
                        new Point(piece.Position.X - 2, piece.Position.Y - 1)
                    };

                    return knightMoves.Where(p => p.X is >= 1 and <= 8 && p.Y is >= 1 and <= 8).ToArray();

                case PieceMoveStyleEnum.OneAll:

                    var oneStepMoves = new List<Point>();

                    for (int dx = -1; dx <= 1; dx++)
                    {
                        for (int dy = -1; dy <= 1; dy++)
                        {
                            if (dx == 0 && dy == 0)
                                continue;

                            var x = piece.Position.X + dx;
                            var y = piece.Position.Y + dy;

                            if (x is >= 0 and < 8 && y is >= 0 and < 8)
                                oneStepMoves.Add(new Point(x, y));
                        }
                    }

                    return [.. oneStepMoves];

                case PieceMoveStyleEnum.All:
                    var linearMoves = GetPossibleMoves(piece, PieceMoveStyleEnum.Straight);
                    var diagonalMoves = GetPossibleMoves(piece, PieceMoveStyleEnum.Diagonal);

                    return linearMoves.Union(diagonalMoves).ToArray();

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
