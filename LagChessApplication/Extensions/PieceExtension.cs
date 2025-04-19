using LagChessApplication.Domains;
using LagChessApplication.Domains.Enums;
using LagChessApplication.Interfaces;
using System.Drawing;

namespace LagChessApplication.Extensions
{
    public static class PieceExtension
    {
        public static IPiece? CreatePiece(Type type, Point position, PieceColorEnum color) => Activator.CreateInstance(type, position, color) as IPiece;
        public static T CreatePieceWhite<T>(int x, int y) where T : class, IPiece => CreatePiece(typeof(T), new Point(x, y), PieceColorEnum.White) as T ?? throw new Exception($"Failed to cast piece of type '{typeof(T).Name}'.");
        public static T CreatePieceBlack<T>(int x, int y) where T : class, IPiece => CreatePiece(typeof(T), new Point(x, y), PieceColorEnum.Black) as T ?? throw new Exception($"Failed to cast piece of type '{typeof(T).Name}'.");

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

                    return directions.Select(x => Point.Add(piece.Position, x)).Where(Board.IsInBoard).ToArray();

                case PieceMoveStyleEnum.Straight:
                    var linearX = Enumerable.Range(1, 8).Where(x => piece.Position.X != x).Select(x => new Point(x, piece.Position.Y));
                    var linearY = Enumerable.Range(1, 8).Where(y => piece.Position.Y != y).Select(y => new Point(piece.Position.X, y));

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

                                var position = new Point(x, y);

                                diagonal.Add(position);
                            }
                        }
                    }

                    return diagonal.Where(Board.IsInBoard).ToArray();

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

                    return knightMoves.Where(Board.IsInBoard).ToArray();

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

                            var position = new Point(x, y);

                            oneStepMoves.Add(position);
                        }
                    }

                    return oneStepMoves.Where(Board.IsInBoard).ToArray();

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
