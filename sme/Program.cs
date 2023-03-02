using SME;

namespace CNN
{
    class MainClass
    {
        public static void Main(string[] args)
        {

            // using(var sim = new Simulation())
            // {
            //     sim
            //     .BuildCSVFile()
            //     .BuildGraph()
            //     .Run(
            //         new ConvTester()
            //     );
            // }

            ConvTester.testOne();

        }
    }
}
