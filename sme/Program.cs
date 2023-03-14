using SME;
using SME.Components;

namespace CNN
{
    class MainClass
    {
        public static void Main(string[] args)
        {

            using(var sim = new Simulation())
            {
                // !!! RELU TEST !!!
                // var reluTester = new RELUTester();
                // var ctrl = new Ctrl(2,2);
                // var relu = new RELU();

                // ctrl.Input = reluTester.Input;
                // relu.Input = ctrl.Output;
                //
                // reluTester.Result.Data[x] = relu.Output.Value;

                // !!! CONVKERNEL TEST !!!
                var convKernelTester = new ConvKernelTester(2,2);
                var ctrl = new Ctrl(2,2);
                var weightPixel = new WeightPixel();
                var weightRam = new TrueDualPortMemory<double>(2*2);

                ctrl.Input = convKernelTester.Input;
                weightPixel.Input = ctrl.Output;

                convKernelTester.weightRamctrl = weightRam.ControlA;
                weightPixel.weight = weightRam.ReadResultA;

                // I want to create a simple process (ConvKernel) which uses weightPixel
                // in such a way that it can take a Channel input and produce a channel output

                sim
                .Run();
            }

        }
    }
}
