using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TouchAndPlay.engine
{
    class LevelMaker
    {
        public static void instantiateRangeLevels(List<LevelData> levels)
        {
            for (int i = 1; i <= 5; i++)
            {
                int transitionDuration = 40;
                int bubblesToPop = 15;
                int bufferTimeMax = GameConfig.BUBBLE_SOLO_DURATION - GameConfig.SOLOBUBBLE_POP_TIME;
                int duration = bubblesToPop * GameConfig.SOLOBUBBLE_POP_TIME + transitionDuration*40 + 15* (bufferTimeMax - i * 50);
                LevelData levelData = new LevelData(duration, i, bubblesToPop);
                levels.Add(levelData);

                switch (i)
                {
                    case 1:
                        levelData.setGoals(100, 2, 0, 0, 8, 0);
                        break;
                    case 2:
                        levelData.setGoals(200, 3, 0, 0, 0, 3);
                        break;
                    case 3:
                        levelData.setGoals(300, 0, 0, 2, 12, 0); 
                        break;
                    case 4:
                        levelData.setGoals(300, 0, 0, 2, 12, 0); 
                        break;
                    case 5:
                        levelData.setGoals(300, 0, 0, 2, 12, 0); 
                        break;

                }
            }

        }

        public static void instantiateCoordLevels(List<LevelData> levels)
        {
            for (int level = 1; level <= 5; level++)
            {
                int transitionDuration = 40;
                int bubblesPairsToPop = 10;
                int bufferTimeMax = GameConfig.SETBUBBLE_DURATION - GameConfig.SOLOBUBBLE_POP_TIME;
                int duration = bubblesPairsToPop *GameConfig.SOLOBUBBLE_POP_TIME + transitionDuration * 40 + 15 * (bufferTimeMax - level * 50);
                LevelData levelData = new LevelData(duration, level, bubblesPairsToPop);

                switch (level)
                {
                    case 1:
                        levelData.setGoals(100, 2, 0, 0, 8, 0);
                        break;
                    case 2:
                        levelData.setGoals(200, 3, 0, 0, 0, 3);
                        break;
                    case 3:
                        levelData.setGoals(300, 0, 0, 2, 12, 0);
                        break;
                    case 4:
                        levelData.setGoals(300, 0, 0, 2, 12, 0);
                        break;
                    case 5:
                        levelData.setGoals(300, 0, 0, 2, 12, 0);
                        break;

                }

                levels.Add(levelData);
            }

        }

        public static void instantiateDragLevels(List<LevelData> levels)
        {
            for (int i = 1; i <= 5; i++)
            {
                int transitionDuration = 40;
                int dragBubblesToPop = 10;
                int bufferTimeMax = GameConfig.DRAGBUBBLE_DURATION - GameConfig.SOLO_BUBBLES_TO_POP;
                int duration = dragBubblesToPop * GameConfig.SOLOBUBBLE_POP_TIME + transitionDuration * 40 + 15 * (bufferTimeMax - i * 50);
                LevelData levelData = new LevelData(duration, i, dragBubblesToPop); 
                

                switch (i)
                {
                    case 1:
                        levelData.setGoals(100, 2, 0, 0, 8, 0);
                        break;
                    case 2:
                        levelData.setGoals(200, 3, 0, 0, 0, 3);
                        break;
                    case 3:
                        levelData.setGoals(300, 0, 0, 2, 12, 0);
                        break;
                    case 4:
                        levelData.setGoals(300, 0, 0, 2, 12, 0);
                        break;
                    case 5:
                        levelData.setGoals(300, 0, 0, 2, 12, 0);
                        break;

                }

                levels.Add(levelData);
            }
        }

        
    }
}
