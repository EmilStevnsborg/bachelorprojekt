using SME;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CNN 
{
    public class MaxPool // : SimpleProcess
    {
        public int numInChannels { get; private set; }
        public (int, int) stride { get; private set; }
        public (int, int) padding { get; private set; }
        public int padVal { get; private set; }
        public MaxPoolKernel maxPoolKernel { get; private set; }

        public MaxPool(int height, int width, (int, int)? stride, (int, int)? padding, int? padVal)
        {
            this.maxPoolKernel = new MaxPoolKernel(height, width);
            this.stride = stride ?? (height, width);
            this.padding = padding ?? (0,0);
            this.padVal = padVal ?? 0;
        }

        public List<Channels> Call(List<Channels> inBatch)
        {
            List<Channels> outBacth = new List<Channels>();
            foreach (Channels channels in inBatch)
            {
                List<Channel> outChannels = new List<Channel>();

                for (int i = 0; i < this.numInChannels; i++)
                {
                    Channel channel = channels.channels[i];
                    Channel outChannel = this.KernelOperation(channel, this.maxPoolKernel);
                    outChannels.Add(outChannel);
                }                
            }
            return outBacth;
        }

        public Channel KernelOperation(Channel channel, MaxPoolKernel kernel)
        {
            channel.channel.PrintArray();
            // Pad the channel
            Channel tempChannel = channel.ApplyPadding(this.padding, this.padVal);
            tempChannel.channel.PrintArray();
            int outHeight = tempChannel.height - (channel.height - 1) - (this.stride.Item1 - 1);
            int outWidth =  tempChannel.width - (kernel.width - 1) - (this.stride.Item2 - 1);

            // initialize the values
            double[,] outChannelValues = new double[outHeight, outWidth];

            for (int i = 0; i < outHeight; i++)
            {
                for (int j = 0; j < outWidth; j++)
                {
                    int a = i * stride.Item1;
                    int b = j * stride.Item2;
                    int x = a + kernel.height;
                    int y = b + kernel.width;

                    // Look at Extensions for documentation
                    double ij = channel.Slice(a, b, x, y).Amax();
                    outChannelValues[i,j] = ij;
                }
            }
            
            Channel outChannel = new Channel(outHeight, outWidth, outChannelValues);
            outChannel.channel.PrintArray();
            return outChannel;
        }
        
        // /// <summary>
        // /// Called on each clock tick.
        // /// </summary>
        // protected override void OnTick()
        // {
        //     //
        // }
    }
}