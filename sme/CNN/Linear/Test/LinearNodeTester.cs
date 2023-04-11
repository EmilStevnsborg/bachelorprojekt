using System;
using System.Threading.Tasks;
using SME;

namespace CNN
{
    [ClockedProcess]
    public class LinearNodeTester : SimulationProcess
    {
        [InputBus]
        public ValueBus Input;
        [OutputBus]
        public ValueBus[] Outputs;

        public LinearNodeTester()
        {
            
            Outputs = new ValueBus[3];
            Outputs[0] = Scope.CreateBus<ValueBus>();
            Outputs[1] = Scope.CreateBus<ValueBus>();
            Outputs[2] = Scope.CreateBus<ValueBus>();
        }

        public override async Task Run()
        {
            await ClockAsync();
            // first round ie. first index in each channel
            for (int i = 0; i < 3; i++)
            {
                Outputs[i].enable = true;
                // Pack test data onto bus
                Outputs[i].Value = (float) i + 1;
                Console.Write((i + 1) + " ");
            }
            Console.WriteLine();
            await ClockAsync();
            // second round ie. second index in each channel
            for (int i = 0; i < 3; i++)
            {
                Outputs[i].enable = true;
                // Pack test data onto bus
                Outputs[i].Value = (float) -i - 10;
                Console.Write((-i - 10) + " ");
            }
            Console.WriteLine();
            await ClockAsync();
            // Data shouldn't be read again
            for (int i = 0; i < 3; i++)
            {
                Outputs[i].enable = false;
            }
            await ClockAsync();
            // Loading input
            while (!(Input.enable)) await ClockAsync();
            Console.WriteLine(Input.Value);
            await ClockAsync();
            Console.WriteLine();
        }
    }
}