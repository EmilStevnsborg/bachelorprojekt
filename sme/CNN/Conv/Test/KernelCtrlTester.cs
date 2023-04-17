using System;
using System.Threading.Tasks;
using SME;

namespace CNN
{
    [ClockedProcess]
    public class KernelCtrlTester : SimulationProcess
    {
        [InputBus]
        public ValueBus InputValue;
        [InputBus]
        public ValueBus InputWeight;
        [OutputBus]
        public ValueBus Output = Scope.CreateBus<ValueBus>();
        public override async Task Run()
        {
            await ClockAsync();
            Output.enable = true;            
            // Pack test data onto bus
            Console.WriteLine("Test channel: ");
            for (int i = 0; i < 16; i++)
            {
                float val = i;
                Output.Value = val;
                Console.Write(val + " ");
                if ((i + 1) % 4 == 0) {Console.WriteLine();}
                await ClockAsync();
            }
            Console.WriteLine();
            // Data shouldn't be read again
            Output.enable = false;
            await ClockAsync();
            for (int i = 0; i < 16; i++)
            {
                while (!InputValue.enable) await ClockAsync();
                Console.WriteLine(InputValue.Value + " " + InputWeight.Value + " ");
                await ClockAsync();
            }
        }
    }
}