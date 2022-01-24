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
        private int numberOfPoints;
        private int minIntensity;
        private int maxIntensity;
        private GenerationType generationType;

        public PollutionRadar(Point location, int radius, int numberOfPoints, int minIntensity, int maxIntensity, GenerationType generationType = GenerationType.UNIFORM)
        {
            this.location = location;
            this.radius = radius;
            this.numberOfPoints = numberOfPoints;
            this.minIntensity = minIntensity;
            this.maxIntensity = maxIntensity;
            this.generationType = generationType;
            this.pollutionPoints = GeneratePollution();

        }

        public Point Location { get => location; set => location = value; }
        public int Radius { get => radius; set => radius = value; }
        public int MinIntensity { get => minIntensity; set => minIntensity = value; }
        public int MaxIntensity { get => maxIntensity; set => maxIntensity = value; }
        public List<PollutionPoint> PollutionPoints { get => pollutionPoints; set => pollutionPoints = value; }
        public int NumberOfPoints { get => numberOfPoints; set => numberOfPoints = value; }

        private List<PollutionPoint> GeneratePollution()
        {

            List<PollutionPoint> pollutionPoints = new List<PollutionPoint>();
            Random random = new Random();
            double radius;

            for (int i = 0; i < NumberOfPoints; i++)
            {

                switch (generationType)
                {
                    case GenerationType.CENTRALIZED:
                        radius = random.NextDouble() * Radius; // rozłożenie punktów scentrowane (im bliżej środka tym więcej punktów) 
                        break;

                    case GenerationType.UNIFORM:
                    default:
                        radius = Math.Sqrt(random.NextDouble()) * Radius; // rozłożenie punktów równomierne
                        break;
                }


                double angle = random.NextDouble() * Math.PI * 2;
                double x = Location.X + radius * Math.Cos(angle);
                double y = Location.Y + radius * Math.Sin(angle);

                byte intensity = (byte)random.Next(minIntensity, maxIntensity);

                if (x >= 0 && x < MainForm.ImageWidth &&
                    y >= 0 && y < MainForm.ImageHeight)

                    pollutionPoints.Add(new PollutionPoint(new Point((int)x, (int)y), intensity));
            }

            return pollutionPoints;
        }

    }
}
