using LagChessApplication.Domains;
using LagChessApplication.Domains.Chess;
using LagChessApplication.Interfaces;
using System.Collections.ObjectModel;

namespace LagChessForm.Forms.Controls
{
    public partial class BoardControl : UserControl
    {
        private readonly ICollection<SquareBoardControl> _squares = [];
        private ChessGame? _chessGame;

        public Board Board { get => _chessGame?.Board ?? throw new ArgumentNullException(); }

        public BoardControl()
        {
            InitializeComponent();

            LoadSquares();
        }

        internal void Init(ChessGame chessGame)
        {
            _chessGame = chessGame;

            Refresh();
        }

        private void Refresh(IEnumerable<Point>? points = null)
        {
            if (points is null)
            {
                foreach (var piece in Board.AvailablePieces)
                {
                    var square = _squares.First(square => square.Point == piece.Position);
                    square.Piece = piece;
                }
            }
            else
            {
                foreach (var point in points)
                {
                    var square = _squares.First(square => square.Point == point);
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
                    var control = new SquareBoardControl(CanMovePiece, new Point(x, y))
                    {
                        Size = new Size(width, height),
                        Location = new Point(width * (x - 1), height * (positionY - y))
                    };

                    _squares.Add(control);
                }

            Controls.AddRange(_squares.ToArray());
        }

        private bool CanMovePiece(Point from, Point to)
        {
            ArgumentNullException.ThrowIfNull(_chessGame);

            try
            {
                var result = _chessGame.Play(from, to);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
