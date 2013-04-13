using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TouchAndPlay.input;

namespace TouchAndPlay.components
{
    class BasicSlider:BasicComponent
    {
        public enum SliderType{
            VERTICAL_SLIDER,
            HORIZONTAL_SLIDER,
        }

        internal int selectionWidth;
        internal int selectionHeight;

        internal string label;

        internal List<BasicRectangle> itemsOnScreen;
        private Texture2D boxTexture;
        private Texture2D upArrowTexture;
        private Texture2D downArrowTexture;
       
        SliderType sliderType;
        internal SpriteFont headerFont;
        internal SpriteFont itemFont;

        Color headerBoxColor;
        Color hoverBoxColor;
        Color mouseOutBoxColor;
        Color itemTextColor;
        Color itemTextColorHovered;

        internal int firstShownIndex;

        private BasicRectangle header;
        private BasicRectangle bg;
        private BasicRectangle bar;

        private ImageButton arrowUp;
        private ImageButton arrowDown;

        private BasicRectangle currentSelectedItem;

        internal int shownItems;

        private int spaceLength;
        private int barTopY;
       
        public BasicSlider(Texture2D boxTexture, Texture2D upArrow, Texture2D downArrow, int xPos, int yPos, int selectionWidth, int selectionHeight, string label, SpriteFont headerFont, SpriteFont itemFont, Color? itemTextColor = null, Color? itemTextColorHovered = null, int shownItems = 5, SliderType sliderType = SliderType.VERTICAL_SLIDER, Color? headerBoxColor = null, Color? mouseOutBoxColor = null, Color? hoverBoxColor = null){
            this.xPos = xPos;
            this.yPos = yPos;

            this.position = new Vector2(xPos, yPos);

            this.selectionWidth = selectionWidth;
            this.selectionHeight = selectionHeight;

            this.boxTexture = boxTexture;
            this.upArrowTexture = upArrow;
            this.downArrowTexture = downArrow;

            this.label = label;
            this.itemFont = itemFont;
            this.headerFont = headerFont;

            this.shownItems = shownItems;

            this.sliderType = sliderType;

            this.headerBoxColor = headerBoxColor.HasValue? headerBoxColor.Value:Color.Blue;
            this.hoverBoxColor = hoverBoxColor.HasValue? hoverBoxColor.Value:Color.DarkBlue;
            this.mouseOutBoxColor = mouseOutBoxColor.HasValue? mouseOutBoxColor.Value:Color.LightBlue;

            this.itemsOnScreen = new List<BasicRectangle>();
            this.itemTextColor = itemTextColor.HasValue ? itemTextColor.Value : Color.Black;
            this.itemTextColorHovered = itemTextColorHovered.HasValue ? itemTextColorHovered.Value : Color.White;

            this.firstShownIndex = 0;

            Initialize();
        }

        private void Initialize()
        {
            header = new BasicRectangle(xPos, yPos, selectionWidth, selectionHeight, boxTexture, headerFont, label, BasicRectangle.Hor_Orientation.CENTER, BasicRectangle.Vert_Orientation.CENTER, headerBoxColor, headerBoxColor);
            header.setTextHoverEffect(Color.White, Color.White);

            bg = new BasicRectangle(xPos, yPos + selectionHeight, selectionWidth, selectionHeight * shownItems, boxTexture, itemFont);
            bg.setBoxHoverEffect(Color.MidnightBlue * 1.8f, Color.MidnightBlue * 1.8f);

            bar = new BasicRectangle((int)xPos + selectionWidth - upArrowTexture.Width, (int)yPos + selectionHeight + upArrowTexture.Height, upArrowTexture.Width, (int)((shownItems * selectionHeight) / 2f), boxTexture, itemFont, "", BasicRectangle.Hor_Orientation.CENTER, BasicRectangle.Vert_Orientation.CENTER, Color.AliceBlue, Color.RoyalBlue);
            spaceLength = (shownItems*selectionHeight) - upArrowTexture.Height - downArrowTexture.Height - bar.height;
            barTopY = (int)(yPos + selectionHeight + upArrowTexture.Height);

            arrowUp = new ImageButton((int)xPos + selectionWidth - upArrowTexture.Width, (int)yPos + selectionHeight, upArrowTexture, boxTexture, itemFont, StringAlignment.CENTER, "", false, false, null, null);
            arrowDown = new ImageButton((int)xPos + selectionWidth - downArrowTexture.Width, (int)yPos + selectionHeight*(shownItems+1) - downArrowTexture.Height, downArrowTexture, boxTexture, itemFont, StringAlignment.CENTER);
        }

        public void addItem(string itemName)
        {
            if (itemName.Length > 0 && !exists(itemName))
            {
                BasicRectangle newItem = new BasicRectangle(xPos, yPos + (itemsOnScreen.Count + 1 - firstShownIndex) * selectionHeight, selectionWidth - upArrowTexture.Width, selectionHeight, boxTexture, itemFont, itemName, BasicRectangle.Hor_Orientation.CENTER, BasicRectangle.Vert_Orientation.CENTER, mouseOutBoxColor, hoverBoxColor);
                newItem.setTextHoverEffect(itemTextColorHovered, itemTextColor);

                itemsOnScreen.Add(newItem);

                updateBarLocation();
            }
        }

