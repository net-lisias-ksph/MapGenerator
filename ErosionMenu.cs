using System;
using System.Drawing;
using System.Windows.Forms;

namespace Noise
{
    public partial class ErosionMenu : Form
    {
        Core operations = new Core();
        public uint[,] originalinput;
        public uint[,] input;
        public uint[,] output;
        Erosion erosionoperations = new Erosion();
        public Bitmap activeimage;
        public Bitmap outputimg;
        private bool firststart = true;
        bool erode = true;
        public ErosionMenu()
        {
            InitializeComponent();
        }


        public void RefreshMap()    //Update the map if a new image was generated
        {
            input = originalinput;
            activeimage = operations.CalculateUp(originalinput, originalinput.GetLength(1), originalinput.GetLength(1));
            operations.globalarray = originalinput;
        }
        private void Erosion_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;    //Disable X button
            Interval.Enabled = true;
            Interval.Start();
            operations.interval.Enabled = false;
        }

        private void ButErodeRivers_Click(object sender, EventArgs e)   //Begin river generation
        {
            
            int cycles = trackErosionCycles.Value;
            int count = trackErosionCount.Value;
            int depth = trackErosionDepth.Value;
            input = operations.ExternalGenerateRivers(input, cycles, count, depth, erosionoperations);
            int size = input.GetLength(0);
            activeimage = operations.CalculateUp(input, size, size);    //Convert output from river generation to image
        }

        private void ButMorphErode_Click(object sender, EventArgs e)
        {
            if (morphMode.Text == "Erode")  //Begin morphological generation
            {
                erode = true;
            }
            if (morphMode.Text == "Dilate")
            {
                erode = false;
            }
            operations.globalarray = input; //Erode/Dilate, then convert output to image
            operations.ExternalMorphologicalErosion(input, erode);
            input = operations.globalarray;
            int size = input.GetLength(0);
            activeimage = operations.CalculateUp(input, size, size);
        }

        private void Interval_Tick(object sender, EventArgs e)
        {
            if (pctOutput.Image != activeimage)
            {
                pctOutput.Image = activeimage;  
            }
            
            if (firststart == true && this.Visible == true && input != null)    //Clear the image and revert to the input
            {
                activeimage = operations.CalculateUp(input, input.GetLength(1), input.GetLength(1));
                firststart = false;
            }
        }
        private void Export()
        {
            outputimg = activeimage;    //Update the main form with the output from this one
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
    }
}
