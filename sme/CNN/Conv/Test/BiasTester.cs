using System;
using SME;

namespace CNN
{
    // Works
    public class BiasTester : SimulationProcess
    {

        [InputBus]
        public ValueBus Input;

        [OutputBus]        
        public ValueBus Output = Scope.CreateBus<ValueBus>();

        public async override System.Threading.Tasks.Task Run()
        {
            await ClockAsync();
            Output.enable = true;
            Output.Value = 2;
            await ClockAsync();
            Output.enable = false;
            while (!Input.enable) await ClockAsync();
            Console.WriteLine("Output after Bias " + Input.Value);
            await ClockAsync();
        }
    }
}