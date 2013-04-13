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
        List<BasicParticle> particles;

        public int xPos;
        public int yPos;

        public Texture2D particleTexture;

        public BasicEffect(int xPos, int yPos, int particleCount, Texture2D particleTexture, Color color)
        {
            this.xPos = xPos;
            this.yPos = yPos;

            this.particleTexture = particleTexture;

            Initialize(particleCount, color);
        }

        private void Initialize(int particleCount, Color color)
        {
            particles = new List<BasicParticle>();

            for (int count = 0; count < particleCount; count++)
            {
                particles.Add(new BasicParticle(xPos,yPos,particleTexture,color));
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
    }
}
