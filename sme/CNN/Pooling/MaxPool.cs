using SME;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CNN 
{
    public class MaxPool : SimpleProcess
    {
        public int numInChannels { get; private set; }
        public MaxPoolKernel maxPoolKernel { get; private set; }

        public MaxPool(MaxPoolKernel maxPoolKernel)
        {
            this.maxPoolKernel = maxPoolKernel;
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

                for (int i = 0; i < this.numInChannels; i++)
                {
                    Channel channel = channels.channels[i];
                    Channel outChannel = this.maxPoolKernel.KernelOperation(channel);
                    outChannels.Add(outChannel);
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