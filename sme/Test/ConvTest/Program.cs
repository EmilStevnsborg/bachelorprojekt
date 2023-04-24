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
            for (int c = 2; c <= 3; c++)
            {
                for (int t = 1; t <= 10; t++)
                {    
                    string config = File.ReadAllText(@"TestConfig" + c + "/config.json");
                    string inputString = File.ReadAllText(@"TestConfig"  + c + "/input" + t +".json");

                    ConvConfig convConfig = JsonSerializer.Deserialize<ConvConfig>(config);
                    convConfig.PushConfig();

                    InputCase input = JsonSerializer.Deserialize<InputCase>(inputString);
                    Tester tester = new Tester(convConfig.numInChannels, 
                                               convConfig.numOutChannels,
                                               (convConfig.channelHeight,convConfig.channelWidth));
                    tester.FillBuffer(input.buffer, input.computed);

                    convConfig.convLayer.Inputs = tester.Outputs;
                    convConfig.convLayer.PushInputs();
                    tester.Inputs = convConfig.convLayer.Outputs;
                }
            }
            sim.Run();
        }
    }
}