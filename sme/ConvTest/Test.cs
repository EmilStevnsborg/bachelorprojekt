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
        public Test() {} 
    }
}