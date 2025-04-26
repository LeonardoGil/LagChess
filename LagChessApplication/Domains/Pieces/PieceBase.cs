using LagChessApplication.Domains.Enums;
using LagChessApplication.Extensions;
using LagChessApplication.Interfaces;
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

        public IPiece Clone() => PieceExtension.CreatePiece(GetType(), Position, Color) ?? throw new Exception($"Failed to create piece of type '{GetType().Name}'.");

        public IPiece ConvertTo(PieceTypeEnum typeEnum)
        {
            var type = PieceExtension.GetType(typeEnum);

            return PieceExtension.CreatePiece(type, Position, Color) ?? throw new Exception($"Failed to create piece of type '{type.Name}'.");
        }

        public override bool Equals(object? obj)
        {
            if (obj is null)
                return false;

            return obj is IPiece piece &&
                    piece.GetType() == GetType() &&
                    piece.Color == Color && (piece.IsDead && IsDead || !piece.IsDead && !IsDead && piece.Position == Position);
        }
    }
}
