using System;
using SME;

namespace CNN
{
    // Works
    [ClockedProcess]
    public class Minus : SimpleProcess
    {
        [InputBus]
        public ValueBus Input;

        [OutputBus]
        public ValueBus Output = Scope.CreateBus<ValueBus>();
        private float val = 0;
        public Minus(float val) 
        {
            this.val = val;
        }

        protected override void OnTick()
        {
            // The flag can be forwarded.
            Output.enable = Input.enable;
            Output.LastValue = false;
            // Output should only be updated when the input is valid.
            if (Input.enable)
            {
                Output.Value = Input.Value - val;
            }
            // else deafult value is 0
            else
            {
                Output.Value = 0;
            }
        }
    }
}