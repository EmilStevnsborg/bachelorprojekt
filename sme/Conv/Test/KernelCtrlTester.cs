using System;
using System.Threading.Tasks;
using SME;
using static CNN.ChannelSizes;
using SME.Components;

namespace CNN
{
    // Works
    public class KernelCtrlTester : SimulationProcess
    {

        [InputBus]
        public ValueBus InputValue;
        [InputBus]
        public ValueBus InputWeight;

        [OutputBus]        
        public SliceBus Output = Scope.CreateBus<SliceBus>();
        private int sliceHeight, sliceWidth;
        public KernelCtrlTester(int sliceHeight, int sliceWidth)
        {
            this.sliceHeight = sliceHeight;
            this.sliceWidth = sliceWidth;
        }

        public async override System.Threading.Tasks.Task Run()
        {

            await ClockAsync();
            Output.enable = true;
            // Pack test data onto bus
            for (int i = 0; i < sliceHeight*sliceWidth; i++)
            {
                Output.ArrData[i] = (float) i;
            }
            Output.Height = sliceHeight;
            Output.Width = sliceWidth;
            await ClockAsync();

            // Ensure that the test data isn't read again.
            Output.enable = false;
            await ClockAsync();

            for (int i = 0; i < sliceHeight; i++)
            {
                for (int j = 0; j < sliceWidth; j++)
                {
                    // Wait until there is something valid on the bus.
                    while (!InputValue.enable) await ClockAsync();
                    Console.Write(InputValue.Value + " * " + InputWeight.Value + ", ");
                    await ClockAsync();
                }
                Console.WriteLine();
            }

            Console.WriteLine("Tester finished");
        }
    }
}