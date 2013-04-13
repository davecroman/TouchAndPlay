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
            STARS,
            MAX_MISS,
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

        public LevelData(int gameDuration, int level_num)
        {
            this.gameDuration = gameDuration;
            this.level_num = level_num;

            this.goals = new Dictionary<Goals, int>();
        }
    }
}
