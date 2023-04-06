using System;
using System.Threading.Tasks;
using SME;

namespace CNN
{
    [ClockedProcess]
    public class FilterTester : SimulationProcess
    {
        [InputBus]
        public ValueBus[] Inputs;
        [OutputBus]
        public ChannelBus[] Outputs;

        // public ValueBus[] GetInputs
        // {
        //     get => Inputs;
        //     set => Inputs = value;
        // }
        // public ChannelBus[] GetOutputs
        // {
        //     get => Outputs;
        //     set => Outputs = value;
        // }

        public FilterTester()
        {
            Outputs = new ChannelBus[2];
            Array.Fill(Outputs, Scope.CreateBus<ChannelBus>());
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
                    float val = j + 1;
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
            var outputOne = new float[4];
            var outputTwo = new float[4];
            await ClockAsync();
            // Loading inputs
            for (int i = 0; i < 4; i++)
            {
                // Two values arrive at the same time
                while (!(Inputs[0].enable && Inputs[1].enable)) await ClockAsync();
                outputOne[i] = Inputs[0].Value;
                outputTwo[i] = Inputs[1].Value;
                await ClockAsync();
            }
            // print results
            for (int j = 0; j < 2; j++)
            {
                for (int i  = 0; i < 4; i++)
                {
                    if (j == 0)
                    {
                        Console.Write(outputOne[i] + " ");
                    }
                    else
                    {
                        Console.Write(outputTwo[i] + " ");
                    }
                    if ((i + 1) % 2 == 0) {Console.WriteLine();}
                }
                Console.WriteLine();
            }
        }
    }
}