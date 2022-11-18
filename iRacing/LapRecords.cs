using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace User.PluginSdkDemo.iRacing
{
    public class LapRecords
    {

        //CSV file adress
        string csvAdress = "";
        string path;
        public int csvIndex = 0;
        public int lapDeltaSections = 120;

        public LapRecords()
        {
            findCSV();
        }


        public void findCSV()
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
        public void addLapRecord(string track, string car, double lapTime, List<double> deltas)
        {
            {
                string[] allLines = File.ReadAllLines(path);
                string allText = File.ReadAllText(path);
                lapDeltaSections = allLines.Length - 1;
                allText = allText + track + "," + car + "," + Convert.ToString(lapTime) + "," + string.Join(",", deltas) + "\n";
                File.WriteAllText(path, allText);
            }

        }

        public void replaceLapRecord(string track, string car, double lapTime, List<double> deltas)
        {
            string[] allLines = File.ReadAllLines(path);
            allLines[csvIndex] = track + "," + car + "," + Convert.ToString(lapTime) + "," + string.Join(",", deltas);
            File.WriteAllLines(path, allLines);
        }

        public void lapFetch(ref bool initiate, string track, string car, ref TimeSpan lapTime, ref List<double> deltas, int deltaSections)
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
                        csvIndex = i;
                        lapTime = TimeSpan.FromMilliseconds(Convert.ToDouble(line[2]));
                        for (int j = 0; j < deltaSections + 1; j++)
                        {
                            deltas[j] = Convert.ToDouble(line[j + 3]);
                        }

                        break;

                    }
                }
            }
        }
    }
}