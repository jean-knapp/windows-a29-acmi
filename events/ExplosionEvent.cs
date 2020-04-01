using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace windows_a29_acmi
{
    public class ExplosionEvent : BaseEvent
    {
        public override void calculatePositions(List<Aircraft> aircrafts, Main main)
        {
            base.calculatePositions(aircrafts, main);

            float interval = 0.05f;
            float maximum = 4;

            for (float i = 0; i <= maximum; i += interval)
            {
                AircraftPosition position = new AircraftPosition()
                {
                    time = positions[positions.Count - 1].time + interval,
                    latitude = positions[positions.Count - 1].latitude,
                    longitude = positions[positions.Count - 1].longitude,
                    altitude = (int)(positions[positions.Count - 1].altitude),
                    roll = 0,
                    pitch = 0,
                    yaw = 0
                };

                int[] heights = getHeightArray();

                int intSecond = (int)Math.Floor(i);
                int interpolateHeight = heights[intSecond] + (int)((heights[intSecond + 1] - heights[intSecond]) * (i - intSecond));
                position.radius = interpolateHeight;

                positions.Add(position);
            }
        }

        private int[] getHeightArray()
        {
            
            switch (type)
            {
                case BaseEvent.TYPE_MK82:
                    return new int[5] { 0, 460, 600, 700, 750 };

                case BaseEvent.TYPE_MK81:
                    return new int[5] { 0, 460, 600, 700, 750 };

                case BaseEvent.TYPE_SBAT70:
                    return new int[5] { 0, 240, 310, 350, 360 };
            }
            return new int[5] { 0, 460, 600, 700, 750 };
        }
    }
}
