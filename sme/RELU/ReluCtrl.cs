using SME;
using SME.Components;
using static System.Math;

namespace CNN
{
    [ClockedProcess]
    public class ReluCtrl : SimpleProcess
    {

        [InputBus] 
        public ChannelBus Input;

        [OutputBus]
        public ValueBus Output = Scope.CreateBus<ValueBus>();

        private int i = 0;
        private int j = 0;
        private int channelHeight;
        private int channelWidth;
        bool bufferValid = false;
        private float [,] buffer;

        public ReluCtrl(int channelHeight, int channelWidth)
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
                i = j = 0;
            }

            Output.enable = bufferValid;

            // Send out one value at a time
            if (bufferValid)
            {
                Output.Value = buffer[i,j];
                // Always increment column index.
                j = (j + 1) % channelWidth;
                // Only increment row index when column have wrapped.
                i = j == 0 ? (i + 1) % channelHeight : i;
                // Check if we have processed the entire image.
                bufferValid = !(i == 0 && j == 0);
            }
        }
    }
}