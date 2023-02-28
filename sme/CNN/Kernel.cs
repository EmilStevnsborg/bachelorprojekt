namespace CNN 
{
    public abstract class Kernel
    {
        public int height { get; private set; }
        public int width { get; private set; }

        public Kernel(int height, int width)
        {
            this.height = height;
            this.width = width;
        }

        public abstract double [,] KernelOperation(Channel channel);

    }
}