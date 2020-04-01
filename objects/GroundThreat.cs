using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace windows_a29_acmi
{
    class GroundThreat : BaseEntity
    {
        public List<AircraftPosition> positions;

        public int radius; // in metres


        public static int TYPE_AAA = 0;
        public static int TYPE_SAM = 1;

        public GroundThreat(int radius) : base()
        {
            positions = new List<AircraftPosition>();

            this.radius = radius;
        }
    }
}
