using System;
using SME;
using SME.Components;

namespace CNN
{
    [ClockedProcess]
    public class KernelCtrl : SimpleProcess
    {
        [InputBus]
        public ValueBus InputValueA;
        [InputBus]
        public ValueBus InputValueB;
        [OutputBus]
        public ValueBus OutputValueA = Scope.CreateBus<ValueBus>();
        [OutputBus]
        public ValueBus OutputWeightA = Scope.CreateBus<ValueBus>();
        [OutputBus]
        public ValueBus OutputValueB = Scope.CreateBus<ValueBus>();
        [OutputBus]
        public ValueBus OutputWeightB = Scope.CreateBus<ValueBus>();
        private int kernelA = 0, kernelB = 1;
        private int kernelHeight, kernelWidth;
        private float [] weights;
        private bool lastKernelVal = false;

        public KernelCtrl((int,int) kernelSize, float[] weights)
        {
            this.kernelHeight = kernelSize.Item1;
            this.kernelWidth = kernelSize.Item2;

            this.weights = weights;
        }
        protected override void OnTick()
        {
            OutputValueA.enable = OutputWeightA.enable = OutputValueA.LastValue = false;
            OutputValueB.enable = OutputWeightB.enable = OutputValueB.LastValue = false;
            if (InputValueA.enable && InputValueB.enable)
            {
                OutputValueA.Value = InputValueA.Value;
                OutputWeightA.Value = weights[kernelA];

                OutputValueB.Value = InputValueB.Value;
                OutputWeightB.Value = weights[kernelB];

                //Is this the last kernel value
                lastKernelVal = (kernelA + 1 == kernelHeight*kernelWidth) || (kernelB + 1 == kernelHeight*kernelWidth);
                OutputValueA.LastValue = lastKernelVal;

                OutputValueA.enable = OutputWeightA.enable = true;
                OutputValueB.enable = OutputWeightB.enable = true;
                
                // weight adress
                kernelA = (kernelA + 2) % (kernelHeight * kernelWidth);
                if (kernelB == 0)
                {
                    kernelA = 0;
                }
                kernelB = (kernelA + 1) % (kernelHeight * kernelWidth);
            }
            else
            {
                lastKernelVal = false;
                kernelA = 0;
                kernelB = 1;
            }
        }
    }
}