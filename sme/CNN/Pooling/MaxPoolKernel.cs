namespace CNN
{
    public class MaxPoolKernel : Kernel
    {
        public (int, int) stride { get; private set; }
        public MaxPoolKernel(int height,
                            int width,
                            double[,] kernel,
                            (int, int)? padding,
                            int? padVal,
                            (int, int)? stride) : base(height, width, padding, padVal)
        {
            this.stride = stride ?? (height, width);
        }

        public override Channel KernelOperation(Channel channel)
        {
            channel.PrintChannel();
            // Pad the channel
            Channel tempChannel = this.ApplyPadding(channel);
            tempChannel.PrintChannel();
            int outHeight = tempChannel.height - (this.height - 1) - (this.stride.Item1 - 1);
            int outWidth =  tempChannel.width - (this.width - 1) - (this.stride.Item2 - 1);

            // initialize the values
            double[,] outChannelValues = new double[outHeight, outWidth];

            for (int i = 0; i < outHeight; i++)
            {
                for (int j = 0; j < outWidth; j++)
                {
                    int a = i * stride.Item1;
                    int b = j * stride.Item2;
                    int x = a + this.height;
                    int y = b + this.width;

                    // Look at Extensions for documentation
                    double ij = channel.Slice(a, b, x, y).Amax();
                    outChannelValues[i,j] = ij;
                }
            }
            
            Channel outChannel = new Channel(outHeight, outWidth, outChannelValues);
            outChannel.PrintChannel();
            return outChannel;
        }
    }
}