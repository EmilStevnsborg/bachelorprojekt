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
        private int channelHeight;
        private int channelWidth;
        public float[][] buffer;
        public Tester(int numInChannels,int numOutChannels,(int,int) channelSize)
        {            
            Outputs = new ValueBus[numInChannels];
            for (int i = 0; i < numInChannels; i++)
            {
                Outputs[i] = Scope.CreateBus<ValueBus>();
            }
            channelHeight = channelSize.Item1;
            channelWidth = channelSize.Item2;
        }
        public void FillBuffer(float[][] buffer)
        {
            this.buffer = new float[channelHeight][];
            for (int i = 0; i < channelHeight; i++)
            {
                this.buffer[i] = new float[channelWidth];
                for (int j = 0; j < channelWidth; j++)
                {
                    this.buffer[i][j] = buffer[i][j];
                }
            }
            Console.WriteLine(buffer[1][1]);
        }
        public override async Task Run()
        {
            await ClockAsync();
            // read data in from json            
        }
    }
}