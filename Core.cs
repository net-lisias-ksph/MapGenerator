using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Management;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
namespace Noise
{
    public partial class Core : Form
    {
        private Bitmap masterimage;
        private Bitmap activeimage = new Bitmap(512, 512);
        private System.Collections.Concurrent.ConcurrentDictionary<int, Bitmap> previews = new System.Collections.Concurrent.ConcurrentDictionary<int, Bitmap>();
        private List<Bitmap> postprocessmaps = new List<Bitmap>();
        private byte[] colourmapoutput;
        private string location = Environment.CurrentDirectory.ToString();
        private ColourMap showcolour;
        private NormalMap shownormal;
        private ErosionMenu showerosion;
        private RenderedPreview showpreview;
        public uint[,] globalarray;
        string blendingmode;
        bool cuda = false;
        bool darkmode = true;
        public Core()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            textThreads.Text = "CPU Threads = " + (Convert.ToInt32(Environment.ProcessorCount.ToString())); //Display CPU logical processor (thread) count
            showcolour = new ColourMap();
            shownormal = new NormalMap();
            showpreview = new RenderedPreview();
            showerosion = new ErosionMenu();

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from " + "Win32_Processor"); //Display processor information

            foreach (ManagementObject ProcessorInfo in searcher.Get())
            {
                foreach (PropertyData ProcessorProperties in ProcessorInfo.Properties)
                {
                    if (ProcessorProperties.Name != null && ProcessorProperties.Name == "Name")
                    {
                        lblCPUName.Text = "CPU = " + ProcessorProperties.Value.ToString();
                    }
                }

            }
            this.AutoScaleMode = AutoScaleMode.Dpi; //Scale text with monitor resolution

