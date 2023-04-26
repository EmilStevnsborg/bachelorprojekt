using System;
using SME;

namespace CNN
{
    // Works
    [ClockedProcess]
    public class Max : SimpleProcess
    {
        [InputBus]
        public ValueBus Input;

        [OutputBus]
        public ValueBus Output = Scope.CreateBus<ValueBus>();

        private float max = -1000;

        protected override void OnTick()
        {
            // The flag can be forwarded.
            Output.enable = Input.LastValue;
            // Output should only be updated when the input is valid.
            if (Input.enable)
            {
                if (Input.Value > max)
                {
                    max = Input.Value;
                }
            }
            Output.Value = max;
            if (Input.LastValue)
            {
                max = -1000;
            }
        }
    }
}