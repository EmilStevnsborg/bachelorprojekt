using SME;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CNN 
{
    public class Conv : SimpleProcess
    {

        // filter x channels x height x width
        public List<Filter> filters { get; private set; }
        // bias for each filter
        public double[] biases { get; private set; }
        public int numInChannels { get; private set; }
        public (int, int) padding { get; private set; }
        public (int, int) stride { get; private set; }

        public Conv(List<Filter> filters , double[] biases, int numInChannels, (int, int)? padding, (int, int)? stride)
        {
            this.filters = filters;
            this.biases = biases;
            this.numInChannels = numInChannels;
            this.padding = padding ?? (0,0);
            this.stride = stride ?? (1,1);
        }

        // Can't handle null
        public interface IInputBatch : IBus
        {
            // batch x channels x height x width
            List<Channels> InputBatch { get; set; }
        }

        // Can't handle null
        public interface IResultBatch : IBus
        {
            // batch x channels x height x width
            public List<Channels> ResultBatch { get; set; }
        }

        [InputBus]
        private readonly IInputBatch Input = Scope.CreateBus<IInputBatch>();
        [OutputBus]
        private readonly IResultBatch Output = Scope.CreateBus<IResultBatch>();

        private void Call()
        {
            foreach (Channels channels in Input.InputBatch)
            {
                List<Channel> outChannels = new List<Channel>();

                int b = 0;
                foreach (Filter filter in this.filters)
                {
                    // init outChannel to first kernel - channel
                    ConvKernel kernelInit = (ConvKernel) filter.filter[0];
                    Channel channelInit = channels.channels[0];

                    Channel outChannel = kernelInit.KernelOperation(channelInit);

                    for (int i = 1; i < this.numInChannels; i++)
                    {
                        ConvKernel kernel = (ConvKernel) filter.filter[i];
                        Channel channel = channels.channels[i];
                        Channel kernelOut = kernel.KernelOperation(channel);
                        outChannel = outChannel.SumPairwise(kernelOut);
                    }

                    outChannels.Add(outChannel.AddBias(biases[b]));
                    b += 1;
                }
            }
        }
        
        /// <summary>
        /// Called on each clock tick.
        /// </summary>
        protected override void OnTick()
        {
            //
        }
         /// <summary>
        /// Run this instance, calling OnTick each clocktick.
        /// </summary>
        public override async Task Run()
        {
            while (true)
            {
                await ClockAsync();
                OnTick();
            }
        }
    }
}