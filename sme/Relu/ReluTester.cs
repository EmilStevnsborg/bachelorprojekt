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

        public async override System.Threading.Tasks.Task Run()
        {

            await ClockAsync();
            Output.enable = true;
            // Pack test data onto bus
            for (int i = 0; i < 6; i++)
            {
                Output.ArrData[i] = (float) i+1;
            }
            Output.Height = 2;
            Output.Width = 3;
            await ClockAsync();

            // Ensure that the test data isn't read again.
            Output.enable = false;
            await ClockAsync();

            float[,] computed = new float[2,3];
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    // Wait until there is something valid on the bus.
                    while (!Input.enable) await ClockAsync();

                    computed[i,j] = Input.Value;
                    await ClockAsync();
                }
            }

            // Output to terminal to indicate everything went ok. If not, the
            // earlier asserts should halt the program.
            Console.WriteLine("Tester finished");
        }
    }
}