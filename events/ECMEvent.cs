using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace windows_a29_acmi
{
    public class ECMEvent : BaseEvent
    {
        public override void calculatePositions(List<Aircraft> aircrafts, Main main)
        {
            base.calculatePositions(aircrafts, main);

            float interval = 1f;
            for (float i = 0; i < int.Parse(main.combatECMTimeout.EditValue.ToString()); i += interval)
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

                positions.Add(position);
            }
        }
    }
}
