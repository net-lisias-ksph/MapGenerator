namespace Noise
{
    partial class RenderedPreview
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pctOutput = new System.Windows.Forms.PictureBox();
            this.butClose = new System.Windows.Forms.Button();
            this.Interval = new System.Windows.Forms.Timer(this.components);
            this.MoveTick = new System.Windows.Forms.Timer(this.components);
            this.butRecentre = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pctHeightErosion = new System.Windows.Forms.PictureBox();
            this.pctColor = new System.Windows.Forms.PictureBox();
            this.pctNormal = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pctOutput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctHeightErosion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctNormal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pctOutput
            // 
            this.pctOutput.Location = new System.Drawing.Point(11, 12);
            this.pctOutput.Name = "pctOutput";
            this.pctOutput.Size = new System.Drawing.Size(691, 691);
            this.pctOutput.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pctOutput.TabIndex = 0;
            this.pctOutput.TabStop = false;
            this.pctOutput.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PctOutput_MouseDown);
            this.pctOutput.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PctOutput_MouseMove);
            this.pctOutput.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PctOutput_MouseUp);
            // 
            // butClose
            // 
            this.butClose.Location = new System.Drawing.Point(588, 709);
            this.butClose.Name = "butClose";
            this.butClose.Size = new System.Drawing.Size(115, 49);
            this.butClose.TabIndex = 2;
            this.butClose.Tag = "Param";
            this.butClose.Text = "Done";
            this.butClose.UseVisualStyleBackColor = true;
            this.butClose.Click += new System.EventHandler(this.ButClose_Click);
            // 
            // Interval
            // 
            this.Interval.Interval = 16;
            this.Interval.Tick += new System.EventHandler(this.Interval_Tick);
            // 
            // MoveTick
            // 
            this.MoveTick.Interval = 5;
            // 
            // butRecentre
            // 
            this.butRecentre.Location = new System.Drawing.Point(467, 709);
            this.butRecentre.Name = "butRecentre";
            this.butRecentre.Size = new System.Drawing.Size(115, 49);
            this.butRecentre.TabIndex = 3;
            this.butRecentre.Tag = "Param";
            this.butRecentre.Text = "Refocus";
            this.butRecentre.UseVisualStyleBackColor = true;
            this.butRecentre.Click += new System.EventHandler(this.ButRecentre_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.pictureBox2.Location = new System.Drawing.Point(756, 12);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(231, 747);
            this.pictureBox2.TabIndex = 5;
            this.pictureBox2.TabStop = false;
            // 
            // pctHeightErosion
            // 
            this.pctHeightErosion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pctHeightErosion.Location = new System.Drawing.Point(792, 123);
            this.pctHeightErosion.Name = "pctHeightErosion";
            this.pctHeightErosion.Size = new System.Drawing.Size(168, 168);
            this.pctHeightErosion.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pctHeightErosion.TabIndex = 6;
            this.pctHeightErosion.TabStop = false;
            this.pctHeightErosion.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PctHeightErosion_MouseDown);
            this.pctHeightErosion.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PctHeightErosion_MouseMove);
            this.pctHeightErosion.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PctHeightErosion_MouseUp);
            // 
            // pctColor
            // 
            this.pctColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pctColor.Location = new System.Drawing.Point(792, 297);
            this.pctColor.Name = "pctColor";
            this.pctColor.Size = new System.Drawing.Size(168, 168);
            this.pctColor.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pctColor.TabIndex = 7;
            this.pctColor.TabStop = false;
            this.pctColor.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PctColor_MouseDown);
            this.pctColor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PctColor_MouseMove);
            this.pctColor.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PctColor_MouseUp);
            // 
            // pctNormal
            // 
            this.pctNormal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pctNormal.Location = new System.Drawing.Point(792, 471);
            this.pctNormal.Name = "pctNormal";
            this.pctNormal.Size = new System.Drawing.Size(168, 168);
            this.pctNormal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pctNormal.TabIndex = 8;
            this.pctNormal.TabStop = false;
            this.pctNormal.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PctNormal_MouseDown);
            this.pctNormal.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PctNormal_MouseMove);
            this.pctNormal.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PctNormal_MouseUp);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(756, -15);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(295, 815);
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // RenderedPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1017, 771);
            this.Controls.Add(this.pctNormal);
            this.Controls.Add(this.pctColor);
            this.Controls.Add(this.pctHeightErosion);
            this.Controls.Add(this.butRecentre);
            this.Controls.Add(this.butClose);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pctOutput);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "RenderedPreview";
            this.Text = "RenderedPreview";
            this.Load += new System.EventHandler(this.RenderedPreview_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pctOutput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctHeightErosion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctNormal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pctOutput;
        private System.Windows.Forms.Button butClose;
        public System.Windows.Forms.Timer Interval;
        public System.Windows.Forms.Timer MoveTick;
        private System.Windows.Forms.Button butRecentre;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pctHeightErosion;
        private System.Windows.Forms.PictureBox pctColor;
        private System.Windows.Forms.PictureBox pctNormal;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}