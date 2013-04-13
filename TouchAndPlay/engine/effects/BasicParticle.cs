using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TouchAndPlay.utils;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TouchAndPlay.effects
{
    class BasicParticle
    {
        public float xVel;
        public float yVel;

        public float xPos;
        public float yPos;

        public float alpha;
        

        public Vector2 origin;

        private List<Vector3> trail;
        private Texture2D texture;
        private float scale;

        private Color color;
        private float gravity;

        public BasicParticle(int xPos, int yPos, Texture2D texture, Color color){
            this.xPos = xPos;
            this.yPos = yPos;

            this.texture = texture;
            this.color = color;

            this.gravity = 0.10f;

            Initialize();  
       }

        private void Initialize()
        {
            this.xVel = Randomizer.random(-8, 8);
            this.yVel = Randomizer.random(-8, 8);

            this.origin = new Vector2(texture.Width / 2, texture.Height / 2);
            this.alpha = 1f;
            this.scale = 1f;

            this.trail = new List<Vector3>();
        }

        public void Update()
        {
            xPos += xVel;
            yPos += yVel;

            yVel += gravity;

            alpha -= 0.01f;
            scale -= 0.01f;

            trail.Add(new Vector3(xPos, yPos,alpha));

            for (int count = 0; count < trail.Count; count++)
            {
                //Z is the alpha value
                trail[count] = new Vector3(trail[count].X, trail[count].Y, trail[count].Z - 0.05f);

                if (trail[count].Z <= 0)
                {
                    trail.RemoveAt(count--);
                }
            }
        }

        public void Draw(SpriteBatch sprite)
        {
            sprite.Draw(texture, new Vector2(xPos, yPos), null, color * alpha, 1f,origin, scale,SpriteEffects.None,0f );

            for (int count = 0; count < trail.Count; count++)
            {
                sprite.Draw(texture, new Vector2(trail[count].X, trail[count].Y), null, color * trail[count].Z, 1f, origin, scale, SpriteEffects.None, 0f);
            }
        }
    }
}
