﻿using LagChessApplication.Domains.Enums;
using LagChessApplication.Domains.Pieces;
using LagChessApplication.Extensions.Boards;
using LagChessApplication.Interfaces;
using System.Drawing;

namespace LagChessApplication.Extensions.Pieces
{
    public static class PieceExtension
    {
        public static IPiece? CreatePiece(Type type, Point position, PieceColorEnum color) => Activator.CreateInstance(type, position, color) as IPiece;
        public static T CreatePieceWhite<T>(int x, int y) where T : class, IPiece => CreatePiece(typeof(T), new Point(x, y), PieceColorEnum.White) as T ?? throw new Exception($"Failed to cast piece of type '{typeof(T).Name}'.");
        public static T CreatePieceBlack<T>(int x, int y) where T : class, IPiece => CreatePiece(typeof(T), new Point(x, y), PieceColorEnum.Black) as T ?? throw new Exception($"Failed to cast piece of type '{typeof(T).Name}'.");


        public static Point[] GetPossibleMoves(this IPiece piece) => piece.GetPossibleMoves(piece.MoveStyle);
        public static Point[] GetPossibleMoves(this IPiece piece, PieceMoveStyleEnum moveStyle)
        {
            switch (moveStyle)
            {
                case PieceMoveStyleEnum.OneStraight:

                    var directions = default(IEnumerable<Size>);

                    if (piece is Pawn)
                    {
                        directions = [new(0, 1)];
                    }
                    else
                    {
                        directions =
                        [
                            new(0, 1),
                            new(0, -1),
                            new(1, 0),
                            new(-1, 0),
                        ];
                    }

                    return directions.Select(x => Point.Add(piece.Position, x)).Where(BoardMoveExtension.IsInBoard).ToArray();

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

                    return diagonal.Where(BoardMoveExtension.IsInBoard).ToArray();

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

                    return knightMoves.Where(BoardMoveExtension.IsInBoard).ToArray();

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

                    return oneStepMoves.Where(BoardMoveExtension.IsInBoard).ToArray();

                case PieceMoveStyleEnum.All:
                    var linearMoves = piece.GetPossibleMoves(PieceMoveStyleEnum.Straight);
                    var diagonalMoves = piece.GetPossibleMoves(PieceMoveStyleEnum.Diagonal);

                    return linearMoves.Union(diagonalMoves).ToArray();

                default:
                    throw new NotImplementedException();
            }
        }

        public static Point[] GetPossibleMovesAndAttacks(this IPiece piece, PieceMoveStyleEnum? moveStyle = null)
        {
            if (!moveStyle.HasValue)
                moveStyle = piece.MoveStyle;

            var moves = piece.GetPossibleMoves(moveStyle.Value);

            if (piece is Pawn)
            {
                var direction = piece.Color == PieceColorEnum.White ? 1 : -1;

                var attacks = new Point[2]
                {
                    new(piece.Position.X - 1, piece.Position.Y + direction),
                    new(piece.Position.X + 1, piece.Position.Y + direction)
                };

                moves = moves.Union(attacks).ToArray();
            }

            return moves;
        }

        public static Type GetType(PieceTypeEnum type)
        {
            return type switch
            {
                PieceTypeEnum.Pawn => typeof(Pawn),
                PieceTypeEnum.Rook => typeof(Rook),
                PieceTypeEnum.Knight => typeof(Knight),
                PieceTypeEnum.Bishop => typeof(Bishop),
                PieceTypeEnum.Queen => typeof(Queen),
                PieceTypeEnum.King => typeof(King),

                _ => throw new ArgumentOutOfRangeException(nameof(type), $"Unexpected piece type: {type}")
            };
        }
    }
}
