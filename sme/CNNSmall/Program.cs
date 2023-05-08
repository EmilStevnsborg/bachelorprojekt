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
        int tests = 100;
        Stats stats = new Stats();
        
        for (int t = 0; t < tests; t++) 
        {
            using(var sim = new Simulation())
            {
                string conv1        = File.ReadAllText(@"Configs/conv1.json");
                string batchNorm1   = File.ReadAllText(@"Configs/batchNorm1.json");
                string relu1        = File.ReadAllText(@"Configs/relu1.json");
                string maxPool1     = File.ReadAllText(@"Configs/maxPool1.json");
                string conv2        = File.ReadAllText(@"Configs/conv2.json");
                string batchNorm2   = File.ReadAllText(@"Configs/batchNorm2.json");
                string relu2        = File.ReadAllText(@"Configs/relu2.json");
                string maxPool2     = File.ReadAllText(@"Configs/maxPool2.json");
                string linear       = File.ReadAllText(@"Configs/linear.json");
                string softmax      = File.ReadAllText(@"Configs/softmax.json");

                // initalizing all layers
                ConvConfig conv1Config  = JsonSerializer.Deserialize<ConvConfig>(conv1);
                conv1Config.PushConfig();
                BatchNormConfig batchNorm1Config = JsonSerializer.Deserialize<BatchNormConfig>(batchNorm1);
                batchNorm1Config.PushConfig();
                ReluConfig relu1Config = JsonSerializer.Deserialize<ReluConfig>(relu1);
                relu1Config.PushConfig();
                MaxPoolConfig maxPool1Config = JsonSerializer.Deserialize<MaxPoolConfig>(maxPool1);
                maxPool1Config.PushConfig();

                ConvConfig conv2Config  = JsonSerializer.Deserialize<ConvConfig>(conv2);
                conv2Config.PushConfig();
                BatchNormConfig batchNorm2Config = JsonSerializer.Deserialize<BatchNormConfig>(batchNorm2);
                batchNorm2Config.PushConfig();
                ReluConfig relu2Config = JsonSerializer.Deserialize<ReluConfig>(relu2);
                relu2Config.PushConfig();
                MaxPoolConfig maxPool2Config = JsonSerializer.Deserialize<MaxPoolConfig>(maxPool2);
                maxPool2Config.PushConfig();

                LinearConfig linearConfig = JsonSerializer.Deserialize<LinearConfig>(linear);
                linearConfig.PushConfig();

                SoftmaxConfig softmaxConfig = JsonSerializer.Deserialize<SoftmaxConfig>(softmax);
                softmaxConfig.PushConfig();

                Tester tester = new Tester(conv1Config.numInChannels, 
                                           linearConfig.numOutChannels,
                                           (conv1Config.channelHeight,conv1Config.channelWidth));

                string inputString  = File.ReadAllText(@"Tests/conv1/inputs/input" + (t+1) + ".json");
                string outputString = File.ReadAllText(@"Tests/softmax/inputs/input" + (t+1) + ".json");
                // input anf output
                InputCase input = JsonSerializer.Deserialize<InputCase>(inputString);
                InputCase output = JsonSerializer.Deserialize<InputCase>(outputString);
                tester.FillBuffer(input.buffer, output.computed);

                // combining layer busses
                conv1Config.convLayer.Inputs = tester.Outputs;
                conv1Config.convLayer.PushInputs();
                batchNorm1Config.batchNormLayer.Inputs = conv1Config.convLayer.Outputs;
                batchNorm1Config.batchNormLayer.PushInputs();
                relu1Config.reluLayer.Inputs = batchNorm1Config.batchNormLayer.Outputs;
                relu1Config.reluLayer.PushInputs();
                maxPool1Config.maxPoolLayer.Inputs = relu1Config.reluLayer.Outputs;
                maxPool1Config.maxPoolLayer.PushInputs();

                conv2Config.convLayer.Inputs = maxPool1Config.maxPoolLayer.Outputs;
                conv2Config.convLayer.PushInputs();
                batchNorm2Config.batchNormLayer.Inputs = conv2Config.convLayer.Outputs;
                batchNorm2Config.batchNormLayer.PushInputs();
                relu2Config.reluLayer.Inputs = batchNorm2Config.batchNormLayer.Outputs;
                relu2Config.reluLayer.PushInputs();
                maxPool2Config.maxPoolLayer.Inputs = relu2Config.reluLayer.Outputs;
                maxPool2Config.maxPoolLayer.PushInputs();
                linearConfig.linearLayer.Inputs = maxPool2Config.maxPoolLayer.Outputs;
                linearConfig.linearLayer.PushInputs();

                softmaxConfig.softmaxLayer.Inputs = linearConfig.linearLayer.Outputs;
                softmaxConfig.softmaxLayer.PushInputs();
                tester.Inputs = softmaxConfig.softmaxLayer.Outputs;
                
                long ticks = 0;

                sim
                .AddTicker(s => ticks = Scope.Current.Clock.Ticks)
                .Run();
                stats.AddStats(tester.Stats);
            
                Console.WriteLine(t);
            }
        }
        // writing results out
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        File.WriteAllText(@"Tests/Network/output.json", JsonSerializer.Serialize(stats.Results, options));
    }
}