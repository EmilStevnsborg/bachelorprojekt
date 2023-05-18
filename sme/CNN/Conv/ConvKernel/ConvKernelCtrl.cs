using System;
using SME;
using SME.Components;

namespace CNN
{
    [ClockedProcess]
    public class ConvKernelCtrl : SimpleProcess
    {
        [InputBus]
        public ValueBus Input;
        [InputBus]
        public TrueDualPortMemory<float>.IReadResult ram_readA;
        [InputBus]
        public TrueDualPortMemory<float>.IReadResult ram_readB;
        [OutputBus]
        public ValueBus OutputValueA = Scope.CreateBus<ValueBus>();
        [OutputBus]
        public ValueBus OutputWeightA = Scope.CreateBus<ValueBus>();
        [OutputBus]
        public ValueBus OutputValueB = Scope.CreateBus<ValueBus>();
        [OutputBus]
        public ValueBus OutputWeightB = Scope.CreateBus<ValueBus>();
        [OutputBus]
        public TrueDualPortMemory<float>.IControl ram_ctrlA;
        [OutputBus]
        public TrueDualPortMemory<float>.IControl ram_ctrlB;
        private int ii = 0, jj = 0, i, j, k, adressA, adressB;
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
            // Console.WriteLine((channelHeight + 2 * padHeight) + " " + (channelWidth + 2 * padWidth));

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
                    buffer[ii+padHeight,jj+padWidth] = Input.Value;

                    // Always increment column index.
                    jj = (jj + 1) % channelWidth;
                    // Only increment row index when column have wrapped.
                    ii = jj == 0 ? (ii + 1) % channelHeight: ii;
                    // Whole channels has been read
                    if (ii == 0 && jj == 0)
                    {
                        bufferValid = true;
                        i = j = k = adressA = 0;
                        adressB = 1;
                    }
                }
            }
            OutputValueA.enable = OutputWeightA.enable = OutputValueA.LastValue = false;
            OutputValueB.enable = OutputWeightB.enable = OutputValueB.LastValue = false;
            // If the buffer is filled, issue a read to the memory at every clock
            // cycle. When the data comes back from the memory, emit the output at
            // each clock cycle.
            if (bufferValid)
            {
                // Issue ram read
                ram_ctrlA.Enabled = true;
                ram_ctrlA.Address = adressA;
                ram_ctrlA.IsWriting = false;
                ram_ctrlA.Data = 0;

                ram_ctrlB.Enabled = true;
                ram_ctrlB.Address = adressB;
                ram_ctrlB.IsWriting = false;
                ram_ctrlB.Data = 0;

                // After two clock cycles, the results come back from memory.
                ramValid = k >= 2;
                k = (k + 1);

                // Console.WriteLine("A: " + adressA);
                // Console.WriteLine("B: " + adressB);

                adressA = (adressA + 2) % (kernelHeight * kernelWidth);
                if (adressB == 0)
                {
                    adressA = 0;
                }
                adressB = (adressA + 1) % (kernelHeight * kernelWidth);

                if (ramValid)
                {
                    // Console.WriteLine("startRow " + startRow + " i: " + (i) + ", startCol " + startCol + " j : " + (j));
                    OutputValueA.Value = buffer[startRow + i, startCol + j];
                    OutputWeightA.Value = ram_readA.Data;

                    // last index last row
                    if ((i + 1) % kernelHeight == 0 && (j + 1) % kernelWidth == 0)
                    {
                        OutputValueB.Value = 0;
                        OutputWeightB.Value = 0;
                    }
                    else
                    {
                        var w = (j + 1) % kernelWidth == 0 ? 0 : j + 1;
                        var h = w == 0 ? i + 1 : i;
                        OutputValueB.Value = buffer[startRow + h, startCol + w];
                        OutputWeightB.Value = ram_readB.Data;
                    }

                    if (j + 2 >= kernelWidth)
                    {
                        if (i + 1 == kernelHeight)
                        {
                            j = 0;
                            i = 0;
                        }
                        else
                        {
                            j = (j + 2) % kernelWidth;
                            i += 1;
                        }
                    }
                    else
                    {
                        j = (j + 2);
                    }

                    // Check if the end index of the slice has been reached
                    OutputValueA.LastValue = (i == 0 && j == 0);

                    OutputValueA.enable = OutputWeightA.enable = ramValid;
                    OutputValueB.enable = OutputWeightB.enable = ramValid;

                    if (i == 0 && j == 0)
                    {
                        if (startCol + kernelWidth == channelWidth + 2 * padWidth)
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
            if (startRow + i == channelHeight + 2 * padHeight)
            {
                bufferValid = ramValid = false;
            }
        }
    }
}