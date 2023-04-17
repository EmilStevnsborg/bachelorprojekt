using SME;

namespace CNN
{
    [ClockedProcess]
    public class Filter
    {
        public ValueBus[] Inputs
        {
            get => inputChannels;
            set => inputChannels = value;
        }
        public ValueBus Output
        {
            get => bias.Output;
            set => bias.Output = value;
        }
        public Filter(int numInChannels, float[][] weights, float biasVal, (int,int) channelSize, (int,int) kernelSize, (int,int) stride, (int,int) padding, float padVal)
        {
            this.numInChannels = numInChannels;
            inputChannels = new ValueBus[numInChannels];
            kernelOutputs = new ValueBus[numInChannels];
            convKernels = new ConvKernel[numInChannels];
            valueArrayCtrl = new ValueArrayCtrl(numInChannels);
            plusCtrl = new PlusCtrl();
            bias = new Bias(biasVal);
            for (int i = 0; i < numInChannels; i++)
            {
                var weightsKernel = weights[i];
                ConvKernel convKernel = new ConvKernel(weightsKernel, channelSize, kernelSize, stride, padding, padVal);
                convKernels[i] = convKernel;
                inputChannels[i] = convKernel.Input;
                kernelOutputs[i] = convKernel.Output;
            }
            // connect busses
            valueArrayCtrl.Input = kernelOutputs;
            plusCtrl.Input = valueArrayCtrl.Output;
            bias.Input = plusCtrl.Output;
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
        private ValueBus[] inputChannels;
        private ValueBus[] kernelOutputs;
        private ValueArrayCtrl valueArrayCtrl;
        private PlusCtrl plusCtrl;
        private Bias bias ;
    }
}