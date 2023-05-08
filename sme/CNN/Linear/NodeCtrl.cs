using System;
using System.Collections.Generic;
using SME;
using SME.Components;

namespace CNN
{

    [ClockedProcess]
    public class NodeCtrl : SimpleProcess
    {
        [InputBus]
        public ValueBus[] Input;
        [InputBus]
        public TrueDualPortMemory<float>.IReadResult ram_read;
        [OutputBus]
        public ValueBus OutputValue = Scope.CreateBus<ValueBus>();
        [OutputBus]
        public ValueBus OutputWeight = Scope.CreateBus<ValueBus>();
        [OutputBus]
        public TrueDualPortMemory<float>.IControl ram_ctrl;
        private int numInChannels;
        private int channelHeight;
        private int channelWidth;
        private int x = 0, i = 0, j = 0, k = 0, adress = 0;
        private float[] buffer;
        private bool ramValid = false;

        public NodeCtrl(int numInChannels, (int,int) channelSize)
        {
            this.numInChannels = numInChannels;
            this.channelHeight = channelSize.Item1;
            this.channelWidth = channelSize.Item2;
            buffer = new float[numInChannels * channelHeight * channelWidth];
        }
        protected override void OnTick()
        {
            // Output should only be updated when the input is valid.
            if (Input.Length > 0)
            {
                for (int ii = 0; ii < numInChannels; ii++)
                {
                    if (Input[ii].enable)
                    {
                        buffer[x] = Input[ii].Value;
                        x = x + 1;
                    }
                }
            }
            OutputValue.enable = OutputWeight.enable = OutputValue.LastValue = false;
            // remember to toss buffer after done (check)
            if (x > 0 && i < x)
            {
                // Issue ram read
                ram_ctrl.Enabled = true;
                ram_ctrl.Address = adress;
                ram_ctrl.IsWriting = false;
                ram_ctrl.Data = 0;

                // After two clock cycles, the results comes back from memory.
                ramValid = k >= 2;
                k = (k + 1);
                // j controls which index in a channel ram selects
                j = k % numInChannels == 0 ? (j + 1) : j;
                // k will keep incrementing and j will be reset
                adress = (k % numInChannels) * (channelHeight * channelWidth) + j;

                // If the results are back from memory, they can be forwarded along
                // side the Value data.
                OutputValue.enable = OutputWeight.enable = ramValid;

                if (ramValid)
                {
                    OutputValue.Value = buffer[i];
                    OutputWeight.Value = ram_read.Data;

                    i = i + 1;

                    // buffer is done
                    if (i == x)
                    {
                        OutputValue.LastValue = true;
                        ramValid = false;
                        x = i = k = j = adress = 0;
                    }
                }
            }
            
        }
    }
}