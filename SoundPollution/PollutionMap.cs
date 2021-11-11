using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;

namespace SoundPollution
{
    class PollutionMap
    {
        public static Bitmap CreateIntensityMask(Bitmap surface, List<PollutionRadar> pollutionRadars)
        {
            Graphics drawSurface = Graphics.FromImage(surface);
            drawSurface.Clear(Color.White);

            foreach (PollutionRadar pollutionRadar in pollutionRadars)
            {
                foreach (PollutionPoint pollutionPoint in pollutionRadar.PollutionPoints)
                {
                    DrawPollutionPoint(drawSurface, pollutionPoint, 25);
                }
            }

            return surface;
        }

        public static void DrawPollutionPoint(Graphics drawSurface, PollutionPoint pollutionPoint, int radius)
        {
            // Create points generic list of points to hold circumference points
            List<System.Drawing.Point> CircumferencePointsList = new List<Point>();
            // Create an empty point to predefine the point struct used in the circumference loop
            System.Drawing.Point CircumferencePoint;
            // Create an empty array that will be populated with points from the generic list
            System.Drawing.Point[] CircumferencePointsArray;
            // Calculate ratio to scale byte intensity range from 0-255 to 0-1
            float fRatio = 1F / Byte.MaxValue;
            // Precalulate half of byte max value
            byte bHalf = Byte.MaxValue / 2;
            // Flip intensity on it's center value from low-high to high-low
            int iIntensity = (byte)(pollutionPoint.Intensity - ((pollutionPoint.Intensity - bHalf) * 2));
            // Store scaled and flipped intensity value for use with gradient center location
            float fIntensity = iIntensity * fRatio;
            // Loop through all angles of a circle
            // Define loop variable as a double to prevent casting in each iteration
            // Iterate through loop on 10 degree deltas, this can change to improve performance
            for (double i = 0; i <= 360; i += 10)
            {
                // Replace last iteration point with new empty point struct
                CircumferencePoint = new System.Drawing.Point();
                // Plot new point on the circumference of a circle of the defined radius
                // Using the point coordinates, radius, and angle
                // Calculate the position of this iterations point on the circle
                CircumferencePoint.X = Convert.ToInt32(pollutionPoint.Location.X + radius * Math.Cos((Math.PI / 180) * (i)));
                CircumferencePoint.Y = Convert.ToInt32(pollutionPoint.Location.Y + radius * Math.Sin((Math.PI / 180) * (i)));
                // Add newly plotted circumference point to generic point list
                CircumferencePointsList.Add(CircumferencePoint);
            }
            // Populate empty points system array from generic points array list
            // Do this to satisfy the datatype of the PathGradientBrush and FillPolygon methods
            CircumferencePointsArray = CircumferencePointsList.ToArray();
            // Create new PathGradientBrush to create a radial gradient using the circumference points
            PathGradientBrush GradientShaper = new PathGradientBrush(CircumferencePointsArray);
            // Create new color blend to tell the PathGradientBrush what colors to use and where to put them
            ColorBlend GradientSpecifications = new ColorBlend(3);
            // Define positions of gradient colors, use intesity to adjust the middle color to
            // show more mask or less mask
            GradientSpecifications.Positions = new float[3] { 0, fIntensity, 1 };
            // Define gradient colors and their alpha values, adjust alpha of gradient colors to match intensity
            GradientSpecifications.Colors = new Color[3]
            {
                Color.FromArgb(0, Color.White),
                Color.FromArgb(pollutionPoint.Intensity, Color.Black),
                Color.FromArgb(pollutionPoint.Intensity, Color.Black)
            };
            // Pass off color blend to PathGradientBrush to instruct it how to generate the gradient
            GradientShaper.InterpolationColors = GradientSpecifications;
            // Draw polygon (circle) using our point array and gradient brush
            drawSurface.FillPolygon(GradientShaper, CircumferencePointsArray);
        }

        public static Bitmap Colorize(Bitmap mask, byte alpha)
        {
            // Create new bitmap to act as a work surface for the colorization process
            Bitmap surfaceBitmap = new Bitmap(mask.Width, mask.Height, PixelFormat.Format32bppArgb);
            // Create a graphics object from our memory bitmap so we can draw on it and clear it's drawing surface
            Graphics surface = Graphics.FromImage(surfaceBitmap);
            surface.Clear(Color.Transparent);
            // Build an array of color mappings to remap our greyscale mask to full color
            // Accept an alpha byte to specify the transparancy of the output image
            ColorMap[] colors = CreatePaletteIndex(alpha);
            // Create new image attributes class to handle the color remappings
            // Inject our color map array to instruct the image attributes class how to do the colorization
            ImageAttributes remapper = new ImageAttributes();
            remapper.SetRemapTable(colors);
            // Draw our mask onto our memory bitmap work surface using the new color mapping scheme
            surface.DrawImage(mask, new Rectangle(0, 0, mask.Width, mask.Height), 0, 0, mask.Width, mask.Height, GraphicsUnit.Pixel, remapper);
            // Send back newly colorized memory bitmap
            return surfaceBitmap;
        }
        public static ColorMap[] CreatePaletteIndex(byte alpha)
        {
            ColorMap[] colorMap = new ColorMap[256];
            // Change this path to wherever you saved the palette image.
            Bitmap paletteBitmap = MainForm.palette;
            // Loop through each pixel and create a new color mapping

            for (int i = 0; i <= 255; i++)
            {
                colorMap[i] = new ColorMap();
                colorMap[i].OldColor = Color.FromArgb(i, i, i);
                colorMap[i].NewColor = Color.FromArgb(alpha, paletteBitmap.GetPixel(i, 0));
            }

            return colorMap;
        }
    }
}
