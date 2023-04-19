using CNN;
using System.Threading.Tasks;
using SME;
using System;

namespace TestConv
{
    [ClockedProcess]
    public class Tester : SimulationProcess
    {
        [InputBus]
        public ValueBus[] Inputs;
        [OutputBus]
        public ValueBus[] Outputs;
        private int numInChannels { get; set; }
        private int numOutChannels { get; set; }
        private int channelHeight { get; set; }
        private int channelWidth { get; set; }
        private float[][] buffer { get; set; }
        private float[][] computed { get; set; }
        public Tester(int numInChannels,int numOutChannels,(int,int) channelSize)
        {            
            Outputs = new ValueBus[numInChannels];
            for (int i = 0; i < numInChannels; i++)
            {
                Outputs[i] = Scope.CreateBus<ValueBus>();
            }
                        
            this.numInChannels = numInChannels;
            this.numOutChannels = numOutChannels;
            channelHeight = channelSize.Item1;
            channelWidth = channelSize.Item2;
        }
        public void FillBuffer(float[][] buffer, float[][] computed)
        {
            this.buffer = buffer;
            this.computed = computed;

        }
        public override async Task Run()
        {
            await ClockAsync();
            for (int i = 0; i < channelHeight; i++)
            {
                for (int j = 0; j < channelWidth; j++)
                {
                    for (int k = 0; k < numInChannels; k++)
                    {
                        Outputs[k].Value = buffer[k][i * channelWidth + j];
                        // Console.WriteLine(i + " " + j + " " + k);
                        Outputs[k].enable = true;
                    }
                    await ClockAsync();                    
                }
            }
            for (int k = 0; k < numInChannels; k++)
            {
                Outputs[k].enable = false;
            }
            // wait for input to arrive
            while(!Inputs[0].enable) await ClockAsync();
            // load streaming input
            int index = 0;
            for (int t = 0; t < 150; t++)
            {
                if (Inputs[0].enable) 
                {
                    Console.WriteLine("Index " + index);
                    for (int c = 0; c < numOutChannels; c++)
                    {
                        Console.Write("Channel" + c + " " + Inputs[c].Value + "-" + computed[c][index] + " ");
                        if (c == numOutChannels-1) 
                        {
                            Console.WriteLine(); 
                            index += 1;
                        }
                    }
                }
                await ClockAsync();
            }
        }
    }
}