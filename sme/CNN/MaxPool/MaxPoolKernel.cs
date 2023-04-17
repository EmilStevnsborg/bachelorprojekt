using SME;
using SME.Components;
using static CNN.ChannelSizes;
using static System.Math;

namespace CNN
{
    [ClockedProcess]
    public class MaxPoolKernel
    {
        public ValueBus Input
        {
            get => kernelCtrl.Input;
            set => kernelCtrl.Input = value;
        }

        public ValueBus Output
        {
            get => max.Output;
            set => max.Output = value;
        }

        public MaxPoolKernel((int,int) channelSize, (int,int) kernelSize, (int,int) stride, (int,int) padding, float padVal)
        {
            kernelCtrl = new PoolKernelCtrl(channelSize, kernelSize, stride, padding, padVal);
            max = new Max();

            // Connect the buses
            max.Input = kernelCtrl.OutputValue;
        }

        private PoolKernelCtrl kernelCtrl;
        private Max max;

    }
}