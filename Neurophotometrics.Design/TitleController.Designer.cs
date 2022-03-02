using System;
using System.Drawing;

namespace Neurophotometrics.Design
{
    partial class TitleController
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
        private void InitializeComponent(Tuple<int, int> InitProps)
        {
            int DefaultWidth = InitProps.Item1;
            int DefaultHeight = InitProps.Item2;

            this.SuspendLayout();
            this.titleLabel = new System.Windows.Forms.Label();
            this.logoBox = new System.Windows.Forms.PictureBox();
            this.components = new System.ComponentModel.Container();
            this.titleLabel.SuspendLayout();


            //
            // titleLabel
            //
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(DefaultWidth - 6 * TitleHorizontalMargin, DefaultHeight - 2 * TitleVerticalMargin);
            this.titleLabel.Text = "Neurophotometrics Data Visualizer";
            this.titleLabel.Location = new System.Drawing.Point(3 * TitleHorizontalMargin, TitleVerticalMargin);
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.titleLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.titleLabel.Font = new System.Drawing.Font("Century Gothic", 15, System.Drawing.FontStyle.Bold);
            this.titleLabel.Visible = true;
            this.titleLabel.Click += new System.EventHandler(titleLabel_Click);
            //
            // logBox
            //
            this.logoBox.Name = "logoBox";
            this.logoBox.Bounds = new Rectangle(0, 0, DefaultHeight, DefaultHeight);
            Image image = resizeImage(Properties.Resources.Neurophotometrics.ToBitmap(), new Size(DefaultWidth, DefaultHeight));
            this.logoBox.Image = image;

            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.logoBox);

            this.Name = "TitleController";
            this.Size = new System.Drawing.Size(DefaultWidth, DefaultHeight);
            this.BackColor = Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        }

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.PictureBox logoBox;
    }
}
