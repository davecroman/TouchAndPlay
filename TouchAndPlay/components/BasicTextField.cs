using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TouchAndPlay.input;

namespace TouchAndPlay.components
{
    class BasicTextField:BasicComponent
    {
        internal enum HoverState
        {
            HOVERED,
            ACTIVE,
            MOUSE_OUT,
        }

        private Texture2D basicBox;
        private Texture2D textCursor;
        private Texture2D textLine;

        private string text;
        private string label;

        private SpriteFont textFont;
        private SpriteFont labelFont;

        private Keys lastPressed;

        private Vector2 labelPosition;
        private Vector2 textPosition;
        private Vector2 scale;
        private Vector2 cursorOrigin;
        private Vector2 cursorPosition;
        private Vector2 textLinePosition;

        private Color textColor;
        private Color labelColor;

        private Rectangle collisionBox;
        private bool isActive;

        private const int blinkDuration = 20;
        private int blinkCounter;
        private float textLineAlpha;

        private HoverState hoverState;

        public BasicTextField(Texture2D textCursor, Texture2D boxTexture, Texture2D textLine, SpriteFont fontStyle, string label, string text, int xPos, int yPos, int width = 100, int height = 25, Color? labelColor = null, Color? textColor = null )
        {
            lastPressed = Keys.None;

            this.xPos = xPos;
            this.yPos = yPos;

            this.label = label;
            this.text = "Sample";

            this.scale = new Vector2(width / 100f, height / 100f);

            this.basicBox = boxTexture;
            this.textCursor = textCursor;
            this.textLine = textLine;
            this.cursorOrigin = new Vector2(textCursor.Width / 2, textCursor.Height / 2);
            this.collisionBox = new Rectangle(xPos, yPos, width, height);

            this.textColor = textColor.HasValue ? textColor.Value : Color.Black;
            this.labelColor = labelColor.HasValue ? labelColor.Value : Color.Black;

            this.labelFont = fontStyle;
            this.textFont = fontStyle;

            this.position = new Vector2(xPos, yPos);
            this.labelPosition = new Vector2(xPos - fontStyle.MeasureString(label).X - 5, yPos + (height - fontStyle.MeasureString("l").Y) / 2f);
            this.textPosition = new Vector2(xPos + 5, yPos + (height - fontStyle.MeasureString("l").Y) / 2f);
            this.cursorPosition = new Vector2(xPos, yPos + 17);
            this.textLinePosition = new Vector2(xPos + fontStyle.MeasureString(this.text).X + 5, yPos + 5);

            this.hoverState = HoverState.MOUSE_OUT;

            this.textLineAlpha = 1f;
        }

        public override void Update()
        {
            if (hidden) { return; }

            if (blinkCounter <= blinkDuration)
            {
                blinkCounter++;
            }
            else
            {
                blinkCounter = 0;
                textLineAlpha = Math.Abs(textLineAlpha - 1);
            }

            if (MyMouse.isColliding(collisionBox))
            {
                if (MyMouse.leftClicked())
                {
                    isActive = true;
                }

                hoverState = HoverState.HOVERED;
            }
            else
            {
                if (MyMouse.leftClicked())
                {
                    isActive = false;
                }

                hoverState = HoverState.MOUSE_OUT;
            }

            if (isActive)
            {
                if (MyKeyboard.isKeyPressed(lastPressed))
                {
                    if (lastPressed >= Keys.A && lastPressed <= Keys.Z)
                    {
                        if (MyKeyboard.shiftPressed())
                        {
                            text += lastPressed.ToString();
                        }
                        else
                        {
                            text += lastPressed.ToString().ToLower();

                        }

                    }
                    else if (lastPressed == Keys.Back)
                    {
                        if (text.Length > 0)
                        {
                            text = text.Substring(0, text.Length - 1);
                        }
                    }
                    else if (lastPressed == Keys.Space)
                    {
                        text += " ";
                    }
                    

                    textLinePosition.X = xPos + textFont.MeasureString(text).X + 5;
                }

            }

            lastPressed = MyKeyboard.getPressedKey();
        }

        public override void Draw(SpriteBatch sprite)
        {
            if (!hidden)
            {
                sprite.DrawString(labelFont, label, labelPosition, labelColor);

                switch (hoverState)
                {
                    case HoverState.HOVERED:
                        cursorPosition.X = MyMouse.getX();
                        sprite.Draw(basicBox, position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                        sprite.Draw(textCursor, cursorPosition, null, Color.White, 0f, cursorOrigin, 1f, SpriteEffects.None, 0f );

                        if (isActive)
                        {
                            sprite.Draw(textLine, textLinePosition, Color.White * textLineAlpha);
                        }
                        break;
                    case HoverState.MOUSE_OUT:
                        if (isActive)
                        {
                            sprite.Draw(basicBox, position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                            sprite.Draw(textLine, textLinePosition, Color.White * textLineAlpha);
                        }
                        else
                        {
                            sprite.Draw(basicBox, position, null, Color.White * 0.6f, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                        }
                        break;
                }

                sprite.DrawString(textFont, text, textPosition, textColor);

            }
        }

        public string getText()
        {
            return text;
        }

        public void clearText()
        {
            text = "";
            textLinePosition.X = xPos + 5;
        }

        public bool enterKeyPressed()
        {
            return lastPressed == Keys.Enter;
        }

        internal void setAsActive()
        {
            isActive = true;
        }
    }
}