            interval.Enabled = true;
            interval.Start();
            location = location.Remove((location.Length - 19), 19) + "images/";
            location = location.Replace('\\', '/'); //File paths are in forward slashes in c#
            lblInstall.Text = "Executing Directory " + location;
            getCudaDeviceInfo();
            DarkMode();
        }
        private void getCudaDeviceInfo()
        {
            try
            {
                Emgu.CV.Cuda.CudaDeviceInfo cdi = new Emgu.CV.Cuda.CudaDeviceInfo();    //Display GPU information to the user
                lblGPU.Text = "GPU = " + cdi.Name;
                lblGPUStatus.Text = "CUDA Compatibility = " + cdi.IsCompatible.ToString();
                lblCudaCount.Text = "CUDA Cores = " + cdi.MultiProcessorCount.ToString();
                lblGPUMem.Text = "GPU Memory = " + (cdi.TotalMemory / (1024 * 1024) + "Mb").ToString();
                cuda = true;
                butCuda.Text = "CUDA: Enabled";
            }
            catch (Exception e)
            {
                //If there is no CUDA support the program will let the user know on startup
                MessageBox.Show("CUDA was unable to be initialized. \nCUDA has been disabled.\n\n" + e.ToString(), "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                butCuda.Text = "CUDA: Disabled";
                cuda = false;
            }
        }
        public Bitmap DetermineResizeMethod(Bitmap source, int width, int height)
        {
            if (cuda == false)  //Trying to process an image with a graphics card that does not support CUDA can cause the program to crash, so determine the resize method here ALWAYS
            {
                try
                {
                    source = ResizeWithoutGPU(source, width, height);
                }
                catch (Exception e)
                {
                    //Warn the user that something went wrong during the image generation, and must be halted.
                    MessageBox.Show("Image resize failed!\n\n" + e.ToString(), "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (cuda == true)
            {
                try
                {
                    source = ResizeWithGPU(source, width, height);
                }
                catch (Exception e)
                {
                    //Warn the user that a lack of CUDA support on their PC may be the issue
                    MessageBox.Show("Image resize failed! Consider disabling CUDA.\n\n" + e.ToString(), "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return source;
        }

        public Bitmap ResizeWithGPU(Bitmap source, int width, int height)   //If CUDA is supported, generate using the graphics card
        {
            Image<Emgu.CV.Structure.Rgba, byte> input = new Image<Emgu.CV.Structure.Rgba, byte>(source);
            Mat ramSource = input.Mat;
            Mat ramDest = new Mat();
            Emgu.CV.Cuda.Stream terrainprogramstream = new Emgu.CV.Cuda.Stream();   //CUDA stream to perform the resize operation on - separates this program from anything else also using the GPU
            using (Emgu.CV.Cuda.GpuMat graphicsSource = new Emgu.CV.Cuda.GpuMat())
            using (Emgu.CV.Cuda.GpuMat graphicsDest = new Emgu.CV.Cuda.GpuMat())
            {
                graphicsSource.Upload(ramSource, terrainprogramstream); //Upload the image from RAM to GPU
                Emgu.CV.Cuda.CudaInvoke.Resize(graphicsSource, graphicsDest, new Size(width, height), 0, 0, Emgu.CV.CvEnum.Inter.Cubic, terrainprogramstream);
                graphicsDest.Download(ramDest, terrainprogramstream);   //Retrieve the image from GPU and store back in RAM where it can be processed later

            }
            Image<Emgu.CV.Structure.Rgba, byte> dest = new Image<Emgu.CV.Structure.Rgba, byte>(ramDest.Bitmap); //Destination byte array formatted as an RGBA (Format32BppArgb)
            Bitmap output = dest.ToBitmap(width, height);   //.ToBitmap() is a method in Emgu.Cv which returns the Bitmap from a Mat
            output = output.Clone(new Rectangle(0, 0, width, height), PixelFormat.Format32bppRgb); //Clone into the new format, otherwise the pixel format is 24bpp, which results in image distortion.
            return output;
        }

        public Bitmap ResizeWithoutGPU(Bitmap source, int width, int height)    //If there is no CUDA support, resize using the CPU
        {
            Bitmap output = new Bitmap(width, height);  //Create the output image and draw the input onto it with the new size
            Graphics g = Graphics.FromImage(output);
            g.DrawImage(source, new Rectangle(0, 0, width, height));    //Draw input on the output image
            return output;
        }
        


        private void DropBlend_SelectedIndexChanged(object sender, EventArgs e)
        {
            blendingmode = dropBlend.Text;
        }
        //This region correlates to when the octaves above the active image are clicked
        #region octaveclicks
        private void Octave0_Click(object sender, EventArgs e)
        {
            if (octave0.Image != null)
            {
                activeimage = (Bitmap)octave0.Image;
            }

        }
        private void Octave1_Click_1(object sender, EventArgs e)
        {
            if (octave1.Image != null)
            {
                activeimage = (Bitmap)octave1.Image;
            }

        }

        private void Octave2_Click(object sender, EventArgs e)
        {
            if (octave2.Image != null)
            {
                activeimage = (Bitmap)octave2.Image;
            }
        }

        private void Octave3_Click(object sender, EventArgs e)
        {
            if (octave3.Image != null)
            {
                activeimage = (Bitmap)octave3.Image;
            }
        }

        private void Octave4_Click(object sender, EventArgs e)
        {
            if (octave4.Image != null)
            {
                activeimage = (Bitmap)octave4.Image;
            }
        }

        private void Octave5_Click(object sender, EventArgs e)
        {
            if (octave5.Image != null)
            {
                activeimage = (Bitmap)octave5.Image;
            }
        }

        private void Octave6_Click(object sender, EventArgs e)
        {
            if (octave6.Image != null)
            {
                activeimage = (Bitmap)octave6.Image;
            }
        }

        private void Octave7_Click(object sender, EventArgs e)
        {
            if (octave7.Image != null)
            {
                activeimage = (Bitmap)octave7.Image;
            }
        }

        private void OctaveOutput_Click(object sender, EventArgs e)
        {
            activeimage = masterimage;
        }
        #endregion  
        private void NormalMap_Click(object sender, EventArgs e)    //Bring the normal map to the display
        {
            if (postprocessmaps.Count() > 0)
            {
                Bitmap tempimage = postprocessmaps[0];
                activeimage = DetermineResizeMethod(tempimage, 512, 512);
            }
        }

        private void ColourMap_Click(object sender, EventArgs e)    //Bring the colour map to the display
        {
            if (postprocessmaps.Count() > 1)
            {
                Bitmap tempimage = postprocessmaps[1];
                activeimage = DetermineResizeMethod(tempimage, 512, 512);
            }
        }

        public uint[,] boxBlur(uint[,] image, int size, int height) //The code is not good to look at, but it's the best way to do a well performing box blur that blurs the edges as well as the middle.
        {
            int rows = size;
            int columns = height;
            //size = size;
            System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
            time.Start();
            Parallel.For(0, size, i =>
            {
                for (int j = 0; j < size; j++)
                {
                    if (i > 0 && i < rows - 1 && j > 0 && j < columns - 1)  //middle
                    {
                        image[i, j] = (image[i, j] + image[i, j + 1] + image[i, j - 1] + image[i + 1, j] + image[i + 1, j + 1] + image[i - 1, j] + image[i - 1, j - 1] + image[i - 1, j + 1] + image[i + 1, j - 1]) / 9;
                    }
                    if (i == 0 && j == 0)       //top left
                    {
                        image[i, j] = (image[i, j] + image[i + 1, j] + image[i + 1, j + 1] + image[i, j + 1] + image[size - 1, j] + image[size - 1, size - 1] + image[i, size - 1] + image[i + 1, size - 1] + image[size - 1, j + 1]) / 9;
                    }
                    if (i == 0 && j == columns - 1) //bottom left
                    {
                        image[i, j] = (image[i, j] + image[i + 1, j] + image[i + 1, j - 1] + image[i, j - 1] + image[size - 1, j] + image[size - 1, j - 1] + image[size - 1, 0] + image[i, 0] + image[i + 1, 0]) / 9;
                    }
                    if (i == rows - 1 && j == 0)    //top right
                    {
                        image[i, j] = (image[i, j] + image[i - 1, j] + image[i - 1, j + 1] + image[i, j + 1] + image[0, j] + image[0, j + 1] + image[size - 1, size - 1] + image[size - 1 - 1, size - 1] + image[0, size - 1]) / 9;
                    }
                    if (i == rows - 1 && j == columns - 1)  //bottom right
                    {
                        image[i, j] = (image[i, j] + image[i - 1, j] + image[i - 1, j - 1] + image[i, j - 1] + image[0, size - 1] + image[1, size - 1] + image[0, 0] + image[size - 1, 0] + image[size - 1, 1]) / 9;
                    }
                    if (i == 0 && j > 0 && j < columns - 1) //left side
                    {
                        image[i, j] = (image[i, j] + image[i, j + 1] + image[i, j - 1] + image[i + 1, j] + image[i + 1, j + 1] + image[i + 1, j - 1] + image[size - 1, j] + image[size - 1, j - 1] + image[size - 1, j + 1]) / 9;
                    }
                    if (i == rows - 1 && j > 0 && j < columns - 1) //right side
                    {
                        image[i, j] = (image[i, j] + image[i, j + 1] + image[i, j - 1] + image[i - 1, j] + image[i - 1, j - 1] + image[i - 1, j + 1] + image[0, j - 1] + image[0, j] + image[0, j + 1]) / 9;
                    }
                    if (j == 0 && i > 0 && i < rows - 1)    //top side
                    {
                        image[i, j] = (image[i, j] + image[i, j + 1] + image[i + 1, j] + image[i + 1, j + 1] + image[i - 1, j] + image[i - 1, j + 1] + image[i - 1, size - 1] + image[i, size - 1] + image[i + 1, size - 1]) / 9;
                    }
                    if (j == columns - 1 && i > 0 && i < rows - 1)  //bottom side
                    {
                        image[i, j] = (image[i, j] + image[i, j - 1] + image[i + 1, j] + image[i - 1, j] + image[i - 1, j - 1] + image[i + 1, j - 1] + image[i - 1, 0] + image[i, 0] + image[i + 1, 0]) / 9;
                    }
                }
            });

            return image;
        }

        private void Interval_Tick(object sender, EventArgs e)  //Update the images on interval tick
        {

            if (activeimage != null && pctOutput.Image != activeimage)
            {
                pctOutput.Image = activeimage;
            }
            if (activeimage != null)
            {
                if (showcolour.outputimg != pctColourOutput.Image)
                {
                    pctColourOutput.Image = showcolour.outputimg;
                }
                if (shownormal.output != pctNormalOutput.Image)
                {
                    pctNormalOutput.Image = shownormal.output;
                }
                if (showerosion.outputimg != pctErosionOutput.Image)
                {
                    pctErosionOutput.Image = showerosion.outputimg;
                }
                if (pctOriginalOutput.Image != masterimage)
                {
                    pctOriginalOutput.Image = masterimage;
                }

            }

        }

        public uint[,] CalculateDown(Bitmap img, int width, int height) //Convert image to array
        {
            System.Drawing.Imaging.BitmapData img2 = new System.Drawing.Imaging.BitmapData();
            uint[,] data = new uint[width, height];
            img2 = img.LockBits(new System.Drawing.Rectangle(0, 0, width, height), System.Drawing.Imaging.ImageLockMode.ReadOnly, img.PixelFormat);
            int stride = 0;             //Lock the bits so that the bitmap data can be manipulated
            for (int i = 0; i < width - 1; i++)
            {
                stride = img2.Stride * i;
                for (int b = 0; b < height - 1; b++)
                {
                    data[i, b] = System.Runtime.InteropServices.Marshal.ReadByte(img2.Scan0, (stride) + (b) * 4);   //Stride is the number of bytes per pixel
                }
            }
            img.UnlockBits(img2);
            return data;
        }

        public Bitmap CalculateUp(uint[,] values, int width, int height)    //Convert array to image
        {
            Bitmap img = new Bitmap(width, height, PixelFormat.Format32bppRgb); //Image to be drawn on to
            BitmapData data = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadWrite, img.PixelFormat);   //Secure bitmap data so it can be manipulated
            int BPP = System.Drawing.Bitmap.GetPixelFormatSize(img.PixelFormat) / 8;    //Bits per pixel = number of bits per channel * channels
            int BC = data.Stride * img.Height;
            byte[] pixels = new byte[BC];
            IntPtr FirstPixel = data.Scan0;
            System.Runtime.InteropServices.Marshal.Copy(FirstPixel, pixels, 0, pixels.Length);
            int pixelheight = data.Height;
            int pixelwidth = data.Width * BPP;
            try
            {
                Parallel.For(0, pixelheight, i =>   //Fill the byte array before transferring to memory
                {

                    int currentline = i * data.Stride;
                    for (int b = 0; b < pixelwidth; b++)
                    {
                        pixels[currentline + b] = (byte)values[i, b / BPP];
                    }
                });
            }
            catch (Exception e)
            {
                MessageBox.Show("Multithreading the image generation encountered an error." + "\n" + "\n" + e.ToString());  //Error detection, will not crash the program
                return new Bitmap(width, height);
            }
            System.Runtime.InteropServices.Marshal.Copy(pixels, 0, FirstPixel, pixels.Length);  //Copy to memory before disposing of the now useless bitmap data
            img.UnlockBits(data);

            return img;
        }
        public uint[,] AdvancedContrast(uint[,] data, int size) //Scale contrast according to image brightness - fill entire colour range
        {
            int[,] values = (int[,])(object)data;
            double multiplier = 0;
            int highest = 0;
            int lowest = 256;
            Parallel.For(0, size, i =>
            {
                Parallel.For(0, size, b =>
                {
                    if (values[i, b] < lowest)
                    {
                        lowest = values[i, b];      //Finding the lowest and highest values determines how much to stretch the pixels by
                    }
                    if (values[i, b] > highest)
                    {
                        highest = values[i, b];
                    }
                    values[i, b] = 127 - values[i, b];
                });
            });
            if (highest == 0)   //Prevent division by 0
            {
                highest = 1;
            }
            if (lowest == 0)
            {
                lowest = 1;
            }

            multiplier = 255 / (double)highest; //Value between 1 and 255, as a multiplier

            Parallel.For(0, size, j =>  //Parallel process the image, scaling it properly. Same formula as contrast.
            {
                for (int k = 0; k < size; k++)
                {
                    values[j, k] = 255 - Convert.ToInt32(((double)values[j, k] * multiplier) + 127);
                }
            });
            data = (uint[,])(object)values;
            return data;
        }

        public void GeneratePreviews()
        {
            octave0.Image = null;   //Clear the images before updating them
            octave1.Image = null;
            octave2.Image = null;
            octave3.Image = null;
            octave4.Image = null;
            octave5.Image = null;
            octave6.Image = null;
            octave7.Image = null;   //Now update all of the images from the dictionary containing the previews
            if (previews.Keys.Count > 0 && previews[0] != null )
            {
                octave0.Image = previews[0];
            }
            if (previews.Keys.Count > 1 && previews[1] != null)
            {
                octave1.Image = previews[1];
            }
            if (previews.Keys.Count > 2 && previews[2] != null)
            {
                octave2.Image = previews[2];
            }
            if (previews.Keys.Count > 3 && previews[3] != null)
            {
                octave3.Image = previews[3];
            }
            if (previews.Keys.Count > 4 && previews[4] != null)
            {
                octave4.Image = previews[4];
            }
            if (previews.Keys.Count > 5 && previews[5] != null)
            {
                octave5.Image = previews[5];
            }
            if (previews.Keys.Count > 6 && previews[6] != null)
            {
                octave6.Image = previews[6];
            }
            if (previews.Keys.Count > 7 && previews[7] != null)
            {
                octave7.Image = previews[7];
            }
        }
        public uint[,] GenerateArray(int seed, double contrast, int size, double contrastinfluence, int thisoctave)
        {
            double generatedbyte;
            contrast = (contrast * contrastinfluence);

            Random rnd = new Random(seed);
            uint[,] values = new uint[size, size];
            if (checkPadded.Checked == false)
            {
                thisoctave = 1; //Since this is used in a division, dividing by 1 returns the same value
            }
            for (int i = 0; i < size; i++)
            {
                for (int b = 0; b < size; b++)
                {

                    generatedbyte = Convert.ToInt32(rnd.Next(255) - (255 / 2)); //normalise the pixel

                    if (generatedbyte > 255 / 2)
                    {
                        generatedbyte = Convert.ToInt32((double)generatedbyte * (double)(contrast / thisoctave)) + (255 / 2);   //Contrast influences the layer very early on to maintain image quality
                    }
                    if (generatedbyte <= 255 / 2)
                    {
                        generatedbyte = Convert.ToInt32((double)generatedbyte * (double)(contrast / thisoctave)) + (255 / 2);
                    }


                    if (generatedbyte > 255)
                    {
                        generatedbyte = 255;
                    }
                    if (generatedbyte < 0)
                    {
                        generatedbyte = 0;
                    }

                    values[i, b] = Convert.ToUInt32(generatedbyte);

                }
            }
            if (checkRaw.Checked == false)
            {
                values = boxBlur(values, size, size);
            }
            if (checkAdvancedContrast.Checked == true)
            {
                values = AdvancedContrast(values, size);    //Forces automatic contrast scaling, which ensures the maximum value is 255 or the minimum value is 0
            }

            return values;
        }
        private uint[,] BlendingMode(System.Collections.Concurrent.ConcurrentDictionary<int, uint[,]> blendingarrays, int size, double persistence)
        {
            uint[,] firstarray = blendingarrays[0]; //Initialize the first array

            if (blendingmode != "SELECT BLENDING MODE")
            {
                for (int a = 1; a < blendingarrays.Count(); a++)
                {
                    if (checkCracks.Checked == true)    //Cracks are generated on octave 0, no need to do any extra processing, except the size is strongly governed by this octave
                    {
                        blendingarrays[a] = PrepareCracks(blendingarrays[a], (int)(128 * ((double)(a + 8) / 4)), size, (int)(5 * ((double)(a + 8) / 4)));
                    }
                    Parallel.For(0, size, b =>
                    {
                        double value = 0;
                        int minimum = 0;
                        int result = 0;
                        for (int c = 0; c < size; c++)
                        {
                            double temp = (double)firstarray[b, c] / 255;    //Convert to percentage first. It makes processing a lot easier
                            double temp2 = (double)blendingarrays[a][b, c] / 255;

                            if (blendingmode == "Difference")
                            {
                                if (a <= 4) //Difference becomes less and less contrasted as octave count increases, so this is a fixed cap
                                {
                                    if (temp - temp2 >= 0)
                                    {
                                        firstarray[b, c] = (uint)((temp - temp2) * (double)persistence * 255);
                                    }
                                    if (temp2 - temp > 0)
                                    {
                                        firstarray[b, c] = (uint)((temp2 - temp) * (double)persistence * 255);
                                    }
                                    if (firstarray[b, c] > 255)
                                    {
                                        firstarray[b, c] = 255;
                                    }
                                    if (firstarray[b, c] < 0)
                                    {
                                        firstarray[b, c] = 0;
                                    }
                                }
                            }
                            if (blendingmode == "Aggressive Overlay")   //Stronger form of Overlay which generates cliffs too
                            {
                                if (firstarray[b, c] > (255 / 2))
                                {
                                    value = (255 - firstarray[b, c]) / 127.5;
                                    minimum = (int)(firstarray[b, c] - (255 - firstarray[b, c]));
                                    if (minimum > 255)
                                    {
                                        minimum = 255;
                                    }
                                    firstarray[b, c] = Convert.ToUInt32((blendingarrays[a][b, c] * value) + minimum);
                                }
                                if (firstarray[b, c] <= (255 / 2))
                                {
                                    value = firstarray[b, c] / 127.5;
                                    if (value > 255)
                                    {
                                        value = 255;
                                    }
                                    result = Convert.ToInt32(blendingarrays[a][b, c] * value);
                                    if (result > 255)
                                    {
                                        result = 255;
                                    }
                                    if (result < 0)
                                    {
                                        result = 0;
                                    }
                                    firstarray[b, c] = (uint)result;
                                }
                            }
                            if (blendingmode == "Overlay")  //Softer form of aggressive overlay, which generates hills and mountains without cliffs
                            {
                                firstarray[b, c] = Convert.ToUInt32(((temp) * (temp2 + 0.5)) * 255);
                                if (firstarray[b, c] > 255)
                                {
                                    firstarray[b, c] = 255;
                                }
                            }
                        }
                    });

                }
            }
            blendingarrays.Clear();
            return firstarray;
        }
        public void CheckPreset()   //Set the various presets
        {
            if (dropPreset.Text == "Alien") //Lots of swirls, low frequency
            {
                trackSize.Value = 3;
                baseLayerMode.Text = "Smooth";
                dropBlend.Text = "Aggressive Overlay";
                trackSmoothGenerations.Value = 4;
                checkCracks.Checked = false;
                checkLakes.Checked = false;
                trackLakes.Value = 0;
            }
            if (dropPreset.Text == "Realistic") //Moderate swirls, trying to simulate mountains
            {
                trackSize.Value = 5;
                baseLayerMode.Text = "Smooth";
                dropBlend.Text = "Aggressive Overlay";
                trackSmoothGenerations.Value = 2;
                trackOctaves.Value = 8;
                checkCracks.Checked = false;
                checkLakes.Checked = false;
                trackLakes.Value = 0;
            }
            if (dropPreset.Text == "Cliffs")    //Aggressive overlay, high frequency, cracks, lakes
            {
                trackSize.Value = 6;
                baseLayerMode.Text = "Smooth";
                dropBlend.Text = "Aggressive Overlay";
                trackSmoothGenerations.Value = 4;
                checkCracks.Checked = true;
                checkLakes.Checked = true;
                trackLakes.Value = 50;
            }
            if (dropPreset.Text == "Cracked")   //Aggressive overlay, high frequency, cracks
            {
                trackSize.Value = 6;
                baseLayerMode.Text = "Smooth";
                dropBlend.Text = "Aggressive Overlay";
                trackSmoothGenerations.Value = 4;
                checkCracks.Checked = true;
                checkLakes.Checked = false;
                trackLakes.Value = 0;
            }
            UpdateText();
        }
        public void ButGenerate_Click(object sender, EventArgs e)
        {
            GenerateImage();
        }
        public void GenerateImage()
        {
            #region locals
            System.Collections.Concurrent.ConcurrentDictionary<int, uint[,]> paralleluints = new System.Collections.Concurrent.ConcurrentDictionary<int, uint[,]>();
            //ConcurrentDictionary MUST be used otherwise parallel generation is impossible
            int size = trackSize.Value * 64;
            int frequency = trackIteration.Value;
            double step = trackFreqMult.Value / 2;
            int octaves = trackOctaves.Value;
            int imagesize = (int)((size / 8) * (octaves) * frequency * (step * ((double)octaves + 1) / 4)); //Use the slider values to determine the internal resolution
            int seed = 0;
            double contrast = (double)trackContrast.Value / 100;
            int renderres = trackRenderRes.Value * 64;
            int smoothgenerations = trackSmoothGenerations.Value;
            double persistence = trackPersistence.Value;
            #endregion
            #region steps
            //1. Generate arrays of various sizes
            //2. Calculate them up to an image, resize them, calculate them down to an array again
            //3. Pass to blending mode
            #endregion

            seed = textSeed.Text.Length;
            foreach (char c in textSeed.Text)   //Replace any non-numerical characters with their numerical equivalent (allowing for worded seeds!)
            {
                seed += ((int)c * (int)c) / 3;
            }

            previews.Clear();
            if (baseLayerMode.Text == "Value" || baseLayerMode.Text == "Smooth")
            {
                #region value
                Parallel.For(0, octaves, counter1 =>                       //Generate array according to image size, since using parallel for these will not be in order
                {
                    int newseed = counter1 + seed;
                    int parallelsize = Convert.ToInt32((size / 8) * (counter1 + 1) * frequency * (step * ((double)counter1 + 1) / 4));
                    double contrastinfluence = (1 - ((double)counter1 / (8 + persistence)));
                    try
                    {
                        //Generate, convert to image, resize, then convert back to array
                        uint[,] temporarygenerated = CalculateDown(DetermineResizeMethod(CalculateUp(GenerateArray(newseed, contrast, parallelsize, contrastinfluence, counter1 + 1), parallelsize, parallelsize), renderres, renderres), renderres, renderres);
                        previews.TryAdd(counter1, CalculateUp(temporarygenerated, renderres, renderres));
                        paralleluints.TryAdd(counter1, temporarygenerated);
                        //TryAdd tries to add the octave to the dictionary
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("There was a problem generating the image. " + "\n" + e.ToString());
                    }
                    
                });
                
                #endregion
            }
            if (baseLayerMode.Text == "Smooth")
            {
                //Same method as the value mode, but the distortion arrays must be generated before the rest of the image generation can proceed.
                #region smooth
                Parallel.For(0, smoothgenerations, trycounter =>
                {
                    List<Bitmap> distortionarraylocal = new List<Bitmap>();
                    seed++;
                    for (int distortcounter = 1; distortcounter < 3; distortcounter++)
                    {
                        int distortsize = Convert.ToInt32((size / 32) * (distortcounter + 1) * frequency * (step * ((double)distortcounter + 1) / 4));
                        distortionarraylocal.Add(CalculateUp(AdvancedContrast(GenerateArray(seed + distortcounter + (trycounter + 1) * distortcounter, contrast, distortsize, 1, distortcounter), distortsize), distortsize, distortsize));
                    }
                    uint[,] tempdistort = CalculateDown(DetermineResizeMethod(CalculateUp(Distort(distortionarraylocal[0], distortionarraylocal[1]), 300, 300), renderres, renderres), renderres, renderres);
                    paralleluints[trycounter] = (tempdistort);
                    previews[trycounter] = (CalculateUp(tempdistort, renderres, renderres));
                    distortionarraylocal.Clear();
                });
                #endregion
            }
            if (baseLayerMode.Text != "SELECT BASE LAYER" && (baseLayerMode.Text == "Smooth" || baseLayerMode.Text == "Value"))  //Convert the generated images and blend them together, before generating the previews above the active image
            {
                activeimage = CalculateUp(paralleluints[0], renderres, renderres);
                uint[,] mainarray = BlendingMode(paralleluints, renderres, (persistence / 4));
                globalarray = mainarray.Clone() as uint[,];
                if (checkLakes.Checked == true) //Generate lakes if enabled. Crack generation is done within the blending mode method.
                {
                    GenerateLakes(globalarray, trackLakes.Value, globalarray.GetLength(0));
                }
                activeimage = CalculateUp(globalarray, renderres, renderres);
                masterimage = activeimage.Clone() as Bitmap;
                
                GeneratePreviews();
            }
            //previewimage
            //Bitmap import = (Bitmap)Bitmap.FromFile("C:/Users/Casper/Pictures/import.png");
            //Bitmap image = new Bitmap(2048, 2048, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            //using (Graphics gr = Graphics.FromImage(image))
            //{
            //    gr.DrawImage(import, new Rectangle(0, 0, 2048, 2048));
            //}
            //activeimage = image;
            //masterimage = image;
            //globalarray = CalculateDown(image, 2048, 2048);
        }

        public void GenerateLakes(uint[,] data, int oceanlevel, int rendersize)
        {
            //OceanLevel is also the range between sea level and land
            int maximum = 0;
            uint[,] values = data.Clone() as uint[,];
            for (int i = 0; i < rendersize; i++)
            {
                for (int b = 0; b < rendersize; b++)
                {
                    if (globalarray[i, b] > maximum)
                    {
                        maximum = (int)globalarray[i, b];   //Find the maximum value before stretching heights
                    }
                    if (globalarray[i, b] < oceanlevel)
                    {
                        values[i, b] = 0;
                    }
                }
            }
            values = boxBlur(values, rendersize, rendersize);   //Box blur removes any pixels that could be a lake, and cleans up the image a bit. It looks a lot nicer.
            for (int i = 0; i < rendersize; i++)
            {
                for (int b = 0; b < rendersize; b++)
                {
                    if (values[i, b] > oceanlevel)
                    {
                        double valuetakeOL = globalarray[i, b] - oceanlevel;
                        if (valuetakeOL < 0)
                        {
                            valuetakeOL = 1;
                        }
                        double maxtakeOL = maximum - oceanlevel;
                        double divider = valuetakeOL / maxtakeOL;
                        double multiplier = 1 - divider;
                        int value = Convert.ToInt32(globalarray[i, b] - (multiplier * (double)oceanlevel)); 
                        if (value < 0) { value = 0; }  //Stretch the ocean downwards instead of setting the height to 0 or subtracting a constant. It looks a lot better and prevents pixellation
                        globalarray[i, b] = (uint)value;
                    }
                    else
                    {
                        globalarray[i, b] = 0;
                    }
                }
            }
        }
        public uint[,] Distort(Bitmap distort1, Bitmap distort2)
        {
            Bitmap base1 = DetermineResizeMethod(distort1, 512, 512);   //Resize the inputs to 512 x 512 to begin "domain warp"
            Bitmap distort = DetermineResizeMethod(distort2, 512, 512);
            uint[,] array1 = CalculateDown(base1, 512, 512);
            uint[,] array2 = CalculateDown(distort, 512, 512);
            uint[,] output = new uint[512, 512];
            Parallel.For(0, 512, i =>
            {
                for(int b = 0; b < 512; b++)
                {
                    int indexx = 0;
                    int indexy = 0;
                    int indextoaddx = (int)array1[Math.Abs(i), Math.Abs(b)] - 127;
                    int indextoaddy = (int)array2[Math.Abs(i), Math.Abs(b)] - 127;
                    if (indextoaddx > 255)
                    {
                        indextoaddx = Math.Abs(255 - indextoaddx);  //These if statements all prevent out of bounds by wrapping around the image
                    }                                               //The actual distort method can use fixed sizes as the input layer defines its own size based off the frequency
                    if (indextoaddy > 255)                          //Which is why I am using 512 x 512 as fixed constants
                    {
                        indextoaddy = Math.Abs(255 - indextoaddy);
                    }
                    if (i + indextoaddx >= 512 && indextoaddx > 0)
                    {
                        indexx = 0 + (i - indextoaddx);
                    }
                    if (b + indextoaddy >= 512 && indextoaddy > 0)
                    {
                        indexy = 0 + (b - indextoaddy);
                    }
                    if (b + indextoaddy < 512 && indextoaddy > 0)
                    {
                        indexy = b + indextoaddy;
                    }
                    if (i + indextoaddx < 512 && indextoaddx > 0)
                    {
                        indexx = i + indextoaddx;
                    }
                    if (i + indextoaddx >= 0 && indextoaddx < 0)
                    {
                        indexx = i + indextoaddx;
                    }
                    if (b + indextoaddy >= 0 && indextoaddy < 0)
                    {
                        indexy = b + indextoaddy;
                    }
                    if (b + indextoaddy < 0 && indextoaddy < 0)
                    {
                        indexy = 0 + (b - indextoaddy);
                    }
                    if (i + indextoaddx < 0 && indextoaddx < 0)
                    {
                        indexx = 0 + (i - indextoaddx);
                    }
                    output[i, b] = array2[indexx, indexy];
                }
            });
            output = boxBlur(output, 512, 512); //Box blur the output to reduce any artifacts that may be generated, such as the sharp splines which make good mountain ranges when blurred.
            output = boxBlur(output, 512, 512);
            return output;
        }


        public Bitmap GenerateNormalMap(Bitmap image, bool overlay, double strength, bool transparent)
        {

            #region Global Variables
            int w = image.Width - 1;
            int h = image.Height - 1;

            int size = image.Width;

            Bitmap normal = new Bitmap(image.Width, image.Height);  //Prepare the image data for when the array is converted to an image
            BitmapData locked = normal.LockBits(new Rectangle(0, 0, size, size), ImageLockMode.ReadWrite, PixelFormat.Format32bppPArgb);
            byte[] normalsize = new byte[locked.Stride * size];
            uint[,] values = CalculateDown(image, size, size);
            IntPtr firstpixel = locked.Scan0;

            #endregion
            #region GenerateNormalMap
            uint[,] temporarynormalmap = CalculateDown(image, size, size);
            Parallel.For(0, w, y =>                              //GET PIXEL doesn't work with parallel for (and it's slow), so I switched it out for the better Marshal.Copy method
            {
                double x_vector;
                double y_vector;
                double sample_left;
                double sample_right;
                double sample_up;
                double sample_down;
                int normalcolor = 0;
                for (int x = 0; x < h; x++)
                {
                    if (x > 0)
                    {
                        sample_left = 255 - ((double)values[x - 1, y] / 255);   
                    }   //Sample the adjacent pixels to calculate the strength of the vector between them
                    else
                    {
                        sample_left = 255 - ((double)values[x, y] / 255);
                    }
                    if (x < w)
                    {
                        sample_right = 255 - ((double)values[x + 1, y] / 255);
                    }
                    else
                    {
                        sample_right = 255 - ((double)values[x, y] / 255);
                    }
                    if (y > 1)
                    {
                        sample_up = 255 - ((double)values[x, y - 1] / 255);
                    }
                    else
                    {
                        sample_up = 255 - ((double)values[x, y] / 255);
                    }
                    if (y < h)
                    {
                        sample_down = 255 - ((double)values[x, y + 1] / 255);
                    }
                    else
                    {
                        sample_down = 255 - ((float)values[x, y] / 255);
                    }
                    x_vector = (double)((((((sample_left - sample_right) + 1) * 0.5d) * 255) - 127) * strength) + 127;  //Using the contrast formula to scale
                    y_vector = (double)((((((sample_up - sample_down) + 1) * 0.5d) * 255) - 127) * strength) + 127;     //the strength of the normal map vector

                    if (x_vector > 255)
                    {
                        x_vector = 255;
                    }
                    if (y_vector > 255)
                    {
                        y_vector = 255;
                    }
                    if (x_vector < 0)
                    {
                        x_vector = 0;
                    }
                    if (y_vector < 0)
                    {
                        y_vector = 0;
                    }
                    Color col = Color.FromArgb(255, (int)y_vector, (int)x_vector, 255);     //Start generating the standard normal map colours
                    if (overlay == true)
                    {
                        normalcolor = Convert.ToInt32(temporarynormalmap[x, y] - ((int)y_vector - (int)x_vector));
                        if (normalcolor < 0)
                        {
                            normalcolor = 0;
                        }
                        if (normalcolor > 255)
                        {
                            normalcolor = 255;
                        }
                        col = Color.FromArgb(255, normalcolor, normalcolor, normalcolor);   //Greyscale
                    }
                    if (transparent == true)    //Transparency allows use in the previews form
                    {
                        col = Color.FromArgb((int)(255 - x_vector), (int)(y_vector / 3), (int)((y_vector / 3)), (int)((y_vector / 3)));
                    }
                    int red = col.R;
                    int green = col.G;
                    int blue = col.B;
                    int alpha = col.A;
                    normalsize[(y + (x * size)) * 4] = (byte)blue;  //ARGB is backwards in the array
                    normalsize[(y + (x * size)) * 4 + 1] = (byte)green;
                    normalsize[(y + (x * size)) * 4 + 2] = (byte)red;
                    normalsize[(y + (x * size)) * 4 + 3] = (byte)alpha;
                }
            });
            System.Runtime.InteropServices.Marshal.Copy(normalsize, 0, firstpixel, normalsize.Length);
            normal.UnlockBits(locked);
            return normal;
            #endregion
        }
        public Bitmap PrepareColourMap(uint[,] data, int width, int height, List<Color> colours, int[] heights)
        {
            //Handle the preparation of the colour map generation - Generating in segments means it can be done in parallel, as all the segments are independent!
            #region PrepareColourMap
            Bitmap previewcolour = new Bitmap(width, height);
            colourmapoutput = new byte[width * height * 4];     //This image format requires 4 bits
            int length = colours.Count();
            int start = 0;
            int end = 255;
            Parallel.For(0, length - 1, i =>
            {
                start = heights[i];
                end = heights[i + 1];
                GenerateColourMap(data, colours[i], colours[i + 1], start, end);    //Generate the colour map between start and end
            });
            try
            {
                BitmapData imagedata = previewcolour.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb);
                System.Runtime.InteropServices.Marshal.Copy(colourmapoutput, 0, imagedata.Scan0, colourmapoutput.Length);
                previewcolour.UnlockBits(imagedata);
                activeimage = previewcolour;
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to convert the array to an image." + "\n" + e.ToString()); //Show the exception and the reason why the generation failed
            }
            return previewcolour;
            #endregion
        }
        public void GenerateColourMap(uint[,] temporaryshadedmap, Color col1, Color col2, int start, int end)
        {
            #region GlobalVariables
            double intervalR = (col2.R - col1.R);
            double intervalG = (col2.G - col1.G);
            double intervalB = (col2.B - col1.B);
            double R = (double)col1.R;
            double B = (double)col1.B;
            double G = (double)col1.G;
            int size = temporaryshadedmap.GetLength(0);
            #endregion
            #region GenerateColourMap
            Bitmap img = new Bitmap(size, size, PixelFormat.Format24bppRgb);
            BitmapData data = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadWrite, img.PixelFormat);   //Prepare conversion from array to image
            int bytesize = data.Stride * img.Height;
            int pixelheight = data.Height;
            int pixelwidth = data.Width;
            Parallel.For(0, size, i =>
            {
                double R1;
                double B1;
                double G1;
                int currentline = i * data.Stride;
                for (int b = 0; b < size; b++)
                {
                    int byteposition = i * b;
                    double percentage = ((double)temporaryshadedmap[i, b] - start) / (double)(end - start);
                    R1 = (double)(col2.R - col1.R) * percentage + (double)col1.R;
                    G1 = (double)(col2.G - col1.G) * percentage + (double)col1.G;   //Linear interpolation
                    B1 = (double)(col2.B - col1.B) * percentage + (double)col1.B;
                    if (R1 > 255)
                    {
                        R1 = 255;
                    }
                    if (G1 > 255)   //Colours don't go higher than 255 in this image format, so ensure that they do not
                    {
                        G1 = 255;
                    }
                    if (B1 > 255)
                    {
                        B1 = 255;
                    }
                    if (R1 < 0)
                    {
                        R1 = 0;
                    }
                    if (B1 < 0)
                    {
                        B1 = 0;
                    }
                    if (G1 < 0)
                    {
                        G1 = 0;
                    }
                    if (temporaryshadedmap[i, b] >= start && temporaryshadedmap[i, b] < end + 1)
                    {
                        byte red = (byte)R1;
                        byte green = (byte)G1;
                        byte blue = (byte)B1;
                        byte unused = (byte)255;
                        int arrayposition = (b + size * i) * 4;
                        colourmapoutput[arrayposition] = blue;
                        colourmapoutput[arrayposition + 1] = green;     //Byte 4 is unused in this image format, but this image format is necessary for the preview form.
                        colourmapoutput[arrayposition + 2] = red;
                        colourmapoutput[arrayposition + 3] = unused;
                    }
                }
            });
            #endregion
        }
        public void SaveImage(Bitmap image, string name)
        {
            image.Save((location + name), ImageFormat.Png);
        }

        public uint[,] ExternalGenerateRivers(uint[,] data, int cycles, int count, int depth, Erosion rivers)
        {
            //Handle all external calls for this method from the Erosion form, since some methods of the Core class are required here.
            #region PrepareRivers
            Random rnd = new Random();
            Random rnd2 = new Random();
            int size = data.GetLength(0);

            if (size != 512)
            {
                //512 x 512 is recommended otherwise it will take longer to erode, and the results will (although still effective) will appear less noticeable.
                DialogResult morphologicalerosion = MessageBox.Show("Erosion Simulation requires a resolution of 512 to work best. " + "\n" + "\n" + "Scale to 512?", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk);
                if (morphologicalerosion == DialogResult.Yes)
                {
                    data = CalculateDown(DetermineResizeMethod(CalculateUp(data, size, size), 512, 512), 512, 512);
                    size = 512;
                }
                if (morphologicalerosion == DialogResult.No)
                {
                    //The program will continue as normal
                }
                if (morphologicalerosion == DialogResult.Cancel)
                {
                    return data;
                }
            }

            for (int a = 0; a < cycles; a++)    //Control the generation of the rivers
            {
                int[] coordinates;
                int x = 0;
                int y = 0;
                rivers.SetErosionArray(data);   //Input the array to the erosion method
                for (int b = 0; b < count; b++)
                {

                    coordinates = rivers.GenerateRiverStart(rnd);
                    x = coordinates[0];
                    y = coordinates[1];
                    if (x != 25565 && y != 25565)
                    {
                        rivers.TraceRiver(x, y, rnd2, size, depth);

                    }
                }
                data = rivers.ReturnArray();
            }
            return data;
            #endregion
        }

        private void ButSave_Click(object sender, EventArgs e)  //Save all images to the current filepath.
        {
            #region SaveData
            if (activeimage != null)
            {
                SaveImage(activeimage, "active-image.png");
            }
            if (shownormal.output != null)
            {
                SaveImage(shownormal.output, "normal-map.png");
            }
            if (showerosion.outputimg != null)
            {
                SaveImage(showerosion.outputimg, "height-map.png");
            }
            if (showcolour.outputimg != null)
            {
                SaveImage(showcolour.outputimg, "colour-map.png");
            }
            if (pctOriginalOutput.Image != null)
            {
                SaveImage((Bitmap)pctOriginalOutput.Image, "base-height-map.png");
            }
            #endregion
        }

        private void ButNormalMap_Click(object sender, EventArgs e)
        {
            if (masterimage != null)    //Validation - Detect if an image exists yet
            {
                shownormal.source = activeimage;
                shownormal.RefreshMap();
                shownormal.Show();
            }
        }
        private void ButColourMap_Click(object sender, EventArgs e) 
        {
            if (masterimage != null)    //Validation - Detect if an image exists yet
            {
                if (showcolour.outputimg != activeimage && shownormal.output != activeimage)
                {
                    showcolour.input = activeimage;
                    showcolour.RefreshMap();
                    showcolour.Show();
                }
            }
            else
            {
                return;
            }
        }
        public void LightMode() //Revert the changes in the DarkMode() method, effectively making this a light mode colour scheme
        {
            this.BackColor = default(Color);
            showcolour.BackColor = default(Color);
            showerosion.BackColor = default(Color);
            shownormal.BackColor = default(Color);
            showpreview.BackColor = default(Color);
            foreach (Control X in this.Controls)
            {
                X.BackColor = default(Color);
                X.ForeColor = default(Color);
            }
            foreach (Control X in showcolour.Controls)
            {
                X.BackColor = default(Color);
                X.ForeColor = default(Color);
            }
            foreach (Control X in shownormal.Controls)
            {
                X.BackColor = default(Color);
                X.ForeColor = default(Color);
            }
            foreach (Control X in showerosion.Controls)
            {
                X.BackColor = default(Color);
                X.ForeColor = default(Color);
            }
            foreach (Control X in showpreview.Controls)
            {
                X.BackColor = default(Color);
                X.ForeColor = default(Color);
            }
        }
        public void DarkMode()  //Make the user interface sleek and convert all controls to their dark mode form
        {
            this.BackColor = Color.FromArgb(255, 31, 31, 31);
            showerosion.BackColor = Color.FromArgb(255, 31, 31, 31);
            showcolour.BackColor = Color.FromArgb(255, 31, 31, 31);
            shownormal.BackColor = Color.FromArgb(255, 31, 31, 31);
            showpreview.BackColor = Color.FromArgb(255, 31, 31, 31);
            foreach (Control X in this.Controls)
            {
                if ((string)X.Tag != "Param" && (string)X.Tag != "Title")
                {
                    X.ForeColor = Color.FromArgb(255, 155, 155, 155);
                }
                X.BackColor = Color.FromArgb(255, 31, 31, 31);

                if ((string)X.Tag == "Param")
                {
                    X.ForeColor = Color.FromArgb(255, 49, 162, 218);
                }
                if ((string)X.Tag == "Title")
                {
                    X.ForeColor = Color.FromArgb(255, 200, 200, 200);
                    X.BringToFront();
                }
            }
            
            foreach (Button X in this.Controls.OfType<Button>())
            {
                X.FlatAppearance.BorderColor = Color.FromArgb(255, 50, 50, 50);
                X.ForeColor = Color.FromArgb(255, 49, 162, 218);
                X.FlatStyle = FlatStyle.Flat;
            }
            foreach (TextBox X in this.Controls.OfType<TextBox>())
            {
                X.BorderStyle = BorderStyle.None;
            }
            foreach (ComboBox X in this.Controls.OfType<ComboBox>())
            {
                X.FlatStyle = FlatStyle.Flat;
            }

            foreach (Control X in showcolour.Controls)
            {
                if (!(X is PictureBox)) //Ignore the colour map boxes
                {
                    X.BackColor = Color.FromArgb(255, 31, 31, 31);
                    X.ForeColor = Color.FromArgb(255, 155, 155, 155);

                }
                if ((string)X.Tag == "Param")
                {
                    X.ForeColor = Color.FromArgb(255, 49, 162, 218);
                }
                if ((string)X.Tag == "Title")
                {
                    X.ForeColor = Color.FromArgb(255, 200, 200, 200);
                }
            }
            foreach (Button X in showcolour.Controls.OfType<Button>())
            {
                X.FlatAppearance.BorderColor = Color.FromArgb(255, 50, 50, 50);
                X.FlatStyle = FlatStyle.Flat;
            }
            foreach (TextBox X in showcolour.Controls.OfType<TextBox>())
            {
                X.BorderStyle = BorderStyle.None;
            }
            foreach (ComboBox X in showcolour.Controls.OfType<ComboBox>())
            {
                X.FlatStyle = FlatStyle.Flat;
            }

            foreach (Control X in shownormal.Controls)
            {
                if (!(X is PictureBox))
                {
                    X.BackColor = Color.FromArgb(255, 31, 31, 31);
                    X.ForeColor = Color.FromArgb(255, 155, 155, 155);
                }

                if ((string)X.Tag == "Param")
                {
                    X.ForeColor = Color.FromArgb(255, 49, 162, 218);
                }
                if ((string)X.Tag == "Title")
                {
                    X.ForeColor = Color.FromArgb(255, 200, 200, 200);
                }
            }
            foreach (Button X in shownormal.Controls.OfType<Button>())
            {
                X.FlatAppearance.BorderColor = Color.FromArgb(255, 50, 50, 50);
                X.FlatStyle = FlatStyle.Flat;
            }
            foreach (TextBox X in shownormal.Controls.OfType<TextBox>())
            {
                X.BorderStyle = BorderStyle.None;
            }
            foreach (ComboBox X in shownormal.Controls.OfType<ComboBox>())
            {
                X.FlatStyle = FlatStyle.Flat;
            }

            foreach (Control X in showerosion.Controls)
            {
                X.BackColor = Color.FromArgb(255, 31, 31, 31);
                X.ForeColor = Color.FromArgb(255, 155, 155, 155);
                if ((string)X.Tag == "Param")
                {
                    X.ForeColor = Color.FromArgb(255, 49, 162, 218);
                }
                if ((string)X.Tag == "Title")
                {
                    X.ForeColor = Color.FromArgb(255, 200, 200, 200);
                }
            }
            foreach (Button X in showerosion.Controls.OfType<Button>())
            {
                X.FlatAppearance.BorderColor = Color.FromArgb(255, 50, 50, 50);
                X.FlatStyle = FlatStyle.Flat;
            }
            foreach (TextBox X in showerosion.Controls.OfType<TextBox>())
            {
                X.BorderStyle = BorderStyle.None;
            }
            foreach (ComboBox X in showerosion.Controls.OfType<ComboBox>())
            {
                X.FlatStyle = FlatStyle.Flat;
            }

            foreach (Control X in showpreview.Controls)
            {
                X.BackColor = Color.FromArgb(255, 31, 31, 31);
                X.ForeColor = Color.FromArgb(255, 255, 255, 255);
            }
            foreach (Button X in showpreview.Controls.OfType<Button>())
            {
                X.FlatAppearance.BorderColor = Color.FromArgb(255, 50, 50, 50);
                X.FlatStyle = FlatStyle.Flat;
            }
            foreach (TextBox X in showpreview.Controls.OfType<TextBox>())
            {
                X.BorderStyle = BorderStyle.None;
            }
            foreach (ComboBox X in showpreview.Controls.OfType<ComboBox>())
            {
                X.FlatStyle = FlatStyle.Flat;
            }
        }


        public uint[,] ExternalMorphologicalErosion(uint[,] input, bool erode)  //Handle external calls of this method from the erosion form, since the methods from Core are required before performing erosion
        {
            Erosion morph = new Erosion();
            int size = globalarray.GetLength(0);
            if (size < 4096)
            {
                DialogResult morphologicalerosion = MessageBox.Show("Morphological Erosion requires a resolution of 4096 or over to work effectively. " + "\n" + "\n" + "Scale to 4096?", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk);
                if (morphologicalerosion == DialogResult.Yes)
                {
                    globalarray = CalculateDown(DetermineResizeMethod(CalculateUp(globalarray, size, size), 4096, 4096), 4096, 4096);   //Resize to 4096 x 4096
                    size = 4096;
                    globalarray = morph.MorphologicalErosion(globalarray, erode);
                    activeimage = CalculateUp(globalarray, size, size);
                }
                if (morphologicalerosion == DialogResult.No)    //Dont resize. Continues as normal
                {

                    globalarray = morph.MorphologicalErosion(globalarray, erode);
                    activeimage = CalculateUp(globalarray, size, size);
                    return input;
                }
                if (morphologicalerosion == DialogResult.Cancel)    //Cancel the generation
                {
                    return input;
                }
            }
            globalarray = morph.MorphologicalErosion(globalarray, erode);
            return globalarray;

        }
        private void ButErosionEditor_Click(object sender, EventArgs e) //Open the erosion editor
        {
            if (masterimage != null)    //Check if an image has been generated before opening
            {
                showerosion.input = globalarray;
                showerosion.originalinput = globalarray;

                showerosion.Show();
            }
            

        }
        private void Memory_Tick(object sender, EventArgs e)    //Get the program memory usage and display to the user
        {
            int memoryusage = (int)(GC.GetTotalMemory(false) / (1000 * 1000));
            lblMemory.Text = ("Memory Usage = " + (memoryusage).ToString() + "Mb");
            if (memoryusage > 1000)
            {
                GC.GetTotalMemory(true);
            }
        }
        #region MouseEffects 
        private void Octave0_MouseEnter(object sender, EventArgs e) //MOUSE EFFECTS just double the size of the octave when hovered over them
        {
            octave0.BringToFront();
            octave0.Location = new Point(octave0.Location.X - octave0.Width / 2, octave0.Location.Y - octave0.Height / 2);
            octave0.Size = new Size(octave0.Width * 2, octave0.Height * 2);

        }

        private void Octave1_MouseEnter(object sender, EventArgs e)
        {
            octave1.BringToFront();
            octave1.Location = new Point(octave1.Location.X - octave1.Width / 2, octave1.Location.Y - octave1.Height / 2);
            octave1.Size = new Size(octave1.Width * 2, octave1.Height * 2);
        }

        private void Octave2_MouseEnter(object sender, EventArgs e)
        {
            octave2.BringToFront();
            octave2.Location = new Point(octave2.Location.X - octave2.Width / 2, octave2.Location.Y - octave2.Height / 2);
            octave2.Size = new Size(octave2.Width * 2, octave2.Height * 2);
        }

        private void Octave3_MouseEnter(object sender, EventArgs e)
        {
            octave3.BringToFront();
            octave3.Location = new Point(octave3.Location.X - octave3.Width / 2, octave3.Location.Y - octave3.Height / 2);
            octave3.Size = new Size(octave3.Width * 2, octave3.Height * 2);
        }

        private void Octave4_MouseEnter(object sender, EventArgs e)
        {
            octave4.BringToFront();
            octave4.Location = new Point(octave4.Location.X - octave4.Width / 2, octave4.Location.Y - octave4.Height / 2);
            octave4.Size = new Size(octave4.Width * 2, octave4.Height * 2);
        }

        private void Octave5_MouseEnter(object sender, EventArgs e)
        {
            octave5.BringToFront();
            octave5.Location = new Point(octave5.Location.X - octave5.Width / 2, octave5.Location.Y - octave5.Height / 2);
            octave5.Size = new Size(octave5.Width * 2, octave5.Height * 2);
        }

        private void Octave6_MouseEnter(object sender, EventArgs e)
        {
            octave6.BringToFront();
            octave6.Location = new Point(octave6.Location.X - octave6.Width / 2, octave6.Location.Y - octave6.Height / 2);
            octave6.Size = new Size(octave6.Width * 2, octave6.Height * 2);
        }

        private void Octave7_MouseEnter(object sender, EventArgs e)
        {
            octave7.BringToFront();
            octave7.Location = new Point(octave7.Location.X - octave7.Width / 2, octave7.Location.Y - octave7.Height / 2);
            octave7.Size = new Size(octave7.Width * 2, octave7.Height * 2);
        }

        private void NormalMap_MouseEnter(object sender, EventArgs e)
        {
            normalMap.BringToFront();
            normalMap.Location = new Point(normalMap.Location.X - normalMap.Width / 2, normalMap.Location.Y - normalMap.Height / 2);
            normalMap.Size = new Size(normalMap.Width * 2, normalMap.Height * 2);
        }

        private void ColourMap_MouseEnter(object sender, EventArgs e)
        {
            colourMap.BringToFront();
            colourMap.Location = new Point(colourMap.Location.X - colourMap.Width / 2, colourMap.Location.Y - colourMap.Height / 2);
            colourMap.Size = new Size(colourMap.Width * 2, colourMap.Height * 2);
        }

        private void OctaveOutput_MouseEnter(object sender, EventArgs e)
        {
            octaveOutput.BringToFront();
            octaveOutput.Location = new Point(octaveOutput.Location.X - octaveOutput.Width / 2, octaveOutput.Location.Y - octaveOutput.Height / 2);
            octaveOutput.Size = new Size(octaveOutput.Width * 2, octaveOutput.Height * 2);
        }

        //////////////////////////////////////

        private void Octave0_MouseLeave(object sender, EventArgs e)
        {

            octave0.Size = new Size(octave0.Width / 2, octave0.Height / 2);
            octave0.Location = new Point(octave0.Location.X + octave0.Width / 2, octave0.Location.Y + octave0.Height / 2);
        }

        private void Octave1_MouseLeave(object sender, EventArgs e)
        {
            octave1.Size = new Size(octave1.Width / 2, octave1.Height / 2);
            octave1.Location = new Point(octave1.Location.X + octave1.Width / 2, octave1.Location.Y + octave1.Height / 2);
        }

        private void Octave2_MouseLeave(object sender, EventArgs e)
        {
            octave2.Size = new Size(octave2.Width / 2, octave2.Height / 2);
            octave2.Location = new Point(octave2.Location.X + octave2.Width / 2, octave2.Location.Y + octave2.Height / 2);
        }

        private void Octave3_MouseLeave(object sender, EventArgs e)
        {
            octave3.Size = new Size(octave3.Width / 2, octave3.Height / 2);
            octave3.Location = new Point(octave3.Location.X + octave3.Width / 2, octave3.Location.Y + octave3.Height / 2);
        }

        private void Octave4_MouseLeave(object sender, EventArgs e)
        {
            octave4.Size = new Size(octave4.Width / 2, octave4.Height / 2);
            octave4.Location = new Point(octave4.Location.X + octave4.Width / 2, octave4.Location.Y + octave4.Height / 2);
        }

        private void Octave5_MouseLeave(object sender, EventArgs e)
        {
            octave5.Size = new Size(octave5.Width / 2, octave5.Height / 2);
            octave5.Location = new Point(octave5.Location.X + octave5.Width / 2, octave5.Location.Y + octave5.Height / 2);
        }

        private void Octave6_MouseLeave(object sender, EventArgs e)
        {
            octave6.Size = new Size(octave6.Width / 2, octave6.Height / 2);
            octave6.Location = new Point(octave6.Location.X + octave6.Width / 2, octave6.Location.Y + octave6.Height / 2);
        }

        private void Octave7_MouseLeave(object sender, EventArgs e)
        {
            octave7.Size = new Size(octave7.Width / 2, octave7.Height / 2);
            octave7.Location = new Point(octave7.Location.X + octave7.Width / 2, octave7.Location.Y + octave7.Height / 2);
        }

        private void NormalMap_MouseLeave(object sender, EventArgs e)
        {
            normalMap.Size = new Size(normalMap.Width / 2, normalMap.Height / 2);
            normalMap.Location = new Point(normalMap.Location.X + normalMap.Width / 2, normalMap.Location.Y + normalMap.Height / 2);
        }

        private void ColourMap_MouseLeave(object sender, EventArgs e)
        {
            colourMap.Size = new Size(colourMap.Width / 2, colourMap.Height / 2);
            colourMap.Location = new Point(colourMap.Location.X + colourMap.Width / 2, colourMap.Location.Y + colourMap.Height / 2);
        }

        private void OctaveOutput_MouseLeave(object sender, EventArgs e)
        {
            octaveOutput.Size = new Size(octaveOutput.Width / 2, octaveOutput.Height / 2);
            octaveOutput.Location = new Point(octaveOutput.Location.X + octaveOutput.Width / 2, octaveOutput.Location.Y + octaveOutput.Height / 2);
        }
        #endregion

        private void PctColourOutput_Click(object sender, EventArgs e)  //Switch to previewing the colour map
        {
            if (showcolour.outputimg != null)
            {
                activeimage = showcolour.outputimg;
                butColourMap.Enabled = false;
                butNormalMap.Enabled = false;
                butErosionEditor.Enabled = false;
            }

        }

        private void PctNormalOutput_Click(object sender, EventArgs e)  //Switch to previewing the normal map
        {
            if (shownormal.output != null)
            {
                activeimage = shownormal.output;
                butColourMap.Enabled = false;
                butNormalMap.Enabled = false;
                butErosionEditor.Enabled = false;
            }

        }

        private void PctErosionOutput_Click(object sender, EventArgs e) //Switch to previewing the erosion image
        {
            if (showerosion.outputimg != null)
            {
                activeimage = showerosion.outputimg;
                shownormal.source = activeimage;
                showcolour.input = activeimage;
                showcolour.RefreshMap();
                shownormal.RefreshMap();
                butColourMap.Enabled = true;
                butNormalMap.Enabled = true;
                butErosionEditor.Enabled = true;

            }

        }

        private void PctOriginalOutput_Click(object sender, EventArgs e)    //Switch to previewing the output image
        {
            if (pctOriginalOutput.Image != null)
            {
                activeimage = (Bitmap)pctOriginalOutput.Image;
                shownormal.source = activeimage;
                showcolour.input = activeimage;
                showcolour.RefreshMap();
                shownormal.RefreshMap();
                butColourMap.Enabled = true;
                butNormalMap.Enabled = true;
                butErosionEditor.Enabled = true;
            }

        }

        private void ButPreviewMaps(object sender, EventArgs e)
        {
            if (shownormal.output == null || showcolour.outputimg == null)  //Validation check before opening the preview form
            {
                DialogResult noNormalMap = MessageBox.Show("You must generate at least a heightmap, colour map and normal map before you can render a result.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                if (noNormalMap == DialogResult.OK)
                {
                    return;
                }
            }
            else
            {
                if (shownormal.transparent == false)
                {
                    DialogResult noTransparency = MessageBox.Show("Your normal map must be generated with the transparency option enabled.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    if (noTransparency == DialogResult.OK)
                    {
                        return;
                    }
                }

                showpreview.overlayednormal = pctOriginalOutput.Image.Clone() as Bitmap;    //Copy the bitmaps into the preview form. MUST be cloned otherwise they are just references that can be overwritten

                showpreview.inputheight = pctOriginalOutput.Image.Clone() as Bitmap;
                if (pctErosionOutput.Image != null)
                {
                    showpreview.inputheighterosion = pctErosionOutput.Image.Clone() as Bitmap;
                }
                showpreview.inputcolour = pctColourOutput.Image.Clone() as Bitmap;
                showpreview.inputnormal = pctNormalOutput.Image.Clone() as Bitmap;
                using (Graphics g = Graphics.FromImage(showpreview.overlayednormal))
                {
                    g.DrawImage(showpreview.inputnormal.Clone() as Bitmap, new Rectangle(0, 0, showpreview.inputnormal.Width, showpreview.inputnormal.Height));
                    //Overlay the normal map now instead of when it's needed - improved performance when overlaying in the preview form
                }
                showpreview.ResetTags();
                showpreview.Show();
                
                showpreview.updated = true;
            }
        }

        private void ButReseed_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            textSeed.Text = rnd.Next(0, 999999999).ToString();  //Create a new seed value
        }
        public uint[,] PrepareCracks(uint[,] data, int size, int unifiedsize, int count)    //Prepare for crack generation - this method handles the workspace and blending the array into the octave
        {
            uint[,] cloneddata = data.Clone() as uint[,];
            Erosion rivers = new Erosion();
            int globalsize = data.GetLength(0);
            Random rnd = new Random();
            Random rnd2 = new Random(rnd.Next());
            uint[,] workspace = new uint[size, size];   //Create the workspace for the cracks based on the image size
            for (int i = 0; i < count; i++)
            {
                workspace = GenerateCracks(workspace, true, new Point(0, 0), rnd, rnd2, 16, size);  //Call the method and generate the workspace before blending
            }
            
            Bitmap resized = DetermineResizeMethod(CalculateUp(workspace, size, size), unifiedsize, unifiedsize);
            workspace = CalculateDown(resized, unifiedsize, unifiedsize);
            if (checkBlurCracks.Checked == true)
            {
                workspace = boxBlur(workspace, workspace.GetLength(0), workspace.GetLength(1)); //Usually reduces resize artifacts, since the cracks are generated at a much lower resolution
            }
            Parallel.For(0, unifiedsize, i =>   //Begin blending the cracks in to the octave
            {
                int value = 0;
                for (int b = 0; b < unifiedsize; b++)
                {
                    if (workspace[i, b] != 0) //When the workspace is 0, there is no crack data there at all. Cracks are actually white when they are generated.
                    {

                        double percentage = (1d - (double)workspace[i, b] / 255d);  //Inverse percentage between 0 and 1. 1d - 1 is 0, so a white crack from the source becomes black in the output
                        value = (int)((double)data[i, b] * (percentage));

                        if (value < 0)  //Value will become an unsigned integer and cannot be negative
                        {
                            value = 0;
                        }
                        int average = 0;
                        int processed = 0;

                        for (int x = i - 1; x < i + 2; x++) //Begin creating a bias which helps with blending the cracks
                        {
                            for (int y = b - 1; y < b + 2; y++)
                            {
                                if (x > 0 && x < unifiedsize - 1 && y > 0 && y < unifiedsize - 1)
                                {
                                    average += (int)cloneddata[x, y];
                                    processed++;
                                }

                            }
                        }
                        data[i, b] = (uint)(average / 9);
                        data[i, b] = (uint)((value + value + value + value + data[i, b]) / 5);  //Heavy bias towards the cracks.
                    }
                }
            });
            return data;
        }
        public uint[,] GenerateCracks(uint[,] workspace, bool nexuspoints, Point start, Random rnd, Random rnd2, int iteration, int size)
        {
            int spawnprobability = trackCrackChance.Value;
            iteration = iteration / 2;
            int locationx;
            int locationy;
            if (nexuspoints == true && iteration > 4)
            {
                locationx = rnd.Next(0, size);  //starting locations
                locationy = rnd2.Next(0, size);

            }
            else
            {
                locationx = start.X;
                locationy = start.Y;
            }

            int originallocationx = locationx;
            int originallocationy = locationy;
            int length;
            if (nexuspoints == true && iteration > 4)   //Determine the length of a new, sub crack, only when iteration is small enough
            {
                length = rnd.Next(size / 3, size * 2);
            }
            else
            {
                length = rnd.Next(1, (iteration * 16) / (int)((double)size / 128d));
            }

            int newlocationx = 0;
            int newlocationy = 0;   //New locations for the crack's next pixel
            int biascounter = 0;
            double oldmagnitude = 0;
            for (int i = 0; i < length; i++)
            {

                double magnitude;   //Check if the crack has gone back on itself or not. If it has, try again until it hasn't
                do
                {
                    newlocationx = rnd.Next(locationx - 1, locationx + 2);
                    newlocationy = rnd.Next(locationy - 1, locationy + 2);
                    magnitude = Math.Sqrt((double)((newlocationx - originallocationx) * (newlocationx - originallocationx)) + ((newlocationy - originallocationy) * (newlocationy - originallocationy)));
                }
                while (magnitude <= oldmagnitude);

                if (newlocationx < 0 || newlocationx > size - 1)    //Out of bounds detection
                {
                    break;
                }
                if (newlocationy < 0 || newlocationy > size - 1)
                {
                    break;
                }

                int nexuschance = rnd.Next(0, 100);
                int spawnchancewithiteration = ((16 - iteration) * spawnprobability);   //Spawn chance increases as iteration decreases - inverse proportion
                if (nexuspoints == true)
                {
                    if (nexuschance > spawnchancewithiteration)
                    {
                        Point nexusstart = new Point(newlocationx, newlocationy);
                        int amounttogenerate = rnd.Next(1, 5);
                        bool repeat;
                        if (iteration > 4)
                        {
                            repeat = true;
                        }
                        else
                        {
                            repeat = false;
                        }
                        GenerateCracks(workspace, repeat, nexusstart, new Random(rnd.Next()), new Random(rnd2.Next()), iteration, size); //Recursion - Start another crack generating if the spawn chance is met.
                    }
                }


                int centrex = newlocationx;
                int centrey = newlocationy;

                double maxmagnitude;
                int crackrange;
                double lightness = ((iteration + iteration / 2) * 20) - 1;
                crackrange = (int)(((iteration * 2) / ((double)size / 90d) + 1));
                maxmagnitude = crackrange / 2;
                for (int x = newlocationx - crackrange; x < newlocationx + crackrange + 1; x++) //Drawing the cracks. Using Pythagoras' theorem to draw a circle. Requires finding the magnitude and checking for out of bounds
                {
                    for (int y = newlocationy - crackrange; y < newlocationy + crackrange + 1; y++)
                    {
                        if (x > 0 && x < size - 1 && y > 0 && y < size - 1)
                        {

                            double magnitudeofzone = Math.Sqrt((double)(((newlocationx - x) * (newlocationx - x)) + ((newlocationy - y) * (newlocationy - y))));
                            double percentagemagnitude = magnitudeofzone / maxmagnitude;
                            if ((1 - percentagemagnitude) * lightness > workspace[x, y])
                            {
                                double temp = (int)((1 - percentagemagnitude) * lightness);
                                workspace[x, y] = (uint)temp;
                            }

                        }
                    }
                }
                locationx = newlocationx;
                locationy = newlocationy;
                oldmagnitude = magnitude;   //Update the locations and previous range-from-start so that the next pixel can start from where the last one left off and not go backwards
                biascounter++;
            }
            return workspace;
        }

        private void CheckCracks_CheckedChanged(object sender, EventArgs e) //Show/Hide the advanced crack options when enabled or disabled.
        {
            if (checkCracks.Checked == true)
            {
                trackCrackChance.Show();
                checkBlurCracks.Show();
                lblCracks.Show();
            }
            else
            {
                trackCrackChance.Hide();
                checkBlurCracks.Hide();
                lblCracks.Hide();
            }
        }

        //CUDA check. If CUDA is mid-identified as being disabled, the user is able to force-enable it, but the program will catch any errors if there truly is no CUDA support
        private void ButCuda_Click(object sender, EventArgs e)  
        {
            cuda = !cuda;
            if (cuda == true)
            {
                butCuda.Text = "Cuda: Enabled";
                MessageBox.Show("WARNING:\nIf your graphics card does not support CUDA acceleration, keep this setting off!", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                butCuda.Text = "Cuda: Disabled";
            }
        }

        private void CheckLakes_CheckedChanged(object sender, EventArgs e)
        {
            if (checkLakes.Checked == true) //Show or hide the advanced lake options when they are enabled - reduces clutter when disabled
            {
                trackLakes.Show();
                lblLakes.Show();

            }
            else
            {
                trackLakes.Hide();
                lblLakes.Hide();
            }
        }

        private void TrackRenderRes_Scroll(object sender, EventArgs e)
        {
            UpdateText();
        }
            //Update the text next to each trackbar that display the values of the variables
        private void TrackIteration_Scroll(object sender, EventArgs e)
        {
            UpdateText();
        }

        private void TrackFreqMult_Scroll(object sender, EventArgs e)
        {
            UpdateText();
        }

        private void TrackOctaves_Scroll(object sender, EventArgs e)
        {
            UpdateText();
        }

        private void TrackPersistence_Scroll(object sender, EventArgs e)
        {
            UpdateText();
        }

        private void TrackSize_Scroll(object sender, EventArgs e)
        {
            UpdateText();
        }

        private void TrackCrackChance_Scroll(object sender, EventArgs e)
        {
            UpdateText();
        }

        private void TrackLakes_Scroll(object sender, EventArgs e)
        {
            UpdateText();
        }

        private void TrackContrast_Scroll(object sender, EventArgs e)
        {
            UpdateText();
        }

        private void TrackSmoothGenerations_Scroll(object sender, EventArgs e)
        {

            UpdateText();
        }
        private void UpdateText()   //Update all of the text when a value is changed. This is done all at once because changing the preset changes all of the text.
        {
            int trackedsize = trackRenderRes.Value * 64;
            lblRenderRes.Text = "Resolution = " + trackedsize.ToString();
            lblSmoothGen.Text = "Generations = " + trackSmoothGenerations.Value.ToString();
            lblContrast.Text = "Contrast = " + ((double)trackContrast.Value / 100).ToString();
            lblLakes.Text = "Height = " + trackLakes.Value.ToString();
            lblCracks.Text = "Chance = " + ((double)trackCrackChance.Value / 10d).ToString();
            lblSize.Text = "Size = " + (trackSize.Value * 64).ToString();
            lblOctaves.Text = "Octaves = " + trackOctaves.Value.ToString();
            lblFreq.Text = "Frequency = " + trackIteration.Value.ToString();
            lblPersistence.Text = "Persistence = " + trackPersistence.Value.ToString();
            lblStep.Text = "Step = " + trackFreqMult.Value.ToString();
        }
        private void DropPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckPreset();
        }

        private void ButSwitchMode_Click(object sender, EventArgs e)    //Switch between light and dark mode, update the button text
        {
            if (darkmode == false)
            {
                butSwitchMode.Text = "Selected: Dark Mode";
                darkmode = true;
                DarkMode();
                return;
            }
            if (darkmode == true)
            {
                butSwitchMode.Text = "Selected: Light Mode";
                darkmode = false;
                LightMode();
                return;
            }
        }
    }
    public class Erosion : Core
    {

        //STEPS:
        //Find a random spot in the mountains to start tracing a river
        //Make river choose the lowest adjacent pixel
        //Make river lower height of the current pixel AFTER it moves to the adjacent pixel ONLY if depth > 0, otherwise just blur it
        //If river hits dead end, attempt to tunnel unless too steep
        //If river hits dead end and is under a certain height, stop
        //Repeat for number of cycles * number of rivers

        #region globals
        uint[,] erosionarray;
        uint[,] originalarray;
        #endregion
        public void SetErosionArray(uint[,] data)
        {
            #region SetErosionArray
            int size = data.GetLength(0);
            erosionarray = data.Clone() as uint[,];
            originalarray = data.Clone() as uint[,];
            originalarray = boxBlur(originalarray, size, size); //Blurring takes time however it increases the chances of a river making it further.
            #endregion
        }
        public int[] GenerateRiverStart(Random rnd)  //Generate the starting position for a river
        {
            #region GenerateRiverStart
            int size = originalarray.GetLength(0);
            int x = rnd.Next(5, size - 5);
            int y = rnd.Next(5, size - 5);
            if (originalarray[x, y] < 20) { return new int[] { 25565, 25565 }; }            //25565 = not a valid spot

            else { return new int[] { x, y }; }
            #endregion
        }
        public int[] NextLowestValue(int coord1, int coord2, Random rnd, int size)  //Finds the next lowest value out of the adjacent pixels
        {
            #region NextLowestValue
            int returncoord1 = 0;
            int returncoord2 = 0;
            if (coord1 < 3 || coord2 < 3 || coord1 >= size - 2 || coord2 >= size - 2)
            {
                return new int[] { coord1, coord2 };

            }
            uint minimumvalue = originalarray[coord1, coord2];
            if (originalarray[coord1 + 1, coord2] <= minimumvalue)  //Check nearby pixels
            {
                minimumvalue = originalarray[coord1 + 1, coord2];
                returncoord1 = coord1 + 1;
                returncoord2 = coord2;
            }
            if (originalarray[coord1 - 1, coord2] <= minimumvalue)
            {
                minimumvalue = originalarray[coord1 - 1, coord2];
                returncoord1 = coord1 - 1;
                returncoord2 = coord2;
            }
            if (originalarray[coord1, coord2 + 1] <= minimumvalue)
            {
                minimumvalue = originalarray[coord1, coord2 + 1];
                returncoord1 = coord1;
                returncoord2 = coord2 + 1;
            }
            if (originalarray[coord1, coord2 - 1] <= minimumvalue)
            {
                minimumvalue = originalarray[coord1, coord2 - 1];
                returncoord1 = coord1;
                returncoord2 = coord2 - 1;
            }
            if (minimumvalue == originalarray[coord1, coord2])  //If the lowest value is the same value as the current pixel
            {                                                   //there has to be a random element whether the river continues or not
                int chancetopush = rnd.Next(0, 100);
                if (chancetopush > 0)
                {
                    int pushdirectionX = rnd.Next(0, 50);
                    int pushdirectionY = rnd.Next(0, 50);
                    if (pushdirectionX > 25)
                    {
                        pushdirectionX = -1;
                    }
                    else
                    {
                        pushdirectionX = 1; //The push directions are generated at random,
                    }                       //since the river is running out of options and might not go very far. Its velocity in real life would be too small to accurately predict its path
                    if (pushdirectionY > 25)
                    {
                        pushdirectionY = -1;
                    }
                    else
                    {
                        pushdirectionY = 1;
                    }
                    return new int[] { coord1 + pushdirectionX, coord2 + pushdirectionY };
                }

            }
            return new int[] { returncoord1, returncoord2 };
            #endregion
        }
        public void TraceRiver(int x, int y, Random rnd, int size, int depth)
        {
            #region TraceRiver
            if (x == 25565 || y == 25565)
            {
                return; //This was not a valid spot for the river to start in. Tacing stops here.
            }
            bool moved = false;
            int[] newpos = new int[2];
            int[] xy;
            if (x >= size - 2)
            {
                return;
            }
            if (y >= size - 2)
            {
                return;
            }
            for (int a = 0; a < 1000; a++)
            {
                xy = NextLowestValue(x, y, rnd, size);
                moved = false;
                if (x == xy[0] && y == xy[1])
                {
                    int chancetopush = rnd.Next(0, 150);    
                    if (x > 5 && x < originalarray.GetLength(0) - 5 && y > 5 && y < originalarray.GetLength(1) - 5 && chancetopush > 20)
                    {
                        xy = NextLowestValue(x, y, rnd, size);  //If in bounds and within the chance to push to another value, go again
                    }
                    else
                    {
                        return;
                    }


                }
                int riverminmax = rnd.Next(0, 10);
                if (originalarray[x, y] > riverminmax)
                {
                    double erosionfactor = ((double)(originalarray[x, y]) / 255) * depth; //The * depth is the value to take away from the terrain
                    if (erosionfactor < 0.5)
                    {
                        erosionfactor = 0;  //Accounting for values less than 0 otherwise rivers turn into lakes (black) or go negative
                    }
                    double differenceRight = (((double)(Math.Abs((double)originalarray[x, y] - (double)originalarray[x + 1, y]) / 255) * (double)originalarray[x, y]));
                    double differenceLeft = ((double)(Math.Abs((double)originalarray[x, y] - (double)originalarray[x - 1, y]) / 255) * (double)originalarray[x, y]);
                    double differenceUp = ((double)(Math.Abs((double)originalarray[x, y] - (double)originalarray[x, y - 1]) / 255) * (double)originalarray[x, y]);
                    double differenceDown = ((double)(Math.Abs((double)originalarray[x, y] - (double)originalarray[x, y + 1]) / 255) * (double)originalarray[x, y]);

                    //Calculating the differences between the adjacent pixels means I can determine how to erode the terrain from there, only if difference / 2 = erosionfactor.

                    if (originalarray[x, y] > erosionfactor + 1)
                    {
                        erosionarray[x, y] = (uint)(((originalarray[x, y] + originalarray[x - 1, y] + originalarray[x + 1, y] + originalarray[x, y - 1] + originalarray[x, y + 1] + originalarray[x + 1, y - 1] + originalarray[x - 1, y + 1] + originalarray[x - 1, y - 1] + originalarray[x + 1, y + 1]) / 9) - erosionfactor);
                        int min = 0;
                        int processed = 0;
                        if (differenceRight / 2 > erosionfactor)
                        {
                            min += (int)originalarray[x + 1, y];
                            processed++;
                        }
                        if (differenceLeft / 2 > erosionfactor)
                        {
                            min += (int)originalarray[x - 1, y];
                            processed++;
                        }
                        if (differenceUp / 2 > erosionfactor)
                        {
                            min += (int)originalarray[x, y - 1];
                            processed++;
                        }
                        if (differenceDown / 2 > erosionfactor)
                        {
                            min += (int)originalarray[x, y + 1];
                            processed++;
                        }
                        if (processed > 0)
                        {
                            min = min / processed;
                            erosionarray[x - 1, y] = (uint)(min - erosionfactor / 2);
                            erosionarray[x + 1, y] = (uint)(min - erosionfactor / 2);
                            erosionarray[x, y - 1] = (uint)(min - erosionfactor / 2);
                            erosionarray[x, y + 1] = (uint)(min - erosionfactor / 2);
                        }
                    }
                }

                if (moved == false) //Invalid movement, river ends here (prevents unecessary slow downs)
                {
                    x = xy[0];
                    y = xy[1];
                }

            }
            #endregion
        }
        public uint[,] ReturnArray()
        {
            return erosionarray;    //Return the erosionarray to the traceriver method - this is the output
        }
        public uint[,] MorphologicalErosion(uint[,] data, bool erode)
        {
            #region MorphologicalOperations
            //Can't use the original array to perform the operation on
            int size = data.GetLength(0);
            uint[,] output = new uint[size, size];

            Parallel.For(1, size - 2, i =>
            {
                int minimum = 255;
                int maximum = 0;
                int processed = 0;
                for (int b = 1; b < size - 2; b++)
                {
                    maximum = 0;
                    minimum = 255;
                    processed = 0;
                    for (int x = i - 1; x < i + 2; x++)
                    {
                        for (int y = b - 1; y < b + 2; y++) //Search area is 3 x 3
                        {
                            processed++;
                            if (x < 0)
                            {
                                x = 1;
                                processed--;
                            }
                            if (y < 0)
                            {
                                y = 1;
                                processed--;    //Prevent out-of-bounds
                            }
                            if (y >= size)
                            {
                                y = size - 1;
                                processed--;
                            }
                            if (x >= size)
                            {
                                x = size - 1;
                                processed--;
                            }
                            if (data[x, y] < minimum)   //Minimum and maximum must be found. Minimum for erosion and maximum for dilation.
                            {
                                minimum = (int)data[x, y];

                            }
                            if (data[x, y] > maximum)
                            {
                                maximum = (int)data[x, y];
                            }
                        }

                    }
                    if (erode == true)
                    {
                        output[i, b] = (uint)minimum;
                    }
                    else
                    {
                        output[i, b] = (uint)maximum;
                    }
                }
            });
            data = output;
            return data;
            #endregion
        }
    }
}