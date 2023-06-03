using SME;
using CNN;
using System.IO;
using System.Text.Json;
using Config;
using Statistics;
using System;
using System.Collections.Generic;

class MainClass
{
    public static void Main(string[] args)
    {
        int tests = 1000;
        bool save = false;
        bool layerOutputs = false;
        // main stats
        Stats stats = new Stats();
        stats.TrueKeyAdd();

        List<string> layers = new List<string>() 
        {
            "conv1", "batchNorm1", "relu1",
            "maxPool1", "conv2", "batchNorm2",
            "relu2", "maxPool2", "linear", "softmax"
        };
        
        for (int t = 0; t < tests; t++) 
        {
            // layer stats
            Stats conv1Stats      = new Stats();
            Stats batchNorm1Stats = new Stats();
            Stats relu1Stats      = new Stats();
            Stats maxPool1Stats   = new Stats();

            Stats conv2Stats      = new Stats();
            Stats batchNorm2Stats = new Stats();
            Stats relu2Stats      = new Stats();
            Stats maxPool2Stats   = new Stats();

            Stats linearStats     = new Stats();
            Stats softmaxStats    = new Stats();

            List<Stats> layerStats = new List<Stats>() 
            {
                conv1Stats, batchNorm1Stats, relu1Stats,
                maxPool1Stats, conv2Stats, batchNorm2Stats,
                relu2Stats, maxPool2Stats, linearStats, softmaxStats
            };

            using(var sim = new Simulation())
            {
                long ticks = 0;
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
                                           softmaxConfig.numOutChannels,
                                           (conv1Config.channelHeight,conv1Config.channelWidth));

                string inputString  = File.ReadAllText(@"Tests/conv1/inputs/input" + (t+1) + ".json");
                string outputString = File.ReadAllText(@"Tests/softmax/inputs/input" + (t+1) + ".json");
                // input and output
                InputCase input = JsonSerializer.Deserialize<InputCase>(inputString);
                InputCase output = JsonSerializer.Deserialize<InputCase>(outputString);
                tester.FillBuffer(input.buffer, output.computed);

                if (!layerOutputs) 
                {
                    // combining layer busses for network
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

                    sim
                    .AddTicker(s => ticks = Scope.Current.Clock.Ticks)
                    .Run();
                    stats.AddStats(tester.Stats);
                }
                // layer outputs testers
                else
                {
                    // Conv1
                    TesterOutputs testerConv1 = new TesterOutputs(conv1Config.numInChannels, 
                                                    conv1Config.numOutChannels,
                                                    (conv1Config.channelHeight,conv1Config.channelWidth),
                                                    (26 * 26 * 3));
                    // BatchNorm1
                    TesterOutputs testerBatchNorm1 = new TesterOutputs(batchNorm1Config.numInChannels, 
                                                        batchNorm1Config.numOutChannels,
                                                        (batchNorm1Config.channelHeight,batchNorm1Config.channelWidth),
                                                        (26 * 26 * 3));
                    // Relu1
                    TesterOutputs testerRelu1 = new TesterOutputs(relu1Config.numInChannels, 
                                                    relu1Config.numOutChannels,
                                                    (relu1Config.channelHeight,relu1Config.channelWidth),
                                                    (26 * 26 * 3));
                    // MaxPool1
                    TesterOutputs testerMaxPool1 = new TesterOutputs(maxPool1Config.numInChannels, 
                                                    maxPool1Config.numInChannels,
                                                    (maxPool1Config.channelHeight,maxPool1Config.channelWidth),
                                                    (13 * 13 * 3));
                    // Conv2
                    TesterOutputs testerConv2 = new TesterOutputs(conv2Config.numInChannels, 
                                                    conv2Config.numOutChannels,
                                                    (conv2Config.channelHeight,conv2Config.channelWidth),
                                                    (9 * 9 * 5));
                    // BatchNorm2
                    TesterOutputs testerBatchNorm2 = new TesterOutputs(batchNorm2Config.numInChannels, 
                                                        batchNorm2Config.numOutChannels,
                                                        (batchNorm2Config.channelHeight,batchNorm2Config.channelWidth),
                                                        (9 * 9 * 5));
                    // Relu2
                    TesterOutputs testerRelu2 = new TesterOutputs(relu2Config.numInChannels, 
                                                    relu2Config.numOutChannels,
                                                    (relu2Config.channelHeight,relu2Config.channelWidth),
                                                    (9 * 9 * 5));
                    // MaxPool2
                    TesterOutputs testerMaxPool2 = new TesterOutputs(maxPool2Config.numInChannels, 
                                                    maxPool2Config.numInChannels,
                                                    (maxPool2Config.channelHeight,maxPool2Config.channelWidth),
                                                    (3 * 3 * 5));
                    // Linear
                    TesterOutputs testerLinear = new TesterOutputs(linearConfig.numInChannels, 
                                                    linearConfig.numOutChannels,
                                                    (linearConfig.channelHeight,linearConfig.channelWidth),
                                                    2);
                    // Softmax
                    TesterOutputs testerSoftmax = new TesterOutputs(softmaxConfig.numInChannels, 
                                                    softmaxConfig.numOutChannels,
                                                    (softmaxConfig.channelHeight,softmaxConfig.channelWidth),
                                                    2);

                    // combining layer busses for network
                    conv1Config.convLayer.Inputs = tester.Outputs;
                    conv1Config.convLayer.PushInputs();
                    // test
                    testerConv1.Inputs = conv1Config.convLayer.Outputs;

                    batchNorm1Config.batchNormLayer.Inputs = conv1Config.convLayer.Outputs;
                    batchNorm1Config.batchNormLayer.PushInputs();
                    // test
                    testerBatchNorm1.Inputs = batchNorm1Config.batchNormLayer.Outputs;

                    relu1Config.reluLayer.Inputs = batchNorm1Config.batchNormLayer.Outputs;
                    relu1Config.reluLayer.PushInputs();
                    // test
                    testerRelu1.Inputs = relu1Config.reluLayer.Outputs;

                    maxPool1Config.maxPoolLayer.Inputs = relu1Config.reluLayer.Outputs;
                    maxPool1Config.maxPoolLayer.PushInputs();
                    // test
                    testerMaxPool1.Inputs = maxPool1Config.maxPoolLayer.Outputs;

                    conv2Config.convLayer.Inputs = maxPool1Config.maxPoolLayer.Outputs;
                    conv2Config.convLayer.PushInputs();
                    // test
                    testerConv2.Inputs = conv2Config.convLayer.Outputs;

                    batchNorm2Config.batchNormLayer.Inputs = conv2Config.convLayer.Outputs;
                    batchNorm2Config.batchNormLayer.PushInputs();
                    // test
                    testerBatchNorm2.Inputs = batchNorm2Config.batchNormLayer.Outputs;

                    relu2Config.reluLayer.Inputs = batchNorm2Config.batchNormLayer.Outputs;
                    relu2Config.reluLayer.PushInputs();
                    // test
                    testerRelu2.Inputs = relu2Config.reluLayer.Outputs;

                    maxPool2Config.maxPoolLayer.Inputs = relu2Config.reluLayer.Outputs;
                    maxPool2Config.maxPoolLayer.PushInputs();
                    // test
                    testerMaxPool2.Inputs = maxPool2Config.maxPoolLayer.Outputs;

                    linearConfig.linearLayer.Inputs = maxPool2Config.maxPoolLayer.Outputs;
                    linearConfig.linearLayer.PushInputs();
                    // test
                    testerLinear.Inputs = linearConfig.linearLayer.Outputs;

                    softmaxConfig.softmaxLayer.Inputs = linearConfig.linearLayer.Outputs;
                    softmaxConfig.softmaxLayer.PushInputs();
                    // test
                    testerSoftmax.Inputs = softmaxConfig.softmaxLayer.Outputs;

                    tester.Inputs = softmaxConfig.softmaxLayer.Outputs;

                    sim
                    .AddTicker(s => ticks = Scope.Current.Clock.Ticks)
                    .Run();
                    conv1Stats.AddStats(testerConv1.Stats);
                    batchNorm1Stats.AddStats(testerBatchNorm1.Stats);
                    relu1Stats.AddStats(testerRelu1.Stats);
                    maxPool1Stats.AddStats(testerMaxPool1.Stats);

                    conv2Stats.AddStats(testerConv2.Stats);
                    batchNorm2Stats.AddStats(testerBatchNorm2.Stats);
                    relu2Stats.AddStats(testerRelu2.Stats);
                    maxPool2Stats.AddStats(testerMaxPool2.Stats);

                    linearStats.AddStats(testerLinear.Stats);
                    softmaxStats.AddStats(testerSoftmax.Stats);
                }
                Console.WriteLine(t + " " + ticks);
            }
            if (layerOutputs)
            {
                for (int i = 0; i < layers.Count; i++)
                {
                    string layer = layers[i];
                    Stats ls = layerStats[i];
                    // writing results out
                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true
                    };
                    File.WriteAllText(@"Tests/Network/outputs/test" + t + "/" + layer + ".json", JsonSerializer.Serialize(ls.Results, options));
                }
            }
        }
        if (save)
        {
            // writing results out
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            File.WriteAllText(@"Tests/Network/output.json", JsonSerializer.Serialize(stats.Results, options));         
        }
    }
}