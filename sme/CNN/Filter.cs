using System.Collections.Generic;

namespace CNN 
{
    public class Filter
    {
        public List<Kernel> filter { get; private set; }

        public Filter(List<Kernel> filter)
        {
            this.filter = filter;
        }

    }
}