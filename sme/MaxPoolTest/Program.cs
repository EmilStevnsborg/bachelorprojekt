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
                for (int t = 1; t <= 1; t++)
                {    
                    string config = File.ReadAllText(@"TestConfig" + c + "/config.json");
                    string inputString = File.ReadAllText(@"TestConfig"  + c + "/input" + t +".json");

                    MaxPoolConfig maxPoolConfig = JsonSerializer.Deserialize<MaxPoolConfig>(config);
                    maxPoolConfig.PushConfig();

                    InputCase input = JsonSerializer.Deserialize<InputCase>(inputString);
                    Tester tester = new Tester(maxPoolConfig.numInChannels, 
                                               maxPoolConfig.numInChannels,
                                               (maxPoolConfig.channelHeight,maxPoolConfig.channelWidth));
                    tester.FillBuffer(input.buffer, input.computed);

                    maxPoolConfig.maxPoolLayer.Inputs = tester.Outputs;
                    maxPoolConfig.maxPoolLayer.PushInputs();
                    tester.Inputs = maxPoolConfig.maxPoolLayer.Outputs;
                }
            }
            sim.Run();
        }
    }
}