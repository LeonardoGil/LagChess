using LagChessApplication.Domains.Enums;
using LagChessApplication.Interfaces;
using LagChessForm.Resources;
using System.ComponentModel;

namespace LagChessForm.Forms.Controls
{
    public partial class SquareBoardControl : UserControl
    {
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

        public SquareBoardControl(Point? point = null)
        {
            InitializeComponent();

            _point = point ?? new Point(0, 0);

            var pointIsEven = (_point.X + _point.Y) % 2 == 0;

            BackColor = pointIsEven ? Color.FromArgb(238, 238, 210) : Color.FromArgb(118, 150, 86);
        }

        private void RefreshControlView()
        {
            if (_piece is null)
            {
                piecePictureBox.Image = default;
            }
            else
            {
                piecePictureBox.Image = GetImage(_piece.Type, _piece.Color);
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
    }
}
