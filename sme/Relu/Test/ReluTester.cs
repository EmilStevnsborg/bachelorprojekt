using System;
using System.Threading.Tasks;
using SME;

namespace CNN
{
    public class ReluTester : SimulationProcess
    {

        [InputBus]
        public ValueBus Input;

        [OutputBus]        
        public ChannelBus Output = Scope.CreateBus<ChannelBus>();
        private int outHeight, outWidth;
        public ReluTester(int outHeight, int outWidth)
        {
            this.outHeight = outHeight;
            this.outWidth = outWidth;
        }

        public async override System.Threading.Tasks.Task Run()
        {

            await ClockAsync();
            Output.enable = true;
            // Pack test data onto bus
            for (int i = 0; i < outHeight*outWidth; i++)
            {
                Output.ArrData[i] = (float) i+1;
            }
            Output.Height = outHeight;
            Output.Width = outWidth;
            await ClockAsync();

            // Ensure that the test data isn't read again.
            Output.enable = false;
            await ClockAsync();

            float[,] computed = new float[outHeight,outWidth];
            for (int i = 0; i < outHeight; i++)
            {
                for (int j = 0; j < outWidth; j++)
                {
                    // Wait until there is something valid on the bus.
                    while (!Input.enable) await ClockAsync();

                    computed[i,j] = Input.Value;
                    await ClockAsync();
                }
            }

            Console.WriteLine("Tester finished");
        }
    }
}