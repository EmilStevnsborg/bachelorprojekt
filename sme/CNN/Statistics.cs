using System;
using System.Collections.Generic;

namespace Statistics
{
    public class Stats
    {
        public Dictionary<string, List<float>> Results = new Dictionary<string, List<float>>();
        public Stats()
        {
            Results["True"] = new List<float>();
            Results["Pred"] = new List<float>();
        }

        public void AddStats(List<(float, float)> stats)
        {
            foreach ((float,float) vals in stats)
            {
                Results["True"].Add(vals.Item1);
                Results["Pred"].Add(vals.Item2);
            }
        }

    }
}