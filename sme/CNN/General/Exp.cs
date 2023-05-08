using System;
using SME;

namespace CNN
{
    [ClockedProcess]
    public class Exp : SimpleProcess
    {
        [InputBus]
        public ValueBus Input;
        [OutputBus]
        public ValueBus Output = Scope.CreateBus<ValueBus>();
        protected override void OnTick()
        {
            Output.Value = 0;
            Output.enable = false;
            Output.LastValue = false;
            // Output should only be updated when the input is valid.
            if (Input.enable)
            {
                Output.Value = (float) Math.Exp(Input.Value);
                Output.enable = true;
            }
        }
    }
}