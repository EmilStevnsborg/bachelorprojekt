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
                var reluCtrl = new RELUCtrl(2,2);
                var relu = new RELU();

                reluCtrl.Input = reluTester.Input;
                relu.Input = reluCtrl.Output;
                //
                // reluTester.Result.Data[x] = relu.Output.Value;

                sim
                .Run();
            }

        }
    }
}
