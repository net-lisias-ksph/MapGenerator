namespace Noise
{
    partial class ErosionMenu
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
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.trackErosionCycles = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.trackErosionCount = new System.Windows.Forms.TrackBar();
            this.label7 = new System.Windows.Forms.Label();
            this.trackErosionDepth = new System.Windows.Forms.TrackBar();
            this.butErodeRivers = new System.Windows.Forms.Button();
            this.morphMode = new System.Windows.Forms.ComboBox();
            this.butMorphErode = new System.Windows.Forms.Button();
            this.Interval = new System.Windows.Forms.Timer(this.components);
            this.butClose = new System.Windows.Forms.Button();
            this.butRefresh = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pctOutput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackErosionCycles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackErosionCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackErosionDepth)).BeginInit();
            this.SuspendLayout();
            // 
            // pctOutput
            // 
            this.pctOutput.Location = new System.Drawing.Point(53, 47);
            this.pctOutput.Name = "pctOutput";
            this.pctOutput.Size = new System.Drawing.Size(680, 680);
            this.pctOutput.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pctOutput.TabIndex = 0;
            this.pctOutput.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.label5.Location = new System.Drawing.Point(757, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(293, 37);
            this.label5.TabIndex = 44;
            this.label5.Text = "Rainfall and Glacial";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.label1.Location = new System.Drawing.Point(757, 527);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(217, 37);
            this.label1.TabIndex = 45;
            this.label1.Text = "Morphological";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.label2.Location = new System.Drawing.Point(760, 567);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(569, 40);
            this.label2.TabIndex = 46;
            this.label2.Text = "Useful for reducing debris left over from rainfall erosion.\r\nMorphological operat" +
    "ions use a mathematical formula to distort the entire image";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.label3.Location = new System.Drawing.Point(760, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(443, 40);
            this.label3.TabIndex = 47;
            this.label3.Text = "Simulates surface erosion from rainfall or glaciers.\r\nRainfall operations trace t" +
    "he path a stream or river might take.";
            // 
            // trackErosionCycles
            // 
            this.trackErosionCycles.LargeChange = 25;
            this.trackErosionCycles.Location = new System.Drawing.Point(924, 143);
            this.trackErosionCycles.Maximum = 100;
            this.trackErosionCycles.Minimum = 1;
            this.trackErosionCycles.Name = "trackErosionCycles";
            this.trackErosionCycles.Size = new System.Drawing.Size(313, 69);
            this.trackErosionCycles.SmallChange = 5;
            this.trackErosionCycles.TabIndex = 48;
            this.trackErosionCycles.TickFrequency = 5;
            this.trackErosionCycles.Value = 20;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label4.Location = new System.Drawing.Point(759, 143);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 29);
            this.label4.TabIndex = 49;
            this.label4.Tag = "Param";
            this.label4.Text = "Cycles";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label6.Location = new System.Drawing.Point(759, 199);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(159, 29);
            this.label6.TabIndex = 51;
            this.label6.Tag = "Param";
            this.label6.Text = "Stream Count";
            // 
            // trackErosionCount
            // 
            this.trackErosionCount.LargeChange = 50;
            this.trackErosionCount.Location = new System.Drawing.Point(924, 199);
            this.trackErosionCount.Maximum = 1000;
            this.trackErosionCount.Minimum = 1;
            this.trackErosionCount.Name = "trackErosionCount";
            this.trackErosionCount.Size = new System.Drawing.Size(313, 69);
            this.trackErosionCount.SmallChange = 10;
            this.trackErosionCount.TabIndex = 50;
            this.trackErosionCount.TickFrequency = 25;
            this.trackErosionCount.Value = 100;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label7.Location = new System.Drawing.Point(759, 253);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(160, 29);
            this.label7.TabIndex = 53;
            this.label7.Tag = "Param";
            this.label7.Text = "Stream Depth";
            // 
            // trackErosionDepth
            // 
            this.trackErosionDepth.Location = new System.Drawing.Point(924, 253);
            this.trackErosionDepth.Maximum = 20;
            this.trackErosionDepth.Minimum = 1;
            this.trackErosionDepth.Name = "trackErosionDepth";
            this.trackErosionDepth.Size = new System.Drawing.Size(313, 69);
            this.trackErosionDepth.TabIndex = 52;
            this.trackErosionDepth.Value = 1;
            // 
            // butErodeRivers
            // 
            this.butErodeRivers.Location = new System.Drawing.Point(764, 306);
            this.butErodeRivers.Name = "butErodeRivers";
            this.butErodeRivers.Size = new System.Drawing.Size(473, 56);
            this.butErodeRivers.TabIndex = 55;
            this.butErodeRivers.Tag = "Param";
            this.butErodeRivers.Text = "Simulate Erosion";
            this.butErodeRivers.UseVisualStyleBackColor = true;
            this.butErodeRivers.Click += new System.EventHandler(this.ButErodeRivers_Click);
            // 
            // morphMode
            // 
            this.morphMode.FormattingEnabled = true;
            this.morphMode.Items.AddRange(new object[] {
            "Erode",
            "Dilate"});
            this.morphMode.Location = new System.Drawing.Point(764, 626);
            this.morphMode.Name = "morphMode";
            this.morphMode.Size = new System.Drawing.Size(473, 28);
            this.morphMode.TabIndex = 56;
            this.morphMode.Tag = "Param";
            this.morphMode.Text = "SELECT EROSION MODE";
            // 
            // butMorphErode
            // 
            this.butMorphErode.Location = new System.Drawing.Point(764, 671);
            this.butMorphErode.Name = "butMorphErode";
            this.butMorphErode.Size = new System.Drawing.Size(473, 56);
            this.butMorphErode.TabIndex = 57;
            this.butMorphErode.Tag = "Param";
            this.butMorphErode.Text = "Simulate Erosion";
            this.butMorphErode.UseVisualStyleBackColor = true;
            this.butMorphErode.Click += new System.EventHandler(this.ButMorphErode_Click);
            // 
            // Interval
            // 
            this.Interval.Interval = 20;
            this.Interval.Tick += new System.EventHandler(this.Interval_Tick);
            // 
            // butClose
            // 
            this.butClose.Location = new System.Drawing.Point(1298, 12);
            this.butClose.Name = "butClose";
            this.butClose.Size = new System.Drawing.Size(109, 41);
            this.butClose.TabIndex = 58;
            this.butClose.Tag = "Param";
            this.butClose.Text = "Done";
            this.butClose.UseVisualStyleBackColor = true;
            this.butClose.Click += new System.EventHandler(this.ButClose_Click);
            // 
            // butRefresh
            // 
            this.butRefresh.Location = new System.Drawing.Point(62, 677);
            this.butRefresh.Name = "butRefresh";
            this.butRefresh.Size = new System.Drawing.Size(109, 44);
            this.butRefresh.TabIndex = 59;
            this.butRefresh.Tag = "Param";
            this.butRefresh.Text = "Clear";
            this.butRefresh.UseVisualStyleBackColor = true;
            this.butRefresh.Click += new System.EventHandler(this.ButRefresh_Click);
            // 
            // ErosionMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1419, 785);
            this.Controls.Add(this.butRefresh);
            this.Controls.Add(this.butClose);
            this.Controls.Add(this.butMorphErode);
            this.Controls.Add(this.morphMode);
            this.Controls.Add(this.butErodeRivers);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.trackErosionDepth);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.trackErosionCount);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.trackErosionCycles);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.pctOutput);
            this.Name = "ErosionMenu";
            this.Text = "Erosion Editor";
            this.Load += new System.EventHandler(this.Erosion_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pctOutput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackErosionCycles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackErosionCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackErosionDepth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pctOutput;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar trackErosionCycles;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TrackBar trackErosionCount;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TrackBar trackErosionDepth;
        private System.Windows.Forms.Button butErodeRivers;
        private System.Windows.Forms.ComboBox morphMode;
        private System.Windows.Forms.Button butMorphErode;
        private System.Windows.Forms.Button butClose;
        private System.Windows.Forms.Button butRefresh;
        public System.Windows.Forms.Timer Interval;
    }
}