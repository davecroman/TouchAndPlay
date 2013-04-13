using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TouchAndPlay.components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms;
using TouchAndPlay.utils;

namespace TouchAndPlay.screens
{
    class MenuScreen:BasicScreen
    {
        public Texture2D gameLogo;
        //private Texture2D menuScreenBG;
        private Texture2D welcomeLogo;
        private Texture2D startGameIcon;
        private Texture2D statIcon;
        private Texture2D optIcon;
        private Texture2D exitIcon;
        private Texture2D updateProfile;

        private ImageButton expandContractBtn;

        private GraphicsDeviceManager graphics;
        private Game game;

        private BasicText welcomeText;
        private ScoreChart scoreChart;

        public MenuScreen(Color screenColor, GraphicsDeviceManager graphics, Game game)
        {
            this.screenColor = screenColor;
            this.graphics = graphics;
            this.targetScreen = ScreenState.MENU_SCREEN;
            this.game = game;
            this.scoreChart = new ScoreChart();

            Initialize();
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadBasicContent(content);

            gameLogo = content.Load<Texture2D>("logo");
            welcomeLogo = content.Load<Texture2D>("screens/menuscreen/img_welcomebanner");
            startGameIcon = content.Load<Texture2D>("screens/menuscreen/icon_startgame");
            statIcon = content.Load<Texture2D>("screens/menuscreen/icon_statistics");
            optIcon = content.Load<Texture2D>("screens/menuscreen/icon_options");
            exitIcon = content.Load<Texture2D>("screens/menuscreen/icon_exit");
            updateProfile = content.Load<Texture2D>("screens/menuscreen/update_profile");
        
        }

        public override void createScreen(){
            //change background color
            setScreenColor(Color.Black);

            BasicImage img = addImage(Gallery.BG_MAIN, 0, 0, 1f, 0.4f);
            //add images
            addImage(gameLogo, GameConfig.APP_WIDTH / 2 - gameLogo.Width / 2, 60);

            //add Buttons
            addImageButton(0, 10, welcomeLogo, "Change Profile", StringAlignment.LEFT_JUSTIFIED, false, false);
            addImageButton(5, 10, updateProfile, "Change Profile", StringAlignment.BOTTOM_LEFT, true, false, null, null, FontType.CG_12_REGULAR);
            addImageButton(185, 300, startGameIcon, "START GAME", StringAlignment.BOTTOM_CENTERED, true, false);
            addImageButton(260, 300, statIcon, "STATISTICS", StringAlignment.BOTTOM_CENTERED, true, false);
            addImageButton(335, 300, optIcon, "OPTIONS", StringAlignment.BOTTOM_CENTERED, true, false);
            addImageButton(410, 300, exitIcon, "QUIT TAP", StringAlignment.BOTTOM_CENTERED, true, false);

            expandContractBtn = addImageButton(GameConfig.APP_WIDTH - 60, 10, icon_expand, "Expand", StringAlignment.BOTTOM_CENTERED, true, false, Color.White, Color.Black, FontType.CG_12_REGULAR);

            welcomeText = addText(39, 15, "Hello there, " + GameConfig.CURRENT_PROFILE, Color.White, FontType.CG_14_REGULAR);
            addTextCenteredHorizontal(GameConfig.APP_HEIGHT - 20, "Created by Team Itatap Mo. All rights reserved.", Color.White, FontType.CG_12_REGULAR);
        }

        public override void Update()
        {
            base.Update();
            //update button states

            for (int count = 0; count < buttonsOnScreen.Count; count++)
            {
                if (buttonsOnScreen[count].isClicked())
                {
                    switch (buttonsOnScreen[count].label)
                    {
                        case "START GAME":
                            targetScreen = ScreenState.SELECT_GAME_SCREEN;
                            transitionState = TransitionState.TRANSITION_OUT;
                            break;
                        case "STATISTICS":
                            if (!scoreChart.Visible)
                            {
                                scoreChart.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                                scoreChart.TopMost = true;

                                scoreChart.UpdateFromDB();
                                scoreChart.updateBubblesPoppedTab();
                                scoreChart.updateMotionRangeTab();
                                scoreChart.updateScoreTab();
                                scoreChart.ShowDialog();
                            }
                            break;
                        case "OPTIONS":
                            targetScreen = ScreenState.OPTION_SCREEN;
                            transitionState = TransitionState.TRANSITION_OUT;
                            break;
                        case "Change Profile":
                            targetScreen = ScreenState.CREATE_PROFILE_SCREEN;
                            transitionState = TransitionState.TRANSITION_OUT;
                            break;
                        case "QUIT TAP":
                            game.Exit();
                            break;
                        case "Contract":
                        case "Expand":
                            graphics.ToggleFullScreen();
                            break;
                    }
                }
            }

            if (graphics.IsFullScreen && expandContractBtn.label == "Expand")
            {
                expandContractBtn.changeImage(icon_contract);
                expandContractBtn.changeText("Contract");
            }
            else if ( !graphics.IsFullScreen  && expandContractBtn.label == "Contract" ) 
            {
                expandContractBtn.changeImage(icon_expand);
                expandContractBtn.changeText("Expand");
            }

            welcomeText.setLabel("Hello there, " + GameConfig.CURRENT_PROFILE + "!");
            
        }
    }
}
