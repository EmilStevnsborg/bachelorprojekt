using System.Threading.Tasks;
using SME;

namespace CNN
{
    public class RELUTester : SimulationProcess
    {
        [InputBus]
        public Channel Result;

        [OutputBus]
        public Channel Input = Scope.CreateOrLoadBus<Channel>();

        public override async Task Run()
        {
            await ClockAsync();
            // 2x2 input channel
            Input.Height = 2;
            Input.Width = 2;
            for (int i = 0; i < Input.Height; i++)
            {
                for (int j = 0; j < Input.Width; j++)
                {
                    Input.Data[i*Input.Width+j] = (double) i;
                }
            }

            await ClockAsync();

        }
    }
}