using CNN;
using System.Threading.Tasks;
using SME;
using System;
using System.Collections.Generic;

namespace CNN
{
    [ClockedProcess]
    public class TesterOutputs : SimulationProcess
    {
        [OutputBus]
        public ValueBus[] Inputs;

        private int numInChannels { get; set; }
        private int numOutChannels { get; set; }
        private int channelHeight { get; set; }
        private int channelWidth { get; set; }
        private int expectedOutputs { get; set; }
        private int NumInputs = 0;
        public List<float> Stats = new List<float>();

        public TesterOutputs(int numInChannels,int numOutChannels,(int,int) channelSize, int expectedOutputs)
        {
            this.numInChannels = numInChannels;
            this.numOutChannels = numOutChannels;
            channelHeight = channelSize.Item1;
            channelWidth = channelSize.Item2;
            this.expectedOutputs = expectedOutputs;
        }
        public override async Task Run()
        {
            await ClockAsync();
            // wait for input to arrive
            while(!Inputs[0].enable) await ClockAsync();
            // load streaming input, remember that individual send values at different times
            for (int t = 0; t < 10000; t++)
            {
                // This is to make sure to not go through unecessary clock cycles
                if (NumInputs == expectedOutputs) 
                {
                    NumInputs = 0;
                    break;
                }
                // Console.WriteLine(t + " " + NumInputs);
                if (Inputs[0].enable)
                {
                    for (int c = 0; c < numOutChannels; c++)
                    { 
                        NumInputs += 1;
                        Stats.Add(Inputs[c].Value);
                    }
                }
                await ClockAsync();
            }
            Console.WriteLine(expectedOutputs + " " + numInChannels + " " + channelHeight + " " + channelWidth + " " + NumInputs);
        }
    }
}