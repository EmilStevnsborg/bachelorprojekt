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
        private int padHeight, padWidth;
        private int kernelHeight, kernelWidth;
        private int strideRow, strideCol;
        private int startRow = 0, startCol = 0;
        bool bufferValid = false;
        private float [,] buffer;
        private float padVal;

        public PoolKernelCtrl((int,int) channelSize, (int,int) kernelSize, (int,int) stride, (int,int) padding, float padVal)
        {
            this.padHeight = padding.Item1;
            this.padWidth = padding.Item2;   

            this.channelHeight = channelSize.Item1;
            this.channelWidth = channelSize.Item2;

            this.kernelHeight = kernelSize.Item1;
            this.kernelWidth = kernelSize.Item2;

            this.strideRow = stride.Item1;
            this.strideCol = stride.Item2;

            this.buffer = new float[channelHeight + 2 * padHeight,channelWidth + 2 * padWidth];

            this.padVal = padVal;

            // fill in padding
            Helper.Padding(ref buffer, channelHeight, channelWidth, padHeight, padWidth, padVal);
        }
        protected override void OnTick()
        {
            // No buffer loaded
            if (!bufferValid)
            {
                // Load Input into buffer
                if (Input.enable)
                {
                    for (int ii = 0; ii < channelHeight; ii++)
                    {
                        for (int jj = 0; jj < channelWidth; jj++)
                        {
                            buffer[ii+padHeight,jj+padWidth] = Input.ArrData[ii*channelWidth + jj];
                        }
                    }
                    bufferValid = true;
                }
                OutputValue.enable = OutputValue.LastValue = false;
            }

            // If the buffer is filled, emit the output at each clock cycle.
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
                    if (startCol + strideCol == channelWidth + 2 * padWidth)
                    {
                        startCol = 0;
                        startRow = startRow + strideRow;
                    }
                    else
                    {
                        startCol = startCol + strideCol;
                    }
                }
            }
            // Check if we have processed the entire channel.
            if (startRow == channelHeight + 2 * padHeight)
            {
                bufferValid = false;
            }
        }
    }
}