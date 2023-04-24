using System;
using SME;
using CNN;
using System.IO;
using System.Text.Json;
using Config;

class MainClass
{
    public static void Main(string[] args)
    {
        using(var sim = new Simulation())
        {
            for (int c = 1; c <= 1; c++)
            {
                for (int t = 1; t <= 10; t++)
                {    
                    // reads correctly
                    string config = File.ReadAllText(@"TestConfig" + c + "/config.json");
                    string inputString = File.ReadAllText(@"TestConfig"  + c + "/input" + t +".json");

                    ReluConfig reluConfig = JsonSerializer.Deserialize<ReluConfig>(config);
                    reluConfig.PushConfig();

                    InputCase input = JsonSerializer.Deserialize<InputCase>(inputString);
                    Tester tester = new Tester(reluConfig.numInChannels, 
                                               reluConfig.numOutChannels,
                                               (reluConfig.channelHeight,reluConfig.channelWidth));
                    tester.FillBuffer(input.buffer, input.computed);

                    reluConfig.reluLayer.Inputs = tester.Outputs;
                    reluConfig.reluLayer.PushInputs();
                    tester.Inputs = reluConfig.reluLayer.Outputs;
                }
            }
            sim.Run();
        }
    }
}