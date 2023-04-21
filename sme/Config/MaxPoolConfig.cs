using CNN;

namespace Config
{
    public class MaxPoolConfig
    {
        public int numInChannels { get; set; }
        public int channelHeight { get; set; }
        public int  channelWidth { get; set; }
        public int kernelHeight { get; set; }
        public int kernelWidth { get; set; }
        public int strideRow { get; set; }
        public int strideCol { get; set; }
        public int padHeight { get; set; }
        public int padWidth { get; set; }
        public float padVal { get; set; }
        public MaxPoolConfig() {} 
        public MaxPoolLayer maxPoolLayer { get; set; }
        public void PushConfig()
        {
            this.maxPoolLayer = new MaxPoolLayer(numInChannels,
                                                 (channelHeight,channelWidth),
                                                 (kernelHeight,kernelWidth),
                                                 (strideRow,strideCol),
                                                 (padHeight,padWidth),
                                                 padVal);
        }
    }
}