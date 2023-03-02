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

        public static void PrintArray(this double [,] array)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++) {
                    Console.Write("{0} ", array[i, j]);
                }
                Console.WriteLine();
            }
        }
        public static Channel AddBias(this Channel channel, double bias)
        {
            double [,] outChannelValues = new double[channel.height, channel.width];

            for (int i = 0; i < channel.height; i++)
            {
                for (int j = 0; j < channel.width; j++)
                {
                    outChannelValues[i,j] = channel.channel[i,j]+bias;
                }
            }
            
            Channel outChannel = new Channel(channel.height, channel.width, outChannelValues);
            return outChannel;
        }
        public static Channel SumPairwise(this Channel channel, Channel coChannel)
        {
            double [,] outChannelValues = new double[channel.height, channel.width];

            for (int i = 0; i < channel.height; i++)
            {
                for (int j = 0; j < channel.width; j++)
                {
                    outChannelValues[i,j] = channel.channel[i,j]+coChannel.channel[i,j];
                }
            }
            
            Channel outChannel = new Channel(channel.height, channel.width, outChannelValues);
            return outChannel;
        }

        // returns channel and applies padding if needed
        public static Channel ApplyPadding(this Channel channel, (int, int) padding, int padVal)
        {
            if (padding.Item1 == 0 && padding.Item2 == 0)
            {
                return channel;
            }
            else
            {
                (int pr, int pc) = padding;
                int padHeight = channel.height + 2 * pr;
                int padWidth = channel.width + 2 * pc;
                double [,] newChannelValues = new double [padHeight, padWidth];
                for (int i = 0; i < padHeight; i++)
                {
                    for (int j = 0; j < padWidth; j++)
                    {
                        if (i < pr || j < pc || i >= channel.height + pr || j >= channel.width + pc)
                        {
                            newChannelValues[i,j] = padVal;
                        }
                        else
                        {
                            newChannelValues[i,j] = channel.channel[i-pr,j-pc];
                        }
                    }
                }

                Channel newChannel = new Channel(padHeight, padWidth, newChannelValues);
                return newChannel;
            }
        }
    }
}