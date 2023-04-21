using System;
using SME;

namespace CNN
{
    [ClockedProcess]
    public class LinearLayer
    {
        public ValueBus[] Inputs
        {
            get => inputChannels;
            set => inputChannels = value;
        }
        public ValueBus[] Outputs
        {
            get => outputValues;
            set => outputValues = value;
        }
        public LinearLayer(int numInChannels, int numOutChannels, float[][] weights, float[] biases, (int,int) channelSize)
        {
            this.numInChannels = numInChannels;
            this.numOutChannels = numOutChannels;
            nodes = new LinearNode[numOutChannels];
            outputValues = new ValueBus[numOutChannels];
            for (int i = 0; i < numOutChannels; i++)
            {
                var weightsNode = weights[i];
                var bias = biases[i];
                LinearNode node = new LinearNode(numInChannels, weightsNode, bias, channelSize);
                nodes[i] = node;
                outputValues[i] = node.Output;
            }
        }
        public void PushInputs()
        {
            for (int i = 0; i < numOutChannels; i++)
            {
                nodes[i].Inputs = inputChannels;
            }
        }
        private int numInChannels;
        private int numOutChannels;
        private LinearNode[] nodes;
        private ValueBus[] inputChannels;
        private ValueBus[] outputValues;
    }
}