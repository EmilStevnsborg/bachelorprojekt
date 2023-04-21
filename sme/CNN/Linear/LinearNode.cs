using SME;
using SME.Components;
using System;

namespace CNN
{
    [ClockedProcess]
    public class LinearNode
    {
        public ValueBus[] Inputs
        {
            get => nodeCtrl.Input;
            set => nodeCtrl.Input = value;
        }
        public ValueBus Output
        {
            get => bias.Output;
            set => bias.Output = value;
        }
        public LinearNode(int numInChannels, float[] weights,float biasVal, (int,int) channelSize)
        {
            this.numInChannels = numInChannels;
            this.channelHeight = channelSize.Item1;
            this.channelWidth = channelSize.Item2;

            // Instantiate the processes
            ram  = new TrueDualPortMemory<float>(numInChannels * channelHeight * channelWidth, weights);
            nodeCtrl = new NodeCtrl(numInChannels, channelSize);
            weightValue = new WeightValue();
            plusCtrl = new PlusCtrl();
            bias = new Bias(biasVal);

            // Connect the buses
            nodeCtrl.ram_ctrl = ram.ControlA;
            nodeCtrl.ram_read = ram.ReadResultA;

            weightValue.InputValue = nodeCtrl.OutputValue;
            weightValue.InputWeight = nodeCtrl.OutputWeight;

            plusCtrl.Input = weightValue.Output;

            bias.Input = plusCtrl.Output;
        }
        private int numInChannels;
        private int channelHeight;
        private int channelWidth;

        // Hold the internal processes as fields
        private TrueDualPortMemory<float> ram;
        private NodeCtrl nodeCtrl;
        private WeightValue weightValue;
        private PlusCtrl plusCtrl;
        private Bias bias;
    }
}