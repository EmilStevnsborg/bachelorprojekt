namespace CNN
{
    public class ConvKernel : Kernel
    {
        public double [,] kernel { get; private set; }
        public (int, int) stride { get; private set; }
        public ConvKernel(int height, int width, double [,] kernel, (int, int)? stride) : base(height, width)
        {
            this.kernel = kernel;
            this.stride = stride ?? (1,1);
        }

        public override double [,] KernelOperation(Channel channel)
        {
            int outHeight = channel.height - (this.height - 1) - (this.stride.Item1 - 1);
            int outWidth =  channel.width - (this.width - 1) - (this.stride.Item2 - 1);

            double[,] outChannel = new double[outHeight, outWidth];

            for (int i = 0; i < outHeight; i++)
            {
                for (int j = 0; j < outWidth; j++)
                {
                    int a = i * stride.Item1;
                    int b = j * stride.Item2;
                    int x = a + this.height;
                    int y = b + this.width;

                    // Look at Extensions for documentation
                    double ij = channel.Slice(a, b, x, y).Multiply(this.kernel).Sum();
                    outChannel[i,j] = ij;
                }
            }
                
            return outChannel;
        }
    }
}