using System.Collections.Generic;
using System.Threading.Tasks;
using SME;

namespace CNN 
{
    /// <summary>
    /// Helper process that simulates a conv process.
    /// Since this is a simulation process, it will not be rendered as hardware
    /// and we can use any code and dynamic properties we want
    /// </summary>
    static public class ConvTester // : SimulationProcess
    {

        public static void testOne()
        {

            double [,] channelValues1 = new double[2,2] {{0,1}, {1,0}};
            double [,] channelValues2 = new double[2,2] {{0,2}, {2,0}};
            Channel channel1 = new Channel(2,2,channelValues1);
            Channel channel2 = new Channel(2,2,channelValues2);
            List<Channel> channelsValues = new List<Channel>(){channel1, channel2};
            Channels channels = new Channels(channelsValues);
            List<Channels> inBatch = new List<Channels>();
            inBatch.Add(channels);
            
            double[,] convKernelValues1 = new double[2,2] {{2,2}, {3,3}};
            ConvKernel convKernel1 = new ConvKernel(2,2,convKernelValues1);
            double[,] convKernelValues2 = new double[2,2] {{2,2}, {3,3}};
            ConvKernel convKernel2 = new ConvKernel(2,2,convKernelValues1);
            List<ConvKernel> filterKernels = new List<ConvKernel>() {convKernel1, convKernel2};
            Filter filter = new Filter(filterKernels);
            List<Filter> filters = new List<Filter>() {filter};
            double[] biases = {0};
            Conv conv = new Conv(filters, biases, 2, (2,2), (1,1), null); 

            conv.Call(inBatch);

        }
    }
}