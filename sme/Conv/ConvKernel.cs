using SME;
using SME.Components;
using static CNN.ChannelSizes;
using static System.Math;

namespace CNN
{
    // Wrapper class for instantiating the internal network, only exposing input
    // and output.
    public class ConvKernel
    {
        public ChannelBus input
        {
            get => sliceCtrl.Input;
            set => sliceCtrl.Input = value;
        }

        public ValueBus output
        {
            get => plusCtrl.Output;
            set => plusCtrl.Output = value;
        }

        public ConvKernel(float[] weights = null) // add slice info as parameter
        {
            // Instantiate the processes
            
            ram  = new TrueDualPortMemory<float>(STANDARD_SAFE_SIZE, weights);

            // Connect the buses
        }

        // Hold the internal processes as fields
        private SliceCtrl sliceCtrl;
        private KernelCtrl kernelCtrl;
        private PlusCtrl plusCtrl;
        private TrueDualPortMemory<float> ram;
    }
}