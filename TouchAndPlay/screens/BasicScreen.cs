using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TouchAndPlay.components;
using Microsoft.Xna.Framework;
using TouchAndPlay.components.gamespecific;

namespace TouchAndPlay.screens
{
    class BasicScreen
    {
        private Texture2D basicBox;
        internal Texture2D icon_expand;
        internal Texture2D icon_contract;
        internal Texture2D icon_back;
        internal Texture2D icon_save;
        internal Texture2D text_cursor;
        internal Texture2D text_line;
        internal Texture2D arrow_up;
        private Texture2D arrow_down;
        private Texture2D radioButtonChecked;
        private Texture2D radioButtonUnchecked;

        protected Color screenColor;

        protected List<BasicButton> buttonsOnScreen;
        protected List<BasicText> textLabelsOnScreen;

        protected List<BasicComponent> componentsOnScreen;

        private Vector2 screenScale;

        private SpriteFont buttonFontStyle1;
        private SpriteFont buttonFontStyle2;

        internal ScreenState targetScreen;

        internal TransitionState transitionState;
        internal float transitionBoxAlpha;


        public void Initialize()
        {
            if (screenColor == null)
            {
                screenColor = Color.Black;
            }

            buttonsOnScreen = new List<BasicButton>();
            textLabelsOnScreen = new List<BasicText>();
            componentsOnScreen = new List<BasicComponent>();

            this.transitionBoxAlpha = 1.0f;

            this.transitionState = TransitionState.TRANSITION_IN;
        }

        public virtual void LoadContent(ContentManager content)
        {
            this.LoadBasicContent(content);
        }

        internal void LoadBasicContent(ContentManager content)
        {
            basicBox = content.Load<Texture2D>("stageboxes/basic_box_white");
            icon_expand = content.Load<Texture2D>("screens/menuscreen/icon_fullscreen");
            icon_contract = content.Load<Texture2D>("screens/menuscreen/icon_contract");
            icon_back = content.Load<Texture2D>("screens/createprofilescreen/back");
            icon_save = content.Load<Texture2D>("screens/createprofilescreen/save_button");
            text_cursor = content.Load<Texture2D>("text_cursor");
            text_line = content.Load<Texture2D>("text_line");
            arrow_up = content.Load<Texture2D>("arrow_up");
            arrow_down = content.Load<Texture2D>("arrow_down");

            radioButtonChecked = content.Load<Texture2D>("radioChecked");
            radioButtonUnchecked = content.Load<Texture2D>("radioUnchecked");

            buttonFontStyle1 = content.Load<SpriteFont>("fonts/Button_FontStyle1");
            buttonFontStyle2 = content.Load<SpriteFont>("fonts/Button_FontStyle2");

            screenScale = new Vector2(GameConfig.APP_WIDTH / (float)basicBox.Width, GameConfig.APP_HEIGHT / (float)basicBox.Height);
        }

        public virtual void createScreen()
        {
            //override this
        }

        public virtual void Update()
        {   //override this
            this.UpdateComponents();
        }

        internal void UpdateComponents()
        {
            if (transitionState == TransitionState.ON_SCREEN_ACTIVE)
            {
                for (int count = 0; count < componentsOnScreen.Count; count++)
                {
                    componentsOnScreen[count].Update();
                }
            }
        }

        public virtual void Draw(SpriteBatch sprite)
        {
            //draw background color
            sprite.Draw(basicBox, Vector2.Zero, null, screenColor, 0f, Vector2.Zero, screenScale, SpriteEffects.None, 1f);

            //draw all components
            for (int count = 0; count < componentsOnScreen.Count; count++)
            {
                componentsOnScreen[count].Draw(sprite);
            }

            //draw transition box
            DrawTransitionBox(sprite);
        }

