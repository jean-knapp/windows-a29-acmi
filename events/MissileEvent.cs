using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace windows_a29_acmi
{
    public class MissileEvent : BaseEvent
    {
        public override void calculatePositions(List<Aircraft> aircrafts, Main main)
        {
            base.calculatePositions(aircrafts, main);

            double speed = 0.4629603 / 60;
            double speedFt = 2813.32 * 0.3048;

            float fov = float.Parse(main.combatMissileFOV.EditValue.ToString());
            float interval = 0.05f;
            int timeout = int.Parse(main.combatMissileTimeout.EditValue.ToString());

            for (float i = 0; i < timeout; i += interval)
            {
                AircraftPosition position = new AircraftPosition()
                {
                    time = positions[positions.Count - 1].time + interval,
                    latitude = positions[positions.Count - 1].latitude + speed * interval * Math.Cos(positions[positions.Count - 1].pitch * Math.PI / 180.0f) * Math.Cos(positions[positions.Count - 1].yaw * Math.PI / 180),
                    longitude = positions[positions.Count - 1].longitude + speed * interval * Math.Cos(positions[positions.Count - 1].pitch * Math.PI / 180.0f) * Math.Sin(positions[positions.Count - 1].yaw * Math.PI / 180),
                    altitude = (int)(positions[positions.Count - 1].altitude + speedFt * interval * Math.Sin(positions[positions.Count - 1].pitch * Math.PI / 180.0f)),
                    roll = 0,
                    pitch = positions[positions.Count - 1].pitch,
                    yaw = positions[positions.Count - 1].yaw,
                    locked = null
                };

                //position.label = "" + (int)position.yaw;

                bool targetLocked = false;

                foreach (Aircraft aircraft in aircrafts)
                {

                    if (aircraft == parent) // Don't shoot yourself
                        continue;

                    List<AircraftPosition> positions = new List<AircraftPosition>();

                    foreach (BaseEvent ev in aircraft.events.Where(n => n.type == BaseEvent.TYPE_ECM))
                    {
                        AircraftPosition evPosition = ev.getPosition(position.time);
                        if (evPosition != null)
                            positions.Add(evPosition);
                    }

                    AircraftPosition tPosition = aircraft.getPosition(position.time);
                    positions.Add(tPosition);

                    for (int j = 0; j < positions.Count; j++)
                    {
                        AircraftPosition targetPosition = positions[j];
                        double yaw = Math.Atan2((targetPosition.longitude - position.longitude), (targetPosition.latitude - position.latitude)) * 180.0f / Math.PI;
                        double distance = Math.Sqrt(Math.Pow((targetPosition.longitude - position.longitude), 2) + Math.Pow(targetPosition.latitude - position.latitude, 2)) * 60 * 1852; // in metres
                        double height = targetPosition.altitude - position.altitude; // in metres

                        double d3Distance = Math.Sqrt(Math.Pow(distance, 2) + Math.Pow(height, 2));

                        // Explode if distance < 150
                        int threshold = int.Parse(main.combatMissileTriggerDistance.EditValue.ToString());
                        if (d3Distance < threshold && j == positions.Count - 1)
                        {
                            aircraft.hits.Add(new BaseEvent()
                            {
                                time = position.time,
                                latitude = position.latitude,
                                longitude = position.longitude,
                                altitude = position.altitude,
                                type = TYPE_AIM9L,
                                parent = parent
                            });
                            return;
                        }

                        if (yaw < 0)
                        {
                            yaw += 360;
                        }

                        // TODO calculate pitch too

                        if (Math.Abs(yaw - position.yaw) <= fov / 2)
                        {
                            double pitch = Math.Atan2(height, distance) * 180 / Math.PI;
                            if (Math.Abs(pitch - position.pitch) <= fov / 2)
                            {
                                position.yaw = yaw;
                                position.pitch = pitch;

                                if (j < positions.Count - 1)
                                {
                                    // Locked on ECM
                                    position.locked = null;
                                    position.label = "Jammed by ECM";
                                }
                                else
                                {
                                    // Locked on aircraft
                                    position.locked = aircraft;
                                }

                                targetLocked = true;
                                break;
                            }
                        }
                    }

                    if (targetLocked)
                    {
                        break;
                    }
                }
                positions.Add(position);
            }
        }
    }
}
