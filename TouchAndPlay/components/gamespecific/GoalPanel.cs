using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using TouchAndPlay.effects;

namespace TouchAndPlay.components.gamespecific
{
    public class GoalPanel
    {
        SpriteFont labelFont;
        SpriteFont valueFont;

        Texture2D basicBox;

        Vector2 panelPosition;
        Vector2 lineScale;
        Vector2 scale;

        int panelHeight;

        private Dictionary<string, int> goals;

        private const string GOAL_SCORE_TEXT = "Score to Earn";
        private const string GOAL_STARS_TEXT = "Stars to Collect";
        private const string GOAL_MAXMISS_TEXT = "Miss Left";
        private const string GOAL_COMBO_TEXT = "Goal Combo";
        private const string GOAL_BUBBLES_TEXT = "Bubbles to Pop";
        private const string GOAL_MAXREDHIT_TEXT = "Red Hits Left";

        private int scoreToEarn;
        private int starsToCollect;
        private int missLeft;
        private int comboLeft;
        private int bubblesLeft;
        private int redHitsLeft;

        private int goalScore;
        private int goalStars;
        private int goalMiss;
        private int goalCombo;
        private int goalBubbles;
        private int goalRedHits;

        public int medalsEarned = 0;

        private float medalPartShown = 1.0f;

        private EffectHandler effectHandler;

        public GoalPanel(EffectHandler effectHandler)
        {
            panelHeight = 20;

            this.effectHandler = effectHandler;
            this.panelPosition = new Vector2(0, GameConfig.APP_HEIGHT - panelHeight);
            this.goals = new Dictionary<string, int>();
            
        }

        public void setupPanel(int goalScore, int goalStars, int goalMiss, int goalCombo, int goalBubbles, int goalRedHits)
        {
            this.goalScore = goalScore;
            this.goalStars = goalStars;
            this.goalMiss = goalMiss;
            this.goalCombo = goalCombo;
            this.goalBubbles = goalBubbles;
            this.goalRedHits = goalRedHits;

            scoreToEarn = goalScore;
            starsToCollect = goalStars;
            missLeft = goalMiss;
            comboLeft = goalCombo;
            bubblesLeft = goalBubbles;
            redHitsLeft = goalRedHits;

            goals[GOAL_BUBBLES_TEXT] = goalBubbles;
            goals[GOAL_STARS_TEXT] = goalStars;
            goals[GOAL_COMBO_TEXT] = goalCombo;
            goals[GOAL_SCORE_TEXT] = goalScore;
            goals[GOAL_MAXMISS_TEXT] = goalMiss;
            goals[GOAL_MAXREDHIT_TEXT] = goalRedHits;
        }

        public void LoadContent(ContentManager content)
        {
            labelFont = content.Load<SpriteFont>("fonts/GoalPanelLabelFont");
            valueFont = content.Load<SpriteFont>("fonts/GoalPanelValueFont");
            basicBox = content.Load<Texture2D>("stageboxes/basic_box_white");

            this.scale = new Vector2(GameConfig.APP_WIDTH / (float)basicBox.Width, panelHeight / (float)basicBox.Height);
            this.lineScale = new Vector2(2 / (float)basicBox.Width, panelHeight / (float)basicBox.Height);
        }

