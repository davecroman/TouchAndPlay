using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TouchAndPlay.engine
{
    public class LevelData
    {
        public enum Goals
        {
            SCORE,
            STARS,//
            MAX_MISS,//
            COMBO,
            BUBBLES,
            MAX_REDHIT,
        }

        public int gameDuration;
        public int level_num;

        public int bubblesToPop;
        public int starBubbles;
        public int redBubbles;

        public Dictionary<Goals, int> goals;

        public LevelData(int gameDuration, int level_num, int bubblesToPop)
        {
            this.gameDuration = gameDuration;
            this.level_num = level_num;

            this.goals = new Dictionary<Goals, int>();

            this.bubblesToPop = bubblesToPop;
        }

        public void setGoals(int goalScore, int goalStars, int goalMaxMiss, int goalCombo, int goalBubbles, int goalMaxRedHit ){
            goals[Goals.SCORE] = goalScore;
            goals[Goals.STARS] = goalStars;
            goals[Goals.MAX_MISS] = goalMaxMiss;
            goals[Goals.COMBO] = goalCombo;
            goals[Goals.BUBBLES] = goalBubbles;
            goals[Goals.MAX_REDHIT] = goalMaxRedHit;

        }
    }
}
