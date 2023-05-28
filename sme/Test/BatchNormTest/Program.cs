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
        string batchNormLayer = "batchNorm2";
        int tests = 1000;
        Stats stats = new Stats();
        stats.TrueKeyAdd();

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
        else if (CNNSmallTest)
        {
            using(var sim = new Simulation())
            {
                string config = File.ReadAllText(@"../../CNNSmall/Configs/" + batchNormLayer + ".json");
                BatchNormConfig[] configs = new BatchNormConfig[tests];
                Tester[] testers = new Tester[tests];

                for (int t = 0; t < tests; t++) 
                {
                    BatchNormConfig batchNormConfig = JsonSerializer.Deserialize<BatchNormConfig>(config);
                    batchNormConfig.PushConfig();

                    Tester tester = new Tester(batchNormConfig.numInChannels, 
                                               batchNormConfig.numOutChannels,
                                               (batchNormConfig.channelHeight,batchNormConfig.channelWidth));
                    configs[t] = batchNormConfig;
                    testers[t] = tester;
                }
                for (int t = 1; t <= tests; t++)
                { 
                    BatchNormConfig batchNormConfig = configs[t-1];
                    Tester tester = testers[t-1];

                    string inputString = File.ReadAllText(@"../../CNNSmall/Tests/" + batchNormLayer + "/inputs/input" + t + ".json");
                    InputCase input = JsonSerializer.Deserialize<InputCase>(inputString);

                    tester.FillBuffer(input.buffer, input.computed);

                    batchNormConfig.batchNormLayer.Inputs = tester.Outputs;
                    batchNormConfig.batchNormLayer.PushInputs();
                    tester.Inputs = batchNormConfig.batchNormLayer.Outputs;
                } 
                long ticks = 0;

                sim
                .AddTicker(s => ticks = Scope.Current.Clock.Ticks)
                .Run();

                for (int t = 0; t < tests; t++) 
                {               
                    stats.AddStats(testers[t].Stats);
                }

                Console.WriteLine(ticks);
            }
            // writing results out
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            File.WriteAllText(@"../../CNNSmall/Tests/" + batchNormLayer + "/output.json", JsonSerializer.Serialize(stats.Results, options));
        }
    }
}