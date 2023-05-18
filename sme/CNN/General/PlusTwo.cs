using System;
using SME;

namespace CNN
{

    [ClockedProcess]
    public class PlusTwo : SimpleProcess
    {
        [InputBus]
        public ValueBus InputA;
        [InputBus]
        public ValueBus InputB;
        [OutputBus]
        public ValueBus Output = Scope.CreateBus<ValueBus>();

        protected override void OnTick()
        {
            // Output should only be updated when the input is valid.
            if (InputA.enable && InputB.enable)
            {
                Output.Value = InputA.Value + InputB.Value;
            }
            Output.enable = InputA.enable;
            Output.LastValue = InputA.LastValue;
        }
    }
}