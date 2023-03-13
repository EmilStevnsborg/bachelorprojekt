using SME;
using SME.Components;
using static System.Math;

namespace CNN
{
    public class RELUChannel : SimpleProcess
    {

        [InputBus] 
        public Channel Input;

        [OutputBus]
        public Channel Output = Scope.CreateBus<Channel>();

        protected override void OnTick()
        {
            for (int i = 0; i < Input.Height; i++)
            {
                for (int j = 0; j < Input.Width; j++)
                {
                    double x = Input.Data[i*Input.Width+j];
                    if (x > 0.0)
                    {
                        Output.Data[i*Input.Width+j] = x;
                    }
                    else
                    {
                        Output.Data[i*Input.Width+j] = 0.0;
                    }
                }
            }
        }
    }
}