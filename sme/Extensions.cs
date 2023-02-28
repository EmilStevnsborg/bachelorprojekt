using System;
using System.Linq;

namespace CNN
{
    static class Extensions
    {
        // Takes largest value in array
        public static double Amax(this Array array)
        {
            return array.Cast<double>().Max();
        }
        //
        public static double Sum(this Array array)
        {
            return array.Cast<double>().Sum();
        }
        public static double [,] SumPairwise(this double [,] array, double [,] coArray)
        {
            int height = array.GetLength(0);
            int width = array.GetLength(1);
            double [,] outArray = new double[height, width];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    outArray[i,j] = array[i,j]+coArray[i,j];
                }
            }
            return outArray;
        }
        //
        public static double [,] Multiply(this double [,] array, double [,] coArray)
        {
            int height = array.GetLength(0);
            int width = array.GetLength(1);
            double [,] outArray = new double[height, width];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    outArray[i,j] = array[i,j]*coArray[i,j];
                }
            }
            return outArray;
        }
    }
}