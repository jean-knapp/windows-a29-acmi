using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace windows_a29_acmi
{
    public class BaseEvent
    {
        public const int TYPE_AIM9L = 1;
        public const int TYPE_INGUN = 2;
        public const int TYPE_ECM = 3;
        public const int TYPE_MK82 = 4;
        public const int TYPE_MK81 = 5;
        public const int TYPE_EXPLOSION = 6;
        public const int TYPE_SBAT70 = 7;

        public int type;
        public float time;   // In seconds
        public double longitude;
        public double latitude;
        public int altitude;
        public double roll; // In degrees
        public double pitch; // In degrees
        public double yaw; // In degrees relative to the true north
        public Aircraft parent;

        public List<AircraftPosition> positions;

        public virtual void calculatePositions(List<Aircraft> aircrafts, Main main)
        {
            positions = new List<AircraftPosition>();
            positions.Add(new AircraftPosition()
            {
                time = time,
                latitude = latitude,
                longitude = longitude,
                altitude = altitude,
                roll = 0,
                pitch = pitch,
                yaw = yaw
            });
        }

        public AircraftPosition getPosition(double time)
        {
            if (time < positions[0].time)
            {
                return null;
            }

            if (time > positions[positions.Count - 1].time)
            {
                return null;
            }

            // Smart search (fast)
            for (int i = 0; i < positions.Count + 599; i += 600)    // Minute search
            {
                if (positions[Math.Min(i, positions.Count - 1)].time > time)
                {
                    for (i = Math.Max(i - 600, 0); i < positions.Count + 9; i += 10)    // Second search
                    {
                        if (positions[Math.Min(i, positions.Count - 1)].time > time)
                        {
                            for (i = Math.Max(i - 10, 0); i < positions.Count; i += 1)    // Normal search
                            {
                                if (positions[i].time > time)
                                {
                                    return positions[i];
                                }
                            }
                        }
                    }
                }
            }

            return positions[positions.Count - 1];
        }
    }
}
