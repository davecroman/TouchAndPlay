using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TouchAndPlay.input;

namespace TouchAndPlay.components
{
    class ImageButton:BasicButton
    {
        private Texture2D image;
        private Vector2 imagePos;
        private Vector2 imageOrigin;

        public ImageButton(int xPos, int yPos, Texture2D image, Texture2D basicBox, SpriteFont spriteFont, StringAlignment alignment,  string label = "", bool showTextOnHover = true, bool showTextOnAway = true, Color? textColorOnHover = null, Color? textColorOnAway = null)
            :base(xPos, yPos, image.Width, image.Height, basicBox, spriteFont, label, StringAlignment.LEFT_JUSTIFIED, showTextOnHover, showTextOnAway, textColorOnHover, textColorOnAway)
        {
            this.image = image;
            this.imagePos = new Vector2(xPos, yPos);
            this.imageOrigin = new Vector2(image.Width / 2, image.Height / 2);

            base.Initialize();

            base.collisionBox = new Rectangle(xPos, yPos, image.Width, image.Height);

            modifyTextPosition(alignment, spriteFont);
            
        }

        private void modifyTextPosition(StringAlignment alignment, SpriteFont spriteFont)
        {
            switch (alignment)
            {
                case StringAlignment.BOTTOM_CENTERED:
                    base.stringPosition.Y = yPos + image.Height + 5;
                    base.stringPosition.X = xPos - (int)spriteFont.MeasureString(label).X / 2 + image.Width / 2;
                    break;
                case StringAlignment.LEFT_CENTERED:
                    base.stringPosition.X = xPos - (int)spriteFont.MeasureString(label).X/2 + image.Width/2;
                    base.stringPosition.Y = yPos;
                    break;
                case StringAlignment.BOTTOM_LEFT:
                    base.stringPosition.X = xPos;
                    base.stringPosition.Y = yPos + image.Height + 5;
                    break;
                case StringAlignment.BOTTOM_RIGHT:
                    base.stringPosition.X = xPos + image.Width;
                    base.stringPosition.Y = yPos + image.Height + 5;
                    break;
            }
        }

        public override void Update()
        {
            base.Update();
        }

        public void changeImage(Texture2D image)
        {
            this.image = image;
        }

        public override void Draw(SpriteBatch sprite)
        {
            if (hidden) { return; }

            switch (currentState)
            {
                case ButtonState.MOUSE_OUT:
                    sprite.Draw(image, imagePos + imageOrigin, null, Color.White, 0f, imageOrigin, 1f, SpriteEffects.None, 0f);
                    break;
                case ButtonState.HOVERED:
                    sprite.Draw(image, imagePos + imageOrigin, null, Color.White, 0f, imageOrigin, 1.15f, SpriteEffects.None, 0f);
                    break;

            }

            base.Draw(sprite);
            
        }



        
    }
}
