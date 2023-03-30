using System;
using System.Threading.Tasks;
using SME;

namespace CNN
{
    [ClockedProcess]
    public class UpSampleTester : SimulationProcess
    {

        [InputBus]
        public ChannelBus Input;

        [OutputBus]        
        public ValueBus Output = Scope.CreateBus<ValueBus>();

        public async override System.Threading.Tasks.Task Run()
        {
            await ClockAsync();
            Output.enable = true;
            Output.Value = 1;
            await ClockAsync();
            Output.Value = 3;
            await ClockAsync();
            Output.Value = 10;
            await ClockAsync();
            Output.Value = -4;
            await ClockAsync();
            Output.enable = false;
            while (!Input.enable) await ClockAsync();
            for (int ii = 0; ii < Input.Height; ii++)
            {
                for (int jj = 0; jj < Input.Width; jj++)
                {
                    Console.Write(Input.ArrData[ii*Input.Width + jj] + ", ");
                }
                Console.WriteLine();
            }
            await ClockAsync();
        }
    }
}