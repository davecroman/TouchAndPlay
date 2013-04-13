using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TouchAndPlay.input;

namespace TouchAndPlay.components
{
    class BasicRectangle:BasicComponent
    {
        public enum HoverState
        {
            HOVERED,
            MOUSE_OUT,
        }

        public enum Hor_Orientation
        {
            LEFT,
            CENTER,
            RIGHT,
        }

        public enum Vert_Orientation
        {
            TOP,
            CENTER,
            BOTTOM,
        }

        internal int width;
        internal int height;

        internal Vector2 scale;
        internal Color boxMouseOutColor;
        internal Color boxHoverColor;

        internal HoverState hoverState;

        private string label;
        private SpriteFont labelFont;
        private Vector2 labelPosition;
        private Color labelHoverColor;
        private Color labelMouseOutColor;

        Texture2D basicBox;

        public Rectangle collisionBox;

        private int labelYMargin;
        private int labelXMargin;
        private Hor_Orientation horOrientation;
        private Vert_Orientation vertOrientation;

        public BasicRectangle(float xPos, float yPos, int width, int height, Texture2D boxTexture, SpriteFont labelFont, string label = "", Hor_Orientation horOrientation = Hor_Orientation.CENTER, Vert_Orientation vertOrientation = Vert_Orientation.CENTER, Color? boxMouseOutColor = null, Color? boxHoverColor = null)
        {
            this.xPos = xPos;
            this.yPos = yPos;

            this.labelYMargin = 2;
            this.labelXMargin = 2;

            this.position = new Vector2(xPos, yPos);
            this.scale = new Vector2( width/100f, height/100f);

            this.boxMouseOutColor = boxMouseOutColor.HasValue? boxMouseOutColor.Value:Color.Transparent;
            this.boxHoverColor = boxHoverColor.HasValue? boxHoverColor.Value:Color.Transparent;

            this.collisionBox = new Rectangle((int)xPos, (int)yPos, width, height);

            this.width = width;
            this.height = height;

            this.label = label;
            this.labelFont = labelFont;
            this.labelPosition = new Vector2();

            this.basicBox = boxTexture;

            this.hoverState = HoverState.HOVERED;

            this.labelHoverColor = Color.Black;
            this.labelMouseOutColor = Color.White;

            this.horOrientation = horOrientation;
            this.vertOrientation = vertOrientation;
            setLabelOrientation(horOrientation, vertOrientation, labelXMargin, labelYMargin);
        }

        public void setLabelOrientation(Hor_Orientation horOrientation, Vert_Orientation vertOrientation, int labelXMargin = 2, int labelYMargin = 2)
        {
            switch (vertOrientation)
            {
                case Vert_Orientation.TOP:
                    labelPosition.Y = yPos + labelYMargin;
                    break;
                case Vert_Orientation.CENTER:
                    labelPosition.Y = yPos + (this.height - labelFont.MeasureString(label).Y) / 2f;
                    break;
                case Vert_Orientation.BOTTOM:
                    labelPosition.Y = yPos + (this.height - labelFont.MeasureString(label).Y) - labelYMargin;
                    break;
            }

            switch (horOrientation)
            {
                case Hor_Orientation.LEFT:
                    labelPosition.X = xPos + labelXMargin;
                    break;
                case Hor_Orientation.CENTER:
                    labelPosition.X = xPos + (this.width - labelFont.MeasureString(label).X) / 2f;
                    break;
                case Hor_Orientation.RIGHT:
                    labelPosition.X = xPos + (this.width - labelFont.MeasureString(label).X) - labelXMargin;
                    break;
            }
        }

        public void setBoxHoverEffect(Color hoverColor, Color mouseOutColor)
        {
            this.boxHoverColor = hoverColor;
            this.boxMouseOutColor = mouseOutColor;
        }

        public void setTextHoverEffect(Color hoverColor, Color mouseOutColor)
        {
            this.labelHoverColor = hoverColor;
            this.labelMouseOutColor = mouseOutColor;
        }

        public bool isHovered()
        {
            return hoverState == HoverState.HOVERED;
        }

        public string getLabel()
        {
            return label;
        }

        public override void Update()
        {
            if (MyMouse.isColliding(collisionBox))
            {
                hoverState = HoverState.HOVERED;
            }
            else
            {
                hoverState = HoverState.MOUSE_OUT;
            }
        }

        public override void Draw(SpriteBatch sprite)
        {
            if (hidden) { return; }

            switch (hoverState)
            {
                case HoverState.HOVERED:
                    sprite.Draw(basicBox, position, null, boxHoverColor, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                    sprite.DrawString(labelFont, label, labelPosition, labelHoverColor);
                    break;
                case HoverState.MOUSE_OUT:
                    sprite.Draw(basicBox, position, null, boxMouseOutColor, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                    sprite.DrawString(labelFont, label, labelPosition, labelMouseOutColor);
                    break;
            }
        }

        public void movePosition(int deltaX, int deltaY)
        {
            position.X = position.X + deltaX;
            position.Y = position.Y + deltaY;

            this.xPos = xPos;
            this.yPos = yPos;

            labelPosition.X = labelPosition.X + deltaX;
            labelPosition.Y = labelPosition.Y + deltaY;

            collisionBox.X = collisionBox.X + deltaX;
            collisionBox.Y = collisionBox.Y + deltaY;
        }

        public void setPosition(float xPos, float yPos)
        {
            this.xPos = xPos;
            this.yPos = yPos;

            position.X = xPos;
            position.Y = yPos;

            collisionBox.X = (int)xPos;
            collisionBox.Y = (int)yPos;

            setLabelOrientation(horOrientation, vertOrientation);
        }

        internal void setLabel(string label)
        {
            this.label = label;
        }
    }
}
