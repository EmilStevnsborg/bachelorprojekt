using System.Collections.Generic;
using SME;

namespace CNN
{

    [ClockedProcess]
    public class ValueArrayCtrl : SimpleProcess
    {
        [InputBus]
        public ValueBus[] Input;
        [OutputBus]
        public ValueBus Output = Scope.CreateBus<ValueBus>();
        private int numInChannels;
        private int i = 0;
        private List<float> buffer = new List<float>();
        public ValueArrayCtrl(int numInChannels)
        {
            this.numInChannels = numInChannels;
        }
        protected override void OnTick()
        {
            Output.enable = Output.LastValue = false;
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

                Output.Value = buffer[i];
                Output.enable = true;
                i = i + 1;
                if (i % numInChannels == 0)
                {
                    Output.LastValue = true;
                }
                if (i == buffer.Count)
                {
                    buffer = new List<float>();
                    i = 0;
                }
            }
            
        }
    }
}