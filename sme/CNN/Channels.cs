using System.Collections.Generic;

namespace CNN 
{
    public class Channels
    {
        public List<Channel> channels { get; private set; }

        public Channels(List<Channel> channels)
        {
            this.channels = channels;
        }
        public Channels()
        {
            this.channels = new List<Channel>();
        }

    }
}
