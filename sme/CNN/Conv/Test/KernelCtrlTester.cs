using System;
using System.Threading.Tasks;
using SME;
using SME.Components;

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
        public ChannelBus Output = Scope.CreateBus<ChannelBus>();
        private int channelHeight, channelWidth;
        public KernelCtrlTester(int channelHeight, int channelWidth)
        {
            this.channelHeight = channelHeight;
            this.channelWidth = channelWidth;
        }

        public async override System.Threading.Tasks.Task Run()
        {

            await ClockAsync();
            Output.enable = true;
            // Pack test data onto bus
            Console.WriteLine("Test channel: ");
            for (int i = 0; i < channelHeight*channelWidth; i++)
            {
                Output.ArrData[i] = (float) i;
                Console.Write(i + " ");
                if ((i + 1) % 4 == 0) {Console.WriteLine();}
            }
            Console.WriteLine();
            Output.Height = channelHeight;
            Output.Width = channelWidth;
            await ClockAsync();
            // Ensure that the test data isn't read again.
            Output.enable = false;
            await ClockAsync();
            for (int x = 0; x < 4; x++)
            {
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        // Wait until there is something valid on the bus.
                        while (!InputValue.enable) await ClockAsync();
                        Console.Write(InputValue.Value + " * " + InputWeight.Value + ", ");
                        await ClockAsync();
                    }
                    Console.WriteLine();
                }
            }
            Console.WriteLine("Tester finished");
        }
    }
}