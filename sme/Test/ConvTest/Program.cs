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
        int tests = 50;
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
                string config = File.ReadAllText(@"../../CNNSmall/Configs/conv1.json");
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

                    string inputString = File.ReadAllText(@"../../CNNSmall/Tests/Conv1/input" + t + ".json");
                    InputCase input = JsonSerializer.Deserialize<InputCase>(inputString);

                    tester.FillBuffer(input.buffer, input.computed);

                    convConfig.convLayer.Inputs = tester.Outputs;
                    convConfig.convLayer.PushInputs();
                    tester.Inputs = convConfig.convLayer.Outputs;
                } 
                sim.Run();

                for (int t = 0; t < tests; t++) 
                {               
                    stats.Add(testers[t].Stats);
                }
            }

            Console.WriteLine("The mean of the losses is: " + stats.Mean());
            Console.WriteLine("The variance of the losses is: " + stats.Var());
            Console.WriteLine("The max value of the losses is: " + stats.Max());
        }
    }
}