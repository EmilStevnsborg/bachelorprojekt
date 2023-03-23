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

                // Says Input is null
                // ReluCtrl reluCtrl = new ReluCtrl(2,3);
                ReluCore reluCore = new ReluCore();
                // ReluTester reluTester = new ReluTester();
                ReluCoreTester reluCoreTester = new ReluCoreTester();             
                // Connect the buses.
                // reluCtrl.Input = reluTester.Output;
                reluCore.Input = reluCoreTester.Output;
                reluCoreTester.Input = reluCore.Output;
                // reluTester.Input = reluCore.Output;

                sim.Run();
            }

        }
    }
}
