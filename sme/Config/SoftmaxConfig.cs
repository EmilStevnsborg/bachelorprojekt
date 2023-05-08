using CNN;

namespace Config
{
    public class SoftmaxConfig
    {
        public int numInChannels { get; set; }
        public int numOutChannels { get; set; }
        public int channelHeight { get; set; }
        public int  channelWidth { get; set; }
        public SoftmaxConfig() {} 
        public SoftmaxLayer softmaxLayer { get; set; }
        public void PushConfig()
        {
            this.softmaxLayer = new SoftmaxLayer(numInChannels, (channelHeight,channelWidth));
        }
    }
}