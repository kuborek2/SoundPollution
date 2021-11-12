using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoundPollution
{
    public partial class MainForm : Form
    {
        private Bitmap mainBackground = (Bitmap)Bitmap.FromFile(Environment.CurrentDirectory + @"\cityMap.png");
        private Bitmap marker = (Bitmap)Bitmap.FromFile(Environment.CurrentDirectory + @"\marker.png");
        public static Bitmap palette = (Bitmap)Bitmap.FromFile(Environment.CurrentDirectory + @"\colormap.jpg");

        private List<PollutionRadar> pollutionRadars = new List<PollutionRadar>();

        private static int imageWidth;
        private static int imageHeight;

        public static int ImageWidth { get => imageWidth; set => imageWidth = value; }
        public static int ImageHeight { get => imageHeight; set => imageHeight = value; }

        public MainForm()
        {
            InitializeComponent();
            pictureBox1.Image = mainBackground;

            ImageWidth = pictureBox1.Image.Width;
            ImageHeight = pictureBox1.Image.Height;

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            int radius = int.Parse(radiusTextBox.Text.ToString());
            int numberOfPoints = int.Parse(numberOfPointsTextBox.Text.ToString());
            int minIntensity = int.Parse(minIntensityTextBox.Text.ToString());
            int maxIntensity = int.Parse(maxIntensityTextBox.Text.ToString());
            GenerationType generationType = radioButton1.Checked ? GenerationType.CENTRALIZED : GenerationType.UNIFORM;

            pollutionRadars.Add(
                new PollutionRadar(
                    ((MouseEventArgs)e).Location,
                    radius,
                    numberOfPoints,
                    minIntensity,
                    maxIntensity,
                    generationType));

            pictureBox1.Invalidate();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {


            if (pollutionRadars.Count > 0)
            {
                int width = pictureBox1.Width;
                int heigth = pictureBox1.Height;

                Bitmap bitmap = new Bitmap(width, heigth);
                bitmap = PollutionMap.CreateIntensityMask(bitmap, pollutionRadars);
                bitmap = PollutionMap.Colorize(bitmap, 100);

                Bitmap pollutionMap = (Bitmap)mainBackground.Clone();
                Graphics graphics = Graphics.FromImage(pollutionMap);
                graphics.DrawImage(bitmap, 0, 0);

                foreach (PollutionRadar pollutionRadar in pollutionRadars)
                {
                    graphics.DrawImage(marker,
                        pollutionRadar.Location.X - (marker.Width / 2),
                        pollutionRadar.Location.Y - (marker.Height / 2),
                        marker.Width,
                        marker.Height);
                }

                pictureBox1.Image = pollutionMap;

            }
            else
            {
                pictureBox1.Image = mainBackground;
            }
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            pollutionRadars.Clear();
            pictureBox1.Invalidate();
        }
    }
}
