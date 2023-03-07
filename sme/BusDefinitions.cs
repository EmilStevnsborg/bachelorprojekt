using SME;

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
    //  A bus for reading a channel channel
    //</summary>
    public interface ChannelInput : IBus
    {
        [InitialValue]
        bool IsValid { get; set; }
        uint Height { get; set; }
        uint Width { get; set; }
        double[,] Values { get; set; }
        // SME.IFixedArray<double> Values { get; set; }

    }
    //<summary>
    //  A bus for one channel
    //</summary>
    public interface ChannelOutput : IBus
    {
        [InitialValue]
        bool IsValid { get; set; }
        uint Height { get; set; }
        uint Width { get; set; }
        double[,] Values { get; set; }
        // SME.IFixedArray<double> Values { get; set; }
    }
}