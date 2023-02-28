namespace CNN
{
    public class PoolKernel : Kernel
    {
        public (int, int) stride { get; private set; }
        public PoolKernel(int height, int width, (int, int)? stride) : base(height, width)
        {
            this.stride = stride ?? (height, width);
        }

        public override double [,] KernelOperation(Channel channel)
        {
            int outHeight = (int) ((channel.height - (this.height - 1) -1) / this.stride.Item1 + 1);
            int outWidth =  (int) ((channel.width - (this.width - 1) -1) / this.stride.Item2 + 1);

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
                    double ij = channel.Slice(a, b, x, y).Amax();
                    outChannel[i,j] = ij;
                }
            }
                
            return outChannel;
        }
    }
}