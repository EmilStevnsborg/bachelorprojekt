using SME;

namespace CNN
{
    public interface Pixel : IBus
    {
        double Value { get; set; }
    }
}