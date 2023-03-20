using SME;
using SME.Components;

namespace CNN
{
    class MainClass
    {
        public static void Main(string[] args)
        {

            using(var sim = new Simulation())
            {
                sim
                .Run();
            }

        }
    }
}
