using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Noise
{
    public partial class ColourMap : Form
    {
        bool updated = false;
        public Core global = new Core();
        public Bitmap input;
        public Bitmap colourmap;
        public Bitmap outputimg;
        public uint[,] output;
        private List<PictureBox> colourdialogs = new List<PictureBox>();
        private List<TrackBar> heights = new List<TrackBar>();
        private List<int> heightvalues = new List<int>();
        private List<Color> finalcolours = new List<Color>();
        uint[,] preview;
        public int trackbarcount = 0;

        public ColourMap()
        {
            InitializeComponent();
            this.ControlBox = false;    //Disable the X button
        }
        public void RefreshMap()    //Update the map if it changes in the main form
        {
            Core functions = new Core();
            preview = functions.CalculateDown(functions.DetermineResizeMethod(input, 256, 256), 256, 256); //Generate a preview
            updated = true;
        }
        public void ColourMap_Load(object sender, EventArgs e)
        {
            refreshRate.Start();
            refreshRate.Enabled = true;
            Core functions = new Core();
            preview = functions.CalculateDown(functions.DetermineResizeMethod(input, 256, 256), 256, 256); //Generate a preview
            heights.Add(heights0);
            heights.Add(heights1);
            heights.Add(heights2);
            heights.Add(heights3);
            heights.Add(heights4);
            heights.Add(heights5);
            heights.Add(heights6);
            heights.Add(heights7);
            colourdialogs.Add(color0);  //Initialize all of the trackbars and colour dialogs - add them to the list so they can be targeted
            colourdialogs.Add(color1);  //in for loops
            colourdialogs.Add(color2);
            colourdialogs.Add(color3);
            colourdialogs.Add(color4);
            colourdialogs.Add(color5);
            colourdialogs.Add(color6);
            colourdialogs.Add(color7);
            trackbarcount = 0;
            heightvalues.Add(0);
            heightvalues.Add(1);
            heightvalues.Add(85);
            heightvalues.Add(170);
            heightvalues.Add(255);
            heightvalues.Add(255);
            heightvalues.Add(heights6.Value);
            heightvalues.Add(heights7.Value);
            finalcolours.Add(Color.FromArgb(255, 159, 205, 205));
            finalcolours.Add(Color.FromArgb(255, 67, 155, 86));
            finalcolours.Add(Color.FromArgb(255, 121, 153, 46));
            finalcolours.Add(Color.FromArgb(255, 176, 121, 96));
            finalcolours.Add(Color.FromArgb(255, 255, 255, 255));
            finalcolours.Add(Color.FromArgb(255, 0, 0, 0));
            finalcolours.Add(Color.FromArgb(255, 0, 0, 0));
            finalcolours.Add(Color.FromArgb(255, 0, 0, 0));
            AddColor();
            AddColor();
            AddColor();
            AddColor();
            AddColor();
            updated = true;
            
        }


        private void RefreshRate_Tick(object sender, EventArgs e)
        {
            int activecounter = 0;
            if (updated == true)            //Update the active counter so that 
            {                               //the correct amount of colours can be processed
                if (heights0.Visible == true)
                {
                    activecounter++;
                }
                if (heights1.Visible == true)
                {
                    activecounter++;
                }
                if (heights2.Visible == true)
                {
                    activecounter++;
                }
                if (heights3.Visible == true)
                {
                    activecounter++;
                }
                if (heights4.Visible == true)
                {
                    activecounter++;
                }
                if (heights5.Visible == true)
                {
                    activecounter++;
                }
                if (heights6.Visible == true)
                {
                    activecounter++;
                }
                if (heights7.Visible == true)
                {
                    activecounter++;
                }
                int[] passheights = new int[activecounter];
                List<Color> passcolours = new List<Color>();
                for (int i = 0; i < activecounter; i++) //PassHeights and passColours are used in generating the actual colour map
                {                                       //They are the final step before being passed to the instance of the Core class
                    passheights[i] = heightvalues[i];
                    passcolours.Add(finalcolours[i]);
                }
                pictureBox1.Image = global.PrepareColourMap(preview, 256, 256, passcolours, passheights);
                updated = false;
            }
        }

        private void AddColor_Click(object sender, EventArgs e)
        {
            AddColor(); //Spawn new trackbar and colour dialog
        }
        private void DelColor_Click(object sender, EventArgs e)
        {
            DelColor(); //Remove trackbar and dialog
        }
        private void AddColor() //Show the new controls on the form, and increase the form width to fit them in
        {
            if (trackbarcount < 8)
            {
                this.Size = new Size(this.Size.Width + 49, this.Size.Height);
                colourdialogs[trackbarcount].Show();
                heights[trackbarcount].Show();
                heights[trackbarcount].Value = heightvalues[trackbarcount];
                colourdialogs[trackbarcount].BackColor = finalcolours[trackbarcount];
                trackbarcount++;
                addColor.Location = new Point(heights[trackbarcount - 1].Location.X, addColor.Location.Y);
                delColor.Location = new Point(heights[trackbarcount - 1].Location.X, delColor.Location.Y);
                butClose.Location = new Point(butClose.Location.X + 49, butClose.Location.Y);
                updated = true;
            }
        }
        private void DelColor() //Hide the new controls on the form and decrease the form width
        {
            if (trackbarcount > 1)
            {
                this.Size = new Size(this.Size.Width - 49, this.Size.Height);
                trackbarcount--;
                colourdialogs[trackbarcount].Hide();
                heights[trackbarcount].Hide();

                addColor.Location = new Point(heights[trackbarcount - 1].Location.X, addColor.Location.Y);
                delColor.Location = new Point(heights[trackbarcount - 1].Location.X, delColor.Location.Y);
                butClose.Location = new Point(butClose.Location.X - 49, butClose.Location.Y);
                updated = true;
            }
        }

        //VALUES CHANGED region - Updates the corresponding value in the list
        private void Heights0_ValueChanged(object sender, EventArgs e)
        {


            if (heights0.Value > heights1.Value)
            {
                heights0.Value = heights1.Value;
            }
            else
            {
                heightvalues[0] = heights0.Value;
            }
            updated = true;
        }

        private void Heights1_ValueChanged(object sender, EventArgs e)
        {
            if (heights1.Value > heights2.Value)
            {
                heights1.Value = heights2.Value;
            }
            else
            {
                heightvalues[1] = heights1.Value;
            }


            if (heights1.Value < heights0.Value)
            {
                heights1.Value = heights0.Value;
            }
            updated = true;
        }

        private void Heights2_ValueChanged(object sender, EventArgs e)
        {
            if (heights2.Value > heights3.Value)
            {
                heights2.Value = heights3.Value;
            }
            else
            {
                heightvalues[2] = heights2.Value;
            }
            if (heights2.Value < heights1.Value)
            {
                heights2.Value = heights1.Value;
            }
            updated = true;
        }

        private void Heights3_ValueChanged(object sender, EventArgs e)
        {
            if (heights3.Value > heights4.Value)
            {
                heights3.Value = heights4.Value;
            }
            else
            {
                heightvalues[3] = heights3.Value;
            }
            if (heights3.Value < heights2.Value)
            {
                heights3.Value = heights2.Value;
            }
            updated = true;
        }

        private void Heights4_ValueChanged(object sender, EventArgs e)
        {
            if (heights4.Value > heights5.Value)
            {
                heights4.Value = heights5.Value;
            }
            else
            {
                heightvalues[4] = heights4.Value;
            }
            if (heights4.Value < heights3.Value)
            {
                heights4.Value = heights3.Value;
            }
            updated = true;
        }

        private void Heights5_ValueChanged(object sender, EventArgs e)
        {
            if (heights5.Value > heights6.Value)
            {
                heights5.Value = heights6.Value;
            }
            else
            {
                heightvalues[5] = heights5.Value;
            }
            if (heights5.Value < heights4.Value)
            {
                heights5.Value = heights4.Value;
            }
            updated = true;
        }

        private void Heights6_ValueChanged(object sender, EventArgs e)
        {
            if (heights6.Value > heights7.Value)
            {
                heights6.Value = heights7.Value;
            }
            else
            {
                heightvalues[6] = heights6.Value;
            }
            if (heights6.Value < heights5.Value)
            {
                heights6.Value = heights5.Value;
            }
            updated = true;
        }

        private void Heights7_ValueChanged(object sender, EventArgs e)
        {
            if (heights7.Value < heights6.Value)
            {
                heights7.Value = heights6.Value;
            }
            updated = true;
        }
        //Manage opening the colour dialog that corresponds to the picture box that was clicked
        private void Color0_Click(object sender, EventArgs e)
        {
           
            ColorDialog colourpicker = new ColorDialog();
            colourpicker.Color = color0.BackColor;
            colourpicker.ShowDialog();

            finalcolours[0] = Color.FromArgb(255, colourpicker.Color.R, colourpicker.Color.G, colourpicker.Color.B);
            color0.BackColor = finalcolours[0];
            updated = true;
        }

        private void Color1_Click(object sender, EventArgs e)
        {
            ColorDialog colourpicker = new ColorDialog();
            colourpicker.Color = color1.BackColor;
            colourpicker.ShowDialog();
            finalcolours[1] = Color.FromArgb(255, colourpicker.Color.R, colourpicker.Color.G, colourpicker.Color.B);
            color1.BackColor = finalcolours[1];
            updated = true;
        }

        private void Color2_Click(object sender, EventArgs e)
        {
            ColorDialog colourpicker = new ColorDialog();
            colourpicker.Color = color2.BackColor;
            colourpicker.ShowDialog();
            finalcolours[2] = Color.FromArgb(255, colourpicker.Color.R, colourpicker.Color.G, colourpicker.Color.B);
            color2.BackColor = finalcolours[2];
            updated = true;
        }

        private void Color3_Click(object sender, EventArgs e)
        {
            ColorDialog colourpicker = new ColorDialog();
            colourpicker.Color = color3.BackColor;
            colourpicker.ShowDialog();
            finalcolours[3] = Color.FromArgb(255, colourpicker.Color.R, colourpicker.Color.G, colourpicker.Color.B);
            color3.BackColor = finalcolours[3];
            updated = true;
        }

        private void Color4_Click(object sender, EventArgs e)
        {
            ColorDialog colourpicker = new ColorDialog();
            colourpicker.Color = color4.BackColor;
            colourpicker.ShowDialog();
            finalcolours[4] = Color.FromArgb(255, colourpicker.Color.R, colourpicker.Color.G, colourpicker.Color.B);
            color4.BackColor = finalcolours[4];
            updated = true;
        }

        private void Color5_Click(object sender, EventArgs e)
        {
            ColorDialog colourpicker = new ColorDialog();
            colourpicker.Color = color5.BackColor;
            colourpicker.ShowDialog();
            finalcolours[5] = Color.FromArgb(255, colourpicker.Color.R, colourpicker.Color.G, colourpicker.Color.B);
            color5.BackColor = finalcolours[5];
            updated = true;
        }

        private void Color6_Click(object sender, EventArgs e)
        {
            ColorDialog colourpicker = new ColorDialog();
            colourpicker.Color = color6.BackColor;
            colourpicker.ShowDialog();
            finalcolours[6] = Color.FromArgb(255, colourpicker.Color.R, colourpicker.Color.G, colourpicker.Color.B);
            color6.BackColor = finalcolours[6];
            updated = true;
        }

        private void Color7_Click(object sender, EventArgs e)
        {
            ColorDialog colourpicker = new ColorDialog();
            colourpicker.Color = color7.BackColor;
            colourpicker.ShowDialog();
            finalcolours[7] = Color.FromArgb(255, colourpicker.Color.R, colourpicker.Color.G, colourpicker.Color.B);
            color7.BackColor = finalcolours[7];
            updated = true;
        }
        private void ExportImage()  //When the done button is clicked, pass back to main form
        {                           //which allows the images to be used in the preview form
            int activecounter = 0;
            if (heights0.Visible == true)
            {
                activecounter++;
            }
            if (heights1.Visible == true)
            {
                activecounter++;
            }
            if (heights2.Visible == true)
            {
                activecounter++;
            }
            if (heights3.Visible == true)
            {
                activecounter++;
            }
            if (heights4.Visible == true)
            {
                activecounter++;
            }
            if (heights5.Visible == true)
            {
                activecounter++;
            }
            if (heights6.Visible == true)
            {
                activecounter++;
            }
            if (heights7.Visible == true)
            {
                activecounter++;
            }
            int[] passheights = new int[activecounter];
            List<Color> passcolours = new List<Color>();
            for (int i = 0; i < activecounter; i++) //Generate the FULL RESOLUTION map instead of a preview
            {
                passheights[i] = heightvalues[i];
                passcolours.Add(finalcolours[i]);
            }

            uint[,] fullres = global.CalculateDown(input, input.Width, input.Height);
            Bitmap img = global.PrepareColourMap(fullres, input.Width, input.Height, passcolours, passheights);
            outputimg = img;
            //img.Save("C:/Users/Casper.MIDDLELENCHBARN/Pictures/Colourmap.png", System.Drawing.Imaging.ImageFormat.Png);
        }

        private void ButClose_Click_1(object sender, EventArgs e)
        {
            ExportImage();
            this.Hide();

        }

        private void ButResetColours_Click(object sender, EventArgs e)
        {
            for (int i = (heights.Capacity - 1); i > -1; i--)   //Set all heights to 255, set all colours to black
            {
                heights[i].Value = 255;
                colourdialogs[i].BackColor = Color.FromArgb(255, 0, 0, 0);
                finalcolours[i] = Color.FromArgb(255, 0, 0, 0);
            }
            
        }
    }
}


