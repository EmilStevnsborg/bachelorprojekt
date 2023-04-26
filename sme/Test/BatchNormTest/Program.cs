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
                string config = File.ReadAllText(@"../../CNNSmall/Configs/batchNorm1.json");
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
                    BatchNormConfig convConfig = configs[t-1];
                    Tester tester = testers[t-1];

                    string inputString = File.ReadAllText(@"../../CNNSmall/Tests/BatchNorm1/input" + t + ".json");
                    InputCase input = JsonSerializer.Deserialize<InputCase>(inputString);

                    tester.FillBuffer(input.buffer, input.computed);

                    convConfig.batchNormLayer.Inputs = tester.Outputs;
                    convConfig.batchNormLayer.PushInputs();
                    tester.Inputs = convConfig.batchNormLayer.Outputs;
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