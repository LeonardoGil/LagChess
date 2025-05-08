using LagChessApplication.Extensions;
using System.Drawing;

namespace LagChessApplication.Domains
{
    public readonly struct Square
    {
        public readonly Point Point { get; }

        public readonly char Row
        {
            get => Point.Y.ToString()[0];
        }
        public readonly char Column 
        {
            get => _valuesX[Point.X - 1];
        }

        private static readonly char[] _valuesX = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h',
                                                   'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H'];

        public override readonly string ToString() => $"{Column}{Row}";

        public Square(Point position)
        {
            if (!BoardExtension.IsInBoard(position))
                throw new ArgumentOutOfRangeException(nameof(position), $"Invalid position: {position}. Position must be between (1,1) and (8,8).");

            Point = position;
        }

        public Square(string square)
        {
            if (string.IsNullOrWhiteSpace(square) || square.Length != 2)
                throw new ArgumentException("Invalid square notation. Must be in format like 'A1' to 'H8'.", nameof(square));

            var xChar = square[0];
            var yChar = square[1];

            if (!_valuesX.Contains(xChar))
                throw new ArgumentException($"Invalid column character '{xChar}'. Must be a letter from A-H or a-h.", nameof(square));

            if (!int.TryParse(yChar.ToString(), out int y) || y is < 1 or > 8)
                throw new ArgumentException($"Invalid row character '{yChar}'. Must be a digit from 1 to 8.", nameof(square));

            var index = Array.IndexOf(_valuesX, xChar) + 1;
            var x = index <= 8 ? index : index - 8;

            Point = new Point(x, y);
        }

        public static implicit operator Square(Point i) => new(i);
        public static implicit operator Point(Square i) => i.Point;

        public static implicit operator Square(string i) => new(i);
        public static implicit operator string(Square i) => i.ToString();

        #region Squares
        public static readonly Square A1 = new("A1");
        public static readonly Square A2 = new("A2");
        public static readonly Square A3 = new("A3");
        public static readonly Square A4 = new("A4");
        public static readonly Square A5 = new("A5");
        public static readonly Square A6 = new("A6");
        public static readonly Square A7 = new("A7");
        public static readonly Square A8 = new("A8");

        public static readonly Square B1 = new("B1");
        public static readonly Square B2 = new("B2");
        public static readonly Square B3 = new("B3");
        public static readonly Square B4 = new("B4");
        public static readonly Square B5 = new("B5");
        public static readonly Square B6 = new("B6");
        public static readonly Square B7 = new("B7");
        public static readonly Square B8 = new("B8");

        public static readonly Square C1 = new("C1");
        public static readonly Square C2 = new("C2");
        public static readonly Square C3 = new("C3");
        public static readonly Square C4 = new("C4");
        public static readonly Square C5 = new("C5");
        public static readonly Square C6 = new("C6");
        public static readonly Square C7 = new("C7");
        public static readonly Square C8 = new("C8");

        public static readonly Square D1 = new("D1");
        public static readonly Square D2 = new("D2");
        public static readonly Square D3 = new("D3");
        public static readonly Square D4 = new("D4");
        public static readonly Square D5 = new("D5");
        public static readonly Square D6 = new("D6");
        public static readonly Square D7 = new("D7");
        public static readonly Square D8 = new("D8");

        public static readonly Square E1 = new("E1");
        public static readonly Square E2 = new("E2");
        public static readonly Square E3 = new("E3");
        public static readonly Square E4 = new("E4");
        public static readonly Square E5 = new("E5");
        public static readonly Square E6 = new("E6");
        public static readonly Square E7 = new("E7");
        public static readonly Square E8 = new("E8");

        public static readonly Square F1 = new("F1");
        public static readonly Square F2 = new("F2");
        public static readonly Square F3 = new("F3");
        public static readonly Square F4 = new("F4");
        public static readonly Square F5 = new("F5");
        public static readonly Square F6 = new("F6");
        public static readonly Square F7 = new("F7");
        public static readonly Square F8 = new("F8");

        public static readonly Square G1 = new("G1");
        public static readonly Square G2 = new("G2");
        public static readonly Square G3 = new("G3");
        public static readonly Square G4 = new("G4");
        public static readonly Square G5 = new("G5");
        public static readonly Square G6 = new("G6");
        public static readonly Square G7 = new("G7");
        public static readonly Square G8 = new("G8");

        public static readonly Square H1 = new("H1");
        public static readonly Square H2 = new("H2");
        public static readonly Square H3 = new("H3");
        public static readonly Square H4 = new("H4");
        public static readonly Square H5 = new("H5");
        public static readonly Square H6 = new("H6");
        public static readonly Square H7 = new("H7");
        public static readonly Square H8 = new("H8");
        #endregion
    }
}
