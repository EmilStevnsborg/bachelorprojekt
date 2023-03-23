using SME;
using SME.Components;
using static System.Math;

namespace CNN
{
    [ClockedProcess]
    public class SliceCtrl : SimpleProcess
    {
        [InputBus] 
        public ChannelBus Input;
        // This bus ONLY tells something about the slice
        // It is a product of kernel size and stride determined in a higher level conv class
        [InputBus]
        public SliceInfoBus SliceInfo;
        [OutputBus]
        public SliceBus Output = Scope.CreateBus<SliceBus>();

        private int channelHeight;
        private int channelWidth;
        bool bufferValid = false;
        private float [,] buffer;

        public SliceCtrl(int channelHeight, int channelWidth)
        {
            this.channelHeight = channelHeight;
            this.channelWidth = channelWidth;
            this.buffer = new float[channelHeight,channelWidth];
        }


        protected override void OnTick()
        {
            // Load the bus into the buffer
            if (!bufferValid && Input.enable)
            {
                for (int ii = 0; ii < channelHeight; ii++)
                {
                    for (int jj = 0; jj < channelWidth; jj++)
                    {
                        buffer[ii,jj] = Input.ArrData[ii*channelWidth+jj];
                    }
                }
                // buffer is now loaded fully
                bufferValid = true;
            }
            Output.enable = bufferValid;
            if (bufferValid && SliceInfo.enable)
            {
                int h = SliceInfo.Data[2]-SliceInfo.Data[0];
                int w = SliceInfo.Data[3]-SliceInfo.Data[1];

                Output.Height = h;
                Output.Width = w;

                for (int ii = 0; ii < h; ii++)
                {
                    for (int jj = 0; jj < w; jj++)
                    {
                        Output.ArrData[ii*h + jj] = buffer[SliceInfo.Data[0]+ii, SliceInfo.Data[1]+jj];
                    }
                }
            }
        }
    }
}