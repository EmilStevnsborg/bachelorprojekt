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
                    float val = (i+1)*j;
                    Outputs[i].ArrData[j] = val;
                    Console.Write(val + " ");
                    if ((j + 1) % 4 == 0) {Console.WriteLine();}
                }
                Outputs[i].Height = 4;
                Outputs[i].Width = 4;
            }
            Console.WriteLine();
            await ClockAsync();
            Console.WriteLine("her: " + Outputs[0].enable);
            // Data shouldn't be read again
            for (int i = 0; i < 2; i++)
            {
                Outputs[i].enable = false;
            }
            await ClockAsync();
            for (int i = 0; i < 4; i++)
            {
                // Two values arrive at the same time
                for (int j = 0; j < 2; j++)
                {
                    while (!Inputs[j].enable) await ClockAsync();
                    Console.Write(Inputs[j].Value + " ");
                    if ((i + 1) % 2 == 0) {Console.WriteLine();}
                    await ClockAsync();
                }
            }
        }
    }
}