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
        public ValueBus Output
        {
            get => bias.Output;
            set => bias.Output = value;
        }
        public Filter(int numInChannels, float[][] weights, float biasVal, (int,int) channelSize, (int,int) kernelSize, (int,int) stride)
        {
            this.numInChannels = numInChannels;
            inputChannels = new ChannelBus[numInChannels];
            kernelOutputs = new ValueBus[numInChannels];
            convKernels = new ConvKernel[numInChannels];
            valueArrayCtrl = new ValueArrayCtrl(numInChannels);
            plusCtrl = new PlusCtrl();
            bias = new Bias(biasVal);
            for (int i = 0; i < numInChannels; i++)
            {
                var weightsKernel = weights[i];
                ConvKernel convKernel = new ConvKernel(weightsKernel, channelSize, kernelSize, stride);
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
        private ChannelBus[] inputChannels;
        private ValueBus[] kernelOutputs;
        private ValueArrayCtrl valueArrayCtrl;
        private PlusCtrl plusCtrl;
        private Bias bias ;
    }
}