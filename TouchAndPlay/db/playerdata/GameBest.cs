using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TouchAndPlay.engine;

namespace TouchAndPlay.db.playerdata
{
    public class GameBest
    {
        public GameType gameType;
        public List<LevelBest> levels;

        public GameBest()
        {
            //parameterless constructor
        }

        public GameBest(GameType gameType)
        {
            levels = new List<LevelBest>();

            for (int levelNum = 1; levelNum <= 5; levelNum++)
            {
                levels.Add(new LevelBest());
            }
        }

        internal void compare(int level, int playerScore, int medalsEarned)
        {
            levels[level - 1].compare(playerScore, medalsEarned);
        }

        internal int getMedalsEarned(int level)
        {
            return levels[level - 1].mostMedals;
        }

        internal int getScoreEarned(int level)
        {
            return levels[level - 1].bestScore;
        }
    }
}
