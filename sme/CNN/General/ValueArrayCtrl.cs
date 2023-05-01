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
        private int numInChannels, channelHeight, channelWidth;
        private int i = 0, k = 0;
        private float[] buffer;
        public ValueArrayCtrl(int numInChannels, (int, int) channelSize)
        {
            this.numInChannels = numInChannels;
            this.channelHeight = channelSize.Item1;
            this.channelWidth = channelSize.Item2;
            buffer = new float[numInChannels * channelHeight * channelWidth];
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
                        buffer[i] = Input[ii].Value;
                        i = i + 1;
                    }
                }
            }
            // remember to toss buffer after done (check)
            if (i > 0 && k < i)
            {
                Output.Value = buffer[k];
                Output.enable = true;
                k = k + 1;
                if (k % numInChannels == 0)
                {
                    Output.LastValue = true;
                }
                if (k == i)
                {
                    i = 0;
                    k = 0;
                }
            }
            
        }
    }
}