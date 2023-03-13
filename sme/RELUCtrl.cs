using SME;
using SME.Components;
using static System.Math;

namespace CNN
{
    public class RELUCtrl : SimpleProcess
    {

        [InputBus] 
        public Channel Input;

        [OutputBus]
        public Pixel Output = Scope.CreateBus<Pixel>();

        private int i = 0;
        private int j = 0;
        private int height;
        private int width;
        private double [,] tmp;


        public RELUCtrl(int height, int width)
        {
            tmp = new double[height,width];
        }
        protected override void OnTick()
        {
            if (i == 0 && j == 0)
            {
                for (int ii = 0; ii < height; ii++)
                {
                    for (int jj = 0; jj < width; jj++)
                    {
                        tmp[ii,jj] = Input.Data[ii*width+jj];
                    }
                }
                Output.Value = tmp[i,j];
                j += 1;
            }
            else 
            {
                Output.Value = tmp[i,j];
                j = (j+1)%width;
                if (j == 0)
                {
                    i = (i+1)%height;
                    if (i == 0)
                    {
                        // new image
                    }
                }
            }
        }
    }
}