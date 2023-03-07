namespace CNN
{
    public class ConvKernel : Kernel
    {
        public double [,] kernel { get; private set; }
        public ConvKernel(int height, int width, double[,] kernel) : base(height, width)
        {
            this.kernel = kernel;
        }
    }
}