namespace CNN 
{
    public abstract class Kernel
    {
        public int height { get; private set; }
        public int width { get; private set; }
        public (int, int) padding { get; private set; }
        public int padVal { get; private set; }

        public Kernel(int height, int width, (int, int)? padding, int? padVal)
        {
            this.height = height;
            this.width = width;
            this.padding = padding ?? (0,0);
            this.padVal = padVal ?? 0;
        }

        public abstract Channel KernelOperation(Channel channel);

        // returns channel and applies padding if needed
        public Channel ApplyPadding(Channel channel)
        {
            if (padding.Item1 == 0 && padding.Item2 == 0)
            {
                return channel;
            }
            else
            {
                int padHeight = height + 2 * padding.Item1;
                int padWidth = width + 2 * padding.Item2;
                double [,] newChannelValues = new double [padHeight, padWidth];
                for (int i = 0; i < padHeight; i++)
                {
                    for (int j = 0; j < padWidth; j++)
                    {
                        if (i < padHeight || j < padWidth || i >= height + padHeight || j >= width + padWidth)
                        {
                            newChannelValues[i,j] = padVal;
                        }
                        else
                        {
                            newChannelValues[i,j] = channel.channel[i,j];
                        }
                    }
                }

                Channel newChannel = new Channel(padHeight, padWidth, newChannelValues);
                return newChannel;
            }
        }

    }
}