namespace Noise
{
    partial class NormalMap
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
            this.pctNormalMap = new System.Windows.Forms.PictureBox();
            this.trackNormalStrength = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.checkOverlay = new System.Windows.Forms.CheckBox();
            this.interval = new System.Windows.Forms.Timer(this.components);
            this.butClose = new System.Windows.Forms.Button();
            this.checkTransparent = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pctNormalMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackNormalStrength)).BeginInit();
            this.SuspendLayout();
            // 
            // pctNormalMap
            // 
            this.pctNormalMap.Location = new System.Drawing.Point(12, 12);
            this.pctNormalMap.Name = "pctNormalMap";
            this.pctNormalMap.Size = new System.Drawing.Size(383, 383);
            this.pctNormalMap.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pctNormalMap.TabIndex = 1;
            this.pctNormalMap.TabStop = false;
            // 
            // trackNormalStrength
            // 
            this.trackNormalStrength.Location = new System.Drawing.Point(415, 12);
            this.trackNormalStrength.Maximum = 30;
            this.trackNormalStrength.Minimum = 1;
            this.trackNormalStrength.Name = "trackNormalStrength";
            this.trackNormalStrength.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackNormalStrength.Size = new System.Drawing.Size(69, 360);
            this.trackNormalStrength.TabIndex = 2;
            this.trackNormalStrength.Value = 3;
            this.trackNormalStrength.ValueChanged += new System.EventHandler(this.TrackNormalStrength_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.label1.Location = new System.Drawing.Point(401, 375);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 20);
            this.label1.TabIndex = 3;
            this.label1.Tag = "Param";
            this.label1.Text = "Strength";
            // 
            // checkOverlay
            // 
            this.checkOverlay.AutoSize = true;
            this.checkOverlay.Location = new System.Drawing.Point(12, 401);
            this.checkOverlay.Name = "checkOverlay";
            this.checkOverlay.Size = new System.Drawing.Size(194, 24);
            this.checkOverlay.TabIndex = 4;
            this.checkOverlay.Tag = "Param";
            this.checkOverlay.Text = "Overlay On Heightmap";
            this.checkOverlay.UseVisualStyleBackColor = true;
            this.checkOverlay.CheckedChanged += new System.EventHandler(this.CheckOverlay_CheckedChanged);
            // 
            // interval
            // 
            this.interval.Interval = 50;
            this.interval.Tick += new System.EventHandler(this.Interval_Tick);
            // 
            // butClose
            // 
            this.butClose.Location = new System.Drawing.Point(269, 401);
            this.butClose.Name = "butClose";
            this.butClose.Size = new System.Drawing.Size(126, 31);
            this.butClose.TabIndex = 5;
            this.butClose.Tag = "Param";
            this.butClose.Text = "Apply";
            this.butClose.UseVisualStyleBackColor = true;
            this.butClose.Click += new System.EventHandler(this.ButClose_Click);
            // 
            // checkTransparent
            // 
            this.checkTransparent.AutoSize = true;
            this.checkTransparent.Location = new System.Drawing.Point(12, 431);
            this.checkTransparent.Name = "checkTransparent";
            this.checkTransparent.Size = new System.Drawing.Size(193, 24);
            this.checkTransparent.TabIndex = 6;
            this.checkTransparent.Tag = "Param";
            this.checkTransparent.Text = "Generate Transparent";
            this.checkTransparent.UseVisualStyleBackColor = true;
            this.checkTransparent.CheckedChanged += new System.EventHandler(this.CheckTransparent_CheckedChanged);
            // 
            // NormalMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 468);
            this.Controls.Add(this.checkTransparent);
            this.Controls.Add(this.butClose);
            this.Controls.Add(this.checkOverlay);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.trackNormalStrength);
            this.Controls.Add(this.pctNormalMap);
            this.Name = "NormalMap";
            this.Text = "NormalMap";
            this.Load += new System.EventHandler(this.NormalMap_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pctNormalMap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackNormalStrength)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pctNormalMap;
        private System.Windows.Forms.TrackBar trackNormalStrength;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkOverlay;
        private System.Windows.Forms.Button butClose;
        private System.Windows.Forms.CheckBox checkTransparent;
        public System.Windows.Forms.Timer interval;
    }
}