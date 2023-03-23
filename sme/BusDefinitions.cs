using SME;
using static CNN.ChannelSizes;

namespace CNN
{
    //<summary>
    //  A bus for one channel
    //</summary>
    public interface ChannelBus : IBus
    {
        [InitialValue(false)]
        bool enable { get; set; }
        int Height { get; set; }
        int Width { get; set; }
        [FixedArrayLength(STANDARD_SAFE_SIZE)]
        IFixedArray<float> ArrData { get; set; }
    }
    public interface SliceBus : IBus
    {
        [InitialValue(false)]
        bool enable { get; set; }
        int Height { get; set; }
        int Width { get; set; }
        [FixedArrayLength(STANDARD_SAFE_SIZE)]
        IFixedArray<float> ArrData { get; set; }
    }
    public interface SliceInfoBus : IBus
    {
        [InitialValue(false)]
        bool enable { get; set; }
        // start and end i,j
        [FixedArrayLength(4)]
        IFixedArray<int> Data { get; set; }
    }

    [InitializedBus]
    public interface ValueBus : IBus
    {
        bool enable { get; set; }
        float Value { get; set; }
        bool LastValue { get; set; }
    }
}