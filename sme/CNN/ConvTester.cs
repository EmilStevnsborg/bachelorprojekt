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
    public class ConvTester : SimulationProcess
    {
    [InputBus, OutputBus]
    private readonly Conv.IInputBatch InputBatch = Scope.CreateOrLoadBus<Conv.IInputBatch>();
    [InputBus]
    private readonly Conv.IResultBatch ResultBatch = Scope.CreateOrLoadBus<Conv.IResultBatch>();

    public override async Task Run()
    {
        await ClockAsync();

        double [,] channelValues1 = new double[2,2] {{0,1}, {1,0}};
        double [,] channelValues2 = new double[2,2] {{0,2}, {2,0}};
        Channel channel1 = new Channel(2,2,channelValues1);
        Channel channel2 = new Channel(2,2,channelValues2);
        List<Channel> channelsValues = new List<Channel>(){channel1, channel2};
        Channels channels = new Channels(channelsValues);
        
        double[,] convKernelValues1 = new double[2,2] {{2,2}, {3,3}};
        ConvKernel convKernel1 = new ConvKernel(2,2,convKernelValues1,(1,1), 0, null);
        double[,] convKernelValues2 = new double[2,2] {{2,2}, {3,3}};
        ConvKernel convKernel2 = new ConvKernel(2,2,convKernelValues1,(1,1), 0, null);
        List<ConvKernel> filterKernels = new List<ConvKernel>() {convKernel1, convKernel2};
        Filter filter = new Filter(filterKernels);
        List<Filter> filters = new List<Filter>() {filter};
        double[] biases = {0};
        Conv conv = new Conv(filters, biases, 2, (1,1), null); 


        // InputBatch.InputBatch = testChannels
    }
    }
}