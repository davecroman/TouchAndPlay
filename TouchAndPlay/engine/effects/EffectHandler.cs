using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace TouchAndPlay.effects
{
    class EffectHandler
    {
        List<BasicEffect> effectsOnScreen;

        Texture2D basicParticleTexture;

        public EffectHandler()
        {
            Initialize();
        }

        public void Initialize()
        {
            effectsOnScreen = new List<BasicEffect>();
        }

        public void LoadContent(ContentManager content)
        {
            basicParticleTexture = content.Load<Texture2D>("effects/basic_particle");
        }

        public void addEffect(int xPos, int yPos, int particleCount, Color? color= null)
        {

            effectsOnScreen.Add(new BasicEffect(xPos, yPos, particleCount, basicParticleTexture, color==null?GameConfig.DEFAULT_EFFECT_COLOR:color.Value));
        }

        public void Update()
        {
            for (int count = 0; count < effectsOnScreen.Count; count++)
            {
                effectsOnScreen[count].Update();
            }
        }

        public void Draw(SpriteBatch sprite)
        {
            for (int count = 0; count < effectsOnScreen.Count; count++)
            {
                effectsOnScreen[count].Draw(sprite);
            }
        }
    }
}
