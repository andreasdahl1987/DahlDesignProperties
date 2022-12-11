using System;
using SimHub.Plugins;
using System.Collections.Generic;

namespace DahlDesign.Plugin.Categories
{
    /// <summary>
    /// Collection of calculations
    /// </summary>
    public class Calculation
    {

        public static double StandardDeviation (List<double> raw)
        {

            double average = 0;
            for (int i = 0; i < raw.Count;i++)
            {
                average+= raw[i];
            }
            average /= raw.Count;

            if (average == 0)
            {
                return 0;
            }

            double squareSum = 0;
            for (int i = 0; i < raw.Count;i++) 
            {
                squareSum+= Math.Pow((raw[i]-average),2);
            }
            squareSum/= (raw.Count-1);

            return Math.Sqrt(squareSum);
        }

        public static List<double> SampleExtractFromPosition(int currentPosition, int maxItems, int minItems, List<double> raw)
        {

            if (currentPosition < (minItems-1))
            {
                List<double> zero = new List<double> { 0 };
                return zero;
            }
            int itemCount = Math.Min(maxItems-1, currentPosition);

            List<double> extraction = new List<double>();
            for(int i = currentPosition-itemCount; i < currentPosition+1; i++)
            {
                extraction.Add(raw[i]);
            }
            return extraction;
        }

        public static double AverageFromSample(int currentPosition, int maxItems, int minItems, List<double> raw)
        {

            double average = 0;

            if (currentPosition < (minItems - 1))
            {
                return average;
            }

            int itemCount = Math.Min(maxItems - 1, currentPosition);

            for (int i = currentPosition - itemCount; i < currentPosition + 1; i++)
            {
                average += raw[i];
            }

            average /= itemCount;

            return average;
        }

    }
}
