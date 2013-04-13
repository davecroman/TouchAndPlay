using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TouchAndPlay.components
{
    class BasicImage:BasicComponent
    {
        private Texture2D imageTexture;
        private float scale;

        public float alpha;

        public BasicImage(Texture2D imageTexture, float xPos, float yPos, float scale = 1.0f, float alpha = 1.0f)
        {
            this.imageTexture = imageTexture;
            this.xPos = xPos;
            this.yPos = yPos;
            this.scale = scale;
            this.alpha = alpha;

            this.position = new Vector2(xPos, yPos);
        }

        public override void Update()
        {


        }

        public override void Draw(SpriteBatch sprite)
        {
            if (hidden) { return; }

            sprite.Draw(imageTexture, position, null, Color.White * alpha, 0f, Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        public void setX(float xNew)
        {
            this.xPos = xNew;
            position.X = xNew;
        }

        public void setY(float yNew)
        {
            this.yPos = yNew;
            position.Y = yNew;
        }

        public float getWidth()
        {
            return imageTexture.Width* scale;
        }
    }
}
