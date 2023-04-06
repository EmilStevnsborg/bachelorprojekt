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

        float buffer = 0;

        protected override void OnTick()
        {
            // Output should only be updated when the input is valid.
            if (Input.enable)
            {
                buffer += Input.Value;
            }
            Output.Value = buffer;
            Output.enable = Input.LastValue;
            Output.LastValue = false;
            // Console.WriteLine("add " + Input.Value);
            if (Input.LastValue)
            {
                Console.WriteLine("Result of plus and multiply: " + buffer);
                buffer = 0;
            }
        }
    }
}