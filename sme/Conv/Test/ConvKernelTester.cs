using System;
using System.Threading.Tasks;
using SME;

namespace CNN
{
    [ClockedProcess]
    public class ConvKernelTester : SimulationProcess
    {
        [InputBus]
        public ChannelBus Input;
        [OutputBus]
        public ChannelBus Output = Scope.CreateBus<ChannelBus>();

        public override async Task Run()
        {
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
            
            Console.WriteLine("Clock await after sending: 1");
            await ClockAsync();
            Console.WriteLine("Clock await after sending: 2");
            await ClockAsync();
            Console.WriteLine("Clock await after sending: 3");
            await ClockAsync();
            Console.WriteLine("Clock await after sending: 4");
            await ClockAsync();
            Console.WriteLine("Clock await after sending: 5");
            await ClockAsync();
            Console.WriteLine("Clock await after sending: 6");
            await ClockAsync();
            Console.WriteLine("Clock await after sending: 7");
            if (Input.enable) 
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
            }
        }
    }
}