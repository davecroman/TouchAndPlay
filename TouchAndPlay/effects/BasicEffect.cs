using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TouchAndPlay.effects
{
    class BasicEffect
    {
        internal List<BasicParticle> particles;

        public int xPos;
        public int yPos;

        public Texture2D particleTexture;

        public BasicEffect(int xPos, int yPos, int particleCount, Texture2D particleTexture, Color color, float? xVel = null, float? yVel = null, bool allowGravity = true, int maxVel = 5, int minVel = -5)
        {
            this.xPos = xPos;
            this.yPos = yPos;

            this.particleTexture = particleTexture;

            particles = new List<BasicParticle>();

            for (int count = 0; count < particleCount; count++)
            {
                particles.Add(new BasicParticle(xPos, yPos, particleTexture, color, xVel, yVel, allowGravity, maxVel, minVel));
            }
        }


        public void Update()
        {
            for (int count = 0; count < particles.Count; count++)
            {
                particles[count].Update();

                if (particles[count].alpha <= 0)
                {
                    particles.RemoveAt(count--);
                }
            }
        }

        public void Draw(SpriteBatch sprite)
        {
            for (int count = 0; count < particles.Count; count++)
            {
                particles[count].Draw(sprite);
            }
        }

        public bool isReadForRemoval()
        {
            return particles.Count == 0;
        }

        
    }
}
