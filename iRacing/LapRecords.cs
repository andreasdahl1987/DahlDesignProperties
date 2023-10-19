using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace DahlDesign.Plugin.iRacing
{
    class LapRecords
    {
        public static void findCSV(ref string path)
        {
            string findCSV = Assembly.GetExecutingAssembly().Location;
            findCSV = findCSV.Substring(0, findCSV.Length - 14);
            path = findCSV + "DahlDesignLapRecords.csv";

            if (!File.Exists(path))
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, true))
                {
                    file.WriteLine("\nLap records: \n\nThe record time is the 3rd item, listed after track and car model. \nThe value is lap time in milliseconds, it can be edited. \nThe remaining 121 numbers are for delta calculations. \nYou may also sort the lap times in Excel. \n\nEditing the lap time will make the delta calculations bug out, I suggest replacing all values after the lap time with '-1'\n\n");
                }
            }
        }
        public static void addLapRecord(string track, string car, double lapTime, List<double> deltas, string path, ref int index, List<double> speedRegister)
        {
            {
                string[] allLines = File.ReadAllLines(path);
                string allText = File.ReadAllText(path);
                index = allLines.Length - 1;
                allText = allText + track + "," + car + "," + Convert.ToString(lapTime) + "," + string.Join(",", deltas) + "," + string.Join(",", speedRegister) + "\n";
                File.WriteAllText(path, allText);
            }

        }

        public static void replaceLapRecord(string track, string car, double lapTime, List<double> deltas, string path, int index, List<double> speedRegister)
        {
            string[] allLines = File.ReadAllLines(path);
            allLines[index] = track + "," + car + "," + Convert.ToString(lapTime) + "," + string.Join(",", deltas) + "," + string.Join(",", speedRegister);
            File.WriteAllLines(path, allLines);
        }

        public static void lapFetch(ref bool initiate, string path, ref int index, string track, string car, ref TimeSpan lapTime, ref List<double> deltas, int deltaSections)
        {
            if (initiate)
            {
                initiate = false;
                string[] allLines = File.ReadAllLines(path);

                for (int i = 0; i < allLines.Length; i++)
                {
                    string[] line = allLines[i].Split(',');
                    if (line[0] == track && line[1] == car)
                    {
                        index = i;
                        lapTime = TimeSpan.FromMilliseconds(Convert.ToDouble(line[2]));
                        for (int j = 0; j < deltaSections + 1; j++)
                        {
                            deltas[j] = Convert.ToDouble(line[j + 3]);
                        }

                        break;

                    }
                    index = 0;
                    lapTime = new TimeSpan(0);
                    for (int a = 0; a < deltaSections + 1; a++)
                    {
                        deltas[a] = -1;
                    }
                }
            }
        }

        public static void deleteLapRecord(string track, string car, string path, int index, int deltaSections)
        {
            List<int> emptyDelta = new List<int>();
            List<int> emptySpeed = new List<int>();

            emptyDelta.Add(-2);

            for (int a = 1; a < deltaSections + 1; a++)
            {
                emptyDelta.Add(-1);
            }

            for (int a = 0; a < deltaSections; a++)
            {
                emptySpeed.Add(-1);
            }

            string[] allLines = File.ReadAllLines(path);
            allLines[index] = track + "," + car + "," + Convert.ToString(0) + "," + string.Join(",", emptyDelta) + "," + string.Join(",", emptySpeed);
            File.WriteAllLines(path, allLines);
        }
    }
}