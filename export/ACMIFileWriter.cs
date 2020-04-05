using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace windows_a29_acmi
{
    class ACMIFileWriter
    {
        private static List<Aircraft> CalculateEventPositions(List<Aircraft> aircrafts, Main main)
        {
            for (int id = 0; id < aircrafts.Count; id++)
            {
                // Clear aircraft hits
                aircrafts[id].hits = new List<BaseEvent>();
                aircrafts[id].kills = 0;
                aircrafts[id].deaths = 0;
                aircrafts[id].assists = 0;

                // Calculate ECMs first
                foreach (BaseEvent ev in aircrafts[id].events.Where(n => n.type == BaseEvent.TYPE_ECM))
                {
                    ev.calculatePositions(aircrafts, main);
                }
            }

            for (int id = 0; id < aircrafts.Count; id++)

                // Calculate the other events
                for (int i = 0; i < aircrafts[id].events.Count; i++)
                {
                    BaseEvent ev = aircrafts[id].events[i];
                    ev.calculatePositions(aircrafts, main);
                }

                return aircrafts;
        }

        private static void CalculateKills(List<Aircraft> aircrafts, Main main)
        {
            // Aircraft hits
            int currentHits = 0;
            int hitTimeout = int.Parse(main.combatHitTimeout.EditValue.ToString());
            int hitsToKill = int.Parse(main.combatGunsHitsToKill.EditValue.ToString());
            for (int id = 0; id < aircrafts.Count; id++)
            {
                Aircraft aircraft = aircrafts[id];
                for (int i = 0; i < aircraft.hits.Count; i++)
                {
                    BaseEvent ev = aircraft.hits[i];

                    switch (ev.type)
                    {
                        case BaseEvent.TYPE_INGUN:
                            {
                                currentHits++;

                                if (i >= aircraft.hits.Count - 1 || aircraft.hits[i + 1].type != BaseEvent.TYPE_INGUN || aircraft.hits[i + 1].time >= ev.time + hitTimeout)
                                {
                                    if (currentHits < hitsToKill)
                                    {
                                        aircraft.deaths += 1;
                                        ev.parent.kills += 1;
                                    }

                                    currentHits = 0;
                                }
                            }
                            break;
                        case BaseEvent.TYPE_AIM9L:
                            {
                                aircraft.deaths += 1;
                                ev.parent.kills += 1;
                                currentHits = 0;
                            }
                            break;
                        case BaseEvent.TYPE_EXPLOSION:
                            {
                                aircraft.deaths += 1;
                            }
                            break;
                    }
                }
            }
        }

        public static void writeACMIFile(String path, List<Aircraft> aircrafts, Main main)
        {
            CalculateEventPositions(aircrafts, main);
            CalculateKills(aircrafts, main);

            var csv = new StringBuilder();
            csv.AppendLine("FileType=text/acmi/tacview");
            csv.AppendLine("FileVersion=2.1");
            csv.AppendLine("Author=Jean Knapp");

            // TODO set mission title
            if (main.titleEditBar.EditValue != null)
                csv.AppendLine("Title=" + main.titleEditBar.EditValue.ToString());

            // TODO set mission category (i.e. Close air support)
            if (main.categoryEditBar.EditValue != null)
                csv.AppendLine("Category=" + main.categoryEditBar.EditValue.ToString());

            // TODO set reference time
            if (main.dateEditBar.EditValue != null)
                csv.AppendLine("0,ReferenceTime=" + main.dateEditBar.EditValue.ToString() + "T00:00:00Z");
            else
                csv.AppendLine("0,ReferenceTime=2000-01-01T00:00:00Z");

            int eventId = 1;
            for (int id = 0; id < aircrafts.Count; id++)
            {
                Aircraft aircraft = aircrafts[id];

                String aircraftIdHex = (eventId).ToString("X7");
                eventId++;
                aircraft.hexId = aircraftIdHex;

                for (int i = 0; i < aircraft.positions.Count; i++)
                {
                    AircraftPosition position = aircraft.positions[i];

                    csv.AppendLine(string.Format("#{0}", position.time.ToString()));

                    // First line representing the aircraft should be complete
                    if (i == 0)
                    {
                        var newLine = string.Format("{0},T={1}|{2}|{3}|{4}|{5}|{6},Name={7},Type={8},ShortName={9},LongName={10},CallSign={11},Registration={12},Pilot={13},Group={14},Country={15},Coalition={16},Color={17},Shape={18}",
                            aircraftIdHex,
                            position.longitude.ToString(),
                            position.latitude.ToString(),
                            position.altitude.ToString(),
                            position.roll.ToString(),
                            position.pitch.ToString(),
                            position.yaw.ToString(),
                            "A-29",
                            "Air+FixedWing",
                            "A-29",
                            "A-29 Super Tucano",
                            aircraft.callSign,
                            aircraft.tailnumber,
                            aircraft.pilot,
                            aircraft.group,
                            "br",
                            aircraft.coalition,
                            aircraft.color,
                            "FixedWing.Tex2.obj"
                        );
                        csv.AppendLine(newLine);
                    }
                    else
                    {
                        var newLine = string.Format("{0},T={1}|{2}|{3}|{4}|{5}|{6},FuelWeight={7}",
                            aircraftIdHex,
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
                }

                // Events
                for (int i = 0; i < aircraft.events.Count; i++)
                {
                    BaseEvent ev = aircraft.events[i];

                    String eventIdHex = (eventId).ToString("X7");
                    eventId++;

                    for (int pos = 0; pos < ev.positions.Count; pos++)
                    {
                        AircraftPosition position = ev.positions[pos];

                        var timeLine = string.Format("#{0}",
                            position.time.ToString()
                        );
                        csv.AppendLine(timeLine);

                        if (pos == 0)
                        {
                            switch (ev.type)
                            {
                                case BaseEvent.TYPE_AIM9L:
                                    {
                                        var newLine = string.Format("{0},T={1}|{2}|{3}|{4}|{5}|{6},Name={7},Type={8},ShortName={9},LongName={10},Color={11},Label={12}",
                                        eventIdHex,
                                        position.longitude.ToString(),
                                        position.latitude.ToString(),
                                        position.altitude.ToString(),
                                        position.roll.ToString(),
                                        position.pitch.ToString(),
                                        position.yaw.ToString(),
                                        "AIM9L",
                                        "Missile",
                                        "AIM9L",
                                        "AIM9L Sidewinder",
                                        aircraft.color,
                                        (position.locked != null ? "Locked on " + position.locked.pilot : (position.label != null ? position.label : "No lock"))
                                        );
                                        csv.AppendLine(newLine);
                                    }
                                    break;
                                case BaseEvent.TYPE_INGUN:
                                    {
                                        var newLine = string.Format("{0},T={1}|{2}|{3}|{4}|{5}|{6},Name={7},Type={8},ShortName={9},Color={10}",
                                        eventIdHex,
                                        position.longitude.ToString(),
                                        position.latitude.ToString(),
                                        position.altitude.ToString(),
                                        position.roll.ToString(),
                                        position.pitch.ToString(),
                                        position.yaw.ToString(),
                                        ".50cal",
                                        "Bullet",
                                        ".50cal",
                                        aircraft.color
                                        );
                                        csv.AppendLine(newLine);
                                    }
                                    break;
                                case BaseEvent.TYPE_ECM:
                                    {
                                        var newLine = string.Format("{0},T={1}|{2}|{3},Type={4}",
                                                eventIdHex,
                                                ev.longitude.ToString(),
                                                ev.latitude.ToString(),
                                                ev.altitude.ToString(),
                                                "Flare"
                                                );
                                        csv.AppendLine(newLine);

                                        csv.AppendLine(string.Format("0,Event=Message|{0}|{1} used ECM",
                                                eventIdHex,
                                                "A-29 (" + aircraft.pilot + ")"
                                                ));
                                    }
                                    break;
                                case BaseEvent.TYPE_MK82:
                                    {
                                        var newLine = string.Format("{0},T={1}|{2}|{3}|{4}|{5}|{6},Name={7},Type={8},ShortName={9},Color={10}",
                                        eventIdHex,
                                        position.longitude.ToString(),
                                        position.latitude.ToString(),
                                        position.altitude.ToString(),
                                        position.roll.ToString(),
                                        position.pitch.ToString(),
                                        position.yaw.ToString(),
                                        "MK-82",
                                        "Bomb",
                                        "MK-82",
                                        aircraft.color
                                        );
                                        csv.AppendLine(newLine);
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            var newLine = string.Format("{0},T={1}|{2}|{3}|{4}|{5}|{6},Label={7}",
                                eventIdHex,
                                position.longitude.ToString(),
                                position.latitude.ToString(),
                                position.altitude.ToString(),
                                position.roll.ToString(),
                                position.pitch.ToString(),
                                position.yaw.ToString(),
                                (position.locked != null ? "Locked on " + position.locked.pilot : (position.label != null ? position.label : "No lock"))
                            );
                            csv.AppendLine(newLine);
                        }
                    }
                    csv.AppendLine("-" + eventIdHex);
                }
            }

            // Aircraft hits
            int currentHits = 0;
            int hitTimeout = int.Parse(main.combatHitTimeout.EditValue.ToString());
            int hitsToKill = int.Parse(main.combatGunsHitsToKill.EditValue.ToString());
            for (int id = 0; id < aircrafts.Count; id++)
            {
                Aircraft aircraft = aircrafts[id];
                for (int i = 0; i < aircraft.hits.Count; i++)
                {
                    BaseEvent ev = aircraft.hits[i];

                    switch (ev.type) {
                        case BaseEvent.TYPE_INGUN:
                            {
                                currentHits++;

                                String eventIdHex = (eventId).ToString("X7");
                                eventId++;

                                // Add hit object
                                var timeLine = string.Format("#{0}",
                                    ev.time.ToString()
                                );
                                csv.AppendLine(timeLine);

                                var newLine = string.Format("{0},T={1}|{2}|{3},Type={4},Radius=2",
                                        eventIdHex,
                                        ev.longitude.ToString(),
                                        ev.latitude.ToString(),
                                        ev.altitude.ToString(),
                                        "Explosion"
                                        );
                                csv.AppendLine(newLine);

                                if (i >= aircraft.hits.Count - 1 || aircraft.hits[i + 1].type != BaseEvent.TYPE_INGUN || aircraft.hits[i + 1].time >= ev.time + hitTimeout)
                                {
                                    csv.AppendLine(string.Format("0,Event=Message|{0}|{1} has been hit by x{2} .50cal",
                                        aircraft.hexId,
                                        "A-29 (" + aircraft.pilot + ")",
                                        currentHits
                                        ));

                                    if (currentHits < hitsToKill)
                                    {
                                        //aircraft.deaths += 1;
                                        //ev.parent.kills += 1;

                                        csv.AppendLine(string.Format("0,Event=Message|{0}|{1} has been killed by {2}",
                                            aircraft.hexId,
                                            "A-29 (" + aircraft.pilot + ")",
                                            "A-29 (" + ev.parent.pilot + ")"
                                        ));
                                    }
                                    
                                    currentHits = 0;
                                }


                                // Kill hit object
                                timeLine = string.Format("#{0}",
                                        (ev.time + 1).ToString()
                                    );
                                csv.AppendLine(timeLine);

                                newLine = string.Format("-{0}",
                                        eventIdHex
                                        );
                                csv.AppendLine(newLine);

                            }
                            break;
                        case BaseEvent.TYPE_AIM9L:
                            {
                                //aircraft.deaths += 1;
                                //ev.parent.kills += 1;
                                currentHits = 0;

                                // Add hit object
                                var timeLine = string.Format("#{0}",
                                    ev.time.ToString()
                                );
                                csv.AppendLine(timeLine);

                                csv.AppendLine(string.Format("0,Event=Message|{0}|{1} has been killed by {2}",
                                            aircraft.hexId,
                                            "A-29 (" + aircraft.pilot + ")",
                                            "A-29 (" + ev.parent.pilot + ")"
                                        ));
                            }
                            break;
                        case BaseEvent.TYPE_EXPLOSION:
                            {
                                String eventIdHex = (eventId).ToString("X7");
                                eventId++;

                                for (int pos = 0; pos < ev.positions.Count; pos++)
                                {
                                    AircraftPosition position = ev.positions[pos];

                                    var timeLine = string.Format("#{0}",
                                        position.time.ToString()
                                    );
                                    csv.AppendLine(timeLine);

                                    if (pos == 0)
                                    {
                                        var newLine = string.Format("{0},T={1}|{2}|{3},Type={4},Radius={5}",
                                                eventIdHex,
                                                ev.longitude.ToString(),
                                                ev.latitude.ToString(),
                                                ev.altitude.ToString(),
                                                "Explosion",
                                                position.radius
                                                );
                                        csv.AppendLine(newLine);

                                        csv.AppendLine(string.Format("0,Event=Message|{0}|Explosion!",
                                                eventIdHex,
                                                "A-29 (" + aircraft.pilot + ")"
                                                ));
                                    }
                                    else
                                    {
                                        var newLine = string.Format("{0},T={1}|{2}|{3},Radius={4}",
                                            eventIdHex,
                                            position.longitude.ToString(),
                                            position.latitude.ToString(),
                                            position.altitude.ToString(),
                                            position.radius
                                        );
                                        csv.AppendLine(newLine);
                                    }
                                }
                                csv.AppendLine("-" + eventIdHex);
                            }
                            break;
                    }
                }
            }

            // Scores
            for (int id = 0; id < aircrafts.Count; id++)
            {
                Aircraft aircraft = aircrafts[id];
                var timeLine = string.Format("#{0}",
                            aircraft.positions[aircraft.positions.Count - 1].time.ToString()
                        );
                csv.AppendLine(timeLine);

                csv.AppendLine(string.Format("0,Event=Message|{0}|{1} score: {2} kills\\, {3} assists\\, {4} deaths",
                        aircraft.hexId,
                        "A-29 (" + aircraft.pilot + ")",
                        aircraft.kills,
                        aircraft.assists,
                        aircraft.deaths
                    ));

            }

            // Threats
            for (int id = 0; id < main.Threats.Count; id++)
            {
                GroundThreat threat = main.Threats[id];

                String aircraftIdHex = (eventId).ToString("X7");
                eventId++;
                threat.hexId = aircraftIdHex;

                for (int i = 0; i < threat.positions.Count; i++)
                {
                    AircraftPosition position = threat.positions[i];

                    var timeLine = string.Format("#{0}",
                        position.time.ToString()
                    );
                    csv.AppendLine(timeLine);

                    if (i == 0)
                    {
                        var newLine = string.Format("{0},T={1}|{2}|{3},Name={4},Type={5},ShortName={6},LongName={7},EngagementRange={8},Color={9}",
                            aircraftIdHex,
                            position.longitude.ToString(),
                            position.latitude.ToString(),
                            0,
                            (threat.Name != "" ? threat.Name : "Avoidance Area"),
                            "AntiAircraft",
                            (threat.Name != "" ? threat.Name : "AVD"),
                            (threat.Name != "" ? threat.Name : "Avoidance Area"),
                            threat.radius,
                            "Red"
                        );
                        csv.AppendLine(newLine);
                    }
                }
            }

            //after your loop
            File.WriteAllText(path, csv.ToString());

            Process fileopener = new Process();
            fileopener.StartInfo.FileName = "explorer";
            fileopener.StartInfo.Arguments = "\"" + path + "\"";
            fileopener.Start();

        }
    }
}
