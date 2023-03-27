using System;
using System.Threading.Tasks;
using SME;

namespace CNN
{
    public class SliceTester : SimulationProcess
    {
        [InputBus]
        public SliceBus Input;
        [OutputBus]
        public ChannelBus Output = Scope.CreateBus<ChannelBus>();
        [OutputBus]
        public SliceInfoBus SliceInfo = Scope.CreateBus<SliceInfoBus>();

        public override async Task Run()
        {
            await ClockAsync();
            Output.enable = true;
            SliceInfo.enable = true;
            
            // Pack test data onto bus
            for (int i = 0; i < 6; i++)
            {
                Output.ArrData[i] = (float) i;
            }
            Output.Height = 2;
            Output.Width = 3;
            // Slice info
            SliceInfo.startRow = 0;
            SliceInfo.startCol = 0;
            SliceInfo.endRow = 2;
            SliceInfo.endCol = 2;
            await ClockAsync();
            // Data shouldn't be read again
            Output.enable = false;
            SliceInfo.enable = false;
            await ClockAsync();

            if (Input.enable) 
            {
                var ih = Input.Height;
                var iw = Input.Width;
                for (int i = 0; i < ih; i++)
                {
                    for (int j = 0; j < iw; j++)
                    {
                        Console.Write(Input.ArrData[i*iw + j]);
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}