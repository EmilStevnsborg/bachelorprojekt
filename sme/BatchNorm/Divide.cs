using System;
using SME;
using static System.Math;

namespace CNN
{
    // Works
    [ClockedProcess]
    public class Divide : SimpleProcess
    {
        [InputBus]
        public ValueBus Numerator;

        [OutputBus]
        public ValueBus Output = Scope.CreateBus<ValueBus>();
        private float denominator;
        public Divide(float denominator)
        {
            this.denominator = denominator;
        }

        protected override void OnTick()
        {
            // The flag can be forwarded.
            Output.enable = Numerator.enable;
            Output.LastValue = false;
            // Output should only be updated when the input is valid.
            if (Numerator.enable)
            {
                Output.Value = Numerator.Value / denominator;
            }
            // else deafult value is 0
            else
            {
                Output.Value = 0;
            }
        }
    }
}