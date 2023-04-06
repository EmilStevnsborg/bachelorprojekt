﻿using System;
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
                // var kernelCtrl = new KernelCtrl((4,4),(2,2),(2,2));
                // var kernelTester = new KernelCtrlTester(4,4);
                // var weights = new float[4] {0,1,2,3};
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

                // // ConvKernel Test
                // var weights = new float[4] {1,0,0,1};
                // // print weights
                // Console.WriteLine("Test weights: ");
                // for (int i = 0; i < 4; i++)
                // {
                //     Console.Write(weights[i] + " ");
                //     if ((i + 1) % 2 == 0) {Console.WriteLine();}
                // }
                // ConvKernel convKernel = new ConvKernel(weights, (4,4), (2,2), (2,2));
                // ConvKernelTester convKernelTester = new ConvKernelTester();
                // convKernel.Input = convKernelTester.Output;
                // convKernelTester.Input = convKernel.Output;

                // // MaxPoolKernel Test
                // var weights = new float[4] {1,2,3,4};
                // MaxPoolKernel maxPoolKernel = new MaxPoolKernel((4,4), (2,2), (2,2));
                // MaxPoolKernelTester maxPoolKernelTester = new MaxPoolKernelTester();
                // maxPoolKernel.Input = maxPoolKernelTester.Output;
                // maxPoolKernelTester.Input = maxPoolKernel.Output;

                // // ValueArrayCtrl Test
                // ValueArrayCtrl valueArrayCtrl = new ValueArrayCtrl(3);
                // PlusCtrl plusCtrl = new PlusCtrl();
                // Bias bias = new Bias(1);
                // ValueArrayCtrlTester tester = new ValueArrayCtrlTester();
                // valueArrayCtrl.Input = tester.Outputs;
                // plusCtrl.Input = valueArrayCtrl.Output;
                // bias.Input = plusCtrl.Output;
                // tester.Input = bias.Output;

                // Filter Test (Looks only at channel 2 for some reason)
                float[][] weights = { new float[4] {1,0,3,2}, new float[4] {0,4,0,1}};
                for (int k = 0; k < 2; k++)
                {
                    Console.WriteLine("weights " + (k + 1));
                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            Console.Write(weights[k][i * 2 + j] + " ");
                        }
                        Console.WriteLine();
                    }
                }
                Filter filter = new Filter(2, weights, 1, (4,4), (2,2), (2,2));
                ValueArrayCtrl valueArrayCtrl = new ValueArrayCtrl(2);
                PlusCtrl plusCtrl = new PlusCtrl();
                Bias bias = new Bias(1);
                FilterTester filterTester = new FilterTester();
                filter.Inputs = filterTester.Outputs;
                filter.PushInputs();
                valueArrayCtrl.Input = filter.Output;
                plusCtrl.Input = valueArrayCtrl.Output;
                bias.Input = plusCtrl.Output;
                filterTester.Input = bias.Output;

                sim.Run();
            }

        }
    }
}
