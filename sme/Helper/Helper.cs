using System;

namespace CNN
{
    public static class Helper 
    {
        public static void Padding(ref float[,] buffer, int channelHeight, int channelWidth,int padHeight, int padWidth, float padVal)
        {
            // top and bottom
            for (int ii = 0; ii < padHeight; ii++)
            {
                for (int jj = 0; jj < channelWidth + 2 * padWidth; jj++)
                {
                    buffer[ii,jj] = padVal;

                    buffer[channelHeight + 2 * padHeight - ii - 1,jj] = padVal;
                }
            }

            // sides
            for (int jj = 0; jj < padWidth; jj++)
            {
                for (int ii = padHeight; ii < channelHeight; ii++)
                {
                    buffer[ii,jj] = padVal;

                    buffer[ii,channelWidth + 2 * padWidth - jj - 1] = padVal;
                }
            }
        }
    }   
}