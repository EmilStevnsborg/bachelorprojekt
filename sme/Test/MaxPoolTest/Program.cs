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
        string mpLayer = "maxPool2";
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
        else if (CNNSmallTest)
        {
            using(var sim = new Simulation())
            {
                string config = File.ReadAllText(@"../../CNNSmall/Configs/" + mpLayer + ".json");
                MaxPoolConfig[] configs = new MaxPoolConfig[tests];
                Tester[] testers = new Tester[tests];

                for (int t = 0; t < tests; t++) 
                {
                    MaxPoolConfig maxPoolConfig = JsonSerializer.Deserialize<MaxPoolConfig>(config);
                    maxPoolConfig.PushConfig();

                    Tester tester = new Tester(maxPoolConfig.numInChannels, 
                                               maxPoolConfig.numInChannels,
                                               (maxPoolConfig.channelHeight,maxPoolConfig.channelWidth));
                    configs[t] = maxPoolConfig;
                    testers[t] = tester;
                }
                for (int t = 1; t <= tests; t++)
                { 
                    MaxPoolConfig maxPoolConfig = configs[t-1];
                    Tester tester = testers[t-1];

                    string inputString = File.ReadAllText(@"../../CNNSmall/Tests/" + mpLayer + "/inputs/input" + t + ".json");
                    InputCase input = JsonSerializer.Deserialize<InputCase>(inputString);

                    tester.FillBuffer(input.buffer, input.computed);

                    maxPoolConfig.maxPoolLayer.Inputs = tester.Outputs;
                    maxPoolConfig.maxPoolLayer.PushInputs();
                    tester.Inputs = maxPoolConfig.maxPoolLayer.Outputs;
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
            File.WriteAllText(@"../../CNNSmall/Tests/" + mpLayer + "/output.json", JsonSerializer.Serialize(stats.Results, options));
        }
    }
}