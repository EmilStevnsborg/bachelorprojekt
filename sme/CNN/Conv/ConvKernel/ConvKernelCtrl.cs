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
        private int ii = 0, jj = 0, i, j, k, kernelA, kernelB;
        private int channelHeight, channelWidth;
        private int padHeight, padWidth;
        private int newHeight, newWidth;
        private int kernelHeight, kernelWidth;
        private int strideRow, strideCol;
        private int startRow = 0, startCol = 0;
        private bool bufferValid = false;
        private bool ramValid = false;
        private bool loaded = false;
        private bool wholeChannel = false;
        private bool lastKernelVal = false;
        private float [] weights;

        public ConvKernelCtrl((int,int) channelSize, (int,int) kernelSize, (int,int) stride, (int,int) padding, float[] weights)
        {
            this.channelHeight = channelSize.Item1;
            this.channelWidth = channelSize.Item2;

            this.padHeight = padding.Item1;
            this.padWidth = padding.Item2;

            this.newHeight = channelHeight + 2 * padHeight;
            this.newWidth = channelWidth + 2 * padWidth;

            this.kernelHeight = kernelSize.Item1;
            this.kernelWidth = kernelSize.Item2;

            this.strideRow = stride.Item1;
            this.strideCol = stride.Item2;

            this.weights = weights;
        }
        protected override void OnTick()
        {
            OutputValueA.enable = OutputWeightA.enable = OutputValueA.LastValue = false;
            OutputValueB.enable = OutputWeightB.enable = OutputValueB.LastValue = false;
            // No buffer loaded
            if (!bufferValid)
            {
                // Takes one tick after buffer loaded
                if (loaded)
                {
                    bufferValid = true;
                    loaded = false;
                    i = j = k = kernelA = 0;
                    kernelB = 1;
                }
                // Load Input into buffer
                if (Input.enable)
                {
                    // Console.WriteLine((ii + padHeight) * (channelWidth + 2 * padWidth) + (jj + padWidth) + Input.Value);
                    // Issue ram write
                    ram_ctrlA.Enabled = true;
                    ram_ctrlA.Address = (ii + padHeight) * newWidth + (jj + padWidth);
                    ram_ctrlA.IsWriting = true;
                    ram_ctrlA.Data = Input.Value;
                    // Always increment column index.
                    jj = (jj + 1) % channelWidth;
                    // Only increment row index when column have wrapped.
                    ii = jj == 0 ? (ii + 1) % channelHeight: ii;
                    // Whole channels has been read
                    loaded = (ii == 0 && jj == 0);
                }
            }
            // If the buffer is filled, issue a read to the memory at every clock
            // cycle. When the data comes back from the memory, emit the output at
            // each clock cycle.
            if (bufferValid)
            {
                int bufferA = (startRow + i) * newWidth + (startCol + j);
                
                // Issue ram read of buffer from first adress
                ram_ctrlA.Enabled = !wholeChannel;
                ram_ctrlA.Address = bufferA;
                ram_ctrlA.IsWriting = false;
                ram_ctrlA.Data = 0;

                // check if bufferA read is the index in whole channel
                wholeChannel = !wholeChannel ? (bufferA + 1 == newWidth * newHeight) : true;


                var w = (j + 1) % kernelWidth;
                var h = w == 0 ? i + 1 : i;
                int bufferB = (startRow + h) * newWidth + (startCol + w);

                // Issue ram read of buffer from second adress
                ram_ctrlB.Enabled = !wholeChannel;
                ram_ctrlB.Address = bufferB < newHeight * newWidth ? bufferB : 0; // Avoid last index error
                ram_ctrlB.IsWriting = false;
                ram_ctrlB.Data = 0;

                // if whole channel has been processed else check if bufferB read is the last index
                wholeChannel = !wholeChannel ? (bufferB + 1 == newWidth * newHeight) : true;

                // After two clock cycles, the results come back from memory.
                ramValid = k >= 2;
                k = (k + 1);

                // Channel hasn't been processed
                if (!wholeChannel)
                {
                    // skip to next row in kernel or shift kernel
                    if (j + 2 >= kernelWidth)
                    {
                        // shift kernel
                        if (i + 1 == kernelHeight)
                        {
                            j = i = 0;
                            // shift kernel by stride on rows
                            if (startCol + kernelWidth == newWidth)
                            {
                                startCol = 0;
                                startRow = startRow + strideRow;
                            }
                            // shift kernel by stride on columns
                            else
                            {
                                startCol = startCol + strideCol;
                            }
                        }
                        // go to next row in kernel
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
                }
                else
                {
                    ii += 1;
                }

                if (ramValid)
                {
                    OutputValueA.Value = ram_readA.Data;
                    OutputWeightA.Value = weights[kernelA];

                    // last index in kernel
                    if (kernelA > kernelB)
                    {
                        OutputValueB.Value = 0;
                        OutputWeightB.Value = 0;
                    }
                    else
                    {
                        OutputValueB.Value = ram_readB.Data ;
                        OutputWeightB.Value = weights[kernelB];
                    }

                    //Is this the last kernel value
                    lastKernelVal = (kernelA + 1 == kernelHeight*kernelWidth) || (kernelB + 1 == kernelHeight*kernelWidth);
                    OutputValueA.LastValue = lastKernelVal;

                    // Console.WriteLine(bufferA + " " + bufferB + " " + kernelA + " " + kernelB + " " + lastKernelVal + " " + ii);

                    OutputValueA.enable = OutputWeightA.enable = true;
                    OutputValueB.enable = OutputWeightB.enable = true;
                    
                    // Check if all data has been processed.
                    if (lastKernelVal && ii == 2)
                    {
                        bufferValid = ramValid = wholeChannel = lastKernelVal = false;
                        i = j = ii = k = startRow = startCol = kernelA = 0;
                        kernelB = 1;
                    }
                    else
                    {
                        // weight adress
                        kernelA = (kernelA + 2) % (kernelHeight * kernelWidth);
                        if (kernelB == 0)
                        {
                            kernelA = 0;
                        }
                        kernelB = (kernelA + 1) % (kernelHeight * kernelWidth);
                    }
                }
            }
        }
    }
}