        internal void DrawTransitionBox(SpriteBatch sprite)
        {
            switch (transitionState)
            {
                case TransitionState.TRANSITION_IN:
                    transitionBoxAlpha -= 0.05f;
                    if (transitionBoxAlpha <= 0)
                    {
                        transitionState = TransitionState.ON_SCREEN_ACTIVE;
                    }
                    break;
                case TransitionState.ON_SCREEN_ACTIVE:
                    transitionBoxAlpha = 0;
                    return;
                case TransitionState.TRANSITION_OUT:
                    transitionBoxAlpha += 0.05f;
                    if (transitionBoxAlpha >= 1.0f)
                    {
                        transitionState = TransitionState.GO_TO_TARGET_SCREEN;
                    }
                    break;
            }

            sprite.Draw(basicBox, Vector2.Zero, null, Color.Black * transitionBoxAlpha, 0f, Vector2.Zero, screenScale, SpriteEffects.None, 1f);
        }

        /* ==========================================================
         * PUBLIC METHODS
         * ==========================================================
         */

        public void resetTransitionState()
        {
            transitionState = TransitionState.TRANSITION_IN;
            transitionBoxAlpha = 1f;
        }

        public BasicSlider addSlider(int xPos, int yPos, int itemWidth, int itemHeight, int numOfShownItems = 5, string headerLabel = "", Color? itemTextColor = null, Color? itemTextColorHovered = null, BasicSlider.SliderType sliderType = BasicSlider.SliderType.VERTICAL_SLIDER, Color? headerBoxColor = null, Color? mouseOutBoxColor = null, Color? hoverBoxColor = null)
        {
            BasicSlider slider = new BasicSlider(basicBox, arrow_up, arrow_down, xPos, yPos, itemWidth, itemHeight, headerLabel, buttonFontStyle1, buttonFontStyle2, itemTextColor, itemTextColorHovered, numOfShownItems, sliderType, headerBoxColor, mouseOutBoxColor, hoverBoxColor);

            componentsOnScreen.Add(slider);
            return slider;
        }

        public LevelSelector addLevelSelector(int xPos, int yPos, int itemWidth, int itemHeight, int numOfShownItems = 5, string headerLabel = "", Color? itemTextColor = null, Color? itemTextColorHovered = null, BasicSlider.SliderType sliderType = BasicSlider.SliderType.VERTICAL_SLIDER, Color? headerBoxColor = null, Color? mouseOutBoxColor = null, Color? hoverBoxColor = null)
        {
            LevelSelector levelSelector = new LevelSelector(basicBox, arrow_up, arrow_down, xPos, yPos, itemWidth, itemHeight, headerLabel, buttonFontStyle1, buttonFontStyle2, itemTextColor, itemTextColorHovered, numOfShownItems, sliderType, headerBoxColor, mouseOutBoxColor, hoverBoxColor);

            componentsOnScreen.Add(levelSelector);

            return levelSelector;
        }

        public BasicRectangle addRectangle(int xPos, int yPos, int width, int height, string label = "", FontType? labelFont = null, BasicRectangle.Hor_Orientation horOrientation = BasicRectangle.Hor_Orientation.CENTER, BasicRectangle.Vert_Orientation vertOrientation = BasicRectangle.Vert_Orientation.CENTER, Color? mouseOutColor = null, Color? hoverColor = null)
        {
            BasicRectangle rect = new BasicRectangle(xPos, yPos, width, height, basicBox, getSpriteFont(labelFont), label, horOrientation, vertOrientation, mouseOutColor, hoverColor);

            componentsOnScreen.Add(rect);

            return rect;
        }

        public BasicButton addButton(int xPos, int yPos, int width, int height, string label = "", bool showText = true, StringAlignment alignment = StringAlignment.CENTER, Color? textColorOnAway = null, Color? textColorOnHover = null, Color? boxColor = null, Color? boxHoverColor = null)
        {
            BasicButton button = new BasicButton(xPos, yPos, width, height, basicBox, buttonFontStyle1, label, alignment, showText, true, textColorOnHover, textColorOnAway, boxColor, boxHoverColor);

            componentsOnScreen.Add(button);
            buttonsOnScreen.Add(button);

            return button;
        }

