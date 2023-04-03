using SME;
using SME.Components;
using static CNN.ChannelSizes;
using static System.Math;

namespace CNN
{
    [ClockedProcess]
    // Wrapper class for instantiating the internal network, only exposing input
    // and output.
    public class ConvKernel
    {
        public ChannelBus Input
        {
            get => sliceCtrl.Input;
            set => sliceCtrl.Input = value;
        }

        public ChannelBus Output
        {
            get => upSample.Output;
            set => upSample.Output = value;
        }

        public ConvKernel(float[] weights, float biasVal, (int,int) channelSize, (int,int) kernelSize, (int,int) stride)
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
            sliceCtrl = new SliceCtrl(channelSize, kernelSize, stride);
            ram  = new TrueDualPortMemory<float>(kh*kw, weights);
            kernelCtrl = new KernelCtrl(kh, kw);
            weightValue = new WeightValue();
            plusCtrl = new PlusCtrl();
            bias = new Bias(biasVal);
            upSample = new UpSample(uh, uw);

            // Connect the buses
            kernelCtrl.Input = sliceCtrl.Output;
            kernelCtrl.ram_ctrl = ram.ControlA;
            kernelCtrl.ram_read = ram.ReadResultA;

            weightValue.InputValue = kernelCtrl.OutputValue;
            weightValue.InputWeight = kernelCtrl.OutputWeight;
            
            plusCtrl.Input = weightValue.Output;
            
            bias.Input = plusCtrl.Output;
            
            upSample.Input = bias.Output;
        }

        // Hold the internal processes as fields
        private SliceCtrl sliceCtrl;
        private TrueDualPortMemory<float> ram;
        private KernelCtrl kernelCtrl;
        private WeightValue weightValue;
        private PlusCtrl plusCtrl;
        private Bias bias;
        private UpSample upSample;
    }
}