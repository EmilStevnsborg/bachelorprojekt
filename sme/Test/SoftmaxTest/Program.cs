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
        bool CNNSmallTest = true;
        int tests = 1000;
        Stats stats = new Stats();

        if (CNNSmallTest)
        {
            using(var sim = new Simulation())
            {
                string config = File.ReadAllText(@"../../CNNSmall/Configs/softmax.json");
                SoftmaxConfig[] configs = new SoftmaxConfig[tests];
                Tester[] testers = new Tester[tests];

                for (int t = 0; t < tests; t++) 
                {
                    SoftmaxConfig softmaxConfig = JsonSerializer.Deserialize<SoftmaxConfig>(config);
                    softmaxConfig.PushConfig();

                    Tester tester = new Tester(softmaxConfig.numInChannels, 
                                               softmaxConfig.numOutChannels,
                                               (softmaxConfig.channelHeight,softmaxConfig.channelWidth));
                    configs[t] = softmaxConfig;
                    testers[t] = tester;
                }
                for (int t = 1; t <= tests; t++)
                { 
                    SoftmaxConfig softmaxConfig = configs[t-1];
                    Tester tester = testers[t-1];

                    string inputString = File.ReadAllText(@"../../CNNSmall/Tests/softmax/inputs/input" + t + ".json");
                    InputCase input = JsonSerializer.Deserialize<InputCase>(inputString);

                    tester.FillBuffer(input.buffer, input.computed);

                    softmaxConfig.softmaxLayer.Inputs = tester.Outputs;
                    softmaxConfig.softmaxLayer.PushInputs();
                    tester.Inputs = softmaxConfig.softmaxLayer.Outputs;
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
            File.WriteAllText(@"../../CNNSmall/Tests/softmax/output.json", JsonSerializer.Serialize(stats.Results, options));
        }
    }
}