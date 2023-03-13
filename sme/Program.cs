using SME;

namespace CNN
{
    class MainClass
    {
        public static void Main(string[] args)
        {

            using(var sim = new Simulation())
            {
                var reluTester = new RELUTester();
                var reluChannel = new RELUChannel();

                reluChannel.Input = reluTester.Input;
                reluTester.Result = reluChannel.Output;

                sim
                .AddTopLevelInputs(reluChannel.Input)
                .AddTopLevelOutputs(reluChannel.Output)
                .Run();
            }

        }
    }
}
