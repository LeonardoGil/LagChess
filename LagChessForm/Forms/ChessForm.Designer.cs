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
            panel1 = new Panel();
            PlayerLabel = new Label();
            ButtonIniciar = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // BoardControl
            // 
            BoardControl.Location = new Point(12, 117);
            BoardControl.Name = "BoardControl";
            BoardControl.Size = new Size(600, 600);
            BoardControl.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ControlDark;
            panel1.Controls.Add(PlayerLabel);
            panel1.Location = new Point(618, 117);
            panel1.Name = "panel1";
            panel1.Size = new Size(378, 600);
            panel1.TabIndex = 1;
            // 
            // PlayerLabel
            // 
            PlayerLabel.AutoSize = true;
            PlayerLabel.Location = new Point(24, 28);
            PlayerLabel.Name = "PlayerLabel";
            PlayerLabel.Size = new Size(56, 17);
            PlayerLabel.TabIndex = 0;
            PlayerLabel.Text = "Player: ";
            // 
            // ButtonIniciar
            // 
            ButtonIniciar.BackColor = SystemColors.ControlDark;
            ButtonIniciar.FlatStyle = FlatStyle.Flat;
            ButtonIniciar.Location = new Point(12, 12);
            ButtonIniciar.Name = "ButtonIniciar";
            ButtonIniciar.Size = new Size(70, 35);
            ButtonIniciar.TabIndex = 2;
            ButtonIniciar.Text = "Iniciar";
            ButtonIniciar.UseVisualStyleBackColor = false;
            ButtonIniciar.Click += ButtonIniciar_Click;
            // 
            // ChessForm
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = SystemColors.ControlDarkDark;
            ClientSize = new Size(1008, 729);
            Controls.Add(ButtonIniciar);
            Controls.Add(panel1);
            Controls.Add(BoardControl);
            Font = new Font("Cambria", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ForeColor = SystemColors.HighlightText;
            Margin = new Padding(4);
            MinimumSize = new Size(800, 600);
            Name = "ChessForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Panel panel1;
        private Label PlayerLabel;
        protected Forms.Controls.BoardControl BoardControl;
        private Button ButtonIniciar;
    }
}
