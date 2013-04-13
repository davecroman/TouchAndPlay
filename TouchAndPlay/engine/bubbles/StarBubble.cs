using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using TouchAndPlay.utils;
using Microsoft.Xna.Framework;
using TouchAndPlay.effects;

namespace TouchAndPlay.engine.bubbles
{
    class StarBubble
    {
        public Vector2 position;
        private Vector2 origin;

        public float xSpeed;
        public float ySpeed;
        public Rectangle collisionBox;

        private Texture2D starTexture;
        private EffectHandler effectHandler;

        private int effCtr;
        private float rotation;

        public StarBubble(Texture2D starTexture, EffectHandler effectHandler)
        {
            this.starTexture = starTexture;
            this.effectHandler = effectHandler;

            this.origin = new Vector2(starTexture.Width / 2, starTexture.Height / 2);
            randomizePosition();
        }

        private void randomizePosition()
        {
            int edge = Randomizer.random(1, 4);
            int xPos = 0;
            int yPos = 0;

            Vector2 target = new Vector2();

            switch (edge)
            {
                //UP
                case 1:
                    yPos = -GameConfig.BUBBLE_WIDTH;
                    xPos = Randomizer.random(0, GameConfig.APP_WIDTH);
                    target.X = Randomizer.random(0, GameConfig.APP_WIDTH);
                    target.Y = GameConfig.APP_HEIGHT + GameConfig.BUBBLE_WIDTH;
                    break;
                //DOWN
                case 2:
                    yPos = GameConfig.APP_HEIGHT + GameConfig.BUBBLE_WIDTH;
                    xPos = Randomizer.random(0, GameConfig.APP_WIDTH);
                    target.X = Randomizer.random(0, GameConfig.APP_WIDTH);
                    target.Y = -GameConfig.BUBBLE_WIDTH;
                    break;
                //RIGHT
                case 3:
                    xPos = GameConfig.APP_WIDTH + GameConfig.BUBBLE_WIDTH;
                    yPos = Randomizer.random(0, GameConfig.APP_HEIGHT);
                    target.Y = Randomizer.random(0, GameConfig.APP_HEIGHT);
                    target.X = -GameConfig.BUBBLE_WIDTH;
                    break;
                //LEFT
                case 4:
                    xPos = -GameConfig.BUBBLE_WIDTH;
                    yPos = Randomizer.random(0, GameConfig.APP_HEIGHT);
                    target.Y = Randomizer.random(0, GameConfig.APP_HEIGHT);
                    target.X = GameConfig.APP_WIDTH + GameConfig.BUBBLE_WIDTH;
                    break;
            }

            this.position = new Vector2(xPos, yPos);

            float angle = MathUtil.getPIAngle(this.position, target);
            xSpeed = (float)Math.Cos(angle) * 4;
            ySpeed = (float)Math.Sin(angle) * 4;

            collisionBox = new Rectangle(xPos + GameConfig.BUBBLE_WIDTH / 2, yPos + GameConfig.BUBBLE_WIDTH / 2, (int)(GameConfig.BUBBLE_WIDTH * 0.60f), (int)(GameConfig.BUBBLE_WIDTH * 0.60f));
        }

        public void Update()
        {
            position.X += xSpeed;
            position.Y += ySpeed;

            effCtr -= 1;
            rotation += 0.1f;

            //trail handler===========================================
            if (effCtr <= 0)
            {
                effectHandler.addParticleEffect((int)position.X + Randomizer.random(-10, 10), (int)position.Y + Randomizer.random(-10,10), 1, Color.Yellow, 0, 0, false);
                
                effCtr = Randomizer.random(3,5);
            }
            //========================================================

            collisionBox.X = (int)position.X;
            collisionBox.Y = (int)position.Y;
        }

        public void Draw(SpriteBatch sprite)
        {
            sprite.Draw(starTexture, position, null, Color.White * 0.8f, rotation, origin, 0.60f, SpriteEffects.None, 0f );
        }

        public bool hasPassed()
        {
            return position.X > GameConfig.APP_WIDTH + starTexture.Width ||
                   position.X < 0 - starTexture.Width ||
                   position.Y > GameConfig.APP_HEIGHT + starTexture.Height ||
                   position.Y < 0 - starTexture.Height;
         
        }
    }
}
