using CNN;
using System.Threading.Tasks;
using SME;
using System;
using System.Collections.Generic;

namespace CNN
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
        private int NumInputs = 0;
        public List<(float, float)> Stats = new List<(float,float)>();

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
            int index = 0;

            await ClockAsync();
            for (int i = 0; i < channelHeight; i++)
            {
                for (int j = 0; j < channelWidth; j++)
                {
                    for (int k = 0; k < numInChannels; k++)
                    {
                        Outputs[k].Value = buffer[k][i * channelWidth + j];
                        Outputs[k].enable = true;
                    }
                    await ClockAsync();
                    // check if input is back during sending
                    if (Inputs[0].enable) 
                    {
                        // Console.WriteLine(NumInputs);
                        for (int c = 0; c < numOutChannels; c++)
                        {
                            NumInputs += 1;
                            var loss = Math.Abs(Inputs[c].Value - computed[c][index]);
                            Stats.Add((computed[c][index], Inputs[c].Value));
                            if (loss > 0.0000001)
                            {
                                // Console.WriteLine("The loss was higher than 10^(-7): " + loss);
                            }
                            if (c == numOutChannels-1) 
                            {
                                index += 1;
                            }
                        }
                    }               
                }
            }
            for (int k = 0; k < numInChannels; k++)
            {
                Outputs[k].enable = false;
            }
            await ClockAsync();
            // wait for input to arrive
            while(!Inputs[0].enable) await ClockAsync();
            // load streaming input, remember that individual send values at different times
            var expectedOutputs = computed.Length * computed[0].Length;
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
                        Stats.Add((computed[c][index], Inputs[c].Value));
                        // Console.WriteLine("pred: " + Inputs[c].Value + " " + computed[c][index] + " " + (Inputs[c].Value - computed[c][index]));
                        if (c == numOutChannels-1) 
                        {
                            index += 1;
                        }
                    }
                }
                await ClockAsync();
            }
            // Console.WriteLine(NumInputs);
        }
    }
}