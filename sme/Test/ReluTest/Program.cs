using System;
using SME;
using CNN;
using System.IO;
using System.Text.Json;
using Config;
using Statistics;

class MainClass
{
    public static void Main(string[] args)
    {
        bool configTest = false;
        bool CNNSmallTest = !configTest;
        string reluLayer = "relu2";
        int tests = 1000;
        Stats stats = new Stats();

        if (configTest) 
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
        else if (CNNSmallTest)
        {
            using(var sim = new Simulation())
            {
                string config = File.ReadAllText(@"../../CNNSmall/Configs/" + reluLayer + ".json");
                ReluConfig[] configs = new ReluConfig[tests];
                Tester[] testers = new Tester[tests];

                for (int t = 0; t < tests; t++) 
                {
                    ReluConfig reluConfig = JsonSerializer.Deserialize<ReluConfig>(config);
                    reluConfig.PushConfig();

                    Tester tester = new Tester(reluConfig.numInChannels, 
                                               reluConfig.numOutChannels,
                                               (reluConfig.channelHeight,reluConfig.channelWidth));
                    configs[t] = reluConfig;
                    testers[t] = tester;
                }
                for (int t = 1; t <= tests; t++)
                { 
                    ReluConfig reluConfig = configs[t-1];
                    Tester tester = testers[t-1];

                    string inputString = File.ReadAllText(@"../../CNNSmall/Tests/" + reluLayer + "/inputs/input" + t + ".json");
                    InputCase input = JsonSerializer.Deserialize<InputCase>(inputString);

                    tester.FillBuffer(input.buffer, input.computed);

                    reluConfig.reluLayer.Inputs = tester.Outputs;
                    reluConfig.reluLayer.PushInputs();
                    tester.Inputs = reluConfig.reluLayer.Outputs;
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
            File.WriteAllText(@"../../CNNSmall/Tests/" + reluLayer + "/output.json", JsonSerializer.Serialize(stats.Results, options));
        }
    }
}