using SME;

namespace CNN
{
    class MainClass
    {
        public static void Main(string[] args)
        {

            using(var sim = new Simulation())
            {
                sim
                .Run(
                    new ELUChannel(1.0),
                    new ELUTester()
                );
            }

        }
    }
}
