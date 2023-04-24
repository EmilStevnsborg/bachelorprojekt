using SME;

namespace CNN
{
    //<summary>
    //  A bus for one value
    //</summary>
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