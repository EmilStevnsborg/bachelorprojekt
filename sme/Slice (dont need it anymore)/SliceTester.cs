using System;
using System.Threading.Tasks;
using SME;

namespace CNN
{
    [ClockedProcess]
    public class SliceTester : SimulationProcess
    {
        [InputBus]
        public SliceBus Input;
        [OutputBus]
        public ChannelBus Output = Scope.CreateBus<ChannelBus>();
        [OutputBus]

        public override async Task Run()
        {
            var count = 0;
            await ClockAsync();
            Output.enable = true;
            
            // Pack test data onto bus
            for (int i = 0; i < 16; i++)
            {
                Output.ArrData[i] = (float) i;
            }
            Output.Height = 4;
            Output.Width = 4;
            await ClockAsync();
            // Data shouldn't be read again
            Output.enable = false;
            await ClockAsync();
            while (Input.enable && count < 4)
            {
                var ih = Input.Height;
                var iw = Input.Width;
                for (int i = 0; i < ih; i++)
                {
                    for (int j = 0; j < iw; j++)
                    {
                        Console.Write(Input.ArrData[i*iw + j]);
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
                count = count + 1;
                await ClockAsync();
            }
            await ClockAsync();
        }
    }
}