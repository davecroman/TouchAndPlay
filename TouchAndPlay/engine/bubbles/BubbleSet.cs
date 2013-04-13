using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TouchAndPlay.utils;
using TouchAndPlay.effects;
using TouchAndPlay.engine.bubbles;

namespace TouchAndPlay.bubbles
{
    class BubbleSet
    {
        public List<Bubble> bubbles;
        private BubbleSetType type;
        private Dictionary<BubbleState, Texture2D> handTextureList;
        private Texture2D lineTexture;

        internal bool popped;
        internal Vector2 midPoint;

        private int iter1;
        private int iter2;

        private float lineAlpha;
        private float lineRotation;
        private float scaleX;
        private Vector2 scaleVector;

        public BubbleSetState currentState;

        private EffectHandler effectHandler;

        public BubbleSet(BubbleSetType type, Dictionary<BubbleState, Texture2D> textureList, Texture2D progressCircle, Texture2D lineTexture, EffectHandler effectHandler)
        {
            this.type = type;
            this.currentState = BubbleSetState.APPEARING;
            this.handTextureList = textureList;
            this.lineTexture = lineTexture;
            this.lineAlpha = GameConfig.LINE_ALPHA;
            this.effectHandler = effectHandler;
            createSet(progressCircle);
        }

        public BubbleSet(BubbleSetType type, Dictionary<BubbleState, Texture2D> textureList, Texture2D progressCircle, Texture2D lineTexture, EffectHandler effectHandler, Vector2 pos1, Vector2 pos2)
        {
            this.type = type;
            this.currentState = BubbleSetState.APPEARING;
            this.handTextureList = textureList;
            this.lineTexture = lineTexture;
            this.lineAlpha = GameConfig.LINE_ALPHA;
            this.effectHandler = effectHandler;

            bubbles = new List<Bubble>();

            switch (type)
            {
                case BubbleSetType.HANDHAND:
                    Vector2 randomPoint = Randomizer.createRandomPoint();

                    //create first hand bubble
                    Bubble bubble1 = new Bubble(pos1.X, pos1.Y, handTextureList, progressCircle, BubbleType.HAND);
                    bubble1.setStayingDuration(GameConfig.SETBUBBLE_DURATION);
                    bubbles.Add(bubble1);

                    //create second hand bubble
                    Bubble bubble2 = new Bubble(pos2.X, pos2.Y, handTextureList, progressCircle, BubbleType.HAND);
                    bubble2.setStayingDuration(GameConfig.SETBUBBLE_DURATION);
                    bubbles.Add(bubble2);

                    lineRotation = (float)Math.Atan2(bubble2.yPos - bubble1.yPos, bubble2.xPos - bubble1.xPos);
                    scaleX = (float)(Math.Sqrt((Math.Pow(bubble2.xPos - bubble1.xPos, 2) + Math.Pow(bubble2.yPos - bubble1.yPos, 2))) / GameConfig.BUBBLE_WIDTH);
                    scaleVector = new Vector2(scaleX, 1.0f);
                    break;

            }
        }

