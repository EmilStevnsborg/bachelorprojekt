using System.Threading.Tasks;
using SME;

namespace CNN
{
    public class ELUTester : SimulationProcess
    {
        [InputBus]
        private readonly ChannelInput IInput = Scope.CreateOrLoadBus<ChannelInput>();
         [InputBus]
        private readonly ChannelOutput Result = Scope.CreateOrLoadBus<ChannelOutput>();
        public override async Task Run()
        {
            await ClockAsync();
            //
            IInput.Height = 2;
            IInput.Width = 2;
            IInput.Values = new double[2,2] {{0,1},{10,1}};

            await ClockAsync();

        }
    }
}