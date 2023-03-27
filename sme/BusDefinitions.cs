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
        [FixedArrayLength(STANDARD_SAFE_SIZE)]
        IFixedArray<int> Data { get; set; }
    }
    public interface ValueBus : IBus
    {
        [InitialValue(false)]
        bool enable { get; set; }
        [InitialValue(0)]
        float Value { get; set; }
        [InitialValue(false)]
        bool LastValue { get; set; }
    }
}