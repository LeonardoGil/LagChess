using LagChessApplication.Domains.Chess;
using LagChessApplication.Domains.Enums;
using LagChessApplication.Extensions;
using LagChessApplication.Interfaces;
using LagChessForm.Extensions;

namespace LagChessForm
{
    public partial class ChessForm : Form
    {
        private ChessGame? _chessGame;
        public ChessGame? ChessGame 
        {
            get => _chessGame;
            set
            {
                _chessGame = value;
                
                BoardControl.ChessGame = value;

                RefreshPanelInfo();
            }
        }

        public ChessForm()
        {
            InitializeComponent();

            BackColor = Program.Theme.BackgroundLighter;

            BoardControl.OnUpdateInfo += PanelInfo_Refresh;
        }

        private void Start() => ChessGame = ChessGameExtension.Create();

        private void RefreshPanelInfo(string error = "")
        {
            labelError.Text = error;

            labelPlayerWhite.Text = ChessGame?.White.Name ?? "White Player";
            labelPlayerBlack.Text = ChessGame?.Black.Name ?? "Black Player";

            RefreshFlowLayoutPanelPiecesDead();
        }

        private void RefreshFlowLayoutPanelPiecesDead()
        {
            flowLayoutPanelBlackPiecesDead.Controls.Clear();
            flowLayoutPanelWhitePiecesDead.Controls.Clear();

            if (ChessGame is null)
                return;

            var piecesDead = ChessGame.Board.Pieces.Where(x => x.IsDead);

            Func<IPiece, PictureBox> mapper = x => new PictureBox
            {
                Image = ResourceExtensions.GetPieceImage(x.Type, x.Color),
                Size = new Size(20, 20),
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            flowLayoutPanelWhitePiecesDead.Controls.AddRange(piecesDead.Where(x => x.Color == PieceColorEnum.White).Select(mapper).ToArray());
            flowLayoutPanelBlackPiecesDead.Controls.AddRange(piecesDead.Where(x => x.Color == PieceColorEnum.Black).Select(mapper).ToArray());
        }

        private void ButtonIniciar_Click(object sender, EventArgs e) => Start();

        private void PanelInfo_Refresh(object? sender, EventArgs e)
        {
            if (sender is ChessMove)
            {
                RefreshPanelInfo();
            }
            else if (sender is Exception ex)
            {
                RefreshPanelInfo(ex.Message);
            }

        }
    }
}
