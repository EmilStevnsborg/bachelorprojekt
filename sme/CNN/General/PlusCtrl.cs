using System;
using SME;

namespace CNN
{

    [ClockedProcess]
    public class PlusCtrl : SimpleProcess
    {
        [InputBus]
        public ValueBus Input;
        [OutputBus]
        public ValueBus Output = Scope.CreateBus<ValueBus>();

        private float buffer = 0;

        protected override void OnTick()
        {
            // Output should only be updated when the input is valid.
            if (Input.enable)
            {
                buffer += Input.Value;
            }
            Output.Value = buffer;
            Output.enable = Output.LastValue = Input.LastValue;
            if (Input.LastValue)
            {
                // Console.WriteLine("Result of plus: " + buffer);
                buffer = 0;
            }
        }
    }
}