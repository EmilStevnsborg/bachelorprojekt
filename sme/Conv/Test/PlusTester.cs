using System;
using System.Threading.Tasks;
using SME;

namespace CNN
{
    // Works
    public class PlusTester : SimulationProcess
    {

        [InputBus]
        public ValueBus Input;

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
            Output.LastValue = true;
            await ClockAsync();
            Output.enable = false;
            while (!Input.enable) await ClockAsync();
            Console.WriteLine("Output after Plus " + Input.Value);
            await ClockAsync();
        }
    }
}