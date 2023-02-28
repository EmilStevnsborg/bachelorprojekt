using System.Collections.Generic;

namespace CNN 
{
    public class Filter
    {
        public List<ConvKernel> filter { get; private set; }

        public Filter(List<ConvKernel> filter)
        {
            this.filter = filter;
        }

    }
}