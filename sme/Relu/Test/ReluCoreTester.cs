using System;
using System.Threading.Tasks;
using SME;

namespace CNN
{
    // Works
    public class ReluCoreTester : SimulationProcess
    {

        [InputBus]
        public ValueBus Input;

        [OutputBus]        
        public ValueBus Output = Scope.CreateBus<ValueBus>();

        public async override System.Threading.Tasks.Task Run()
        {
            await ClockAsync();
            Output.enable = true;
            Output.Value = -1;
            await ClockAsync();
            Output.enable = false;
            Console.WriteLine("Output " + Output.Value + " and after Relu " + Input.Value);
            await ClockAsync();
        }
    }
}