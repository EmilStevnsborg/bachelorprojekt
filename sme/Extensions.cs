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
    }
}