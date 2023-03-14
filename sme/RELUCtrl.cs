using System.Threading.Tasks;
using SME;

namespace CNN
{
    public class RELUCtrl : SimulationProcess
    {

        public RELUCtrl(int height, int width) 
        {
            // Read data -- Below is just for testing
            tmp[0] = 1;
            tmp[1] = 2;
            tmp[2] = 3;
            tmp[3] = 4;
        }

        [OutputBus]
        public Pixel Output = Scope.CreateOrLoadBus<Pixel>();

        public uint[] tmp = new uint[4];

        public override async Task Run()
        {
            await ClockAsync();

            //
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Output.Value = tmp[i * 2 + j];
                }
            }

            await ClockAsync();

        }
    }
}