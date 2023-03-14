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
                var reluTester = new RELUTester(2,2);
                var relu = new RELU();

                relu.Input = (Pixel) reluTester.Output;
                
                
                

                sim
                .Run();
            }

        }
    }
}
