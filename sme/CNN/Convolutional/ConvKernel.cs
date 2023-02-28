namespace CNN
{
    public class ConvKernel : Kernel
    {
        public double [,] kernel { get; private set; }
        public (int, int) stride { get; private set; }
        public ConvKernel(int height,
                          int width,
                          double[,] kernel,
                          (int, int)? padding,
                          int? padVal,
                          (int, int)? stride) : base(height, width, padding, padVal)
        {
            this.kernel = kernel;
            this.stride = stride ?? (1,1);
        }

        public override Channel KernelOperation(Channel channel)
        {
            channel.PrintChannel();
            // Apply the padding
            Channel tempChannel = this.ApplyPadding(channel);
            tempChannel.PrintChannel();
            int outHeight = tempChannel.height - (this.height - 1) - (this.stride.Item1 - 1);
            int outWidth =  tempChannel.width - (this.width - 1) - (this.stride.Item2 - 1);

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
                    double ij = tempChannel.Slice(a, b, x, y).Multiply(this.kernel).Sum();
                    outChannelValues[i,j] = ij;
                }
            }
            
            Channel outChannel = new Channel(outHeight, outWidth, outChannelValues);
            outChannel.PrintChannel();
            return outChannel;
        }
    }
}