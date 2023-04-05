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
        public ValueBus[] Outputs
        {
            get => outputValues;
            set => outputValues = value;
        }
        public int NumInChannels
        {
            get => numInChannels;
        }
        public Filter(int numInChannels, float[][] weights, (int,int) channelSize, (int,int) kernelSize, (int,int) stride)
        {
            this.numInChannels = numInChannels;
            inputChannels = new ChannelBus[numInChannels];
            outputValues = new ValueBus[numInChannels];
            for (int i = 0; i < numInChannels; i++)
            {
                var weightsKernel = weights[i];
                ConvKernel convKernel = new ConvKernel(weightsKernel, channelSize, kernelSize, stride);
                inputChannels[i] = convKernel.Input;
                outputValues[i] = convKernel.Output;
            }
        }
        private int numInChannels;
        private ChannelBus[] inputChannels;
        private ValueBus[] outputValues;
    }
}