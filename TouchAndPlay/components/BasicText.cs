using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TouchAndPlay.components
{
    class BasicText:BasicComponent
    {
        SpriteFont fontStyle;
        internal string label;


        private Vector2 stringOrigin;
        private Vector2 stringPosition;

        private StringAlignment alignment;

        private Color color;

        public BasicText(int xPos, int yPos, SpriteFont fontStyle, string label, Color color, StringAlignment alignment = StringAlignment.LEFT_JUSTIFIED)
        {
            this.xPos = xPos;
            this.yPos = yPos;
            //this.position = new Vector2(xPos, yPos);

            this.fontStyle = fontStyle;
            this.label = label;
            this.color = color;

            this.alignment = alignment;

            Initialize();

        }

        private void Initialize()
        {
            switch (alignment)
            {
                case StringAlignment.LEFT_JUSTIFIED:
                    stringOrigin = Vector2.Zero;
                    stringPosition = new Vector2(xPos, yPos);
                    break;
                case StringAlignment.CENTER:
                    stringOrigin = fontStyle.MeasureString(label) / 2;
                    stringPosition = new Vector2(xPos, yPos);
                    break;
            }
            
        }

        public void setLabel(string newLabel)
        {
            label = newLabel;
        }

        public override void Update()
        {
            //do something
        }

        public override void Draw(SpriteBatch sprite)
        {
            if (hidden) { return; }

            sprite.DrawString(fontStyle, label, stringPosition, color, 0f, stringOrigin, 1f, SpriteEffects.None, 0f);
        }
    }
}
