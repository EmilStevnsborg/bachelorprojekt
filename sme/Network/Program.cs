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
            string config = File.ReadAllText(@"config.json");
            string inputString = File.ReadAllText(@"Tests/input1.json");

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


            sim.Run();
        }
    }
}