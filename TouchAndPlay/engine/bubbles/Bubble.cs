using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TouchAndPlay.bubbles
{
    class Bubble
    {
        public BubbleState currentState;
        public BubbleType bubbleType;

        public float xPos;
        public float yPos;
        public Vector2 pos;

        
        public Microsoft.Kinect.JointType jointHovering;

        private Dictionary<BubbleState, Texture2D> textureList;
        private Texture2D progressCircle;

        private float maxScale;

        public Rectangle collisionBox;

        public int popTime;
        public int popCounter;

        public int stayingDuration;
        public int stayingDurationCtr;

        public int radius = 25;
        public float scale = 0.0f;

        public float alpha;

        public Vector2 referencePoint;
        private Vector2 origin;

        internal float flickr;

        public Bubble(float xPos, float yPos, Dictionary<BubbleState,Texture2D> textureList, Texture2D progressCircle, BubbleType bubbleType, Vector2? referencePoint = null )
        {
            currentState = BubbleState.APPEARING;
            this.textureList = textureList;

            this.referencePoint = referencePoint.HasValue ? referencePoint.Value : Vector2.Zero;

            this.pos = new Vector2(xPos, yPos);
            this.xPos = xPos;
            this.yPos = yPos;
            this.maxScale = 0.50f;
            this.bubbleType = bubbleType;

            this.flickr = 1f;

            this.progressCircle = progressCircle;

            this.origin = new Vector2(50, 50);

            //time required for the joint to stay in this bubble so that it will pop
            this.popTime = GameConfig.SOLOBUBBLE_POP_TIME;

            //alpha value, 1.0f is default
            this.alpha = 0.90f;

            //collision radius
            this.radius = 25;
            this.collisionBox = new Rectangle((int)xPos + (int)this.referencePoint.X - radius, (int)yPos + (int)this.referencePoint.Y - radius, radius*2, radius*2);

            //the duration this bubble remains n screen
            //when duration is over, it will go to the disappearing state
            this.stayingDuration = GameConfig.BUBBLE_SOLO_DURATION;
            this.stayingDurationCtr = stayingDuration;
        }

        public void Update()
        {
            updateCounter();

            switch (currentState)
            {
                case BubbleState.NORMAL_STATE:
                    popCounter = 0;
                    break;
                case BubbleState.HIGHLIGHTED_STATE:
                    if (this.bubbleType != BubbleType.INACTIVE_STATIC_HAND)
                    {
                        popCounter += 1;
                        if (popCounter >= popTime)
                        {
                            currentState = BubbleState.POP_STATE;
                        }
                    }
                    break;
                case BubbleState.APPEARING:
                    if (scale < 0.60f){
                       scale += 0.05f;
                    }
                    else if (scale > 0.50f && scale < 0.60f)
                    {
                        scale = maxScale;
                    }
                    else
                    {
                        currentState = BubbleState.NORMAL_STATE;
                    }
                    break;
                case BubbleState.POP_STATE:
                    scale += 0.03f;
                    alpha -= 0.02f;
                    if (alpha <= 0)
                    {
                        currentState = BubbleState.REMOVAL_STATE;
                    }
                    break;
                case BubbleState.DISAPPEARING:
                    scale -= 0.01f;
                    alpha -= 0.02f;
                    if (alpha <= 0)
                    {
                        currentState = BubbleState.REMOVAL_STATE;
                    }
                    break;
                case BubbleState.REMOVAL_STATE:
                    alpha = 0;
                    break;
            }
        }

        private void updateCounter()
        {
            stayingDurationCtr--;

            if (stayingDurationCtr <= 0 && currentState != BubbleState.POP_STATE)
            {
                this.currentState = BubbleState.DISAPPEARING;

            }
        }

        public void Draw(SpriteBatch sprite)
        {
            if (this.bubbleType != BubbleType.INACTIVE_STATIC_HAND)
            {
                switch (currentState)
                {
                    case BubbleState.APPEARING:
                        sprite.Draw(textureList[BubbleState.NORMAL_STATE], this.pos + referencePoint, null, Color.White * alpha, 0.0f, origin, scale, SpriteEffects.None, 0);
                        break;
                    case BubbleState.NORMAL_STATE:
                    case BubbleState.LOCKED_IN:
                    case BubbleState.HIGHLIGHTED_STATE:
                        //draw highlighted state here
                        updateFlickerValue();

                        sprite.Draw(textureList[currentState], this.pos + referencePoint, null, Color.White * alpha * flickr, 0.0f, origin, scale, SpriteEffects.None, 0);
                        sprite.Draw(progressCircle, this.pos + referencePoint, null, Color.White, 0f, origin, popCounter / (float)popTime * maxScale, SpriteEffects.None, 0f);
                        break;
                    case BubbleState.POP_STATE:
                        sprite.Draw(textureList[BubbleState.HIGHLIGHTED_STATE], this.pos + referencePoint, null, Color.White * alpha, 0.0f, origin, scale, SpriteEffects.None, 0);
                        break;
                    case BubbleState.DISAPPEARING:
                        sprite.Draw(textureList[BubbleState.NORMAL_STATE], this.pos + referencePoint, null, Color.White * alpha, 0.0f, origin, scale, SpriteEffects.None, 0);
                        break;
                    case BubbleState.REMOVAL_STATE:
                        break;
                }
            }
            else
            {
                updateFlickerValue();
                if (currentState != BubbleState.HIGHLIGHTED_STATE)
                {
                    sprite.Draw(textureList[BubbleState.STATIC_INACTIVE], this.pos + referencePoint, null, Color.White * alpha * flickr, 0.0f, new Vector2(50, 50), scale, SpriteEffects.None, 0);
                }
                else
                {
                    sprite.Draw(textureList[BubbleState.HIGHLIGHTED_STATE], this.pos + referencePoint, null, Color.White * alpha * flickr, 0.0f, new Vector2(50, 50), scale, SpriteEffects.None, 0);
                }
            }
        }

        private void updateFlickerValue(){
            if (stayingDurationCtr < 150)
            {
                flickr = (stayingDurationCtr % 10) * 0.1f;
            }
        }

        public Vector2 getAbsolutePos()
        {
            return pos + referencePoint;
        }

        public void setState(BubbleState newState)
        {
            currentState = newState;
        }

        public void setStayingDuration(int duration)
        {
            stayingDuration = duration;
        }

        public void resetPopCounter()
        {
            popCounter = 0;
        }

        public bool isActive()
        {
            return (currentState == BubbleState.HIGHLIGHTED_STATE || currentState == BubbleState.NORMAL_STATE || currentState == BubbleState.LOCKED_IN);
        }

        public void setReferencePoint(Vector2 refPoint)
        {
            this.referencePoint.X = refPoint.X;
            this.referencePoint.Y = refPoint.Y;

            this.collisionBox.X = (int)xPos + (int)this.referencePoint.X - radius;
            this.collisionBox.Y = (int)yPos + (int)this.referencePoint.Y - radius;
        }

        public bool isReadyForRemoval()
        {
            return currentState == BubbleState.REMOVAL_STATE;
        }

        public bool isReadyToPop()
        {
            return this.popCounter == this.popTime - 1;

        }

        public bool wasMissed()
        {
            return currentState == BubbleState.DISAPPEARING && alpha - 0.02f < 0;
        }
    }
}