        public bool exists(string itemName)
        {
            for (int i = 0; i < itemsOnScreen.Count; i++)
            {
                if (itemsOnScreen[i].getLabel().Equals(itemName))
                {
                    return true;
                }
            }

            return false;
        }

        private void updateBarLocation()
        {
            if (firstShownIndex + shownItems >= itemsOnScreen.Count)
            {
                bar.setPosition((int)bar.getXPos(), barTopY + spaceLength);
            }
            else
            {
                bar.setPosition(bar.getXPos(), barTopY + ((firstShownIndex) /(float)(itemsOnScreen.Count - shownItems)) * spaceLength);
            }
       }

        public override void Update()
        {
            if (hidden) { return; }

            arrowDown.Update();
            arrowUp.Update();

            for (int i = firstShownIndex; i < firstShownIndex + shownItems && i < itemsOnScreen.Count; i++)
            {
                itemsOnScreen[i].Update();

                if (itemsOnScreen[i].isHovered() && MyMouse.leftClicked())
                {
                    selectItem(itemsOnScreen[i]);
                }
            }

            if (arrowDown.isClicked())
            {
                moveDown();
            }
            else if (arrowUp.isClicked())
            {
                moveUp();
            }

            while (firstShownIndex > Math.Abs(itemsOnScreen.Count - shownItems))
            {
                moveUp();
            }

        }

        public void moveDown()
        {
            if (firstShownIndex + 1 + shownItems <= itemsOnScreen.Count)
            {
                firstShownIndex += 1;

                for (int i = 0; i < itemsOnScreen.Count; i++)
                {
                    itemsOnScreen[i].movePosition(0, -selectionHeight);
                }
            }

            updateBarLocation();
        }

        public void moveUp()
        {
            if (firstShownIndex - 1 >= 0)
            {
                firstShownIndex -= 1;

                for (int i = 0; i < itemsOnScreen.Count; i++)
                {
                    itemsOnScreen[i].movePosition(0, selectionHeight);
                }
            }

            updateBarLocation();
        }

        private void selectItem(BasicRectangle item)
        {
            //restore previous hover effect
            if (currentSelectedItem != null)
            {
                currentSelectedItem.setTextHoverEffect(itemTextColorHovered, itemTextColor);
                currentSelectedItem.setBoxHoverEffect(hoverBoxColor, mouseOutBoxColor);
            }

            currentSelectedItem = item;

            //set new hover effect
            currentSelectedItem.setBoxHoverEffect(Color.White, Color.White);
            currentSelectedItem.setTextHoverEffect(Color.Black, Color.Black);
        }

        public string getSelectedItem()
        {
            if (currentSelectedItem != null)
            {
                return currentSelectedItem.getLabel();
            }
            else
            {
                return null;
            }
        }

        public void clearSelectedItem()
        {
            if (currentSelectedItem != null)
            {
                currentSelectedItem.setTextHoverEffect(itemTextColorHovered, itemTextColor);
                currentSelectedItem.setBoxHoverEffect(hoverBoxColor, mouseOutBoxColor);
                currentSelectedItem = null;
            }
        }

        public void deleteItem(string itemToDelete)
        {
            if (currentSelectedItem != null)
            {
                for (int i = 0; i < itemsOnScreen.Count; i++)
                {
                    if (itemsOnScreen[i].getLabel() == itemToDelete)
                    {
                        if (currentSelectedItem.getLabel() == itemToDelete)
                        {
                            currentSelectedItem = null;
                        }

                        itemsOnScreen.RemoveAt(i);

                        for (int j = i; j < itemsOnScreen.Count; j++)
                        {
                            itemsOnScreen[j].movePosition(0, -selectionHeight);
                        }

                        break;
                    }
                }
            }
        }

        public override void Draw(SpriteBatch sprite)
        {
            header.Draw(sprite);
            bg.Draw(sprite);

            for (int i = firstShownIndex; i < firstShownIndex + shownItems && i < itemsOnScreen.Count; i++)
            {
                itemsOnScreen[i].Draw(sprite);
            }

            arrowUp.Draw(sprite);
            arrowDown.Draw(sprite);

            if (itemsOnScreen.Count > shownItems)
            {
                bar.Draw(sprite);
            }
        }

        internal bool hasSelectedItem()
        {
            return currentSelectedItem != null;
        }

        internal void setSelectedItem(string selection)
        {
            for (int i = 0; i < itemsOnScreen.Count; i++)
            {
                if (itemsOnScreen[i].getLabel() == selection)
                {
                    selectItem(itemsOnScreen[i]);
                }
            }
        }

        internal void setDefaultSelectedItem()
        {
            if (itemsOnScreen.Count > 0)
            {
                selectItem(itemsOnScreen[0]);
            }
   
        }
    }
}
