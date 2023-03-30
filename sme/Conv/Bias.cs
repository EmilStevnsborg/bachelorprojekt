using System;
using SME;

namespace CNN
{

    [ClockedProcess]
    public class Bias : SimpleProcess
    {
        [InputBus]
        public ValueBus Input;
        [OutputBus]
        public ValueBus Output = Scope.CreateBus<ValueBus>();
        private float bias;
        public Bias(float bias)
        {
            this.bias = bias;
        } 

        protected override void OnTick()
        {
            Output.Value = Input.Value;
            // Output should only be updated when the input is valid.
            if (Input.enable)
            {
                Output.Value = Input.Value + bias;
            }
            Output.enable = Input.enable;
            Output.LastValue = Input.LastValue;
        }
    }
}