using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace windows_a29_acmi
{
    public class MK82Event : BaseEvent
    {

        public int tas = 0;
        public int impactAltitude = 0;
        public override void calculatePositions(List<Aircraft> aircrafts, Main main)
        {
            base.calculatePositions(aircrafts, main);

            AircraftPosition parentPosition = parent.getPosition(time);

            double speed = parentPosition.tas / 216000;   // Original speed in knots. final in degrees/sec
            double speedFt = parentPosition.tas / 3600 * 1852;   // Original speed in knots. final in metres/sec

            positions[0].latitude = parentPosition.latitude;
            positions[0].longitude = parentPosition.longitude;
            positions[0].altitude = parentPosition.altitude;

            //int timeout = 30;
            float interval = 0.1f;
            for (float i = 0; true; i += interval)
            {
                AircraftPosition position = new AircraftPosition()
                {
                    time = positions[positions.Count - 1].time + interval,
                    latitude = positions[positions.Count - 1].latitude + speed * interval * Math.Cos(positions[positions.Count - 1].pitch * Math.PI / 180.0f) * Math.Cos(positions[positions.Count - 1].yaw * Math.PI / 180),
                    longitude = positions[positions.Count - 1].longitude + speed * interval * Math.Cos(positions[positions.Count - 1].pitch * Math.PI / 180.0f) * Math.Sin(positions[positions.Count - 1].yaw * Math.PI / 180),
                    altitude = (int)(positions[positions.Count - 1].altitude + (speedFt * interval * Math.Sin(positions[positions.Count - 1].pitch * Math.PI / 180.0f)) - 9.807 * i * interval),
                    roll = 0,
                    pitch = positions[positions.Count - 1].pitch,
                    yaw = positions[positions.Count - 1].yaw
                };

                if (position.altitude < impactAltitude)
                {
                    position.altitude = impactAltitude;
                    positions.Add(position);

                    ExplosionEvent impactEvent = new ExplosionEvent()
                    {
                        time = position.time,
                        latitude = position.latitude,
                        longitude = position.longitude,
                        altitude = impactAltitude,
                        roll = 0,
                        pitch = 0,
                        yaw = 0
                    };
                    impactEvent.type = BaseEvent.TYPE_EXPLOSION;
                    impactEvent.calculatePositions(null, main);

                    parent.hits.Add(impactEvent);
                    return;
                }


                positions.Add(position);
            }
        }
    }
}
