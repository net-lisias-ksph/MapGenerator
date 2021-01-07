using System;
using System.Drawing;
using System.Windows.Forms;

namespace Noise
{
    public partial class NormalMap : Form
    {
        bool updated;
        public bool transparent = false;
        public Core global = new Core();
        public double strength;
        public bool overlay = false;
        private Bitmap normalmap = new Bitmap(256, 256);
        public Bitmap source;
        private Bitmap preview;
        public Bitmap output;
        public NormalMap()
        {
            InitializeComponent();
            this.ControlBox = false;    //Disable X button
        }

        private void NormalMap_Load(object sender, EventArgs e)
        {
            updated = true;
            preview = global.DetermineResizeMethod(source, 512, 512);  //Generate a low resolution preview
            strength = trackNormalStrength.Value;
            interval.Enabled = true;
        }
        public void RefreshMap()
        {
            
            preview = global.DetermineResizeMethod(source, 512, 512);  //Refresh low resolution preview
            updated = true;
        }

        private void Interval_Tick(object sender, EventArgs e)
        {
            if (updated == true)
            {
                normalmap = global.GenerateNormalMap(preview, overlay, strength, transparent);   //Every time the normal map needs to be redrawn, it is passed off to Global
                pctNormalMap.Image = normalmap;
                updated = false;
            }
        }

        private void TrackNormalStrength_ValueChanged(object sender, EventArgs e)   //Update the normal map strength
        {
            strength = trackNormalStrength.Value;
            updated = true;
        }

        private void CheckOverlay_CheckedChanged(object sender, EventArgs e)    //Update the global variables to account for the
        {                                                                       //overlay mode
            if (checkOverlay.Checked == true)
            {
                transparent = false;
                overlay = true;
                checkTransparent.Checked = false;
            }
            else
            {
                overlay = false;
            }
            updated = true;
        }
        public void Export()
        {
            output = global.GenerateNormalMap(source, overlay, strength, transparent);  //Render FULL RESOLUTION image
        }
        private void ButClose_Click(object sender, EventArgs e)
        {
            Export();
            this.Hide();
        }

        private void ButRefresh_Click(object sender, EventArgs e)
        {
            RefreshMap();
        }

        private void CheckTransparent_CheckedChanged(object sender, EventArgs e)    //Update global variables to account for
        {                                                                           //the transparent mode
            if (checkTransparent.Checked == true)               
            {
                transparent = true;
                overlay = false;
                checkOverlay.Checked = false;
            }
            else
            {
                transparent = false;
            }
            updated = true;
        }
    }
}


