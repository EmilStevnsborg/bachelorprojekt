using SME;
using CNN;

namespace Config
{
    public class ConvConfig
    {
        public int numInChannels { get; set; }
        public int numOutChannels { get; set; }
        public int channelHeight { get; set; }
        public int  channelWidth { get; set; }
        public int kernelHeight { get; set; }
        public int kernelWidth { get; set; }
        public int strideRow { get; set; }
        public int strideCol { get; set; }
        public int padHeight { get; set; }
        public int padWidth { get; set; }
        public float padVal { get; set; }
        public float[][][] weights { get; set; }
        public float[] biases { get; set; }
        public ConvConfig() {} 
        public ConvLayer convLayer { get; set; }
        public void PushConfig()
        {
            this.convLayer = new ConvLayer(numInChannels,
                                           numOutChannels,
                                           weights,
                                           biases,
                                           (channelHeight,channelWidth),
                                           (kernelHeight,kernelWidth),
                                           (strideRow,strideCol),
                                           (padHeight,padWidth),
                                           padVal);
        }
    }
}