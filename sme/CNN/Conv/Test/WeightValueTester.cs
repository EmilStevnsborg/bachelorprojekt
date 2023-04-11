using System;
using System.Threading.Tasks;
using SME;

namespace CNN
{
    [ClockedProcess]
    public class WeightValueTester : SimulationProcess
    {

        [InputBus]
        public ValueBus Input;

        [OutputBus]        
        public ValueBus OutputValue = Scope.CreateBus<ValueBus>();
        [OutputBus]        
        public ValueBus OutputWeight = Scope.CreateBus<ValueBus>();

        public async override System.Threading.Tasks.Task Run()
        {
            await ClockAsync();
            OutputValue.enable = OutputWeight.enable = true;
            OutputValue.Value = -2;
            OutputWeight.Value = -2;
            await ClockAsync();
            OutputValue.enable = OutputWeight.enable = false;
            while(!Input.enable) await ClockAsync();
            Console.WriteLine(OutputValue.Value + " * " + OutputWeight.Value + " = " + Input.Value);
            await ClockAsync();
        }
    }
}