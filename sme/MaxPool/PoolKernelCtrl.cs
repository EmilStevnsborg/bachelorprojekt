using System;
using SME;
using SME.Components;

namespace CNN
{
    [ClockedProcess]
    public class PoolKernelCtrl : SimpleProcess
    {
        [InputBus]
        public ChannelBus Input;
        [OutputBus]
        public ValueBus OutputValue = Scope.CreateBus<ValueBus>();

        private int i, j;
        private int channelHeight, channelWidth;
        private int strideRow, strideCol;
        private int kernelHeight, kernelWidth;
        private int startRow = 0, startCol = 0;
        bool bufferValid = false;
        private float [,] buffer;

        public PoolKernelCtrl((int,int) channelSize, (int,int) kernelSize, (int,int) stride)
        {
            this.channelHeight = channelSize.Item1;
            this.channelWidth = channelSize.Item2;

            this.kernelHeight = kernelSize.Item1;
            this.kernelWidth = kernelSize.Item2;

            this.strideRow = stride.Item1;
            this.strideCol = stride.Item2;

            this.buffer = new float[channelHeight,channelWidth];
        }
        protected override void OnTick()
        {
            // Load values from bus into buffer.
            if (!bufferValid && Input.enable)
            {
                for (int ii = 0; ii < channelHeight; ii++)
                {
                    for (int jj = 0; jj < channelWidth; jj++)
                    {
                        buffer[ii,jj] = Input.ArrData[ii*channelWidth + jj];
                    }
                }
                bufferValid = true;
                i = j = 0;
            }

            // If the buffer is filled, issue a read to the memory at every clock
            // cycle. When the data comes back from the memory, emit the output at
            // each clock cycle.
            if (bufferValid)
            {                
                OutputValue.enable = bufferValid;
                OutputValue.Value = buffer[startRow + i, startCol + j];
                OutputValue.LastValue = false;
                // Always increment column index.
                j = (j + 1) % kernelWidth;
                // Only increment row index when column have wrapped.
                i = j == 0 ? (i + 1) % kernelHeight: i;
                // Check if the end index of the slice has been reached
                if (i == 0 && j == 0)
                {
                    OutputValue.LastValue = true;
                    if (startCol + strideCol == channelWidth)
                    {
                        startCol = 0;
                        startRow = startRow + strideRow;
                    }
                    else
                    {
                        startCol = startCol + strideCol;
                    }
                }
                // Check if we have processed the entire channel.
                if (startRow == channelHeight)
                {
                    bufferValid = false;
                }
            }
        }
    }
}