using System;
using SME;
using SME.Components;

namespace CNN
{
    [ClockedProcess]
    public class ConvKernel
    {
        public ValueBus InputA
        {
            get => kernelCtrl.InputValueA;
            set => kernelCtrl.InputValueA = value;
        }
        public ValueBus InputB
        {
            get => kernelCtrl.InputValueB;
            set => kernelCtrl.InputValueB = value;
        }

        public ValueBus Output
        {
            get => plusCtrl.Output;
            set => plusCtrl.Output = value;
        }

        public ConvKernel((int,int) kernelSize, float[] weights)
        {            
            // Instantiate the processes
            kernelCtrl = new KernelCtrl(kernelSize, weights);
            weightValueA = new WeightValue();
            weightValueB = new WeightValue();
            plusTwo = new PlusTwo();
            plusCtrl = new PlusCtrl();

            weightValueA.InputValue = kernelCtrl.OutputValueA;
            weightValueA.InputWeight = kernelCtrl.OutputWeightA;
            weightValueB.InputValue = kernelCtrl.OutputValueB;
            weightValueB.InputWeight = kernelCtrl.OutputWeightB;

            plusTwo.InputA = weightValueA.Output;
            plusTwo.InputB = weightValueB.Output;
            
            plusCtrl.Input = plusTwo.Output;
        }

        // Hold the internal processes as fields
        private KernelCtrl kernelCtrl;
        private WeightValue weightValueA;
        private WeightValue weightValueB;
        private PlusTwo plusTwo;
        private PlusCtrl plusCtrl;
    }
}