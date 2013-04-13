using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TouchAndPlay.input;

namespace TouchAndPlay.components
{
    class BasicButton:BasicComponent
    {
        Texture2D buttonTexture;

        private int width;
        private int height;

        private Vector2 scale; 
        internal Rectangle collisionBox;

        internal ButtonState currentState;

        private int xMargin;
        private int yMargin;

        SpriteFont fontStyle;
        internal string label;
        private Vector2 stringOrigin;
        internal Vector2 stringPosition;
        private StringAlignment alignment;
        private bool showText;

        internal bool showTextOnHover;
        internal bool showTextOnAway;

        private Color textColorOnHover;
        private Color textColorOnAway;
        private Color boxMouseOutColor;
        private Color boxHoverColor;

        public BasicButton(int xPos, int yPos, int width, int height, Texture2D texture, SpriteFont fontStyle, string label="", StringAlignment alignment = StringAlignment.CENTER, bool showTextOnHover = true, bool showTextOnAway = true, Color? textColorOnHover = null, Color? textColorOnAway = null, Color? boxColor = null, Color? boxHoverColor = null)
        {
            buttonTexture = texture;

            this.xPos = xPos;
            this.yPos = yPos;

            this.width = width;
            this.height = height;

            this.label = label;
            this.fontStyle = fontStyle;
            this.alignment = alignment;

            this.xMargin = 5;
            this.yMargin = 5;

            this.showText = showTextOnAway;
            currentState = ButtonState.MOUSE_OUT;

            this.textColorOnHover = textColorOnHover.HasValue? textColorOnHover.Value:Color.White;
            this.textColorOnAway = textColorOnAway.HasValue? textColorOnAway.Value:Color.Black;

            this.boxMouseOutColor = boxColor.HasValue ? boxColor.Value : Color.Transparent;
            this.boxHoverColor = boxHoverColor.HasValue ? boxHoverColor.Value : Color.Transparent;

            this.showTextOnAway = showTextOnAway;
            this.showTextOnHover = showTextOnHover;
            

            Initialize();
        }

        public void Initialize()
        {
            scale = new Vector2(this.width / (float)buttonTexture.Width, this.height / (float)buttonTexture.Height);
            //scale = new Vector2(1, 1);
            position = new Vector2(xPos, yPos);
            collisionBox = new Rectangle((int)xPos, (int)yPos, width, height);

            switch (alignment)
            {
                case StringAlignment.LEFT_JUSTIFIED:
                    stringOrigin = Vector2.Zero;
                    stringPosition = new Vector2(xPos + xMargin, yPos + yMargin);
                    break;
                case StringAlignment.LEFT_CENTERED:
                    stringPosition = new Vector2(xPos + fontStyle.MeasureString(label).X / 2, yPos + yMargin);
                    break;
                case StringAlignment.BOTTOM_CENTERED:
                case StringAlignment.CENTER:
                    stringOrigin = fontStyle.MeasureString(label) / 2;
                    stringPosition = new Vector2(xPos + width / 2, yPos + height / 2);
                    break;
                case StringAlignment.BOTTOM_RIGHT:
                case StringAlignment.RIGHT_JUSTIFIED:
                    stringOrigin = new Vector2(fontStyle.MeasureString(label).X, 0);
                    stringPosition = new Vector2(xPos + width - xMargin, yPos + yMargin);
                    break;
            }
        }

        public override void Update()
        {
            if (hidden){ return; }

            switch( currentState ) {
                case ButtonState.MOUSE_OUT:
                    if (MyMouse.isColliding(collisionBox))
                    {
                        currentState = ButtonState.HOVERED;
                        showText = showTextOnHover;
                    }
                    break;
                case ButtonState.HOVERED:
                    if (!MyMouse.isColliding(collisionBox))
                    {
                        currentState = ButtonState.MOUSE_OUT;
                        showText = showTextOnAway;
                    }
                    break;
  
            }
        }


        public override void Draw(SpriteBatch sprite)
        {
            if (hidden) { return; }

            base.Draw(sprite);
            //draw the box
            switch (currentState)
            {
                case ButtonState.MOUSE_OUT:
                    sprite.Draw(buttonTexture, position, null, boxMouseOutColor, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                    break;
                case ButtonState.HOVERED:
                    sprite.Draw(buttonTexture, position, null, boxHoverColor, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                    //if hovered show text
                    break;
            }

            //draw the text
            if (this.showText)
            {
                switch (currentState)
                {
                    case ButtonState.MOUSE_OUT:
                        sprite.DrawString(fontStyle, label, stringPosition, textColorOnAway, 0f, stringOrigin, 1f, SpriteEffects.None, 0f);
                        break;
                    case ButtonState.HOVERED:
                        sprite.DrawString(fontStyle, label, stringPosition, textColorOnHover, 0f, stringOrigin, 1f, SpriteEffects.None, 0f);
                        break;
                }
            }
                     

            
        }

        public void setHoverEffect(bool showTextOnHover, bool showTextOnAway, Color textColorOnHover, Color textColorOnAway, Color? boxNormalColor, Color? boxHoverColor = null)
        {
            this.showTextOnHover = showTextOnHover;
            this.showTextOnAway = showTextOnAway;
            this.textColorOnAway = textColorOnAway;
            this.textColorOnHover = textColorOnHover;
        }

        internal void changeText(string newLabel)
        {
            this.label = newLabel;
        }

        internal bool isClicked()
        {
            if (hidden)
            {
                return false;
            }
            if (currentState == ButtonState.HOVERED)
            {
                if (MyMouse.leftClicked())
                {
                    return true;
                }
            }

            return false;
        }

        internal bool isHovered()
        {
            return currentState == ButtonState.HOVERED;
        }
    }
}
