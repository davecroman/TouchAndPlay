using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TouchAndPlay.effects
{
    class BasicScoreText
    {
        public enum BasicScoreTextState
        {
            POP,
            PULL,
            STEADY,
            FADE_OUT,
            READY_FOR_REMOVAL,
        }

        public float xPos;
        public float yPos;
        public Vector2 position;
        private float alpha;
        public BasicScoreTextState currentState;

        public float scale;
        private float maxScale;

        private Vector2 origin;
        private SpriteFont font;

        private string text;

        private Color textColor;

        private float popRate;

        private int duration;
        

        public BasicScoreText(SpriteFont font, float xPos, float yPos, int score, Color? textColor = null)
        {
            if (score < 0)
            {
                score = Math.Abs(score);
                this.text = "-" + Math.Abs(score).ToString();
                
            }
            else
            {
                this.text = "+" + score.ToString();
            }
            this.maxScale = 1.0f + 0.1f * (score / 10);

            Initialize(font, xPos, yPos, textColor.HasValue? textColor.Value: Color.White);
        }

        public BasicScoreText(SpriteFont font, float xPos, float yPos, string text, Color? textColor = null)
        {
            this.text = text;
            this.maxScale = 0.5f;

            Initialize(font, xPos, yPos, textColor.HasValue ? textColor.Value : Color.White);
        }

        private void Initialize(SpriteFont font, float xPos, float yPos, Color color)
        {
            this.xPos = xPos;
            this.yPos = yPos;

            this.position = new Vector2(xPos, yPos);

            this.alpha = 1f;

            this.popRate = 0.1f;

            this.scale = 0f;

            this.origin = new Vector2(font.MeasureString(text).X / 2, font.MeasureString(text).Y / 2);
            this.font = font;

            this.duration = GameConfig.SCORE_TEXT_DURATION;
            this.textColor = color;

            this.currentState = BasicScoreTextState.POP;
        }

        

        public void Update()
        {
            switch (currentState)
            {
                case BasicScoreTextState.POP:
                    if (scale < maxScale + 0.3f)
                    {
                        scale += popRate;
                    }
                    else
                    {
                        currentState = BasicScoreTextState.PULL;
                    }
                    break;
                case BasicScoreTextState.PULL:
                    if (scale > maxScale)
                    {
                        scale -= popRate;
                    }
                    else
                    {
                        currentState = BasicScoreTextState.STEADY;
                    }
                    break;
                case BasicScoreTextState.STEADY:
                    if (duration > 0)
                    {
                        duration--;
                        yPos -= 0.2f;
                        position.Y = yPos;
                    }
                    else
                    {
                        currentState = BasicScoreTextState.FADE_OUT;
                    }
                    break;
                case BasicScoreTextState.FADE_OUT:
                    if (alpha > 0)
                    {
                        alpha -= 0.02f;
                    }
                    else
                    {
                        currentState = BasicScoreTextState.READY_FOR_REMOVAL;
                    }
                    break;
                case BasicScoreTextState.READY_FOR_REMOVAL:
                    break;

            }

        }

        public void Draw(SpriteBatch sprite)
        {
            sprite.DrawString(font, text, position, textColor * alpha, 0f, origin, scale, SpriteEffects.None, 0f);
        }

        public bool isReadyForRemoval()
        {
            return currentState == BasicScoreTextState.READY_FOR_REMOVAL;
        }
        
    }
}
