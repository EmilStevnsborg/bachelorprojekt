using System;
using SME;
using CNN;
using System.IO;
using System.Text.Json;
using TestConv;
using SME.Components;

class MainClass
{
    public static void Main(string[] args)
    {
        using(var sim = new Simulation())
        {            
            // reads correctly
            string config = File.ReadAllText(@"TestConfig1/config.json");
            string input1 = File.ReadAllText(@"TestConfig1/input1.json");

            Test test = JsonSerializer.Deserialize<Test>(config);
            test.PushConfig();

            InputCase input = JsonSerializer.Deserialize<InputCase>(input1);
            test.tester.FillBuffer(input.buffer);

            test.convLayer.Inputs = test.tester.Outputs;
            test.convLayer.PushInputs();
            test.tester.Inputs = test.convLayer.Outputs;

            sim.Run();
        }
    }
}