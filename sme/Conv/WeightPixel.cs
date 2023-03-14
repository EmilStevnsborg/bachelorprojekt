using SME;
using SME.Components;
using static System.Math;

namespace CNN
{
    public class WeightPixel : SimpleProcess
    {

        [InputBus] 
        public Pixel Input;
        [InputBus]
        public TrueDualPortMemory<double>.IReadResultA weight;

        [OutputBus]
        public Pixel Output = Scope.CreateBus<Pixel>();

        // private int RAMIndex;

        public WeightPixel()
        {
        }

        protected override void OnTick()
        {
            //
            Output.Value = weight.Data * Input.Value;
        }
    }
}