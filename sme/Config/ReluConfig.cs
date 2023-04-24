using CNN;

namespace Config
{
    public class ReluConfig
    {
        public int numInChannels { get; set; }
        public int numOutChannels { get; set; }
        public int channelHeight { get; set; }
        public int  channelWidth { get; set; }
        public ReluConfig() {} 
        public ReluLayer reluLayer { get; set; }
        public void PushConfig()
        {
            this.reluLayer = new ReluLayer(numInChannels);
        }
    }
}