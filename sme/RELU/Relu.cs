using SME;
using SME.Components;
using static System.Math;

namespace CNN
{
    public class Relu
    {

        // Connect the wrappers input to the input to ReluCtrl.
        public ChannelBus Input
        {
            get => ctrl.Input;
            set => ctrl.Input = value;
        }

        // Connect the wrappers output to the output of ReluCore.
        public ValueBus Output
        {
            get => core.Output;
            set => core.Output = value;
        }
        public Relu()
        {
            // Instantiate the processes.
            ctrl = new ReluCtrl(Input.Height, Input.Width);
            core = new ReluCore();
        }

        // Hold the internal processes as fields
        private ReluCtrl ctrl;
        private ReluCore core;

    }
}