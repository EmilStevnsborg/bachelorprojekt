using System;
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
        [OutputBus]
        public SliceBus Output = Scope.CreateBus<SliceBus>();
        private int channelHeight, channelWidth;
        private int strideRow, strideCol;
        private int sliceHeight, sliceWidth;
        private int startRow = 0, startCol = 0;
        bool sliceValid = false;
        bool bufferValid = false;
        private float [,] buffer;

        public SliceCtrl((int,int) channelSize, (int,int) sliceSize, (int,int) stride)
        {
            this.channelHeight = channelSize.Item1;
            this.channelWidth = channelSize.Item2;

            this.sliceHeight = sliceSize.Item1;
            this.sliceWidth = sliceSize.Item2;

            this.strideRow = stride.Item1;
            this.strideCol = stride.Item2;

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
                sliceValid = true;
            }
            Output.enable = sliceValid;
            if (bufferValid && sliceValid)
            {
                Output.Height = sliceHeight;
                Output.Width = sliceWidth;

                for (int ii = 0; ii < sliceHeight; ii++)
                {
                    for (int jj = 0; jj < sliceWidth; jj++)
                    {
                        Output.ArrData[ii*sliceWidth + jj] = buffer[startRow + ii, startCol + jj];
                    }
                }
                // increment start column index with the width of the stride column
                startCol = (startCol + strideCol) % channelWidth;
                // increment start column index with the stride row if columns have been traversed on that row
                startRow = startCol == 0 ? startRow + strideRow : startRow;
                // 
                sliceValid = (startRow < channelHeight);
            }
        }
    }
}