using LagChessApplication.Domains;
using LagChessApplication.Domains.Chess;

namespace LagChessForm.Forms.Controls
{
    public partial class BoardControl : UserControl
    {

        private readonly ICollection<SquareBoardControl> _squares = [];

        private ChessGame? _chessGame;

        public Board? Board { get => _chessGame?.Board; }
        public ChessGame? ChessGame
        {
            get => _chessGame;
            set
            {
                _chessGame = value;
                Refresh();
            }
        }

        public EventHandler<EventArgs>? OnUpdateInfo;

        public BoardControl()
        {
            InitializeComponent();

            RefreshSquares();

            Resize += (_, _) => RefreshSquares();
        }

        private new void Refresh() => _squares.ToList().ForEach(square => square.Piece = Board?.GetTryPiece(square.Point));

        private void RefreshSquares()
        {
            var positionX = 8;
            var positionY = 8;

            var width = Width / positionX;
            var height = Height / positionY;

            Func<int, int, int, int, Point> calcLocation = (width, height, x, y) => new Point(width * (x - 1), height * (positionY - y));

            if (_squares.Count == 0)
            {
                for (int x = 1; x <= positionX; x++)
                    for (int y = 1; y <= positionY; y++)
                    {
                        var control = new SquareBoardControl(CanMovePiece, new Point(x, y))
                        {
                            Size = new Size(width, height),
                            Location = calcLocation.Invoke(width, height, x, y)
                        };

                        _squares.Add(control);
                    }

                Controls.AddRange(_squares.ToArray());
            }
            else
            {
                foreach (var square in _squares)
                {
                    square.Size = new Size(width, height);
                    square.Location = calcLocation.Invoke(width, height, square.Point.X, square.Point.Y);
                }
            }
        }

        private bool CanMovePiece(Point from, Point to)
        {
            ArgumentNullException.ThrowIfNull(_chessGame);

            try
            {
                var result = _chessGame.Play(from, to);

                OnUpdateInfo?.Invoke(result, EventArgs.Empty);
                
                return true;
            }
            catch (Exception e)
            {
                OnUpdateInfo?.Invoke(e, EventArgs.Empty);
                
                return false;
            }
        }
    }
}
