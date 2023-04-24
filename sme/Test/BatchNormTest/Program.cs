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
                    string config = File.ReadAllText(@"TestConfig" + c + "/config.json");
                    string inputString = File.ReadAllText(@"TestConfig"  + c + "/input" + t +".json");

                    BatchNormConfig batchNormConfig = JsonSerializer.Deserialize<BatchNormConfig>(config);
                    batchNormConfig.PushConfig();

                    InputCase input = JsonSerializer.Deserialize<InputCase>(inputString);
                    Tester tester = new Tester(batchNormConfig.numInChannels, 
                                               batchNormConfig.numInChannels,
                                               (batchNormConfig.channelHeight,batchNormConfig.channelWidth));
                    tester.FillBuffer(input.buffer, input.computed);

                    batchNormConfig.batchNormLayer.Inputs = tester.Outputs;
                    batchNormConfig.batchNormLayer.PushInputs();
                    tester.Inputs = batchNormConfig.batchNormLayer.Outputs;
                }
            }
            sim.Run();
        }
    }
}