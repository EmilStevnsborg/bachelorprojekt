using System;
using System.Threading.Tasks;
using SME;

namespace CNN
{
    [ClockedProcess]
    public class ConvKernelTester : SimulationProcess
    {
        [InputBus]
        public ValueBus Input;
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
            for (int i = 0; i < 4; i++)
            {
                while (!Input.enable) await ClockAsync();
                Console.Write(Input.Value + " ");
                if (i + 1 % 2 == 0) {Console.WriteLine();}
                await ClockAsync();
            }
        }
    }
}