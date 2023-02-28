using System;

namespace CNN 
{
    public class Channel
    {
        public int height { get; private set; }
        public int width { get; private set; }
        public (int, int) padding { get; private set; }
        public int padVal { get; private set; }
        public double [,] channel { get; private set; }

        public Channel(int height, int width, double [,] channel)
        {
            this.height = height;
            this.width = width;
            this.channel = channel;
        }

        // Applies padding and makes changes to channel
        private void ApplyPadding()
        {
            if (padding.Item1 == 0 && padding.Item2 == 0)
            {
                // nothing
            }
            else
            {
                int padHeight = height + 2 * padding.Item1;
                int padWidth = width + 2 * padding.Item2;
                double [,] newChannel = new double [padHeight, padWidth];
                for (int i = 0; i < padHeight; i++)
                {
                    for (int j = 0; j < padWidth; j++)
                    {
                        if (i < padHeight || j < padWidth || i >= height + padHeight || j >= width + padWidth)
                        {
                            newChannel[i,j] = padVal;
                        }
                        else
                        {
                            newChannel[i,j] = channel[i,j];
                        }
                    }
                }

                channel = newChannel;
                height = padHeight;
                width = padWidth;
            }
        }

        public double [,] Slice(int a, int b, int x, int y)
        {
            double [,] slice = new double[x-a, y-b];
            for (int i = a; i < x; i++)
            {
                for (int j = b; b < y; j++)
                {
                    slice[i-a, j-b] = this.channel[i,j];
                }
            }
            return slice;
        }

        public void PrintChannel()
        {
            for (int i = 0; i < this.channel.GetLength(0); i++)
            {
                for (int j = 0; j < this.channel.GetLength(1); j++) {
                    Console.Write("{0} ", this.channel[i, j]);
                }
                Console.WriteLine();
            }
        }

        public static implicit operator Channel(Channels v)
        {
            throw new NotImplementedException();
        }
    }
}