using SME;
using CNN;
using System;

namespace TestConv
{
    public class Test
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
        public float[] biasVal { get; set; }
        public Test() {} 
        public ConvLayer convLayer { get; set; }
        public Tester tester { get; set; }
        public void PushConfig()
        {
            this.convLayer = new ConvLayer(numInChannels,
                                           numOutChannels,
                                           weights,
                                           biasVal,
                                           (channelHeight,channelWidth),
                                           (kernelHeight,kernelWidth),
                                           (strideRow,strideCol),
                                           (padHeight,padWidth),
                                           padVal);
            this.tester = new Tester(numInChannels, 
                                     numOutChannels,
                                     (channelHeight,channelWidth));
        }
    }
}