        private void createSet(Texture2D progressCircle)
        {
            bubbles = new List<Bubble>();

            switch (type)
            {
                case BubbleSetType.HANDHAND:
                    Vector2 randomPoint = Randomizer.createRandomPoint();

                    //create first hand bubble
                    Bubble bubble1 = new Bubble((int)randomPoint.X, (int)randomPoint.Y, handTextureList, progressCircle, BubbleType.HAND);
                    bubble1.setStayingDuration(GameConfig.SETBUBBLE_DURATION);
                    bubbles.Add(bubble1);

                    //create second hand bubble
                    float randRadian = (float) Randomizer.randomRadian();
                    int randDist = Randomizer.random(150,300);
                    randomPoint = new Vector2(bubble1.xPos + (float)(Math.Cos(randRadian) * randDist), bubble1.yPos + (float)(Math.Sin(randRadian) * randDist));
                    Bubble bubble2 = new Bubble((int)randomPoint.X, (int)randomPoint.Y, handTextureList, progressCircle, BubbleType.HAND);
                    bubble2.setStayingDuration(GameConfig.SETBUBBLE_DURATION);
                    bubbles.Add(bubble2);

                    lineRotation = (float)Math.Atan2(bubble2.yPos - bubble1.yPos, bubble2.xPos - bubble1.xPos);
                    float width = (float)(Math.Sqrt((Math.Pow(bubble2.xPos - bubble1.xPos, 2) + Math.Pow(bubble2.yPos - bubble1.yPos, 2))));
                    scaleX = (float)width / GameConfig.BUBBLE_WIDTH;
                    scaleVector = new Vector2(scaleX, 1.0f);

                    midPoint = new Vector2(bubble1.pos.X + (float)(width / 2f * Math.Cos(lineRotation)), bubble1.pos.Y + (float)(width / 2f * Math.Sin(lineRotation)));
                    break;

            }

            

        }

        private void DrawLine(Bubble bubble1, Bubble bubble2, SpriteBatch sprite)
        {
            if (bubble1.popCounter > 0)
            {
                sprite.Draw(lineTexture, bubble1.pos + bubble1.referencePoint, null, Color.White, lineRotation, Vector2.One * 5, scaleVector, SpriteEffects.None, 0);
            }
            else
            {
                sprite.Draw(lineTexture, bubble1.pos + bubble1.referencePoint, null, Color.White * lineAlpha * bubble1.flickr, lineRotation, Vector2.One * 5, scaleVector, SpriteEffects.None, 0);
            }
        }

        public void resetPopCounters()
        {
            for (iter1 = 0; iter1 < bubbles.Count; iter1++)
            {
                bubbles[iter1].resetPopCounter();
            }

        }

        public void Update()
        {
            updateAllBubbles();
            updateState();

        }

        private void updateState()
        {
            switch (currentState)
            {
                case BubbleSetState.APPEARING:
                    if (bubbles[0].currentState == BubbleState.NORMAL_STATE)
                    {
                        currentState = BubbleSetState.NORMAL;
                    }
                    break;
                case BubbleSetState.NORMAL:
                    
                    break;
                case BubbleSetState.POP_STATE:
                case BubbleSetState.DISAPPEARING:
                    lineAlpha -= 0.02f;
                    if (lineAlpha <= 0f)
                    {
                        currentState = BubbleSetState.REMOVAL_STATE;
                    }
                    break;
            }
        }

        private void updateAllBubbles()
        {
            switch (currentState)
            {
                case BubbleSetState.POP_STATE:
                    for (iter1 = 0; iter1 < bubbles.Count; iter1++)
                    {
                        bubbles[iter1].Update();
                    }
                    break;
                default:
                    for (iter1 = 0; iter1 < bubbles.Count; iter1++)
                    {
                        bubbles[iter1].Update();

                        if (bubbles[iter1].currentState == BubbleState.DISAPPEARING)
                        {
                            currentState = BubbleSetState.DISAPPEARING;
                        }
                        else if (bubbles[iter1].currentState == BubbleState.POP_STATE)
                        {

                            currentState = BubbleSetState.POP_STATE;
                            popped = true;

                            for (iter1 = 0; iter1 < bubbles.Count; iter1++)
                            {
                                effectHandler.addParticleEffect((int)(bubbles[iter1].xPos + bubbles[iter1].referencePoint.X), (int)(bubbles[iter1].yPos + bubbles[iter1].referencePoint.Y), 20);
                            }

                            

                        }
                    }
                    break;
            }
        }

        public void Draw(SpriteBatch sprite)
        {
            
            for (iter2 = 1; iter2 < bubbles.Count; iter2++)
            {
                DrawLine(bubbles[iter2 - 1], bubbles[iter2], sprite);
            }
            

            for (iter2 = 0; iter2 < bubbles.Count; iter2++)
            {
                bubbles[iter2].Draw(sprite);
            }

        }
    }
}
