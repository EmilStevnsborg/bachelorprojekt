using System;
using SME;
using SME.Components;
using static CNN.ChannelSizes;

namespace CNN
{
    class MainClass
    {
        public static void Main(string[] args)
        {

            using(var sim = new Simulation())
            {
                // SliceCtrl Test
                // SliceCtrl slicectrl = new SliceCtrl((4,4), (2,2), (2,2));
                // SliceTester sliceTester = new SliceTester();
                // slicectrl.Input = sliceTester.Output;
                // sliceTester.Input = slicectrl.Output;

                // // ReluCore Test
                // ReluCore reluCore = new ReluCore();
                // ReluCoreTester reluCoreTester = new ReluCoreTester();
                // reluCore.Input = reluCoreTester.Output;
                // reluCoreTester.Input = reluCore.Output;

                // // ReluCtrl Test
                // ReluCtrl reluCtrl = new ReluCtrl(2,3);
                // ReluCore reluCore = new ReluCore();
                // ReluTester reluTester = new ReluTester(2,3);
                // reluCtrl.Input = reluTester.Output;
                // reluCore.Input = reluCtrl.Output;
                // reluTester.Input = reluCore.Output;

                // // PlusCtrl Test
                // PlusCtrl plusCtrl = new PlusCtrl();
                // PlusTester plusTester = new PlusTester();
                // plusCtrl.Input = plusTester.Output;
                // plusTester.Input = plusCtrl.Output;

                // // WeightValue Test
                // WeightValue weiVal= new WeightValue();
                // WeightValueTester weiValTester = new WeightValueTester();
                // weiVal.InputValue = weiValTester.OutputValue;
                // weiVal.InputWeight = weiValTester.OutputWeight;
                // weiValTester.Input = weiVal.Output;

                // // KernelCtrl Test
                // var kernelCtrl = new KernelCtrl(2,3);
                // var kernelTester = new KernelCtrlTester(2,3);
                // var weights = new float[6] {1,2,1,2,1,2};
                // var ram = new TrueDualPortMemory<float>(STANDARD_SAFE_SIZE, weights);
                // kernelCtrl.Input = kernelTester.Output;
                // kernelCtrl.ram_ctrl = ram.ControlA;
                // kernelCtrl.ram_read = ram.ReadResultA;
                // kernelTester.InputValue = kernelCtrl.OutputValue;
                // kernelTester.InputWeight = kernelCtrl.OutputWeight;

                // // Bias Test
                // Bias bias = new Bias(2);
                // BiasTester biasTester = new BiasTester();
                // bias.Input = biasTester.Output;
                // biasTester.Input = bias.Output;

                // // Upsample Test 
                // UpSample upSample = new UpSample(2,2);
                // UpSampleTester upSampleTester = new UpSampleTester();
                // upSample.Input = upSampleTester.Output;
                // upSampleTester.Input = upSample.Output;

                // ConvKernel Test (doesn't work)
                var weights = new float[16];
                Array.Fill(weights, 2);
                ConvKernel convKernel = new ConvKernel(weights, 1, (4,4), (2,2), (2,2));
                ConvKernelTester convKernelTester = new ConvKernelTester();
                convKernel.Input = convKernelTester.Output;
                convKernelTester.Input = convKernel.Output;

                sim.Run();
            }

        }
    }
}
