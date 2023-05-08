using System;
using SME;
using SME.Components;

namespace CNN
{
    [ClockedProcess]
    public class SoftmaxLayer
    {
        public ValueBus[] Inputs
        {
            get => inputChannels;
            set => inputChannels = value;
        }
        public ValueBus[] Outputs
        {
            get => divisionOutputs;
            set => divisionOutputs = value;
        }

        public SoftmaxLayer(int numInChannels, (int,int) channelSize)
        {
            this.numInChannels = numInChannels;
            var amount = numInChannels * channelSize.Item1 * channelSize.Item2;
            exponentials = new Exp[amount];
            divisions = new DivideValue[amount];
            inputChannels = new ValueBus[amount];
            exponentialOutputs = new ValueBus[amount];
            divisionOutputs = new ValueBus[amount];

            valueArrayCtrl = new ValueArrayCtrl(numInChannels, channelSize);
            plusCtrl = new PlusCtrl();
            for (int i = 0; i < amount; i++)
            {
                Exp exp = new Exp();
                exponentials[i] = exp;
                inputChannels[i] = exp.Input;       
                exponentialOutputs[i] = exp.Output;     
            }
            valueArrayCtrl.Input = exponentialOutputs;
            plusCtrl.Input = valueArrayCtrl.Output;

            for (int i = 0; i < amount; i++)
            {
                DivideValue divideValue = new DivideValue();
                divisions[i] = divideValue;
                divisions[i].InputValue = exponentialOutputs[i];
                divisions[i].InputWeight = plusCtrl.Output;
                divisionOutputs[i] = divideValue.Output;
            }

        }
        public void PushInputs()
        {
            for (int i = 0; i < numInChannels; i++)
            {
                exponentials[i].Input = inputChannels[i];
            }
        }
        private int numInChannels;
        private Exp[] exponentials;
        private DivideValue[] divisions;
        private ValueBus[] inputChannels;
        private ValueBus[] exponentialOutputs;
        private ValueBus[] divisionOutputs;
        private ValueArrayCtrl valueArrayCtrl;
        private PlusCtrl plusCtrl;
    }
}