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
        private int i = 0, j = 0, k = 0, adress = 0;
        private List<float> buffer = new List<float>();
        private bool ramValid = false;

        public NodeCtrl(int numInChannels, (int,int) channelSize)
        {
            this.numInChannels = numInChannels;
            this.channelHeight = channelSize.Item1;
            this.channelWidth = channelSize.Item2;
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
                        buffer.Add(Input[ii].Value);
                    }
                }
            }
            // remember to toss buffer after done (check)
            if (buffer.Count > 0 && i < buffer.Count)
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
                    if (i % (numInChannels * channelHeight * channelWidth) == 0)
                    {
                        OutputValue.LastValue = true;
                        buffer = new List<float>();
                        ramValid = false;
                        i = k = j = adress = 0;
                    }
                }
            }
            
        }
    }
}