namespace CNN 
{
    public class Channel
    {
        public int height { get; private set; }
        public int width { get; private set; }
        public (int, int) padding { get; private set; }
        public int padVal { get; private set; }
        private double [,] channel;

        public Channel(int height, int width, double [,] channel, (int, int)? padding, int? padVal)
        {
            this.height = height;
            this.width = width;
            this.channel = channel;
            this.padding = padding ?? (0,0);
            this.padVal = padVal ?? 0;
        }

        // returns channel and applies padding if needed
        public double[,] GetChannel()
        {
            if (padding.Item1 == 0 && padding.Item2 == 0)
            {
                return channel;
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

                return newChannel;
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
    }
}