using System.Diagnostics;
using System.Threading.Tasks;
using SME;

namespace CNN 
{
    /// <summary>
    /// Helper process that simulates a conv process.
    /// Since this is a simulation process, it will not be rendered as hardware
    /// and we can use any code and dynamic properties we want
    /// </summary>
    public class ConvTester : SimulationProcess
    {
    [InputBus, OutputBus]
    private readonly Conv.IInputBatch InputBatch = Scope.CreateOrLoadBus<Conv.IInputBatch>();
    [InputBus]
    private readonly Conv.IResultBatch ResultBatch = Scope.CreateOrLoadBus<Conv.IResultBatch>();

        public override async Task Run()
        {
            await ClockAsync();
        }
    }
}