//ChannelBus1
//ChannelBus2
//ChannelBus3

//ConvKernel1 (process)
//ConvKernel2 (process)
//ConvKernel3 (process)

//OutputBus = ConvKernel1.Out + ConvKernel2.Out + ConvKernel3.Out + bias

//Upsample.Input = OutputBus
//
//
//Print(UpSample)

using SME;
using SME.Components;
using System;
using static CNN.ChannelSizes;
using static System.Math;

namespace CNN
{
    [ClockedProcess]
    public class Filter
    {
        public ChannelBus[] Inputs
        {
            get => inputChannels;
            set => inputChannels = value;
        }
        public ValueBus[] Output
        {
            get => kernelOutputs;
            set => kernelOutputs = value;
        }
        public Filter(int numInChannels, float[][] weights, float biasVal, (int,int) channelSize, (int,int) kernelSize, (int,int) stride)
        {
            this.numInChannels = numInChannels;
            inputChannels = new ChannelBus[numInChannels];
            kernelOutputs = new ValueBus[numInChannels];
            convKernels = new ConvKernel[numInChannels];
            for (int i = 0; i < numInChannels; i++)
            {
                var weightsKernel = weights[i];
                ConvKernel convKernel = new ConvKernel(weightsKernel, channelSize, kernelSize, stride);
                convKernels[i] = convKernel;
                inputChannels[i] = convKernel.Input;
                kernelOutputs[i] = convKernel.Output;
            }
        }
        public int NumInChannels
        {
            get => numInChannels;
        }
        public void PushInputs()
        {
            for (int i = 0; i < numInChannels; i++)
            {
                convKernels[i].Input = inputChannels[i];
            }
        }
        private int numInChannels;
        private ConvKernel[] convKernels;
        private ChannelBus[] inputChannels;
        private ValueBus[] kernelOutputs;
    }
}