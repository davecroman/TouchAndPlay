using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TouchAndPlay.components;
using Microsoft.Xna.Framework;

namespace TouchAndPlay.screens
{
    class OptionScreen : BasicScreen
    {
        public RadioButtonList scoreTextEffectButton;
        public RadioButtonList particleEffectButton;
        public RadioButtonList bubbleDurationButton;
        public RadioButtonList screenButton;
        public RadioButtonList sfxButton;
        public RadioButtonList musicButton;
        private ImageButton saveButton;
        public OptionScreen()
        {
            base.Initialize();
        }
        public override void createScreen()
        {
            base.createScreen();
            //pwede maglagay ng radioList, using RadioButtonList.cs
            addImage(Gallery.BG_OPTIONS, 0, 0, 1f, 0.4f);
            addText(50, 100, "GAMEPLAY AND GRAPHICS", Color.LightBlue);

            addText(75, 120, "Score Text Effect", Color.LightBlue);
            scoreTextEffectButton = addRadioButtonList(100, 140, new List<string>() { "ENABLED", "DISABLED" });
            scoreTextEffectButton.setSelected("ENABLED");

            addText(75, 200, "Particle Effect", Color.LightBlue);
            particleEffectButton = addRadioButtonList(100, 220, new List<string>() { "ENABLED", "DISABLED" });
            particleEffectButton.setSelected("ENABLED");

            addText(275, 120, "Duration of Bubbles", Color.LightBlue);
            bubbleDurationButton = addRadioButtonList(300, 140, new List<string>() { "LONG", "NORMAL", "SHORT" });
            bubbleDurationButton.setSelected("NORMAL");

            addText(275, 200, "Screen", Color.LightBlue);
            screenButton = addRadioButtonList(300, 220, new List<string>() { "WINDOWED", "FULL" });
            screenButton.setSelected("WINDOWED");

            addText(50, 300, "AUDIO", Color.LightBlue);

            addText(75, 320, "Music", Color.LightBlue);
            musicButton = addRadioButtonList(100, 340, new List<string>() { "ENABLE", "DISABLE" });
            musicButton.setSelected("ENABLE");

            addText(275, 320, "SFX", Color.LightBlue);
            sfxButton = addRadioButtonList(300, 340, new List<string>() { "ENABLE", "DISABLE" });
            sfxButton.setSelected("ENABLE");

            addImageButton(530, 10, icon_back, "Back", StringAlignment.BOTTOM_CENTERED, true, false, null, null, FontType.CG_12_REGULAR);

            saveButton = addImageButton(GameConfig.APP_WIDTH - 60, 10, icon_save, "Save", StringAlignment.BOTTOM_CENTERED, true, false, Color.White, Color.Black, FontType.CG_12_REGULAR);
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            base.LoadContent(content);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sprite)
        {
            base.Draw(sprite);
        }

        public override void Update()
        {
            base.Update();

            switch (transitionState)
            {
                case TransitionState.TRANSITION_IN:
                    break;
                case TransitionState.ON_SCREEN_ACTIVE:
                    for (int count = 0; count < buttonsOnScreen.Count; count++)
                    {
                        if (buttonsOnScreen[count].isClicked())
                        {
                            switch (buttonsOnScreen[count].label)
                            {
                                case "Back":
                                    targetScreen = ScreenState.MENU_SCREEN;
                                    transitionState = TransitionState.TRANSITION_OUT;
                                    break;
                                case "Save":
                                    //update all lels
                                    break;
                            }
                            break;
                        }

                    }
                    break;
                case TransitionState.GO_TO_TARGET_SCREEN:
                    break;
            }

        }
    }


}
