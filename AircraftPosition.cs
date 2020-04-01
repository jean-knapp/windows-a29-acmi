using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace windows_a29_acmi
{
    public class AircraftPosition
    {
        public float time;   // In seconds
        public double longitude;
        public double latitude;
        public int altitude;
        public double roll; // In degrees
        public double pitch; // In degrees
        public double yaw; // In degrees relative to the true north

        public double magHead;
        public double trueHead;
        public double tas;
        public double cas;
        public double mach;
        public double aoa;
        public int fuel;
        public int agl;
        public int msMode;
        public int acMode;

        public Aircraft locked = null;
        public String label = null;

        public int radius = 0; // For explosions

    }
}
