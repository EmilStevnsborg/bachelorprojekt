using SME;
using SME.Components;

namespace CNN
{
    [ClockedProcess]
    public class ConvKernel
    {
        public ValueBus Input
        {
            get => kernelCtrl.Input;
            set => kernelCtrl.Input = value;
        }

        public ValueBus Output
        {
            get => plusCtrl.Output;
            set => plusCtrl.Output = value;
        }

        public ConvKernel(float[] weights, (int,int) channelSize, (int,int) kernelSize, (int,int) stride, (int,int) padding, float padVal)
        {
            // channel input
            var ch = channelSize.Item1;
            var cw = channelSize.Item1;

            // kernel (slice height and width)
            var kh = kernelSize.Item1;
            var kw = kernelSize.Item1;

            // stride
            var sr = stride.Item1;
            var sc = stride.Item1;

            // upsample channel output
            var uh = ch - (kh-1) - (sr-1);
            var uw = cw - (kw-1) - (sc-1);

            // Instantiate the processes
            ram  = new TrueDualPortMemory<float>(kh*kw, weights);
            kernelCtrl = new ConvKernelCtrl(channelSize, kernelSize, stride, padding, padVal);
            weightValue = new WeightValue();
            plusCtrl = new PlusCtrl();

            // Connect the buses
            kernelCtrl.ram_ctrl = ram.ControlA;
            kernelCtrl.ram_read = ram.ReadResultA;

            weightValue.InputValue = kernelCtrl.OutputValue;
            weightValue.InputWeight = kernelCtrl.OutputWeight;
            
            plusCtrl.Input = weightValue.Output;
        }

        // Hold the internal processes as fields
        private TrueDualPortMemory<float> ram;
        private ConvKernelCtrl kernelCtrl;
        private WeightValue weightValue;
        private PlusCtrl plusCtrl;
    }
}