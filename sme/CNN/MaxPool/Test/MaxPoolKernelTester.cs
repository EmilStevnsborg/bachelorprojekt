using System;
using System.Threading.Tasks;
using SME;

namespace CNN
{
    [ClockedProcess]
    public class MaxPoolKernelTester : SimulationProcess
    {
        [InputBus]
        public ValueBus Input;
        [OutputBus]
        public ChannelBus Output = Scope.CreateBus<ChannelBus>();

        public override async Task Run()
        {
            await ClockAsync();
            Output.enable = true;
            
            // Pack test data onto bus
            Console.WriteLine("Test channel: ");
            for (int i = 0; i < 16; i++)
            {
                Output.ArrData[i] = (float) i;
                Console.Write(i + " ");
                if ((i + 1) % 4 == 0) {Console.WriteLine();}
            }
            Output.Height = 4;
            Output.Width = 4;
            Console.WriteLine();
            await ClockAsync();
            // Console.WriteLine(Output.ArrData[1]);
            // Data shouldn't be read again
            Output.enable = false;
            await ClockAsync();
            for (int i = 0; i < 9; i++)
            {
                while (!Input.enable) await ClockAsync();
                Console.Write(Input.Value + " ");
                if ((i + 1) % 3 == 0) {Console.WriteLine();}
                await ClockAsync();
            }
        }
    }
}