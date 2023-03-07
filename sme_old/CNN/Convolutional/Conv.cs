using SME;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CNN 
{
    public class Conv // : SimpleProcess
    {

        // filter x channels x height x width
        public List<Filter> filters { get; private set; }
        // bias for each filter
        public double[] biases { get; private set; }
        public int numInChannels { get; private set; }
        public (int, int) padding { get; private set; }
        public (int, int) stride { get; private set; }
        public int padVal { get; private set; }

        public Conv(List<Filter> filters , double[] biases, int numInChannels, (int, int)? stride, (int, int)? padding, int? padVal)
        {
            this.filters = filters;
            this.biases = biases;
            this.numInChannels = numInChannels;
            this.stride = stride ?? (1,1);
            this.padding = padding ?? (0,0);
            this.padVal = padVal ?? 0;

        }

        public List<Channels> Call(List<Channels> inBatch)
        {
            List<Channels> outBacth = new List<Channels>();

            // go through batch, each is a Channels object
            for (int cs = 0; cs < inBatch.Count; cs++)
            {
                Channels channels = inBatch[cs];
                Channels outChannels = new Channels();
                int b = 0;

                // Go through filters
                foreach (Filter filter in this.filters)
                {
                    // initialize outChannel with first KernelOperation
                    ConvKernel kernelInit = (ConvKernel) filter.filter[0];
                    Channel channelInit = channels.channels[0];
                    Channel outChannel = this.KernelOperation(channelInit, kernelInit);

                    for (int i = 1; i < this.numInChannels; i++)
                    {
                        ConvKernel kernel = (ConvKernel) filter.filter[i];
                        Channel channel = channels.channels[i];
                        Channel channelKernel = this.KernelOperation(channel, kernel);
                        outChannel = outChannel.SumPairwise(channelKernel);
                    }

                    outChannels.channels.Add(outChannel.AddBias(biases[b]));
                    b += 1;
                }
                outBacth.Add(outChannels);
            }

            return outBacth;
        }
        public Channel KernelOperation(Channel channel, ConvKernel kernel)
        {
            channel.channel.PrintArray();
            // Apply the padding
            Channel tempChannel = channel.ApplyPadding(this.padding, this.padVal);
            tempChannel.channel.PrintArray();
            int outHeight = tempChannel.height - (kernel.height - 1) - (this.stride.Item1 - 1);
            int outWidth =  tempChannel.width - (kernel.width - 1) - (this.stride.Item2 - 1);

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
                    double ij = tempChannel.Slice(a, b, x, y).Multiply(kernel.kernel).Sum();
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
        //     List<Channels> resultBatch = this.Call();
        //     Output.ResultBatch = resultBatch;
        // }
    }
}