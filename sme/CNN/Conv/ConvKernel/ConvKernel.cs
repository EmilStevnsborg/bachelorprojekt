using System;
using SME;
using SME.Components;

namespace CNN
{
    [ClockedProcess]
    public class ConvKernel
    {
        public ValueBus Input
        {
            get => kernelCtrl.Input;
            set => kernelCtrl.Input = value;
        }

        public ValueBus Output
        {
            get => plusCtrl.Output;
            set => plusCtrl.Output = value;
        }

        public ConvKernel(float[] weights, (int,int) channelSize, (int,int) kernelSize, (int,int) stride, (int,int) padding, float padVal)
        {
            // channel input
            var ch = channelSize.Item1;
            var cw = channelSize.Item2;

            // padding size
            var ph = padding.Item1;
            var pw = padding.Item2;

            float[] buffer = new float[(ch + 2 * ph) * (cw + 2 * pw)];
            // fill in padding
            Helper.Padding(ref buffer, ch, cw, ph, pw, padVal);
            
            // Instantiate the processes
            ram  = new TrueDualPortMemory<float>((ch + 2 * ph) * (cw + 2 * pw), buffer);
            kernelCtrl = new ConvKernelCtrl(channelSize, kernelSize, stride, padding, weights);
            weightValueA = new WeightValue();
            weightValueB = new WeightValue();
            plusTwo = new PlusTwo();
            plusCtrl = new PlusCtrl();

            // Connect the buses
            kernelCtrl.ram_ctrlA = ram.ControlA;
            kernelCtrl.ram_readA = ram.ReadResultA;
            kernelCtrl.ram_ctrlB = ram.ControlB;
            kernelCtrl.ram_readB = ram.ReadResultB;

            weightValueA.InputValue = kernelCtrl.OutputValueA;
            weightValueA.InputWeight = kernelCtrl.OutputWeightA;
            weightValueB.InputValue = kernelCtrl.OutputValueB;
            weightValueB.InputWeight = kernelCtrl.OutputWeightB;

            plusTwo.InputA = weightValueA.Output;
            plusTwo.InputB = weightValueB.Output;
            
            plusCtrl.Input = plusTwo.Output;
        }

        // Hold the internal processes as fields
        private TrueDualPortMemory<float> ram;
        private ConvKernelCtrl kernelCtrl;
        private WeightValue weightValueA;
        private WeightValue weightValueB;
        private PlusTwo plusTwo;
        private PlusCtrl plusCtrl;
    }
}