using SME;
using SME.Components;
using System;
using static CNN.ChannelSizes;
using static System.Math;

namespace CNN
{
    [ClockedProcess]
    public class MaxPoolLayer
    {
        public ChannelBus[] Inputs
        {
            get => inputChannels;
            set => inputChannels = value;
        }
        public ValueBus[] Outputs
        {
            get => kernelOutputs;
            set => kernelOutputs = value;
        }
        public MaxPoolLayer(int numInChannels, (int,int) channelSize, (int,int) kernelSize, (int,int) stride, (int,int) padding, float padVal)
        {
            this.numInChannels = numInChannels;
            inputChannels = new ChannelBus[numInChannels];
            kernelOutputs = new ValueBus[numInChannels];
            maxPoolKernels = new MaxPoolKernel[numInChannels];
            for (int i = 0; i < numInChannels; i++)
            {
                MaxPoolKernel maxPoolKernel = new MaxPoolKernel(channelSize, kernelSize, stride, padding, padVal);
                maxPoolKernels[i] = maxPoolKernel;
                inputChannels[i] = maxPoolKernel.Input;
                kernelOutputs[i] = maxPoolKernel.Output;
            }
        }
        public void PushInputs()
        {
            for (int i = 0; i < numInChannels; i++)
            {
                maxPoolKernels[i].Input = inputChannels[i];
            }
        }
        private int numInChannels;
        private MaxPoolKernel[] maxPoolKernels;
        private ChannelBus[] inputChannels;
        private ValueBus[] kernelOutputs;
    }
}