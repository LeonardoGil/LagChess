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
            piecePictureBox = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)piecePictureBox).BeginInit();
            SuspendLayout();
            // 
            // piecePictureBox
            // 
            piecePictureBox.BackColor = Color.Transparent;
            piecePictureBox.Dock = DockStyle.Fill;
            piecePictureBox.Location = new Point(0, 0);
            piecePictureBox.Name = "piecePictureBox";
            piecePictureBox.Size = new Size(75, 75);
            piecePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            piecePictureBox.TabIndex = 0;
            piecePictureBox.TabStop = false;
            // 
            // SquareBoardControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(piecePictureBox);
            Margin = new Padding(0);
            Name = "SquareBoardControl";
            Size = new Size(75, 75);
            ((System.ComponentModel.ISupportInitialize)piecePictureBox).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox piecePictureBox;
    }
}
