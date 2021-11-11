using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SoundPollution
{
    class PollutionRadar
    {
        private Point location;
        private List<PollutionPoint> pollutionPoints;
        private int radius;
        private int minIntensity;
        private int maxIntensity;

        public PollutionRadar(Point location, int radius, int minIntensity, int maxIntensity)
        {
            this.location = location;
            this.radius = radius;
            this.minIntensity = minIntensity;
            this.maxIntensity = maxIntensity;
            this.pollutionPoints = GeneratePollution();

        }

        public Point Location { get => location; set => location = value; }
        public int Radius { get => radius; set => radius = value; }
        public int MinIntensity { get => minIntensity; set => minIntensity = value; }
        public int MaxIntensity { get => maxIntensity; set => maxIntensity = value; }
        public List<PollutionPoint> PollutionPoints { get => pollutionPoints; set => pollutionPoints = value; }

        private List<PollutionPoint> GeneratePollution()
        {

            List<PollutionPoint> pollutionPoints = new List<PollutionPoint>();
            Random random = new Random();

            int numberOfPoints = (int)Math.Round(Math.Pow(radius, 2)/100);

            for (int i = 0; i < numberOfPoints; i++)
            {
                int x = random.Next(Location.X - Radius, Location.X + Radius);
                int y = random.Next(Location.Y - Radius, Location.Y + Radius);

                byte intensity = (byte)random.Next(minIntensity, maxIntensity);


                if (x >= 0 && x < MainForm.ImageWidth &&
                    y >= 0 && y < MainForm.ImageHeight)

                    pollutionPoints.Add(new PollutionPoint(new Point(x, y), intensity));
            }

            return pollutionPoints;
        }

    }
}
