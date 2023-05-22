using System;
using SME;

namespace CNN
{
    // Works
    [ClockedProcess]
    public class Max : SimpleProcess
    {
        [InputBus]
        public ValueBus InputA;
        [InputBus]
        public ValueBus InputB;

        [OutputBus]
        public ValueBus Output = Scope.CreateBus<ValueBus>();

        private float max = -1000;

        protected override void OnTick()
        {
            // Console.WriteLine(InputA.Value + "," + InputB.Value);
            // Output should only be updated when the input is valid.
            if (InputA.enable && InputB.enable)
            {
                if (InputA.Value > max || InputB.Value > max)
                {
                    if (InputB.Value > InputA.Value)
                    {
                        // Console.WriteLine(InputB.Value);
                        max = InputB.Value;
                    }
                    else
                    {
                        // Console.WriteLine(InputA.Value);
                        max = InputA.Value;
                    }
                }
            }
            Output.Value = max;
            Output.enable = InputA.LastValue;
            if (InputA.LastValue)
            {
                // Console.WriteLine("her: " + InputB.Value);
                max = -1000;
            }
        }
    }
}