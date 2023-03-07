using SME;
using SME.Components;
using static System.Math;

namespace CNN
{
    public class ELUChannel : SimpleProcess
    {

        [InputBus] 
        ChannelInput Input = Scope.CreateBus<ChannelInput>();

        [OutputBus]
        ChannelOutput Output = Scope.CreateBus<ChannelOutput>();

        SimpleDualPortMemory<double> BlockRAM;

        public ELUChannel(double alphaArg)
        {
            double[] alpha = new double[1] {alphaArg};
            BlockRAM = new SimpleDualPortMemory<double>(1, alpha);
        }
        protected override void OnTick()
        {
            for (int i = 0; i < Input.Height; i++)
            {
                for (int j = 0; j < Input.Width; j++)
                {
                    double x = Input.Values[i,j];
                    if (x > 0)
                    {
                        Output.Values[i,j] = x;
                    }
                    else
                    {
                        Output.Values[i,j] = BlockRAM.ReadResult.Data * (System.Math.Exp(x) - 1);
                    }
                }
            }
        }
    }
}