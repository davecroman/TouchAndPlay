using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TouchAndPlay.engine;

namespace TouchAndPlay.db.playerdata
{
    public class LevelBest
    {
        public int bestScore;
        public int mostMedals;

        public LevelBest()
        {
            //parameterless constructor
        }

        public LevelBest(int bestScore = 0, int mostMedals = 0)
        {
            this.bestScore = bestScore;
            this.mostMedals = mostMedals;
        }

        public void compare(int score, int medals)
        {
            if (score > bestScore)
            {
                bestScore = score;
            }

            if (medals > mostMedals)
            {
                mostMedals = medals;
            }

        }
    }
}
