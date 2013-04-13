using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TouchAndPlay.db;
using TouchAndPlay.engine;

namespace TouchAndPlay.components.gamespecific
{
    class LevelSelector:BasicSlider
    {
        Dictionary<int, int> recordMedalOnLevel;
        Dictionary<int, int> bestScoreOnLevel;

        public GameType currentGameType;

        public LevelSelector(Texture2D boxTexture, Texture2D upArrow, Texture2D downArrow, int xPos, int yPos, int selectionWidth, int selectionHeight, string label, SpriteFont headerFont, SpriteFont itemFont, Color? itemTextColor = null, Color? itemTextColorHovered = null, int shownItems = 5, SliderType sliderType = SliderType.VERTICAL_SLIDER, Color? headerBoxColor = null, Color? mouseOutBoxColor = null, Color? hoverBoxColor = null)
            : base(boxTexture, upArrow, downArrow, xPos, yPos, selectionWidth, selectionHeight, label, headerFont, itemFont, itemTextColor, itemTextColorHovered, shownItems, SliderType.VERTICAL_SLIDER, headerBoxColor, mouseOutBoxColor, hoverBoxColor)
        {
            this.currentGameType = GameType.RANGE_EXERCISE;
            this.Initialize(currentGameType);
        }

        public void Initialize(GameType currentGameType)
        {
            this.currentGameType = currentGameType;

            recordMedalOnLevel = new Dictionary<int, int>();
            bestScoreOnLevel = new Dictionary<int, int>();

            recordMedalOnLevel.Clear();
            bestScoreOnLevel.Clear();

            if (TAPDatabase.profileExists(GameConfig.CURRENT_PROFILE))
            {
                for (int levelNum = 1; levelNum <= 5; levelNum++)
                {
                    recordMedalOnLevel[levelNum] = TAPDatabase.getRecordMedals(GameConfig.CURRENT_PROFILE, currentGameType, levelNum);
                    bestScoreOnLevel[levelNum] = TAPDatabase.getRecordScore(GameConfig.CURRENT_PROFILE, currentGameType, levelNum);
                }
            }
            else
            {
                recordMedalOnLevel[1] = 0;
                bestScoreOnLevel[1] = 0;

                recordMedalOnLevel[2] = 0;
                recordMedalOnLevel[3] = 0;
                recordMedalOnLevel[4] = 0;
                recordMedalOnLevel[5] = 0;

                bestScoreOnLevel[2] = 0;
                bestScoreOnLevel[3] = 0;
                bestScoreOnLevel[4] = 0;
                bestScoreOnLevel[5] = 0;
            }
        }

        public override void Draw(SpriteBatch sprite)
        {
            if (hidden) { return; }

            base.Draw(sprite);

            sprite.DrawString(headerFont, "Medals Earned       |", new Vector2(60, yPos + 4), Color.White);
            sprite.DrawString(headerFont, "   |      Best Score", new Vector2(400, yPos + 4), Color.White);

            for (int level = firstShownIndex; level < shownItems + firstShownIndex; level++)
            {
                DrawMedals(sprite, level);

                sprite.DrawString(itemFont, bestScoreOnLevel[level + 1].ToString(), new Vector2(500, itemsOnScreen[level].getYPos() + 5 - selectionHeight * firstShownIndex),  Color.Black);
            }

        }

        private void DrawMedals(SpriteBatch sprite, int level)
        {
            int i;

            for (i = 0; i < recordMedalOnLevel[level + 1]; i++)
            {
                sprite.Draw(Gallery.MEDAL_OBTAINED, new Vector2(100 + i * Gallery.MEDAL_OBTAINED.Width, itemsOnScreen[level].getYPos() + 3 - selectionHeight * firstShownIndex), null, Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f);
            }

            while (i < 3)
            {
                sprite.Draw(Gallery.MEDAL_NOT_EARNED, new Vector2(100 + i * Gallery.MEDAL_NOT_EARNED.Width, itemsOnScreen[level].getYPos() + 3 - selectionHeight * firstShownIndex), null, Color.White * 0.5f, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f);
                i++;
            }
        }

        
    }
}
