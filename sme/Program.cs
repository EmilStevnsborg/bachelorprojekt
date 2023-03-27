using System;
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

                // SliceCtrl slicectrl = new SliceCtrl(2,3);
                // SliceTester sliceTester = new SliceTester();
                // slicectrl.Input = sliceTester.Output;
                // slicectrl.SliceInfo = sliceTester.SliceInfo;
                // sliceTester.Input = slicectrl.Output;

                // // ReluCore test
                // ReluCore reluCore = new ReluCore();
                // ReluCoreTester reluCoreTester = new ReluCoreTester();
                // reluCore.Input = reluCoreTester.Output;
                // reluCoreTester.Input = reluCore.Output;

                // // ReluCtrl test
                // ReluCtrl reluCtrl = new ReluCtrl(2,3);
                // ReluCore reluCore = new ReluCore();
                // ReluTester reluTester = new ReluTester(2,3);
                // reluCtrl.Input = reluTester.Output;
                // reluCore.Input = reluCtrl.Output;
                // reluTester.Input = reluCore.Output;

                // PlusCtrl Test
                PlusCtrl plusCtrl = new PlusCtrl();
                PlusTester plusTester = new PlusTester();
                plusCtrl.Input = plusTester.Output;
                plusTester.Input = plusCtrl.Output;

                // WeightValue Test
                WeightValue weiVal= new WeightValue();
                WeightValueTester weiValTester = new WeightValueTester();
                weiVal.InputValue = weiValTester.OutputValue;
                weiVal.InputWeight = weiValTester.OutputWeight;
                weiValTester.Input = weiVal.Output;

                sim.Run();
            }

        }
    }
}
