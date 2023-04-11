using System;
using SME;

namespace CNN
{

    [ClockedProcess]
    public class UpSample : SimpleProcess
    {
        [InputBus]
        public ValueBus Input;
        [OutputBus]
        public ChannelBus Output = Scope.CreateBus<ChannelBus>();
        private int i = 0, j = 0;
        private int UpSampleHeight, UpSampleWidth;
        bool bufferValid = false;
        private float [,] buffer;
        public UpSample(int UpSampleHeight, int UpSampleWidth)
        {
            this.UpSampleHeight = UpSampleHeight;
            this.UpSampleWidth = UpSampleWidth;
            this.buffer = new float[UpSampleHeight,UpSampleWidth];
        }
        protected override void OnTick()
        {
            Output.Height = UpSampleHeight;
            Output.Width = UpSampleWidth;

            if (Input.enable)
            {
                buffer[i,j] = Input.Value;
                // Always increment column index.
                j = (j + 1) % UpSampleWidth;
                // Only increment row index when column have wrapped.
                i = j == 0 ? (i + 1) % UpSampleHeight: i;
                // Check if we have processed the entire channel.
                bufferValid = (i == 0 && j == 0);
            }
            // Console.WriteLine(bufferValid);
            if (bufferValid)
            {
                for (int ii = 0; ii < UpSampleHeight; ii++)
                {
                    for (int jj = 0; jj < UpSampleWidth; jj++)
                    {
                        Output.ArrData[ii*UpSampleWidth + jj] = buffer[ii,jj];
                    }
                }
            }
            Output.enable = bufferValid;
        }
    }
}