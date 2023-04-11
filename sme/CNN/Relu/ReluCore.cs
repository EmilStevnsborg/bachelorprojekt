using System;
using SME;

namespace CNN
{
    // Works
    [ClockedProcess]
    public class ReluCore : SimpleProcess
    {
        [InputBus]
        public ValueBus Input;

        [OutputBus]
        public ValueBus Output = Scope.CreateBus<ValueBus>();

        protected override void OnTick()
        {
            // The flag can be forwarded.
            Output.enable = Input.enable;
            Output.LastValue = false;
            // Output should only be updated when the input is valid.
            if (Input.enable)
            {
                if (Input.Value > 0)
                {
                    Output.Value = Input.Value;
                }
                else
                {
                    Output.Value = 0;
                }
            }
            // else deafult value is 0
            else
            {
                Output.Value = 0;
            }
        }
    }
}