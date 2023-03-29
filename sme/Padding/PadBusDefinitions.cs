using SME;
namespace CNNPadder
{
    //<summary>
    //  This is a temporary input bus meant for testing. This bus is the output bus of the padder.
    //</summary>
    public interface PaddedChannelBus : IBus
    {
        [InitialValue(false)]
        bool enable { get; set; }
        int Height { get; set; }
        int Width { get; set; }
        [FixedArrayLength(29 * 29)]
        IFixedArray<float> ArrData { get; set; }
    }
    public interface ChannelBus : IBus
    {
        [InitialValue(false)]
        bool enable { get; set; }
        int Height { get; set; }
        int Width { get; set; }
        [FixedArrayLength(28 * 28)]
        IFixedArray<float> ArrData { get; set; }
    } 
}