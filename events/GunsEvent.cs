using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace windows_a29_acmi
{
    public class GunsEvent : BaseEvent
    {
        public override void calculatePositions(List<Aircraft> aircrafts, Main main)
        {
            base.calculatePositions(aircrafts, main);

            double speed = 0.4789244 / 60;
            double speedFt = 2910 * 0.3048;

            // 1.2º de pitch acima do eixo do avião

            int timeout = int.Parse(main.combatGunsBulletTimeout.EditValue.ToString());
            float interval = 0.1f;
            for (float i = 0; i < timeout; i += interval)
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

                foreach (Aircraft aircraft in aircrafts)
                {

                    if (aircraft == parent) // Don't shoot yourself
                        continue;

                    AircraftPosition targetPosition = aircraft.getPosition(position.time);
                    double distance = Math.Sqrt(Math.Pow((targetPosition.longitude - position.longitude), 2) + Math.Pow(targetPosition.latitude - position.latitude, 2)) * 60 * 1852; // in metres

                    // Explode if distance < 20
                    int threshold = int.Parse(main.combatGunsTargetEffectiveRadius.EditValue.ToString());
                    if (distance < threshold)
                    {
                        aircraft.hits.Add(new BaseEvent()
                        {
                            time = targetPosition.time,
                            latitude = targetPosition.latitude,
                            longitude = targetPosition.longitude,
                            altitude = targetPosition.altitude,
                            type = TYPE_INGUN,
                            parent = parent
                        });

                        return;
                    }
                }
                positions.Add(position);
            }
        }
    }
}
