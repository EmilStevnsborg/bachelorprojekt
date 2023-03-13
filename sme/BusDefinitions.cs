using SME;
using static CNN.ChannelSizes;

namespace CNN
{
    // FOR NOW DON'T THINK ABOUT THIS ONE
    //
    //
    //<summary>
    //  A bus for reading Grey scale images from a sensor,
    //  one pixel at a time
    //</summary>
    public interface GreyScaleImageInputLine : IBus
    {
        [InitialValue]
        bool IsValid { get; set; }
        [InitialValue]
        bool LastPixel { get; set; }

        byte Pixel { get; set; }
    }
    //<summary>
    //  A bus for one channel
    //</summary>
    public interface Channel : IBus
    {
        [InitialValue]
        bool IsValid { get; set; }
        int Height { get; set; }
        int Width { get; set; }
        [FixedArrayLength(STANDARD_SAFE_SIZE)]
        IFixedArray<double> Data { get; set; }
    }
}