using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TouchAndPlay.components
{
    class RadioButtonList : BasicComponent
    {
        internal enum ListType
        {
            HORIZONTAL,
            VERTICAL,
        }

        private Texture2D radioButtonUnclicked;
        private Texture2D radioButtonClicked;
        private Texture2D basicBox;

        private SpriteFont spriteFont;

        private List<BasicButton> items;

        private List<bool> indexValues;

        private bool multipleAllowed;

        public RadioButtonList(int xPos, int yPos, Texture2D basicBox, Texture2D radioButtonUnclicked, Texture2D radioButtonClicked, SpriteFont spriteFont, List<string> items = null, ListType listType = ListType.VERTICAL)
        {
            this.xPos = xPos;
            this.yPos = yPos;

            this.radioButtonClicked = radioButtonClicked;
            this.radioButtonUnclicked = radioButtonUnclicked;

            this.basicBox = basicBox;

            this.spriteFont = spriteFont;

            

            this.multipleAllowed = false;

            Initialize(items);
        }

        private void Initialize(List<string> items)
        {
            this.items = new List<BasicButton>();
            this.indexValues = new List<bool>();

            if (items != null)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    int width = (int)spriteFont.MeasureString(items[i]).X;
                    int height = (int)spriteFont.MeasureString(items[i]).Y;
                    BasicButton button = new BasicButton((int)xPos, (int)yPos + i * height, width, height, basicBox, spriteFont, items[i], StringAlignment.CENTER, true, true, Color.White, Color.White * 0.8f);

                    this.items.Add(button);

                    indexValues.Add(false);
                }
            }
        }

        public void setList(List<string> items)
        {
            Initialize(items);
        }

        public void setSelected(string item)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].label.Equals(item))
                {
                    indexValues[i] = true;
                }
            }
        }

        public void setSelected(int index)
        {
            if (multipleAllowed)
            {
                indexValues[index] = !indexValues[index];
            }
            else
            {
                for (int i = 0; i < indexValues.Count; i++)
                {
                    indexValues[i] = false;
                }

                indexValues[index] = true;
            }
        }

        public string getSelectedItem()
        {
            for (int index = 0; index < indexValues.Count; index++)
            {
                if (indexValues[index])
                {
                    return items[index].label;
                }
            }

            return null;
        }

        public List<string> getSelectedList()
        {
            List<string> itemsSelected = new List<string>();

            for (int index = 0; index < indexValues.Count; index++)
            {
                if (indexValues[index])
                {
                    itemsSelected.Add(items[index].label);
                }
            }

            return itemsSelected;
        }

        public void allowMultipleSelections()
        {
            multipleAllowed = true;
        }

        public override void Update()
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].Update();

                if (items[i].isClicked())
                {
                    setSelected(i);
                }
            }
        }

        public override void Draw(SpriteBatch sprite)
        {
            if (hidden) { return; }

            if (items != null)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (!indexValues[i])
                    {
                        if (!items[i].isHovered())
                        {
                            sprite.Draw(radioButtonUnclicked, new Vector2(xPos - radioButtonUnclicked.Width * 0.6f, yPos + i * spriteFont.MeasureString("l").Y), null, Color.White * 0.7f, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
                        }
                        else
                        {
                            sprite.Draw(radioButtonUnclicked, new Vector2(xPos - radioButtonUnclicked.Width * 0.6f, yPos + i * spriteFont.MeasureString("l").Y), null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
                        }
                    }
                    else
                    {
                        sprite.Draw(radioButtonClicked, new Vector2(xPos - radioButtonUnclicked.Width * 0.6f, yPos + i * spriteFont.MeasureString("l").Y), null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
                    }

                    items[i].Draw(sprite);
                }

            }
        }
    }


}