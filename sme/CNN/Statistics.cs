using System;

namespace Statistics
{
    public class Stats
    {
        private float sum = 0;
        private float num = 0;
        private float max = 0;
        public Stats()
        {
        }

        public void Add(float[,] stats)
        {
            int height = stats.GetLength(0); 
            int width = stats.GetLength(1);

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    var val = stats[i,j];
                    if (val > max)
                    {
                        max = val;
                    }
                    sum += stats[i,j];
                    num += 1;
                }
            }
        }

        public float Mean()
        {
            return sum/num;
        }
        public float Var()
        {
            return ((float) Math.Pow(sum, 2)/num - (float) Math.Pow(Mean(), 2));
        }
        public float Max()
        {
            return max;
        }

    }
}