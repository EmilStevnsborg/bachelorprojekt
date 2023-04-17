using System;
using System.Threading.Tasks;
using SME;

namespace CNN
{
    [ClockedProcess]
    public class ConvLayerTester : SimulationProcess
    {
        [InputBus]
        public ValueBus[] Inputs;
        [OutputBus]
        public ValueBus[] Outputs;

        public ConvLayerTester()
        {
            
            Outputs = new ValueBus[2];
            Outputs[0] = Scope.CreateBus<ValueBus>();
            Outputs[1] = Scope.CreateBus<ValueBus>();
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
                    float val = (8-j) * (i+1);
                    Outputs[i].Value = val;
                    Console.Write(val + " ");
                    if ((j + 1) % 4 == 0) {Console.WriteLine();}
                }
            }
            Console.WriteLine();
            await ClockAsync();
            // Data shouldn't be read again
            for (int i = 0; i < 2; i++)
            {
                Outputs[i].enable = false;
            }
            var outputOne = new float[4];
            var outputTwo = new float[4];
            await ClockAsync();
            // Loading inputs
            for (int i = 0; i < 4; i++)
            {
                while (!(Inputs[0].enable)) await ClockAsync();
                outputOne[i] = Inputs[0].Value;
                outputTwo[i] = Inputs[1].Value;
                await ClockAsync();
            }
            // print results
            // 1st
            for (int i  = 0; i < 4; i++)
            {
                Console.Write(outputOne[i] + " ");
                if ((i + 1) % 2 == 0) {Console.WriteLine();}
            }
            Console.WriteLine();
            // 2nd
            for (int i  = 0; i < 4; i++)
            {
                Console.Write(outputTwo[i] + " ");
                if ((i + 1) % 2 == 0) {Console.WriteLine();}
            }
            Console.WriteLine();
        }
    }
}