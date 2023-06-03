using System;
using SME;
using SME.Components;

namespace CNN
{
    [ClockedProcess]
    public class ConvLayer
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
        public ConvLayer(int numInChannels, int numOutChannels, float[][][] weights, float[] biasVal, (int,int) channelSize, (int,int) kernelSize, (int,int) stride, (int,int) padding, float padVal)
        {
            var ch = channelSize.Item1;
            var cw = channelSize.Item2;

            // padding size
            var ph = padding.Item1;
            var pw = padding.Item2;

            this.numInChannels = numInChannels;
            this.numOutChannels = numOutChannels;

            inputCtrls = new InputCtrl[numInChannels];
            rams = new TrueDualPortMemory<float>[numInChannels];
            filters = new Filter[numOutChannels];
            outputValues = new ValueBus[numOutChannels];
            for (int i = 0; i < numInChannels; i++)
            {
                float[] buffer = new float[(ch + 2 * ph) * (cw + 2 * pw)];
                // fill in padding
                Helper.Padding(ref buffer, ch, cw, ph, pw, padVal);

                TrueDualPortMemory<float> ram = new TrueDualPortMemory<float>((ch + 2 * ph) * (cw + 2 * pw), buffer);
                InputCtrl inputCtrl = new InputCtrl(channelSize, kernelSize, stride, padding);

                inputCtrls[i] = inputCtrl;
                rams[i] = ram;

                inputCtrls[i].ram_ctrlA = rams[i].ControlA;
                inputCtrls[i].ram_readA = rams[i].ReadResultA;
                inputCtrls[i].ram_ctrlB = rams[i].ControlB;
                inputCtrls[i].ram_readB = rams[i].ReadResultB;

            }
            for (int i = 0; i < numOutChannels; i++)
            {
                var weightsFilter = weights[i];
                var biasFilter = biasVal[i];
                Filter filter = new Filter(numInChannels, weightsFilter, biasFilter, channelSize, kernelSize, stride, padding, padVal);
                filters[i] = filter;

                for (int j = 0; j < numInChannels; j++)
                {
                    filters[i].convKernels[j].InputA = inputCtrls[j].OutputValueA;
                    filters[i].convKernels[j].InputB = inputCtrls[j].OutputValueB;
                }

                outputValues[i] = filters[i].Output;
            }
        }
        public void PushInputs()
        {
            for (int i = 0; i < numInChannels; i++)
            {
                inputCtrls[i].Input = inputChannels[i];
            }
        }
        private int numInChannels;
        private int numOutChannels;
        private InputCtrl[] inputCtrls;
        private TrueDualPortMemory<float>[] rams;
        private Filter[] filters;
        private ValueBus[] inputChannels;
        private ValueBus[] outputValues;
    }
}