using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Noise
{
    public partial class RenderedPreview : Form
    {
        public bool updated;
        public Bitmap active;
        public Bitmap inputheight;
        public Bitmap inputheighterosion;
        public Bitmap inputcolour;
        public Bitmap inputnormal;
        public Bitmap overlayednormal;
        string currenttag = "";
        Point originalPointHeight2; //Original locations for the pictureboxes
        Point originalPointCol;
        Point originalPointNor;
        int startx;
        int starty;
        bool isMoving = false;
        public RenderedPreview()
        {
            InitializeComponent();
            Interval.Start();
            Interval.Enabled = true;    //Start the timer, and update the maps on form load
        }

        private void RenderedPreview_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
            this.MouseWheel += PctOutput_MouseWheel;
            //originalPointHeight = pctHeightOriginal.Location;
            originalPointHeight2 = pctHeightErosion.Location;
            originalPointCol = pctColor.Location;
            originalPointNor = pctNormal.Location;

            pctOutput.Tag = "HeightMap1";   //Set the tags and original points so that they can be processed when the form is open
            pctNormal.Tag = "NormalMap";
            pctHeightErosion.Tag = "HeightMap2";
            pctColor.Tag = "ColourMap";
        }

        private void ButClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        public void RefreshMap()
        {
            updated = true; //Update the maps when the form is reopened
            pctOutput.Image = active;
            pctHeightErosion.Image = inputheighterosion;
            pctColor.Image = inputcolour;
            pctNormal.Image = inputnormal;
        }
        private void Interval_Tick(object sender, EventArgs e)
        {
            if (updated == true)
            {
                if (inputheight != null && inputcolour != null && inputnormal != null)
                {
                    pctOutput.Image = inputheight;      //Update the maps as soon as they are changed, then disable the timer
                    if (inputheighterosion != null)
                    {
                        pctHeightErosion.Image = inputheighterosion;
                    }   

                    pctNormal.Image = inputnormal;
                    pctColor.Image = inputcolour;
                    updated = false;
                }
                updated = false;
            }



        }
        private void PctOutput_MouseWheel(object sender, MouseEventArgs e)  //Increase or decrease the zoom
        {
            if (e.Delta > 0 && pctOutput.Width < 8192)  //Zooming out
            {
                pctOutput.SendToBack();
                pctOutput.Location = new Point(pctOutput.Location.X - pctOutput.Width / 2, pctOutput.Location.Y - pctOutput.Height / 2);
                pctOutput.Size = new Size(pctOutput.Width * 2, pctOutput.Height * 2);
            }
            if (e.Delta < 0 && pctOutput.Width > 256)   //Zooming in
            {
                pctOutput.SendToBack();
                pctOutput.Size = new Size(pctOutput.Width / 2, pctOutput.Height / 2);
                pctOutput.Location = new Point(pctOutput.Location.X + pctOutput.Width / 2, pctOutput.Location.Y + pctOutput.Height / 2);

            }
        }
        #region PctOutputMove
        private void PctOutput_MouseDown(object sender, MouseEventArgs e)   //PctOutput is selected
        {
            isMoving = true;
            startx = e.X;
            starty = e.Y;
        }

        private void PctOutput_MouseMove(object sender, MouseEventArgs e)   //PctOutput is being moved
        {
            if (isMoving == true)
            {
                pctOutput.Top = pctOutput.Top + (e.Y - starty);
                pctOutput.Left = pctOutput.Left + (e.X - startx);
            }

        }

        private void PctOutput_MouseUp(object sender, MouseEventArgs e) //PctOutput has been moved, so unselect it
        {
            isMoving = false;
        }
        #endregion

        private void ButRecentre_Click(object sender, EventArgs e)  //Return the output image to its original location
        {
            pctOutput.Width = this.Width - 250;
            pctOutput.Height = this.Width - 250; //Make sure it is square
            pctOutput.Location = new Point(8, 8);
        }

        #region Positioning
        private void PctHeightErosion_MouseDown(object sender, MouseEventArgs e)
        {
            isMoving = true;
            startx = e.X;
            starty = e.Y;
        }

        private void PctHeightErosion_MouseUp(object sender, MouseEventArgs e)  //Process movements for the rest of the pictureboxes
        {
            if (pctOutput.Bounds.IntersectsWith(pctHeightErosion.Bounds))
            {
                ProcessNewImage(pctHeightErosion);
            }
            else
            {
                pctHeightErosion.Location = originalPointHeight2;
            }
            isMoving = false;
        }

        private void PctHeightErosion_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMoving == true)
            {
                pctHeightErosion.Top = pctHeightErosion.Top + (e.Y - starty);
                pctHeightErosion.Left = pctHeightErosion.Left + (e.X - startx);
            }
        }

        private void PctColor_MouseDown(object sender, MouseEventArgs e)
        {
            isMoving = true;
            startx = e.X;
            starty = e.Y;
        }

        private void PctColor_MouseUp(object sender, MouseEventArgs e)
        {
            if (pctOutput.Bounds.IntersectsWith(pctColor.Bounds))
            {
                ProcessNewImage(pctColor);
            }
            else
            {
                pctColor.Location = originalPointCol;
            }
            isMoving = false;
        }

        private void PctColor_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMoving == true)
            {
                pctColor.Top = pctColor.Top + (e.Y - starty);
                pctColor.Left = pctColor.Left + (e.X - startx);
            }
        }

        private void PctNormal_MouseDown(object sender, MouseEventArgs e)
        {
            isMoving = true;
            startx = e.X;
            starty = e.Y;
        }

        private void PctNormal_MouseUp(object sender, MouseEventArgs e)
        {
            if (pctOutput.Bounds.IntersectsWith(pctNormal.Bounds))
            {
                ProcessNewImage(pctNormal);

            }
            else
            {
                pctNormal.Location = originalPointNor;
            }
            isMoving = false;
        }

        private void PctNormal_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMoving == true)
            {
                pctNormal.Top = pctNormal.Top + (e.Y - starty);
                pctNormal.Left = pctNormal.Left + (e.X - startx);
            }
        }
        #endregion
        public void ResetTags() //Set the tags to the corresponding images, so that when the form is closed and the images are reset, the tags match
        {
            pctOutput.Tag = "HeightMap1";
            pctNormal.Tag = "NormalMap";
            pctHeightErosion.Tag = "HeightMap2";
            pctColor.Tag = "ColourMap";
        }
        private void ProcessNewImage(PictureBox moved)              //Having to clone everything otherwise the tags can be assigned to different references
        {                                                           //It is not memory inefficient as the old Bitmap is disposed by the garbage collector anyway, but cloning allows a new tag to be assigned to the new image
            if ((string)pctOutput.Tag == "OverlayNormalMap")
            {
                pctOutput.Image = inputnormal.Clone() as Bitmap;
                pctOutput.Tag = "NormalMap";
            }
           
            currenttag = pctOutput.Tag.ToString().Clone() as string;    //Current and movedtag are temporary storage for the output and moved image tags before they are switched
            string movedtag = (moved.Tag.ToString()).Clone() as string;
            Bitmap tempmoved = new Bitmap(1, 1);
            Bitmap current = pctOutput.Image.Clone() as Bitmap;
            Point originalpoint = new Point(0, 0);
            if (moved.Name == "pctNormal")
            {
                originalpoint = originalPointNor;
                tempmoved = pctNormal.Image.Clone() as Bitmap;
            }
            if (moved.Name == "pctColor")
            {   
                originalpoint = originalPointCol;               //The original point must be set to the image currently being moved so that the program knows which tags to swap
                tempmoved = pctColor.Image.Clone() as Bitmap;
            }
            if (moved.Name == "pctHeightErosion")
            {
                originalpoint = originalPointHeight2;   
                if (pctHeightErosion.Image != null)
                {
                    tempmoved = pctHeightErosion.Image.Clone() as Bitmap;
                }
            }



            if ((string)moved.Tag == "NormalMap")   //NormalMap is a special case as it can be overlayed on top of the other maps
            {
                Bitmap currentoverlay = current.Clone() as Bitmap;
                Graphics a = Graphics.FromImage(currentoverlay);
                a.DrawImage(inputnormal.Clone() as Bitmap, new Rectangle(0, 0, currentoverlay.Width, currentoverlay.Height));
                moved.Tag = currenttag.Clone() as string;
                moved.Image = current.Clone() as Bitmap;
                moved.Location = originalpoint;

                DialogResult normaloverlay = MessageBox.Show("Overlay the normal map?", "Action Required", MessageBoxButtons.YesNo);    //Decide whether to overlay the map or not
                if (normaloverlay == DialogResult.Yes)
                {
                    pctOutput.Image = currentoverlay.Clone() as Bitmap;
                    pctOutput.Tag = "OverlayNormalMap";
                    moved.Image = current.Clone() as Bitmap;
                }
                if (normaloverlay == DialogResult.No)   //Process as normal, and swap the images/tags
                {
                    moved.Image = current.Clone() as Bitmap;
                    pctOutput.Image = tempmoved.Clone() as Bitmap;
                    pctOutput.Tag = movedtag.Clone() as string;
                    moved.Tag = currenttag.Clone() as string;
                }
                return;
            }
            if ((string)moved.Tag == "HeightMap1")  //Base heightmap    
            {
                pctOutput.Image = tempmoved.Clone() as Bitmap;
                moved.Image = current.Clone() as Bitmap;

                pctOutput.Tag = movedtag.Clone() as string;
                moved.Tag = currenttag.Clone() as string;

                moved.Location = originalpoint;
                return;

            }
            if ((string)moved.Tag == "HeightMap2")  //Erosion heightmap
            {
                pctOutput.Image = tempmoved.Clone() as Bitmap;
                moved.Image = current.Clone() as Bitmap;

                pctOutput.Tag = movedtag.Clone() as string;
                moved.Tag = currenttag.Clone() as string;

                moved.Location = originalpoint;
                return;
            }
            if ((string)moved.Tag == "ColourMap")   //Colourmap
            {
                pctOutput.Image = tempmoved.Clone() as Bitmap;
                moved.Image = current.Clone() as Bitmap;

                pctOutput.Tag = movedtag.Clone() as string;
                moved.Tag = currenttag.Clone() as string;

                moved.Location = originalpoint;
                return;
            }
            moved.Location = originalpoint;
            currenttag = pctOutput.Tag.ToString().Clone() as string;
        }
    }
}