        public void Draw(SpriteBatch sprite)
        {
            sprite.Draw(basicBox, panelPosition, null, Color.RoyalBlue, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

            sprite.DrawString(labelFont, "GOALS", new Vector2(5, panelPosition.Y + 1), Color.White);

            float currentX = (5 + labelFont.MeasureString("GOALS").X + 5);
            sprite.Draw(basicBox, new Vector2(currentX, panelPosition.Y), null, Color.White, 0f, Vector2.Zero, lineScale, SpriteEffects.None, 0f);

            currentX += 10;

            foreach (KeyValuePair<string, int> goal in goals)
            {
                if (goal.Value > 0)
                {
                    sprite.DrawString(labelFont, goal.Key, new Vector2(currentX, panelPosition.Y), Color.White);
                    currentX += labelFont.MeasureString(goal.Key).X + 10;

                    sprite.DrawString(valueFont, goal.Value.ToString(), new Vector2(currentX, panelPosition.Y), Color.White);
                    currentX += valueFont.MeasureString(goal.Value.ToString()).X + 15;

                    sprite.Draw(basicBox, new Vector2(currentX, panelPosition.Y), null, Color.White, 0f, Vector2.Zero, lineScale, SpriteEffects.None, 0f);
                    currentX += 15;
                }
            }

            for (int count = 0; count < medalsEarned; count++)
            {
                sprite.Draw(Gallery.MEDAL, new Vector2(300 + Gallery.MEDAL.Width * 0.6f * count, -2 - (count+1==medalsEarned?Gallery.MEDAL.Width*(1-medalPartShown):0)), null, Color.White * 0.9f, 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
                /*
                if (medal < medalsEarned)
                {
                    sprite.Draw(Gallery.MEDAL_OBTAINED, new Vector2(GameConfig.APP_WIDTH - Gallery.MEDAL_OBTAINED.Width * (medal + 1), panelPosition.Y - 10), Color.White);
                }
                else
                {
                    sprite.Draw(Gallery.MEDAL_NOT_EARNED, new Vector2(GameConfig.APP_WIDTH - Gallery.MEDAL_NOT_EARNED.Width * (medal + 1), panelPosition.Y - 10), Color.White * 0.3f);
                }*/
            }

            if (medalPartShown < 1.0f)
            {
                medalPartShown += 0.04f;
            }
        }

        internal void addCollectedBubble()
        {
            if (goals.ContainsKey(GOAL_BUBBLES_TEXT))
            {
                if (goals[GOAL_BUBBLES_TEXT] > 0)
                {
                    goals[GOAL_BUBBLES_TEXT] -= 1;
                 
                    if (goals[GOAL_BUBBLES_TEXT] == 0)
                    {
                        effectHandler.addText("GOAL ACCOMPLISHED!", GameConfig.APP_WIDTH / 2, GameConfig.APP_HEIGHT / 2, Color.YellowGreen);
                        effectHandler.addText(goalBubbles + " bubbles collected.", GameConfig.APP_WIDTH / 2, GameConfig.APP_HEIGHT / 2 + 30);
                        medalsEarned++;
                        medalPartShown = 0;
                    }
                }
            }
        }

        internal void addCollectedStar()
        {
            if (goals.ContainsKey(GOAL_STARS_TEXT))
            {
                if (goals[GOAL_STARS_TEXT] > 0)
                {
                    goals[GOAL_STARS_TEXT] -= 1;

                    if (goals[GOAL_STARS_TEXT] == 0)
                    {
                        effectHandler.addText("GOAL ACCOMPLISHED!", GameConfig.APP_WIDTH / 2, GameConfig.APP_HEIGHT / 2, Color.YellowGreen);
                        effectHandler.addText(goalStars + " stars collected.", GameConfig.APP_WIDTH / 2, GameConfig.APP_HEIGHT / 2 + 30);
                        medalsEarned++;
                        medalPartShown = 0;
                    }
                }
            }
        }

        internal void addScore(int scoreValue)
        {
            if (goals.ContainsKey(GOAL_SCORE_TEXT))
            {
                if (goals[GOAL_SCORE_TEXT] > 0)
                {
                    goals[GOAL_SCORE_TEXT] -= scoreValue;

                    if (goals[GOAL_SCORE_TEXT] <= 0)
                    {
                        effectHandler.addText("GOAL ACCOMPLISHED!", GameConfig.APP_WIDTH / 2, GameConfig.APP_HEIGHT / 2, Color.YellowGreen);
                        effectHandler.addText(goalScore + " score earned.", GameConfig.APP_WIDTH / 2, GameConfig.APP_HEIGHT / 2 + 30);
                        medalsEarned++;
                        medalPartShown = 0;
                    }
                }
            }
        }

        internal void resetMedals()
        {
            medalsEarned = 0;
        }
    }
}
