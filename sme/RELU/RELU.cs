using SME;
using SME.Components;
using static System.Math;

namespace CNN
{
    public class RELU : SimpleProcess
    {

        [InputBus] 
        public Pixel Input;

        [OutputBus]
        public Pixel Output = Scope.CreateBus<Pixel>();

        protected override void OnTick()
        {
            double x = Input.Value;
            if (x > 0)
            {
                Output.Value = x;
            }            
            else
            {
                Output.Value = 0;
            }
        }
    }
}