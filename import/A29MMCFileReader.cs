using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace windows_a29_acmi
{
    class A29FileReader
    {
        public static Aircraft ReadMNGFile(String path, Main main)
        {
            // Read the MNG File.
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            String directory = Path.GetDirectoryName(path);

            Aircraft aircraft = new Aircraft();

            // Read all the ACDAT files.
            foreach (XmlNode recordNode in doc.SelectSingleNode("AACMI_MNG").ChildNodes)
            {
                String recordPath = directory + "\\" + recordNode.SelectSingleNode("File_Name").InnerText;
                A29FileReader.ReadACDATFile(aircraft, recordPath);
            }

            // Read the EVENTS file.
            A29FileReader.ReadEVENTSFile(aircraft, directory + "\\EVENTS.xml", main);

            // Show a dialog to get extra data.
            AddAircraft dialog = new AddAircraft();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                aircraft.callSign = dialog.aircraft.callSign;
                aircraft.coalition = dialog.aircraft.coalition;
                aircraft.group = dialog.aircraft.group;
                aircraft.pilot = dialog.aircraft.pilot;
                aircraft.tailnumber = dialog.aircraft.tailnumber;
                aircraft.color = dialog.aircraft.color;

                // Return the new aircraft
                return aircraft;
            }
            return null;
        }
        public static void ReadACDATFile(Aircraft aircraft, String path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            foreach (XmlNode recordNode in doc.SelectSingleNode("AC_DATA").ChildNodes)
            {
                // Required stuff
                float time = (float)Math.Truncate(int.Parse(recordNode.SelectSingleNode("UTC").InnerText) / 10.0f) / 100.0f;    // Time is recorded in milliseconds

                XmlNode acNode = recordNode.SelectSingleNode("AC");
                double longitude = double.Parse(acNode.SelectSingleNode("Long").InnerText) * 180;   // Longitude is recorded from -1 to 1.
                double latitude = double.Parse(acNode.SelectSingleNode("LAT").InnerText) * 180;   // Latitude is recorded from -0.5 to 0.5.
                int altitude = (int)Math.Truncate(double.Parse(acNode.SelectSingleNode("SYS_Alt").InnerText) * 0.3048);
                double roll = double.Parse(acNode.SelectSingleNode("Roll").InnerText);
                double pitch = double.Parse(acNode.SelectSingleNode("Pitch").InnerText);
                double yaw = double.Parse(acNode.SelectSingleNode("True_Head").InnerText);

                // Extra stuff - TODO should I remove this to load the file faster?

                double magHead = double.Parse(acNode.SelectSingleNode("Mag_Head").InnerText);
                double trueHead = double.Parse(acNode.SelectSingleNode("True_Head").InnerText);
                int tas = (int)double.Parse(acNode.SelectSingleNode("TAS").InnerText);
                double mach = double.Parse(acNode.SelectSingleNode("MACH").InnerText);
                int fuel = (int)(int.Parse(acNode.SelectSingleNode("Fuel").InnerText) * 0.804); // Fuel is recorded in litres
                int msMode = (int)(int.Parse(acNode.SelectSingleNode("MS_Mode").InnerText));
                int acMode = (int)(int.Parse(acNode.SelectSingleNode("AC_Mode").InnerText));

                String casStr = acNode.SelectSingleNode("CAS").InnerText;
                int cas = (casStr.Length > 0 ? (int)double.Parse(casStr) : 0);  // CAS can be empty and throw an error

                String aoaStr = acNode.SelectSingleNode("True_AOA").InnerText;
                double aoa = (aoaStr.Length > 0 ? double.Parse(aoaStr) : 0);  // AOA can be empty and throw an error

                String aglStr = acNode.SelectSingleNode("RALT_Alt").InnerText;
                int agl = (aglStr.Length > 0 ? int.Parse(aglStr) : 0);  // AGL can be empty and throw an error

                AircraftPosition aircraftPosition = new AircraftPosition
                {
                    // Required stuff
                    time = time,
                    longitude = longitude,
                    latitude = latitude,
                    altitude = altitude,
                    roll = roll,
                    pitch = pitch,
                    yaw = yaw,

                    // Extra stuff - TODO should I remove it to save space?
                    magHead = magHead,
                    trueHead = trueHead,
                    tas = tas,
                    cas = cas,
                    mach = mach,
                    aoa = aoa,
                    fuel = fuel,
                    agl = agl,
                    msMode = msMode,
                    acMode = acMode
                };

                aircraft.positions.Add(aircraftPosition);
            }
        }

        public static void ReadEVENTSFile(Aircraft aircraft, String path, Main main)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            foreach (XmlNode acNode in doc.SelectSingleNode("EVENT").ChildNodes)
            {
                float time = (float)Math.Truncate(int.Parse(acNode.SelectSingleNode("UTC").InnerText) / 10.0f) / 100.0f;

                if (acNode.Name == "MARK")
                {
                    // TODO register marks
                    continue;
                }

                double longitude = double.Parse(acNode.SelectSingleNode("LONG").InnerText) * 180;
                double latitude = double.Parse(acNode.SelectSingleNode("LAT").InnerText) * 180;
                int altitude = (int)Math.Truncate(double.Parse(acNode.SelectSingleNode("SYS_Alt").InnerText) * 0.3048);
                double roll = double.Parse(acNode.SelectSingleNode("Roll").InnerText);
                double pitch = double.Parse(acNode.SelectSingleNode("Pitch").InnerText);
                double yaw = double.Parse(acNode.SelectSingleNode("True_Heading").InnerText);

                String weaponType = acNode.SelectSingleNode("Weapon_Type").InnerText;

                switch (weaponType)
                {
                    case "AIM9L":
                        {
                            MissileEvent aircraftEvent = new MissileEvent
                            {
                                time = time,
                                longitude = longitude,
                                latitude = latitude,
                                altitude = altitude,
                                roll = roll,
                                pitch = pitch,
                                yaw = yaw,
                                parent = aircraft
                            };

                            aircraftEvent.type = BaseEvent.TYPE_AIM9L;

                            aircraft.events.Add(aircraftEvent);
                        }
                        break;
                    case "INGUN":
                        {
                            float quantity = 0;
                            if (acNode.SelectSingleNode("Quantity") != null)
                            {
                                 quantity = int.Parse(acNode.SelectSingleNode("Quantity").InnerText);
                            } else if (acNode.SelectSingleNode("No_of_Bullets") != null)
                            {
                                quantity = int.Parse(acNode.SelectSingleNode("No_of_Bullets").InnerText);
                            } else
                            {
                                continue;
                            }

                            float rate = 1025.0f / 60.0f;  // per second;
                            float separation = 3.93f; // Distance between the two machineguns;
                            float focalDistance = 300;  // Distance the bullets should cross

                            for (float q = 0; q < quantity; q++)
                            {
                                float qTime = time + q / (rate * 2);

                                AircraftPosition aircraftPosition = aircraft.getPosition(qTime);

                                // TODO - I should create two separate machineguns for a more realistic ballistic calculation

                                // Center "double barrel" machinegun
                                GunsEvent aircraftEvent = new GunsEvent
                                {
                                    time = qTime,
                                    longitude = aircraftPosition.longitude,
                                    latitude = aircraftPosition.latitude,
                                    altitude = aircraftPosition.altitude,
                                    roll = aircraftPosition.roll,
                                    pitch = aircraftPosition.pitch + 2.5 * Math.Cos(aircraftPosition.roll * Math.PI / 180),
                                    yaw = aircraftPosition.yaw + 2.5 * Math.Sin(aircraftPosition.roll * Math.PI / 180),
                                    parent = aircraft
                                };

                                aircraftEvent.type = BaseEvent.TYPE_INGUN;

                                aircraft.events.Add(aircraftEvent);
                            }
                        }
                        break;
                    case "EX11":
                        {
                            if (main.ecmConvertBex.Checked)
                            {
                                float quantity = 6.0f;
                                float rate = 2.0f;  // per second;

                                for (float q = 0; q < quantity; q++)
                                {
                                    float qTime = time + q / rate;

                                    AircraftPosition aircraftPosition = aircraft.getPosition(qTime);

                                    ECMEvent aircraftEvent = new ECMEvent
                                    {
                                        time = qTime,
                                        longitude = aircraftPosition.longitude,
                                        latitude = aircraftPosition.latitude,
                                        altitude = aircraftPosition.altitude,
                                        roll = aircraftPosition.roll,
                                        pitch = aircraftPosition.pitch + 2.5 * Math.Cos(aircraftPosition.roll * Math.PI / 180),
                                        yaw = aircraftPosition.yaw + 2.5 * Math.Sin(aircraftPosition.roll * Math.PI / 180),
                                        parent = aircraft
                                    };

                                    aircraftEvent.type = BaseEvent.TYPE_ECM;

                                    aircraft.events.Add(aircraftEvent);
                                }
                            }
                            
                        }
                        break;
                    case "FG230":
                        {
                            MK82Event aircraftEvent = new MK82Event
                            {
                                time = time,
                                longitude = longitude,
                                latitude = latitude,
                                altitude = altitude,
                                roll = roll,
                                pitch = pitch,
                                yaw = yaw,
                                parent = aircraft
                            };

                            aircraftEvent.type = BaseEvent.TYPE_MK82;

                            int tas = int.Parse(acNode.SelectSingleNode("TAS").InnerText);
                            aircraftEvent.tas = tas;
                            aircraftEvent.impactAltitude = (int)Math.Truncate(double.Parse(acNode.SelectSingleNode("Tgt_Alt").InnerText) * 0.3048);

                            aircraft.events.Add(aircraftEvent);
                        }
                        break;
                    case "FG120":
                        {
                            MK82Event aircraftEvent = new MK82Event
                            {
                                time = time,
                                longitude = longitude,
                                latitude = latitude,
                                altitude = altitude,
                                roll = roll,
                                pitch = pitch,
                                yaw = yaw,
                                parent = aircraft
                            };

                            aircraftEvent.type = BaseEvent.TYPE_MK81;

                            aircraft.events.Add(aircraftEvent);
                        }
                        break;
                }


            }
        }

        public static List<GroundThreat> ReadAVD_AREAFile(String path, Main main)
        {
            // Read the MNG File.
            XmlDocument doc = new XmlDocument();
            //String directory = Path.GetDirectoryName(path);
            //String recordPath = directory + "\\AVD_AREA.xml";
            doc.Load(path);

            //Aircraft aircraft = new Aircraft();
            List<GroundThreat> threats = new List<GroundThreat>();

            // Read all the ACDAT files.
            foreach (XmlNode recordNode in doc.SelectSingleNode("TSD_TAC").ChildNodes)
            {
                int id = int.Parse(recordNode.SelectSingleNode("ID").InnerText);
                double lat = double.Parse(recordNode.SelectSingleNode("LAT").InnerText) * 180;
                double lng = double.Parse(recordNode.SelectSingleNode("LONG").InnerText) * 180;
                int radius = int.Parse(recordNode.SelectSingleNode("RADIUS").InnerText) * 1852; // in metres

                GroundThreat threat = new GroundThreat(radius);
                threat.positions.Add(new AircraftPosition()
                {
                    latitude = lat,
                    longitude = lng,
                    time = 0
                });

                XmlNode nameNode = recordNode.SelectSingleNode("Nome");
                if (nameNode != null)
                {
                    threat.Name = nameNode.InnerText;
                }
                
                threats.Add(threat);
               
            }

            return threats;
        }
    }
}
