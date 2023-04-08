using SME;
using SME.Components;
using System;
using static CNN.ChannelSizes;
using static System.Math;

namespace CNN
{
    [ClockedProcess]
    public class ConvLayer
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
        public ConvLayer(int numInChannels, int numOutChannels, float[][][] weights, float[] biasVal, (int,int) channelSize, (int,int) kernelSize, (int,int) stride)
        {
            this.numInChannels = numInChannels;
            this.numOutChannels = numOutChannels;
            filters = new Filter[numOutChannels];
            outputValues = new ValueBus[numOutChannels];
            for (int i = 0; i < numOutChannels; i++)
            {
                var weightsFilter = weights[i];
                var biasFilter = biasVal[i];
                Filter filter = new Filter(numInChannels, weightsFilter, biasFilter, channelSize, kernelSize, stride);
                filters[i] = filter;
                outputValues[i] = filter.Output;
            }
        }
        public void PushInputs()
        {
            for (int i = 0; i < numInChannels; i++)
            {
                filters[i].Inputs = inputChannels;
                filters[i].PushInputs();
            }
        }
        private int numInChannels;
        private int numOutChannels;
        private Filter[] filters;
        private ChannelBus[] inputChannels;
        private ValueBus[] outputValues;
    }
}