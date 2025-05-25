namespace LagChessForm
{
    partial class ChessForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            BoardControl = new Forms.Controls.BoardControl();
            panelInfo = new Panel();
            flowLayoutPanelBlackPiecesDead = new FlowLayoutPanel();
            labelPlayerBlack = new Label();
            flowLayoutPanelWhitePiecesDead = new FlowLayoutPanel();
            labelPlayerWhite = new Label();
            buttonStart = new Button();
            labelError = new Label();
            panelInfo.SuspendLayout();
            SuspendLayout();
            // 
            // BoardControl
            // 
            BoardControl.ChessGame = null;
            BoardControl.Location = new Point(12, 17);
            BoardControl.Name = "BoardControl";
            BoardControl.Size = new Size(700, 700);
            BoardControl.TabIndex = 0;
            // 
            // panelInfo
            // 
            panelInfo.BackColor = Color.FromArgb(38, 36, 33);
            panelInfo.BorderStyle = BorderStyle.FixedSingle;
            panelInfo.Controls.Add(labelError);
            panelInfo.Controls.Add(flowLayoutPanelBlackPiecesDead);
            panelInfo.Controls.Add(labelPlayerBlack);
            panelInfo.Controls.Add(flowLayoutPanelWhitePiecesDead);
            panelInfo.Controls.Add(labelPlayerWhite);
            panelInfo.Location = new Point(718, 53);
            panelInfo.Name = "panelInfo";
            panelInfo.Size = new Size(278, 664);
            panelInfo.TabIndex = 1;
            // 
            // flowLayoutPanelBlackPiecesDead
            // 
            flowLayoutPanelBlackPiecesDead.Location = new Point(10, 310);
            flowLayoutPanelBlackPiecesDead.Margin = new Padding(10);
            flowLayoutPanelBlackPiecesDead.Name = "flowLayoutPanelBlackPiecesDead";
            flowLayoutPanelBlackPiecesDead.Size = new Size(256, 100);
            flowLayoutPanelBlackPiecesDead.TabIndex = 3;
            // 
            // labelPlayerBlack
            // 
            labelPlayerBlack.AutoSize = true;
            labelPlayerBlack.Location = new Point(10, 283);
            labelPlayerBlack.Name = "labelPlayerBlack";
            labelPlayerBlack.Size = new Size(87, 17);
            labelPlayerBlack.TabIndex = 2;
            labelPlayerBlack.Text = "Black Player";
            // 
            // flowLayoutPanelWhitePiecesDead
            // 
            flowLayoutPanelWhitePiecesDead.Location = new Point(10, 173);
            flowLayoutPanelWhitePiecesDead.Margin = new Padding(10);
            flowLayoutPanelWhitePiecesDead.Name = "flowLayoutPanelWhitePiecesDead";
            flowLayoutPanelWhitePiecesDead.Size = new Size(256, 100);
            flowLayoutPanelWhitePiecesDead.TabIndex = 1;
            // 
            // labelPlayerWhite
            // 
            labelPlayerWhite.AutoSize = true;
            labelPlayerWhite.Location = new Point(10, 146);
            labelPlayerWhite.Name = "labelPlayerWhite";
            labelPlayerWhite.Size = new Size(90, 17);
            labelPlayerWhite.TabIndex = 0;
            labelPlayerWhite.Text = "White Player";
            // 
            // buttonStart
            // 
            buttonStart.BackColor = Color.FromArgb(38, 36, 33);
            buttonStart.FlatAppearance.BorderColor = Color.FromArgb(19, 18, 16);
            buttonStart.FlatStyle = FlatStyle.Flat;
            buttonStart.Location = new Point(926, 12);
            buttonStart.Name = "buttonStart";
            buttonStart.Size = new Size(70, 35);
            buttonStart.TabIndex = 2;
            buttonStart.Text = "Start";
            buttonStart.UseMnemonic = false;
            buttonStart.UseVisualStyleBackColor = false;
            buttonStart.Click += ButtonIniciar_Click;
            // 
            // labelError
            // 
            labelError.AutoSize = true;
            labelError.ForeColor = Color.Red;
            labelError.Location = new Point(10, 632);
            labelError.Name = "labelError";
            labelError.Size = new Size(0, 17);
            labelError.TabIndex = 4;
            labelError.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ChessForm
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = Color.FromArgb(49, 46, 43);
            ClientSize = new Size(1008, 729);
            Controls.Add(buttonStart);
            Controls.Add(panelInfo);
            Controls.Add(BoardControl);
            Font = new Font("Cambria", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ForeColor = SystemColors.HighlightText;
            Margin = new Padding(4);
            MinimumSize = new Size(800, 600);
            Name = "ChessForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            panelInfo.ResumeLayout(false);
            panelInfo.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Panel panelInfo;
        private Label labelPlayerWhite;
        protected Forms.Controls.BoardControl BoardControl;
        private Button buttonStart;
        private FlowLayoutPanel flowLayoutPanelWhitePiecesDead;
        private FlowLayoutPanel flowLayoutPanelBlackPiecesDead;
        private Label labelPlayerBlack;
        private Label labelError;
    }
}
