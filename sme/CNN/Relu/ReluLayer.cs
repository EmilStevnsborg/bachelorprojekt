using SME;

namespace CNN
{
    [ClockedProcess]
    public class ReluLayer
    {
        public ValueBus[] Inputs
        {
            get => inputVals;
            set => inputVals = value;
        }
        public ValueBus[] Outputs
        {
            get => reluOutputs;
            set => reluOutputs = value;
        }
        public ReluLayer(int numInChannels)
        {
            this.numInChannels = numInChannels;
            reluCores = new ReluCore[numInChannels];
            reluOutputs = new ValueBus[numInChannels];
            for (int i = 0; i < numInChannels; i++)
            {
                ReluCore reluCore = new ReluCore();
                reluCores[i] = reluCore;
                reluOutputs[i] = reluCore.Output;
            }
        }
        public void PushInputs()
        {
            for (int i = 0; i < numInChannels; i++)
            {
                reluCores[i].Input = inputVals[i];
            }
        }
        private int numInChannels;
        private ReluCore[] reluCores;
        private ValueBus[] inputVals;
        private ValueBus[] reluOutputs;
    }
}