using System;
using SME;

namespace CNN
{
    // Works
    [ClockedProcess]
    public class DivideValue : SimpleProcess
    {
        [InputBus]
        public ValueBus InputValue;
        [InputBus]
        public ValueBus InputWeight;
        [OutputBus]
        public ValueBus Output = Scope.CreateBus<ValueBus>();
        private float value = 0;
        private float denominator = 1;
        private bool divideValid = false;

        protected override void OnTick()
        {
            Output.enable = false;
            Output.LastValue = false;
            Output.Value = 0;
            if (InputValue.enable)
            {
                value = InputValue.Value;
            }
            if (InputWeight.enable)
            {
                denominator = InputWeight.Value;
                divideValid = true;
            }
            // Output should only be updated when the input is valid.
            if (divideValid)
            {
                Output.Value = value / denominator;
                Output.enable = true;
                divideValid = false;
            }
            // If Input is last value in slice make plus ctrl know
            Output.LastValue = InputValue.LastValue;
        }
    }
}