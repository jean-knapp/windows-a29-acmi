using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace windows_a29_acmi
{
    public class Aircraft : BaseEntity
    {
        public List<AircraftPosition> positions;

        public List<BaseEvent> events;
        public List<BaseEvent> hits;

        public String coalition = "";
        public String group = "";
        public String callSign = "";
        public String pilot = "";
        public String tailnumber = "";
        public String color = "";

        public int kills = 0;
        public int deaths = 0;
        public int assists = 0;        

        public Aircraft() : base()
        {
            positions = new List<AircraftPosition>();

            events = new List<BaseEvent>();
            hits = new List<BaseEvent>();
        }

        public AircraftPosition getPosition(double time)
        {
            // Smart search (fast)
            if (positions.Count == 0)
            {
                return null;
            }

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

            // Fallback method (slow)
            /*for (int i = 0; i < positions.Count; i++)
            {
                if (positions[i].time > time)
                {
                    return positions[i];
                }
            }*/

            return positions[positions.Count - 1];
        }

        public Aircraft getLock(AircraftPosition selfPosition, List<Aircraft> aircrafts)
        {
            int fov = 30;
            foreach(Aircraft aircraft in aircrafts)
            {

                if (aircraft == this)
                    continue;

                AircraftPosition targetPosition = aircraft.getPosition(selfPosition.time);

                double yaw = Math.Atan2((targetPosition.longitude - selfPosition.longitude), (targetPosition.latitude - selfPosition.latitude)) * 180.0f / Math.PI;
                // TODO calculate pitch too

                if (Math.Abs(yaw - selfPosition.yaw) <= fov)
                {
                    return aircraft;
                }
            }

            return null;
        }

  
        public override String Name
        {
            get {
                List<string> list = new List<string>();
                list.Add(coalition);
                list.Add(group);
                list.Add(callSign);
                list.Add(pilot);
                list.Add(tailnumber);
                while (list.Contains(""))
                {
                    list.Remove("");
                }

                return string.Join(" ", list);
            }
            set { name = value; }
        }
    }
}
