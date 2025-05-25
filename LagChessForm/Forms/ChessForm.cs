using LagChessApplication.Domains.Chess;
using LagChessApplication.Extensions;

namespace LagChessForm
{
    public partial class ChessForm : Form
    {
        private ChessGame? ChessGame { get; set; }

        public ChessForm()
        {
            InitializeComponent();
        }

        private void Start()
        {
            ChessGame = ChessGameExtension.Create();

            BoardControl.Init(ChessGame);
        }

        private void ButtonIniciar_Click(object sender, EventArgs e) => Start();
    }
}
