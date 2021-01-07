namespace Noise
{
    partial class ColourMap
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
            this.refreshRate = new System.Windows.Forms.Timer(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.addColor = new System.Windows.Forms.Button();
            this.heights0 = new System.Windows.Forms.TrackBar();
            this.heights1 = new System.Windows.Forms.TrackBar();
            this.heights2 = new System.Windows.Forms.TrackBar();
            this.heights3 = new System.Windows.Forms.TrackBar();
            this.heights4 = new System.Windows.Forms.TrackBar();
            this.heights5 = new System.Windows.Forms.TrackBar();
            this.heights6 = new System.Windows.Forms.TrackBar();
            this.heights7 = new System.Windows.Forms.TrackBar();
            this.color0 = new System.Windows.Forms.PictureBox();
            this.color1 = new System.Windows.Forms.PictureBox();
            this.color2 = new System.Windows.Forms.PictureBox();
            this.color3 = new System.Windows.Forms.PictureBox();
            this.color4 = new System.Windows.Forms.PictureBox();
            this.color5 = new System.Windows.Forms.PictureBox();
            this.color6 = new System.Windows.Forms.PictureBox();
            this.color7 = new System.Windows.Forms.PictureBox();
            this.delColor = new System.Windows.Forms.Button();
            this.butClose = new System.Windows.Forms.Button();
            this.butResetColours = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.heights0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.heights1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.heights2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.heights3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.heights4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.heights5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.heights6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.heights7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.color0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.color1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.color2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.color3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.color4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.color5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.color6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.color7)).BeginInit();
            this.SuspendLayout();
            // 
            // refreshRate
            // 
            this.refreshRate.Interval = 20;
            this.refreshRate.Tick += new System.EventHandler(this.RefreshRate_Tick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 66);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(383, 383);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // addColor
            // 
            this.addColor.Location = new System.Drawing.Point(419, 378);
            this.addColor.Name = "addColor";
            this.addColor.Size = new System.Drawing.Size(67, 34);
            this.addColor.TabIndex = 5;
            this.addColor.Tag = "Param";
            this.addColor.Text = "Add";
            this.addColor.UseVisualStyleBackColor = true;
            this.addColor.Click += new System.EventHandler(this.AddColor_Click);
            // 
            // heights0
            // 
            this.heights0.Location = new System.Drawing.Point(429, 123);
            this.heights0.Maximum = 255;
            this.heights0.Name = "heights0";
            this.heights0.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.heights0.Size = new System.Drawing.Size(69, 252);
            this.heights0.TabIndex = 6;
            this.heights0.TickFrequency = 5;
            this.heights0.ValueChanged += new System.EventHandler(this.Heights0_ValueChanged);
            // 
            // heights1
            // 
            this.heights1.Location = new System.Drawing.Point(504, 123);
            this.heights1.Maximum = 255;
            this.heights1.Name = "heights1";
            this.heights1.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.heights1.Size = new System.Drawing.Size(69, 252);
            this.heights1.TabIndex = 7;
            this.heights1.TickFrequency = 5;
            this.heights1.Value = 255;
            this.heights1.Visible = false;
            this.heights1.ValueChanged += new System.EventHandler(this.Heights1_ValueChanged);
            // 
            // heights2
            // 
            this.heights2.Location = new System.Drawing.Point(579, 123);
            this.heights2.Maximum = 255;
            this.heights2.Name = "heights2";
            this.heights2.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.heights2.Size = new System.Drawing.Size(69, 252);
            this.heights2.TabIndex = 8;
            this.heights2.TickFrequency = 5;
            this.heights2.Value = 255;
            this.heights2.Visible = false;
            this.heights2.ValueChanged += new System.EventHandler(this.Heights2_ValueChanged);
            // 
            // heights3
            // 
            this.heights3.Location = new System.Drawing.Point(654, 123);
            this.heights3.Maximum = 255;
            this.heights3.Name = "heights3";
            this.heights3.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.heights3.Size = new System.Drawing.Size(69, 252);
            this.heights3.TabIndex = 9;
            this.heights3.TickFrequency = 5;
            this.heights3.Value = 255;
            this.heights3.Visible = false;
            this.heights3.ValueChanged += new System.EventHandler(this.Heights3_ValueChanged);
            // 
            // heights4
            // 
            this.heights4.Location = new System.Drawing.Point(729, 123);
            this.heights4.Maximum = 255;
            this.heights4.Name = "heights4";
            this.heights4.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.heights4.Size = new System.Drawing.Size(69, 252);
            this.heights4.TabIndex = 10;
            this.heights4.TickFrequency = 5;
            this.heights4.Value = 255;
            this.heights4.Visible = false;
            this.heights4.ValueChanged += new System.EventHandler(this.Heights4_ValueChanged);
            // 
            // heights5
            // 
            this.heights5.Location = new System.Drawing.Point(804, 123);
            this.heights5.Maximum = 255;
            this.heights5.Name = "heights5";
            this.heights5.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.heights5.Size = new System.Drawing.Size(69, 252);
            this.heights5.TabIndex = 11;
            this.heights5.TickFrequency = 5;
            this.heights5.Value = 255;
            this.heights5.Visible = false;
            this.heights5.ValueChanged += new System.EventHandler(this.Heights5_ValueChanged);
            // 
            // heights6
            // 
            this.heights6.Location = new System.Drawing.Point(879, 123);
            this.heights6.Maximum = 255;
            this.heights6.Name = "heights6";
            this.heights6.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.heights6.Size = new System.Drawing.Size(69, 252);
            this.heights6.TabIndex = 12;
            this.heights6.TickFrequency = 5;
            this.heights6.Value = 255;
            this.heights6.Visible = false;
            this.heights6.ValueChanged += new System.EventHandler(this.Heights6_ValueChanged);
            // 
            // heights7
            // 
            this.heights7.Location = new System.Drawing.Point(954, 123);
            this.heights7.Maximum = 255;
            this.heights7.Name = "heights7";
            this.heights7.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.heights7.Size = new System.Drawing.Size(69, 252);
            this.heights7.TabIndex = 13;
            this.heights7.TickFrequency = 5;
            this.heights7.Value = 255;
            this.heights7.Visible = false;
            this.heights7.ValueChanged += new System.EventHandler(this.Heights7_ValueChanged);
            // 
            // color0
            // 
            this.color0.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.color0.Location = new System.Drawing.Point(419, 66);
            this.color0.Name = "color0";
            this.color0.Size = new System.Drawing.Size(51, 51);
            this.color0.TabIndex = 14;
            this.color0.TabStop = false;
            this.color0.Click += new System.EventHandler(this.Color0_Click);
            // 
            // color1
            // 
            this.color1.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.color1.Location = new System.Drawing.Point(492, 66);
            this.color1.Name = "color1";
            this.color1.Size = new System.Drawing.Size(51, 51);
            this.color1.TabIndex = 15;
            this.color1.TabStop = false;
            this.color1.Visible = false;
            this.color1.Click += new System.EventHandler(this.Color1_Click);
            // 
            // color2
            // 
            this.color2.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.color2.Location = new System.Drawing.Point(567, 66);
            this.color2.Name = "color2";
            this.color2.Size = new System.Drawing.Size(51, 51);
            this.color2.TabIndex = 16;
            this.color2.TabStop = false;
            this.color2.Visible = false;
            this.color2.Click += new System.EventHandler(this.Color2_Click);
            // 
            // color3
            // 
            this.color3.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.color3.Location = new System.Drawing.Point(642, 66);
            this.color3.Name = "color3";
            this.color3.Size = new System.Drawing.Size(51, 51);
            this.color3.TabIndex = 17;
            this.color3.TabStop = false;
            this.color3.Visible = false;
            this.color3.Click += new System.EventHandler(this.Color3_Click);
            // 
            // color4
            // 
            this.color4.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.color4.Location = new System.Drawing.Point(715, 66);
            this.color4.Name = "color4";
            this.color4.Size = new System.Drawing.Size(51, 51);
            this.color4.TabIndex = 18;
            this.color4.TabStop = false;
            this.color4.Visible = false;
            this.color4.Click += new System.EventHandler(this.Color4_Click);
            // 
            // color5
            // 
            this.color5.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.color5.Location = new System.Drawing.Point(791, 66);
            this.color5.Name = "color5";
            this.color5.Size = new System.Drawing.Size(51, 51);
            this.color5.TabIndex = 19;
            this.color5.TabStop = false;
            this.color5.Visible = false;
            this.color5.Click += new System.EventHandler(this.Color5_Click);
            // 
            // color6
            // 
            this.color6.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.color6.Location = new System.Drawing.Point(868, 66);
            this.color6.Name = "color6";
            this.color6.Size = new System.Drawing.Size(51, 51);
            this.color6.TabIndex = 20;
            this.color6.TabStop = false;
            this.color6.Visible = false;
            this.color6.Click += new System.EventHandler(this.Color6_Click);
            // 
            // color7
            // 
            this.color7.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.color7.Location = new System.Drawing.Point(942, 66);
            this.color7.Name = "color7";
            this.color7.Size = new System.Drawing.Size(51, 51);
            this.color7.TabIndex = 21;
            this.color7.TabStop = false;
            this.color7.Visible = false;
            this.color7.Click += new System.EventHandler(this.Color7_Click);
            // 
            // delColor
            // 
            this.delColor.Location = new System.Drawing.Point(419, 416);
            this.delColor.Name = "delColor";
            this.delColor.Size = new System.Drawing.Size(67, 34);
            this.delColor.TabIndex = 23;
            this.delColor.Tag = "Param";
            this.delColor.Text = "Del";
            this.delColor.UseVisualStyleBackColor = true;
            this.delColor.Click += new System.EventHandler(this.DelColor_Click);
            // 
            // butClose
            // 
            this.butClose.Location = new System.Drawing.Point(405, 12);
            this.butClose.Name = "butClose";
            this.butClose.Size = new System.Drawing.Size(109, 41);
            this.butClose.TabIndex = 59;
            this.butClose.Tag = "Param";
            this.butClose.Text = "Done";
            this.butClose.UseVisualStyleBackColor = true;
            this.butClose.Click += new System.EventHandler(this.ButClose_Click_1);
            // 
            // butResetColours
            // 
            this.butResetColours.Location = new System.Drawing.Point(12, 455);
            this.butResetColours.Name = "butResetColours";
            this.butResetColours.Size = new System.Drawing.Size(213, 35);
            this.butResetColours.TabIndex = 60;
            this.butResetColours.Tag = "Param";
            this.butResetColours.Text = "Reset Colours";
            this.butResetColours.UseVisualStyleBackColor = true;
            this.butResetColours.Click += new System.EventHandler(this.ButResetColours_Click);
            // 
            // ColourMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 581);
            this.Controls.Add(this.butResetColours);
            this.Controls.Add(this.butClose);
            this.Controls.Add(this.delColor);
            this.Controls.Add(this.color7);
            this.Controls.Add(this.color6);
            this.Controls.Add(this.color5);
            this.Controls.Add(this.color4);
            this.Controls.Add(this.color3);
            this.Controls.Add(this.color2);
            this.Controls.Add(this.color1);
            this.Controls.Add(this.color0);
            this.Controls.Add(this.heights7);
            this.Controls.Add(this.heights6);
            this.Controls.Add(this.heights5);
            this.Controls.Add(this.heights4);
            this.Controls.Add(this.heights3);
            this.Controls.Add(this.heights2);
            this.Controls.Add(this.heights1);
            this.Controls.Add(this.heights0);
            this.Controls.Add(this.addColor);
            this.Controls.Add(this.pictureBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ColourMap";
            this.Text = "ColourMap";
            this.Load += new System.EventHandler(this.ColourMap_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.heights0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.heights1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.heights2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.heights3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.heights4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.heights5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.heights6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.heights7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.color0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.color1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.color2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.color3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.color4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.color5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.color6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.color7)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button addColor;
        private System.Windows.Forms.TrackBar heights0;
        private System.Windows.Forms.TrackBar heights1;
        private System.Windows.Forms.TrackBar heights2;
        private System.Windows.Forms.TrackBar heights3;
        private System.Windows.Forms.TrackBar heights4;
        private System.Windows.Forms.TrackBar heights5;
        private System.Windows.Forms.TrackBar heights6;
        private System.Windows.Forms.TrackBar heights7;
        private System.Windows.Forms.PictureBox color0;
        private System.Windows.Forms.PictureBox color1;
        private System.Windows.Forms.PictureBox color2;
        private System.Windows.Forms.PictureBox color3;
        private System.Windows.Forms.PictureBox color4;
        private System.Windows.Forms.PictureBox color5;
        private System.Windows.Forms.PictureBox color6;
        private System.Windows.Forms.PictureBox color7;
        private System.Windows.Forms.Button delColor;
        private System.Windows.Forms.Button butClose;
        private System.Windows.Forms.Button butResetColours;
        public System.Windows.Forms.Timer refreshRate;
    }
}