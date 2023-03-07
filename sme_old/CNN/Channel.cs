using System;

namespace CNN 
{
    public class Channel
    {
        public int height { get; private set; }
        public int width { get; private set; }
        public double [,] channel { get; private set; }

        public Channel(int height, int width, double [,] channel)
        {
            this.height = height;
            this.width = width;
            this.channel = channel;
        }



        public double [,] Slice(int a, int b, int x, int y)
        {
            double [,] slice = new double[x-a, y-b];
            for (int i = a; i < x; i++)
            {
                for (int j = b; j < y; j++)
                {
                    slice[i-a, j-b] = this.channel[i,j];
                }
            }
            return slice;
        }
    }
}