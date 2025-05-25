namespace LagChessForm.Forms.Controls
{
    partial class SquareBoardControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            PiecePictureBox = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)PiecePictureBox).BeginInit();
            SuspendLayout();
            // 
            // PiecePictureBox
            // 
            PiecePictureBox.BackColor = Color.Transparent;
            PiecePictureBox.Dock = DockStyle.Fill;
            PiecePictureBox.Location = new Point(0, 0);
            PiecePictureBox.Name = "PiecePictureBox";
            PiecePictureBox.Size = new Size(75, 75);
            PiecePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            PiecePictureBox.TabIndex = 0;
            PiecePictureBox.TabStop = false;
            // 
            // SquareBoardControl
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(PiecePictureBox);
            Margin = new Padding(0);
            Name = "SquareBoardControl";
            Size = new Size(75, 75);
            ((System.ComponentModel.ISupportInitialize)PiecePictureBox).EndInit();
            ResumeLayout(false);
        }

        #endregion

        public PictureBox PiecePictureBox;
        private PictureBox piecePictureBox;
    }
}
