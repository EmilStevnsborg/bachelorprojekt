using SME;

namespace CNN
{
    [ClockedProcess]
    public class BatchNormLayer
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
        public BatchNormLayer(int numInChannels, int numOutChannels, float[] means, float[] vars, float[] gammas, float[] betas)
        {
            this.numInChannels = numInChannels;
            this.numOutChannels = numOutChannels;
            nodes = new BatchNorm[numOutChannels];
            outputValues = new ValueBus[numOutChannels];
            for (int i = 0; i < numOutChannels; i++)
            {
                var mean = means[i];
                var variance = vars[i];
                var gamma = gammas[i];
                var beta = betas[i];
                BatchNorm node = new BatchNorm(gamma, beta, mean, variance);
                nodes[i] = node;
                outputValues[i] = node.Output;
            }
        }
        public void PushInputs()
        {
            for (int i = 0; i < numOutChannels; i++)
            {
                nodes[i].Input = inputChannels[i];
            }
        }
        private int numInChannels;
        private int numOutChannels;
        private BatchNorm[] nodes;
        private ValueBus[] inputChannels;
        private ValueBus[] outputValues;
    }
}
   