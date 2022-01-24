using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SoundPollution
{
    class PollutionPoint
    {
        public Point location;     
        public byte intensity;

        public Point Location
        {
            get { return this.location; }
            set { this.location = value; }
        }
        public byte Intensity
        {
            get { return this.intensity; }
            set { this.intensity = value; }
        }
        public PollutionPoint(Point location, byte intensity)
        {

            this.location = location;
            this.intensity = intensity;
        }
        public PollutionPoint() { }
    }
}
