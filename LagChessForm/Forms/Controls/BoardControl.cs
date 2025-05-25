using LagChessApplication.Domains;
using LagChessApplication.Domains.Chess;
using LagChessApplication.Interfaces;
using System.Collections.ObjectModel;

namespace LagChessForm.Forms.Controls
{
    public partial class BoardControl : UserControl
    {
        private readonly ICollection<SquareBoardControl> Squares = [];
        
        public Board Board { get; private set; }

        public BoardControl()
        {
            InitializeComponent();

            LoadSquares();
        }

        internal void Init(Board board)
        {
            Board = board;

            Refresh();
        }

        private void Refresh(IEnumerable<Point>? points = null)
        {
            if (points is null)
            {
                foreach (var piece in Board.AvailablePieces)
                {
                    var square = Squares.First(square => square.Point == piece.Position);
                    square.Piece = piece;
                }
            }
            else
            {
                foreach (var point in points)
                {
                    var square = Squares.First(square => square.Point == point);
                    square.Piece = square.Piece;
                }
            }
        }

        private void LoadSquares()
        {
            var positionX = 8;
            var positionY = 8;

            var width = Width / positionX;
            var height = Height / positionY;

            for (int x = 1; x <= positionX; x++)
                for (int y = 1; y <= positionY; y++)
                {
                    var control = new SquareBoardControl(new Point(x, y))
                    {
                        Size = new Size(width, height),
                        Location = new Point(width * (x - 1), height * (positionY - y))
                    };

                    Squares.Add(control);
                }

            Controls.AddRange(Squares.ToArray());
        }
    }
}
