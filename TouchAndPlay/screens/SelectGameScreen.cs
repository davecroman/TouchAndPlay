using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TouchAndPlay.components;
using TouchAndPlay.engine;
using TouchAndPlay.components.gamespecific;

namespace TouchAndPlay.screens
{
    class SelectGameScreen:BasicScreen
    {
        private Texture2D welcomeBanner;
        private ImageButton expandContractBtn;
        private GraphicsDeviceManager graphics;

        private BasicSlider gameSelector;
        private LevelSelector levelSelector;

        private BasicButton selectGameBtn;
        private BasicButton customizeGameBtn;

        private BasicRectangle gameCustomizerHeader;

        private BasicImage rangeClip;
        private BasicImage coordClip;
        private BasicImage precisionClip;

        private Texture2D rangeExerciseClip;
        private Texture2D coordExerciseClip;
        private Texture2D precisionExerciseClip;

        private StageScreen stageScreen;

        private RadioButtonList refShoulderSelection;
        private BasicRectangle cPanelBox;
        private BasicText refShoulderText;
        private BasicButton customizeSaveBtn;
        private BasicButton customizeCancelBtn;
        private RadioButtonList popTimeSelection;
        private BasicText popTimeText;
        private BasicText customizeQuadrantText;
        private RadioButtonList customQuadrantSelection;

        public SelectGameScreen(GraphicsDeviceManager graphics, StageScreen stage)
        {
            this.targetScreen = ScreenState.SELECT_GAME_SCREEN;
            this.stageScreen = stage;
            base.Initialize();
            this.graphics = graphics;
        }

        public override void createScreen()
        {
            setScreenColor(Color.Black);

            addImage(Gallery.BG_MAIN, 0, 0, 1, 0.3f);
            addImage(welcomeBanner, 0, 10);

            //===========================GAME SELECTOR===================================//
            addText(10, 15, "SELECT GAME", Color.White);

            gameSelector = addSlider(GameConfig.APP_WIDTH - 400, 70, 400, 30, 5, "Games", Color.White, Color.Black, BasicSlider.SliderType.VERTICAL_SLIDER, Color.MidnightBlue, Color.DodgerBlue, Color.LightBlue);
            gameSelector.addItem("Range Exercise");
            gameSelector.addItem("Coordination Exercise");
            gameSelector.addItem("Precision Exercise");
            gameSelector.setSelectedItem("Range Exercise");

            rangeClip = addImage(rangeExerciseClip, 5, 70);
            coordClip = addImage(coordExerciseClip, 5, 70);
            precisionClip = addImage(precisionExerciseClip, 5, 70);

            coordClip.hide();
            precisionClip.hide();
            rangeClip.hide();

            selectGameBtn = addButton(GameConfig.APP_WIDTH - 400, 255, 200, 30, "Select", true, StringAlignment.CENTER, Color.White, Color.Black, Color.MidnightBlue, Color.RoyalBlue);
            customizeGameBtn = addButton(GameConfig.APP_WIDTH - 195, 255, 195, 30, "Customize", true, StringAlignment.CENTER, Color.White, Color.Black, Color.MidnightBlue, Color.RoyalBlue);

            //==========================LEVEL SELECTOR==================================//
            levelSelector = addLevelSelector(0, 290, GameConfig.APP_WIDTH, 30, 5, "Select Level", Color.White, Color.Black, BasicSlider.SliderType.VERTICAL_SLIDER, Color.MidnightBlue, Color.DodgerBlue, Color.LightBlue);
            levelSelector.addItem("Level 1");
            levelSelector.addItem("Level 2");
            levelSelector.addItem("Level 3");
            levelSelector.addItem("Level 4");
            levelSelector.addItem("Level 5");
            levelSelector.setSelectedItem("Level 1");

            expandContractBtn = addImageButton(GameConfig.APP_WIDTH - 60, 10, icon_expand, "Expand", StringAlignment.BOTTOM_CENTERED, true, false, Color.White, Color.Black, FontType.CG_12_REGULAR);
            addImageButton(530, 10, icon_back, "Back", StringAlignment.BOTTOM_CENTERED, true, false, null, null, FontType.CG_12_REGULAR);


            //=========================GAME CUSTOMIZER PANEL============================//
            gameCustomizerHeader = addRectangle(0, 290, GameConfig.APP_WIDTH, 30, "Customize Game", null);
            gameCustomizerHeader.setTextHoverEffect(Color.White, Color.White);
            gameCustomizerHeader.setBoxHoverEffect(Color.MidnightBlue, Color.MidnightBlue);

            cPanelBox = addRectangle(0, 320, GameConfig.APP_WIDTH, 125, "",null, BasicRectangle.Hor_Orientation.CENTER, BasicRectangle.Vert_Orientation.CENTER, Color.DodgerBlue, Color.DodgerBlue);

            refShoulderText = addText(15, 330, "Reference Shoulder:", Color.White, FontType.CG_14_REGULAR);
            refShoulderSelection = addRadioButtonList(35, 360, new List<string>() { "Left Shoulder", "Right Shoulder" });
            refShoulderSelection.setSelected(1);

            popTimeText = addText(230, 330, "Pop Time:", Color.White, FontType.CG_14_REGULAR);
            popTimeSelection = addRadioButtonList(250, 360, new List<string>() { "Slow", "Normal", "Fast" });
            popTimeSelection.setSelected(1);

            customizeQuadrantText = addText(400, 330, "Quadrants:", Color.White, FontType.CG_14_REGULAR);
            customQuadrantSelection = addRadioButtonList(420, 360, new List<string>() { "Quadrant I", "Quadrant II", "Quadrant III", "Quadrant IV" });
            customQuadrantSelection.setSelected(1);
            customQuadrantSelection.allowMultipleSelections();

            customizeSaveBtn = addButton(0, 450, GameConfig.APP_WIDTH/2, 30, "Save", true, StringAlignment.CENTER, Color.White, Color.White, Color.MidnightBlue, Color.DodgerBlue);
            customizeCancelBtn = addButton(GameConfig.APP_WIDTH/2 + 5, 450, GameConfig.APP_WIDTH / 2, 30, "Cancel", true, StringAlignment.CENTER, Color.White, Color.White, Color.MidnightBlue, Color.DodgerBlue);
            
            
            hideCustomizePanel();
            
        }

        

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            welcomeBanner = content.Load<Texture2D>("screens/menuscreen/img_welcomebanner");

