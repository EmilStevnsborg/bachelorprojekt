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
        string linLayer = "linear";
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
        else if (CNNSmallTest)
        {
            using(var sim = new Simulation())
            {
                string config = File.ReadAllText(@"../../CNNSmall/Configs/" + linLayer + ".json");
                LinearConfig[] configs = new LinearConfig[tests];
                Tester[] testers = new Tester[tests];

                for (int t = 0; t < tests; t++) 
                {
                    LinearConfig linConfig = JsonSerializer.Deserialize<LinearConfig>(config);
                    linConfig.PushConfig();

                    Tester tester = new Tester(linConfig.numInChannels, 
                                               linConfig.numOutChannels,
                                               (linConfig.channelHeight,linConfig.channelWidth));
                    configs[t] = linConfig;
                    testers[t] = tester;
                }
                for (int t = 1; t <= tests; t++)
                { 
                    LinearConfig linConfig = configs[t-1];
                    Tester tester = testers[t-1];

                    string inputString = File.ReadAllText(@"../../CNNSmall/Tests/" + linLayer + "/inputs/input" + t + ".json");
                    InputCase input = JsonSerializer.Deserialize<InputCase>(inputString);

                    tester.FillBuffer(input.buffer, input.computed);

                    linConfig.linearLayer.Inputs = tester.Outputs;
                    linConfig.linearLayer.PushInputs();
                    tester.Inputs = linConfig.linearLayer.Outputs;
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
            File.WriteAllText(@"../../CNNSmall/Tests/" + linLayer + "/output.json", JsonSerializer.Serialize(stats.Results, options));
        }
    }
}