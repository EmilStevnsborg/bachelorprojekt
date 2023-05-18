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

            // kernel (slice height and width)
            var kh = kernelSize.Item1;
            var kw = kernelSize.Item2;

            // stride
            var sr = stride.Item1;
            var sc = stride.Item2;

            // upsample channel output
            var uh = ch - (kh-1) - (sr-1);
            var uw = cw - (kw-1) - (sc-1);

            // Instantiate the processes
            ram  = new TrueDualPortMemory<float>(kh*kw, weights);
            kernelCtrl = new ConvKernelCtrl(channelSize, kernelSize, stride, padding, padVal);
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