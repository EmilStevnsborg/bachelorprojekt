using SME;
using CNN;
using System;

namespace TestConv
{
    public class Test
    {
        private int numInChannels { get; set; }
        private int numOutChannels { get; set; }
        private int channelHeight { get; set; }
        private int  channelWidth { get; set; }
        private int kernelHeight { get; set; }
        private int kernelWidth { get; set; }
        private int strideRow { get; set; }
        private int strideCol { get; set; }
        private int padHeight { get; set; }
        private int padWidth { get; set; }
        private float padVal { get; set; }
        private float[][][] weights { get; set; }
        private float[] biasVal { get; set; }
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