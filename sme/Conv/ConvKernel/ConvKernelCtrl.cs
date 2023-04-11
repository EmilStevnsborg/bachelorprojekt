using System;
using SME;
using SME.Components;

namespace CNN
{
    [ClockedProcess]
    public class ConvKernelCtrl : SimpleProcess
    {
        [InputBus]
        public ChannelBus Input;
        [InputBus]
        public TrueDualPortMemory<float>.IReadResult ram_read;
        [OutputBus]
        public ValueBus OutputValue = Scope.CreateBus<ValueBus>();
        [OutputBus]
        public ValueBus OutputWeight = Scope.CreateBus<ValueBus>();
        [OutputBus]
        public TrueDualPortMemory<float>.IControl ram_ctrl;
        private int i, j, k, adress;
        private int channelHeight, channelWidth;
        private int padHeight, padWidth;
        private int kernelHeight, kernelWidth;
        private int strideRow, strideCol;
        private int startRow = 0, startCol = 0;
        private bool bufferValid = false;
        private bool ramValid = false;
        private float [,] buffer;
        private float padVal;

        public ConvKernelCtrl((int,int) channelSize, (int,int) kernelSize, (int,int) stride, (int,int) padding, float padVal)
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
                    i = j = k = adress = 0;
                }
                OutputValue.enable = OutputWeight.enable = OutputValue.LastValue = false;
            }

            // If the buffer is filled, issue a read to the memory at every clock
            // cycle. When the data comes back from the memory, emit the output at
            // each clock cycle.
            if (bufferValid)
            {
                // Issue ram read
                ram_ctrl.Enabled = true;
                ram_ctrl.Address = adress;
                ram_ctrl.IsWriting = false;
                ram_ctrl.Data = 0;

                // After two clock cycles, the results comes back from memory.
                ramValid = k >= 2;
                k = (k + 1);
                adress = k % (kernelHeight * kernelWidth);

                // If the results are back from memory, they can be forwarded along
                // side the Value data.
                OutputValue.enable = OutputWeight.enable = ramValid;

                if (ramValid)
                {
                    OutputValue.Value = buffer[startRow + i, startCol + j];
                    OutputWeight.Value = ram_read.Data;
                    // Always increment column index.
                    j = (j + 1) % kernelWidth;
                    // Only increment row index when column have wrapped.
                    i = j == 0 ? (i + 1) % kernelHeight: i;
                    // Check if the end index of the slice has been reached
                    OutputValue.LastValue = (i == 0 && j == 0);
                    if (i == 0 && j == 0)
                    {
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
            }
            // Check if we have processed the entire channel.
            if (startRow == channelHeight + 2 * padHeight)
            {
                bufferValid = ramValid = false;
            }
        }
    }
}