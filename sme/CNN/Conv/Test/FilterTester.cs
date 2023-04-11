using System;
using System.Threading.Tasks;
using SME;

namespace CNN
{
    [ClockedProcess]
    public class FilterTester : SimulationProcess
    {
        [InputBus]
        public ValueBus Input;
        [OutputBus]
        public ChannelBus[] Outputs;

        public FilterTester()
        {
            
            Outputs = new ChannelBus[2];
            Outputs[0] = Scope.CreateBus<ChannelBus>();
            Outputs[1] = Scope.CreateBus<ChannelBus>();
        }

        public override async Task Run()
        {
            await ClockAsync();
            for (int i = 0; i < 2; i++)
            {
                Outputs[i].enable = true;
                // Pack test data onto bus
                Console.WriteLine("Test channel: ");
                for (int j = 0; j < 16; j++)
                {
                    float val = (i + 1) * j % 3;
                    Outputs[i].ArrData[j] = val;
                    Console.Write(val + " ");
                    if ((j + 1) % 4 == 0) {Console.WriteLine();}
                }
                Outputs[i].Height = 4;
                Outputs[i].Width = 4;
            }
            Console.WriteLine();
            await ClockAsync();
            // Data shouldn't be read again
            for (int i = 0; i < 2; i++)
            {
                Outputs[i].enable = false;
            }
            var outputOne = new float[9];
            await ClockAsync();
            // Loading inputs
            for (int i = 0; i < 9; i++)
            {
                while (!(Input.enable)) await ClockAsync();
                outputOne[i] = Input.Value;
                await ClockAsync();
            }
            // print results
            for (int i  = 0; i < 9; i++)
            {
                Console.Write(outputOne[i] + " ");
                if ((i + 1) % 3 == 0) {Console.WriteLine();}
            }
            Console.WriteLine();
        }
    }
}