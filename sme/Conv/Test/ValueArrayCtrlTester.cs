using System;
using System.Threading.Tasks;
using SME;

namespace CNN
{
    [ClockedProcess]
    public class ValueArrayCtrlTester : SimulationProcess
    {
        [InputBus]
        public ValueBus Input;
        [OutputBus]
        public ValueBus[] Outputs;

        public ValueArrayCtrlTester()
        {
            Outputs = new ValueBus[3];
            Outputs[0] = Scope.CreateBus<ValueBus>();
            Outputs[1] = Scope.CreateBus<ValueBus>();
            Outputs[2] = Scope.CreateBus<ValueBus>();
        }

        public override async Task Run()
        {
            await ClockAsync();
            for (int i = 0; i < 3; i++)
            {
                Outputs[i].enable = true;
                Outputs[i].Value = i;
            }
            Outputs[2].LastValue = true;
            await ClockAsync();
            for (int i = 0; i < 3; i++)
            {
                Outputs[i].enable = true;
                Outputs[i].Value = i*2;
            }
            Outputs[2].LastValue = true;
            await ClockAsync();
            for (int i = 0; i < 3; i++)
            {
                Outputs[i].enable = false;
                Outputs[i].LastValue = false;
            }
            await ClockAsync();
            Console.WriteLine("out val: " + Input.Value + ", enable: " + Input.enable + ", last: " + Input.LastValue);
            await ClockAsync();
            Console.WriteLine("out val: " + Input.Value + ", enable: " + Input.enable + ", last: " + Input.LastValue);
            await ClockAsync();
            Console.WriteLine("out val: " + Input.Value + ", enable: " + Input.enable + ", last: " + Input.LastValue);
            await ClockAsync();
            Console.WriteLine("out val: " + Input.Value + ", enable: " + Input.enable + ", last: " + Input.LastValue);
            await ClockAsync();
            Console.WriteLine("out val: " + Input.Value + ", enable: " + Input.enable + ", last: " + Input.LastValue);
            await ClockAsync();
            Console.WriteLine("out val: " + Input.Value + ", enable: " + Input.enable + ", last: " + Input.LastValue);
            await ClockAsync();
            Console.WriteLine("out val: " + Input.Value + ", enable: " + Input.enable + ", last: " + Input.LastValue);
            await ClockAsync();
            Console.WriteLine("out val: " + Input.Value + ", enable: " + Input.enable + ", last: " + Input.LastValue);
            await ClockAsync();
            Console.WriteLine("out val: " + Input.Value + ", enable: " + Input.enable + ", last: " + Input.LastValue);
            
        }
    }
}