            rangeExerciseClip = content.Load<Texture2D>("screens/selectgamescreen/rangeExercise");
            coordExerciseClip = content.Load<Texture2D>("screens/selectgamescreen/coordExercise");
            precisionExerciseClip = content.Load<Texture2D>("screens/selectgamescreen/precisionExercise");
        }

        public override void Update()
        {
            base.Update();

            switch (transitionState)
            {
                case TransitionState.TRANSITION_IN:
                    levelSelector.Initialize(getGameType(levelSelector.getSelectedItem()));
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
                                case "Contract":
                                case "Expand":
                                    graphics.ToggleFullScreen();
                                    break;
                                case "Customize":
                                    hideLevelSelector();
                                    showCustomizePanel();
                                    customizeGameBtn.label = "Levels";
                                    break;
                                case "Levels":
                                case "Save":
                                case "Cancel":
                                    showLevelSelector();
                                    hideCustomizePanel();
                                    customizeGameBtn.label = "Customize";
                                    break;
                                case "Select":
                                    GameConfig.CURRENT_LEVEL = getSelectedLevel();

                                    switch (gameSelector.getSelectedItem())
                                    {
                                        case "Range Exercise":
                                            GameConfig.CURRENT_GAME_TYPE = GameType.RANGE_EXERCISE;
                                            break;
                                        case "Coordination Exercise":
                                            GameConfig.CURRENT_GAME_TYPE = GameType.COORD_EXERCISE;
                                            break;
                                        case "Precision Exercise":
                                            GameConfig.CURRENT_GAME_TYPE = GameType.PRECISION_EXERCISE;
                                            break;
                                        
                                    }

                                    List<string> activeQuadrants = customQuadrantSelection.getSelectedList();

                                    stageScreen.setActiveQuadrants(activeQuadrants.Contains("Quadrant I"), activeQuadrants.Contains("Quadrant II"), activeQuadrants.Contains("Quadrant III"), activeQuadrants.Contains("Quadrant IV"));

                                    if (refShoulderSelection.getSelectedItem().Equals("Right Shoulder"))
                                    {
                                        stageScreen.setReferenceJoint(Microsoft.Kinect.JointType.HandRight);
                                    }
                                    else
                                    {
                                        stageScreen.setReferenceJoint(Microsoft.Kinect.JointType.HandLeft);
                                    }

                                    switch (popTimeSelection.getSelectedItem())
                                    {
                                        case "Slow":
                                            GameConfig.SOLOBUBBLE_POP_TIME = 50;
                                            GameConfig.DRAG_BUBBLE_POPCOUNT = 50;
                                            break;
                                        case "Normal":
                                            GameConfig.SOLOBUBBLE_POP_TIME = 30;
                                            GameConfig.DRAG_BUBBLE_POPCOUNT = 30;
                                            break;
                                        case "Fast":
                                            GameConfig.SOLOBUBBLE_POP_TIME = 15;
                                            GameConfig.DRAG_BUBBLE_POPCOUNT = 15;
                                            break;
                                    }

                                    stageScreen.prepareLevel();
                                    targetScreen = ScreenState.STAGE_SCREEN;
                                    transitionState = TransitionState.TRANSITION_OUT;
                                    break;
                            }
                            break;
                        }

                    }
                    break;
                case TransitionState.GO_TO_TARGET_SCREEN:
                    break;
            }

            switch (gameSelector.getSelectedItem())
            {
                case "Range Exercise":
                    coordClip.hide();
                    rangeClip.show();
                    precisionClip.hide();
                    break;
                case "Coordination Exercise":
                    coordClip.show();
                    rangeClip.hide();
                    precisionClip.hide();
                    break;
                case "Precision Exercise":
                    coordClip.hide();
                    rangeClip.hide();
                    precisionClip.show();
                    break;
            }

            if (levelSelector.currentGameType != getGameType(gameSelector.getSelectedItem()))
            {
                levelSelector.Initialize(getGameType(gameSelector.getSelectedItem()));
            }

            updateFullScreenButton();
        }

        public GameType getGameType(string item)
        {
            switch (item)
            {
                case "Range Exercise":
                    return GameType.RANGE_EXERCISE;
                case "Coordination Exercise":
                    return GameType.COORD_EXERCISE;
                case "Precision Exercise":
                    return GameType.PRECISION_EXERCISE;
                default:
                    return GameType.RANGE_EXERCISE;
            }

            
        }

        private void showLevelSelector()
        {
            levelSelector.show();
        }

        private void hideLevelSelector()
        {
            levelSelector.hide();
        }

        private void showCustomizePanel()
        {
            gameCustomizerHeader.show();
            cPanelBox.show();
            refShoulderSelection.show();
            customizeSaveBtn.show();
            customizeCancelBtn.show();
            refShoulderText.show();
            popTimeSelection.show();
            popTimeText.show();
            customQuadrantSelection.show();
            customizeQuadrantText.show();
        }

        private void hideCustomizePanel()
        {
            gameCustomizerHeader.hide();
            cPanelBox.hide();
            refShoulderSelection.hide();
            customizeCancelBtn.hide();
            customizeSaveBtn.hide();
            refShoulderText.hide();
            popTimeSelection.hide();
            popTimeText.hide();
            customQuadrantSelection.hide();
            customizeQuadrantText.hide();
        }

        private int getSelectedLevel()
        {
            return Convert.ToInt32(levelSelector.getSelectedItem().Split()[1]);
        }

        private void updateFullScreenButton()
        {
            if (graphics.IsFullScreen && expandContractBtn.label == "Expand")
            {
                expandContractBtn.changeImage(icon_contract);
                expandContractBtn.changeText("Contract");
            }
            else if (!graphics.IsFullScreen && expandContractBtn.label == "Contract")
            {
                expandContractBtn.changeImage(icon_expand);
                expandContractBtn.changeText("Expand");
            }
        }
    }
}
