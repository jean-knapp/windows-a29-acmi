using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace windows_a29_acmi.export
{
    class TacviewCSVFileWritter
    {
        public static void writeFile(String path, List<Aircraft> aircrafts, Main main)
        {
            for (int id = 0; id < aircrafts.Count; id++)
            {
                var csv = new StringBuilder();

                Aircraft aircraft = aircrafts[id];

                csv.AppendLine("Time,Longitude,Latitude,Altitude,Roll (deg),Pitch (deg),Yaw (deg)");

                for (int i = 0; i < aircraft.positions.Count; i++)
                {
                    AircraftPosition position = aircraft.positions[i];

                    var newLine = string.Format("{0},{1},{2},{3},{4},{5},{6}",
                        position.time.ToString(),
                        position.longitude.ToString(),
                        position.latitude.ToString(),
                        position.altitude.ToString(),
                        position.roll.ToString(),
                        position.pitch.ToString(),
                        position.yaw.ToString(),
                        position.fuel.ToString()
                    );
                    csv.AppendLine(newLine);
                }

                //after your loop
                File.WriteAllText(path + "/A-29 (" + aircraft.pilot + ") [" + aircraft.color + "].csv", csv.ToString());
            }
            Process.Start(path);
        }
    }
}
