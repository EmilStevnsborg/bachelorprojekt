using CNN;

namespace Config
{
    public class LinearConfig
    {
        public int numInChannels { get; set; }
        public int numOutChannels { get; set; }
        public int channelHeight { get; set; }
        public int  channelWidth { get; set; }
        public float[][] weights { get; set; }
        public float[] biases { get; set; }
        public LinearConfig() {} 
        public LinearLayer linearLayer { get; set; }
        public void PushConfig()
        {
            this.linearLayer = new LinearLayer(numInChannels,
                                               numOutChannels,
                                               weights,
                                               biases,
                                               (channelHeight,channelWidth));
        }
    }
}