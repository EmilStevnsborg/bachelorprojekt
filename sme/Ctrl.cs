using SME;
using SME.Components;
using static System.Math;

namespace CNN
{
    public class Ctrl : SimpleProcess
    {

        [InputBus] 
        public Channel Input;

        [OutputBus]
        public Pixel Output = Scope.CreateBus<Pixel>();

        private int i = 0;
        private int j = 0;
        private int channelHeight;
        private int channelWidth;
        private double [,] tmp;


        public Ctrl(int channelHeight, int channelWidth)
        {
            this.channelHeight = channelHeight;
            this.channelWidth = channelWidth;
            this.tmp = new double[channelHeight,channelWidth];
        }
        protected override void OnTick()
        {
            if (i == 0 && j == 0)
            {
                for (int ii = 0; ii < channelHeight; ii++)
                {
                    for (int jj = 0; jj < channelWidth; jj++)
                    {
                        tmp[ii,jj] = Input.Data[ii*channelWidth+jj];
                    }
                }
                Output.Value = tmp[i,j];
                // Output.I = i;
                // Output.J = j;
                j += 1;
            }
            else 
            {
                Output.Value = tmp[i,j];
                j = (j+1)%channelWidth;
                if (j == 0)
                {
                    i = (i+1)%channelHeight;
                    if (i == 0)
                    {
                        // new image
                    }
                }
            }
        }
    }
}