using LagChessApplication.Domains.Enums;
using LagChessApplication.Interfaces;
using LagChessForm.Resources;

namespace LagChessForm.Forms.Controls
{
    public partial class SquareBoardControl : UserControl
    {
        public readonly Func<Point, Point, bool> CanMovePieceCallback;

        private readonly Point _point;
        public Point Point { get => _point; }

        private IPiece? _piece;
        internal IPiece? Piece 
        {
            get => _piece;
            set
            {
                _piece = value;
                RefreshControlView();
            }
        }

        public SquareBoardControl(Func<Point, Point, bool> canMovePieceCallback, Point? point = null)
        {
            InitializeComponent();

            _point = point ?? new Point(0, 0);

            var pointIsEven = (_point.X + _point.Y) % 2 == 0;

            BackColor = pointIsEven ? Color.FromArgb(238, 238, 210) : Color.FromArgb(118, 150, 86);

            CanMovePieceCallback = canMovePieceCallback;
            DragEnter += SquareBoardControl_DragEnter;
            DragDrop += SquareBoardControl_DragDrop;
            PiecePictureBox.MouseDown += PiecePictureBox_MouseDown;
        }

        private void RefreshControlView()
        {
            if (_piece is null)
            {
                PiecePictureBox.Image = default;
            }
            else
            {
                PiecePictureBox.Image = GetImage(_piece.Type, _piece.Color);
            }
        }

        private static Bitmap GetImage(PieceTypeEnum type, PieceColorEnum color) => type switch
        {
            PieceTypeEnum.Pawn => color == PieceColorEnum.White ? PieceResource.WhitePawn : PieceResource.BlackPawn,
            PieceTypeEnum.Rook => color == PieceColorEnum.White ? PieceResource.WhiteRook : PieceResource.BlackRook,
            PieceTypeEnum.Knight => color == PieceColorEnum.White ? PieceResource.WhiteKnight : PieceResource.BlackKnight,
            PieceTypeEnum.Bishop => color == PieceColorEnum.White ? PieceResource.WhiteBishop : PieceResource.BlackBishop,
            PieceTypeEnum.Queen => color == PieceColorEnum.White ? PieceResource.WhiteQueen : PieceResource.BlackQueen,
            PieceTypeEnum.King => color == PieceColorEnum.White ? PieceResource.WhiteKing : PieceResource.BlackKing,
            
            _ => throw new NotImplementedException(),
        };

        private void PiecePictureBox_MouseDown(object? sender, MouseEventArgs e)
        {
            if (_piece is not null && e.Button == MouseButtons.Left)
            {
                DoDragDrop(this, DragDropEffects.Move);
            }
        }

        private void SquareBoardControl_DragEnter(object? sender, DragEventArgs e)
        {
            if (e.Data?.GetDataPresent(typeof(SquareBoardControl)) ?? false)
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void SquareBoardControl_DragDrop(object? sender, DragEventArgs e)
        {
            var from = (SquareBoardControl)e.Data?.GetData(typeof(SquareBoardControl))!;
            var to = (SquareBoardControl)sender!;

            if (from is null || to is null || from == to)
                return;

            if (CanMovePieceCallback(from.Point, to.Point))
            {
                to.Piece = from.Piece;
                from.Piece = null;
            }
            else
            {
                from.ShowError();
            }
        }

        public async void ShowError()
        {
            var originalColor = BackColor;
            BackColor = Color.Red;

            await Task.Delay(800); 
            BackColor = originalColor;
        }
    }
}
