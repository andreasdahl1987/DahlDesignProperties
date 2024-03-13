using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace DahlDesign.Plugin.iRacing
{
    class LapRecords
    {

        /*
         * Line 0 : Track
         * Line 1 : Car
         * Line 2 : Lap time
         * Line 3 -> Delta sections + 1, where the first line gives information about the record with regards to storage/deletion etc., and the remaining are delta values
         * Line <4 + Delta sections> -> is speed delta values
         * Line <4 + Delta sections + Delta sections> : Track temp
         * Line <5 + Delta sections + Delta sections> : Air temp
         * Line <6 + Delta sections + Delta sections> : Setup fixed/open
         */

        public static void findCSV(ref string path)
        {
            string findCSV = Assembly.GetExecutingAssembly().Location;
            findCSV = findCSV.Substring(0, findCSV.Length - 14);
            path = findCSV + "DahlDesignLapRecords.csv";

            if (!File.Exists(path))
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, true))
                {
                    file.WriteLine("\nLap records: \n\nThe record time is the 3rd item, listed after track and car model. \nThe value is lap time in milliseconds, it can be edited. \nThe remaining numbers are for delta calculations, speed delta calculation and some more. \nYou may also sort the lap times in Excel. \n\nEditing the lap time will make the delta calculations bug out, I suggest replacing all values after the lap time with '-1'\n\n");
                }
            }
        }
        public static void addLapRecord(string track, string car, double lapTime, List<double> deltas, string path, ref int index, List<double> speedRegister, double trackTemp, double airTemp, double setupFixed)
        {
            {
                string[] allLines = File.ReadAllLines(path);
                string allText = File.ReadAllText(path);
                index = allLines.Length - 1;
                allText = allText + track + "," + car + "," + Convert.ToString(lapTime) + "," + string.Join(",", deltas) + "," + string.Join(",", speedRegister) + "," + Convert.ToString(trackTemp) + "," + Convert.ToString(airTemp) + "," + Convert.ToString(setupFixed) + "\n";
                File.WriteAllText(path, allText);
            }

        }

        public static void replaceLapRecord(string track, string car, double lapTime, List<double> deltas, string path, int index, List<double> speedRegister, double trackTemp, double airTemp, double setupFixed)
        {
            string[] allLines = File.ReadAllLines(path);
            allLines[index] = track + "," + car + "," + Convert.ToString(lapTime) + "," + string.Join(",", deltas) + "," + string.Join(",", speedRegister) + "," + Convert.ToString(trackTemp) + "," + Convert.ToString(airTemp) + "," + Convert.ToString(setupFixed);
            File.WriteAllLines(path, allLines);
        }



        public static void lapFetch(ref bool initiate, string path, ref int index, string track, string car, double setup, ref TimeSpan lapTime, ref List<double> deltas, ref List<double> speeds, ref double trackTemp, ref double airTemp, ref double setupFixed, int deltaSections)
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
                        //Finding fixed setup. If no label is found, set setup to open.

                        double findSetup = 0;
                        double Setup = setup;

                        if (line.Length > deltaSections + deltaSections + 4)
                        {
                            findSetup = Convert.ToDouble(line[deltaSections + deltaSections + 6]);
                        }
                        else
                        {
                            Setup = 0;
                        }

                        if (findSetup == Setup)
                        {
                            index = i;
                            lapTime = TimeSpan.FromMilliseconds(Convert.ToDouble(line[2]));
                            for (int j = 0; j < deltaSections + 1; j++)
                            {
                                deltas[j] = Convert.ToDouble(line[j + 3]);
                            }
                            if (line.Length >= deltaSections + deltaSections + 3)
                            {
                                for (int k = 0; k < deltaSections; k++)
                                {
                                    speeds[k] = Convert.ToDouble(line[k + deltaSections + 4]);
                                }
                            }
                            else
                            {
                                for (int k = 0; k < deltaSections; k++)
                                {
                                    speeds[k] = -2;
                                }
                            }
                            if (line.Length > deltaSections + deltaSections + 4)
                            {
                                trackTemp = Convert.ToDouble(line[deltaSections + deltaSections + 4]);
                                airTemp = Convert.ToDouble(line[deltaSections + deltaSections + 5]);
                                setupFixed = Convert.ToDouble(line[deltaSections + deltaSections + 6]);
                            }
                            else
                            {
                                trackTemp = -2;
                                airTemp = -2;
                                setupFixed = 0;
                            }

                            break;
                        }

                    }
                    index = 0;
                    lapTime = new TimeSpan(0);
                    for (int a = 0; a < deltaSections + 1; a++)
                    {
                        deltas[a] = -1;
                    }
                    for (int a = 0; a < deltaSections; a++)
                    {
                        speeds[a] = -1;
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