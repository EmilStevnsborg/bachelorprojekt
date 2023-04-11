using System;
using SME;
using CNN;
using System.IO;
using System.Text.Json;
using TestConv;

class MainClass
{
    public static void Main(string[] args)
    {
        // reads correctly
        string text = File.ReadAllText(@"Test1/config.json");
        Test test = JsonSerializer.Deserialize<Test>(text);

        // using(var sim = new Simulation())
        // {
        // }
    }
}