        public BasicTextField addTextField(int xPos, int yPos, int width, int height, FontType fontType, string label, string text = "", Color? textColor = null, Color? labelColor = null)
        {
            BasicTextField tf = new BasicTextField(text_cursor, basicBox, text_line, getSpriteFont(fontType), label, text, xPos, yPos, width, height, labelColor, textColor);

            componentsOnScreen.Add(tf);
            return tf;
        }

        public ImageButton addImageButton(int xPos, int yPos, Texture2D image, string label, StringAlignment alignment, bool showTextOnHover = true, bool showTextOnAway = true, Color? textColorOnHover = null, Color? textColorOnAway = null, FontType fontType = FontType.CG_14_REGULAR)
        {
            ImageButton imageButton = new ImageButton(xPos, yPos, image, basicBox, getSpriteFont(fontType), alignment, label, showTextOnHover, showTextOnAway, textColorOnHover.HasValue ? textColorOnHover.Value : Color.White, textColorOnAway.HasValue ? textColorOnAway : Color.Black);

            componentsOnScreen.Add(imageButton);
            buttonsOnScreen.Add(imageButton);

            return imageButton;
        }

        public BasicText addText(int xPos, int yPos, string text, Color color, FontType font = FontType.CG_14_REGULAR)
        {

            BasicText textToAdd = new BasicText(xPos, yPos, getSpriteFont(font), text, color);

            componentsOnScreen.Add(textToAdd);
            textLabelsOnScreen.Add(textToAdd);

            return textToAdd;
        }

        public RadioButtonList addRadioButtonList(int xPos, int yPos, List<string> items)
        {
            RadioButtonList radioButtonList = new RadioButtonList(xPos, yPos, basicBox, radioButtonUnchecked, radioButtonChecked, getSpriteFont(FontType.CG_12_REGULAR), items);

            componentsOnScreen.Add(radioButtonList);

            return radioButtonList;
        }

        private SpriteFont getSpriteFont(FontType? font = null)
        {
            if (font == null)
            {
                return this.buttonFontStyle1;
            }

            switch (font)
            {
                case FontType.CG_14_REGULAR:
                    return this.buttonFontStyle1;
                case FontType.CG_12_REGULAR:
                    return this.buttonFontStyle2;
                default:
                    return this.buttonFontStyle1;
            }
        }

        public BasicText addTextCenteredHorizontal(int yPos, string text, Color color, FontType font = FontType.CG_14_REGULAR)
        {
            BasicText textToAdd = new BasicText(GameConfig.APP_WIDTH / 2, yPos, getSpriteFont(font), text, Color.White, StringAlignment.CENTER);

            componentsOnScreen.Add(textToAdd);
            textLabelsOnScreen.Add(textToAdd);

            return textToAdd;
        }

        public BasicText addTextCenteredVertical(int xPos, string text, Color color)
        {
            BasicText textToAdd = new BasicText(xPos, GameConfig.APP_HEIGHT / 2, buttonFontStyle1, text, Color.White);

            componentsOnScreen.Add(textToAdd);
            textLabelsOnScreen.Add(textToAdd);

            return textToAdd;
        }


        public BasicText addTextCenteredOnScreen(string text, Color color)
        {
            BasicText textToAdd = new BasicText(GameConfig.APP_WIDTH / 2, GameConfig.APP_HEIGHT / 2, buttonFontStyle1, text, Color.White, StringAlignment.CENTER);

            componentsOnScreen.Add(textToAdd);
            textLabelsOnScreen.Add(textToAdd);

            return textToAdd;
        }

        public BasicImage addImage(Texture2D texture, float xPos, float yPos, float scale = 1f, float alpha = 1f)
        {
            BasicImage image = new BasicImage(texture, xPos, yPos, scale, alpha);
            componentsOnScreen.Add(image);

            return image;
        }

        public void setScreenColor(Color color)
        {
            screenColor = color;
        }

        public TransitionState getTransitionState()
        {
            return transitionState;
        }

        public void ClearScreen()
        {

        }


    }
}
