using SME;
using System;

namespace CNN
{
    class MainClass
    {
        public static void Main(string[] args)
        {

            using(var sim = new Simulation())
            {
                var reluTester = new RELUCtrl(2,2);
                var relu = new RELU();
                var test = new ReLUTest();

                relu.Input = (Pixel) reluTester.Output;
                test.Input = relu.Output;
                
            



                sim
                .Run();
            }

        }
    }
}
