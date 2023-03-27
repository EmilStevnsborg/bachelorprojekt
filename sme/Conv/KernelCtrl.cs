using SME;
using SME.Components;

namespace CNN
{
    public class KernelCtrl : SimpleProcess
    {
        [InputBus]
        public SliceBus Input;
        // Reads from port A
        [InputBus]
        public TrueDualPortMemory<float>.IReadResultA ram_read;
        [OutputBus]
        public ValueBus OutputValue = Scope.CreateBus<ValueBus>();
        [OutputBus]
        public ValueBus OutputWeight = Scope.CreateBus<ValueBus>();
        [OutputBus]
        public TrueDualPortMemory<float>.IControlA ram_ctrl;

        
        private int i, j, k;
        private int sliceHeight, sliceWidth;
        bool bufferValid = false;
        bool ramValid = false;
        private float [,] buffer;

        public KernelCtrl(int sliceHeight, int sliceWidth)
        {
            this.sliceHeight = sliceHeight;
            this.sliceWidth = sliceWidth;
            this.buffer = new float[sliceHeight,sliceWidth];
        }
        protected override void OnTick()
        {
            // Load values from bus into buffer.
            if (!bufferValid && Input.enable)
            {
                for (int ii = 0; ii < sliceHeight; ii++)
                {
                    for (int jj = 0; jj < sliceWidth; jj++)
                    {
                        buffer[ii,jj] = Input.ArrData[ii*sliceWidth + jj];
                    }
                }
                bufferValid = true;
                i = j = k = 0;
            }

            // If the buffer is filled, issue a read to the memory at every clock
            // cycle. When the data comes back from the memory, emit the output at
            // each clock cycle.
            if (bufferValid)
            {
                // Issue ram read
                ram_ctrl.Enabled = true;
                ram_ctrl.Address = k;
                ram_ctrl.IsWriting = false;
                ram_ctrl.Data = 0;

                // After two clock cycles, the results come back from memory.
                ramValid = ramValid || k >= 2;
                k = (k + 1) % sliceWidth;

                // If the results are back from memory, they can be forwarded along
                // side the image data.
                OutputValue.enable = OutputWeight.enable = ramValid;
                if (ramValid)
                {
                    OutputValue.Value = buffer[i,j];
                    OutputWeight.Value = ram_read.Data;
                    // Always increment column index.
                    j = (j + 1) % sliceWidth;
                    // Only increment row index when column have wrapped.
                    i = j == 0 ? (i + 1) % sliceHeight: i;
                    // Check if it is the last value in the slice
                    OutputValue.LastValue = !(i == sliceHeight-1 && j == sliceWidth-1);
                    // Check if we have processed the entire image.
                    bufferValid = !(i == 0 && j == 0);
                }
            }
        }
    }
}