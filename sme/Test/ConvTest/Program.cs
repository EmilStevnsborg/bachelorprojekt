using SME;
using CNN;
using System.IO;
using System.Text.Json;
using Config;
using Statistics;
using System;

class MainClass
{
    public static void Main(string[] args)
    {
        bool configTest = false;
        bool CNNSmallTest = !configTest;
        string convLayer = "conv1";
        int tests = 1000;
        Stats stats = new Stats();

        if (configTest) 
        {
            using(var sim = new Simulation())
            {
                for (int c = 1; c <= 3; c++)
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
        else if (CNNSmallTest)
        {
            using(var sim = new Simulation())
            {
                string config = File.ReadAllText(@"../../CNNSmall/Configs/" + convLayer + ".json");
                ConvConfig[] configs = new ConvConfig[tests];
                Tester[] testers = new Tester[tests];

                for (int t = 0; t < tests; t++) 
                {
                    ConvConfig convConfig = JsonSerializer.Deserialize<ConvConfig>(config);
                    convConfig.PushConfig();

                    Tester tester = new Tester(convConfig.numInChannels, 
                                               convConfig.numOutChannels,
                                               (convConfig.channelHeight,convConfig.channelWidth));
                    configs[t] = convConfig;
                    testers[t] = tester;
                }
                for (int t = 1; t <= tests; t++)
                { 
                    ConvConfig convConfig = configs[t-1];
                    Tester tester = testers[t-1];

                    string inputString = File.ReadAllText(@"../../CNNSmall/Tests/" + convLayer + "/inputs/input" + t + ".json");
                    InputCase input = JsonSerializer.Deserialize<InputCase>(inputString);

                    tester.FillBuffer(input.buffer, input.computed);

                    convConfig.convLayer.Inputs = tester.Outputs;
                    convConfig.convLayer.PushInputs();
                    tester.Inputs = convConfig.convLayer.Outputs;
                } 
                sim.Run();

                for (int t = 0; t < tests; t++) 
                {               
                    stats.AddStats(testers[t].Stats);
                }
            }
            // writing results out
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            File.WriteAllText(@"../../CNNSmall/Tests/" + convLayer + "/output.json", JsonSerializer.Serialize(stats.Results, options));
        }
    }
}