using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TouchAndPlay.engine.bubbles
{
    class RedBubble
    {
        public enum State
        {
            WARNING,
            GROW,
            ACTIVE,
            DISAPPEARING,
            READY_FOR_REMOVAL,

        }
        private Texture2D texture;

        private State currentState;

        private float alpha;
        private float scale;
        private Vector2 position;
        private Vector2 referencePoint;
        private Vector2 origin;

        private int counter;

        public static int RED_BUBBLE_WARNING_DURATION = 100;
        public static int RED_BUBBLE_STAY_DURATION = 150;

        public Rectangle collisionBox;

        public RedBubble(Texture2D texture, Vector2 position, Vector2 referencePoint)
        {
            this.texture = texture;

            this.position = position;
            this.referencePoint = referencePoint;

            this.origin = new Vector2(texture.Width / 2, texture.Height / 2);

            this.collisionBox = new Rectangle((int)(this.position.X + referencePoint.X - 20), (int)(this.position.Y + referencePoint.Y - 20), 35, 35);
            Initialize();
        }

        private void Initialize()
        {
            this.currentState = State.WARNING;
        }

        public void Update()
        {
            counter++;

            switch (currentState)
            {
                case State.WARNING:
                    scale = 0.10f;
                    alpha = (counter % 20) / 20f;
                    if (counter > RED_BUBBLE_WARNING_DURATION)
                    {
                        currentState = State.GROW;
                        counter = 0;
                        alpha = 1f;
                    }
                    break;
                case State.GROW:
                    if (scale < 0.60f)
                    {
                        scale += 0.05f;
                    }
                    else
                    {
                        currentState = State.ACTIVE;
                    }
                    break;
                case State.ACTIVE:
                    if (counter > RED_BUBBLE_STAY_DURATION)
                    {
                        currentState = State.DISAPPEARING;
                    }
                    break;
                case State.DISAPPEARING:
                    scale -= 0.05f;
                    alpha -= 0.1f;
                    if (alpha < 0)
                    {
                        this.currentState = State.READY_FOR_REMOVAL;
                    }
                    break;
                case State.READY_FOR_REMOVAL:
                    break;
            }

        }

        public void Draw(SpriteBatch sprite)
        {
            sprite.Draw(texture, position + referencePoint, null, Color.White * alpha, 0f, origin, scale, SpriteEffects.None, 0f);
        }

        public bool isReadyForRemoval()
        {
            return currentState == State.READY_FOR_REMOVAL;
        }

        public Vector2 getPos()
        {
            return position;
        }

        public bool isActive()
        {
            return currentState == State.ACTIVE;
        }

        internal void GoAway()
        {
            currentState = State.DISAPPEARING;
        }
    }
}
