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
                for (int t = 1; t <= 2; t++)
                {    
                    // reads correctly
                    string config = File.ReadAllText(@"TestConfig" + c + "/config.json");
                    string inputString = File.ReadAllText(@"TestConfig"  + c + "/input" + t +".json");

                    LinearConfig linearConfig = JsonSerializer.Deserialize<LinearConfig>(config);
                    linearConfig.PushConfig();

                    InputCase input = JsonSerializer.Deserialize<InputCase>(inputString);
                    Tester tester = new Tester(linearConfig.numInChannels, 
                                            linearConfig.numOutChannels,
                                            (linearConfig.channelHeight,linearConfig.channelWidth));
                    tester.FillBuffer(input.buffer, input.computed);

                    linearConfig.linearLayer.Inputs = tester.Outputs;
                    linearConfig.linearLayer.PushInputs();
                    tester.Inputs = linearConfig.linearLayer.Outputs;
                }
            }
            sim.Run();
        }
    }
}