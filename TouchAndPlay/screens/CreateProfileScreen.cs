using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TouchAndPlay.components;
using TouchAndPlay.db.playerdata;
using TouchAndPlay.db;

namespace TouchAndPlay.screens
{
    class CreateProfileScreen:BasicScreen
    {
        private ImageButton expandContractBtn;

        private GraphicsDeviceManager graphics;

        private Texture2D welcomeLogo;

        private BasicSlider profileSlider;

        private BasicButton selectProfileBtn;
        private BasicButton deleteProfileBtn;
        private BasicButton createProfileBtn;
        private BasicButton confirmNewProfileBtn;

        private BasicButton yesDeleteBtn;
        private BasicButton cancelDeleteBtn;

        private BasicText confirmDeleteTxt;
        private BasicRectangle profileNameToBeDeletedTxt;
        private BasicTextField newProfileTF;

        public CreateProfileScreen(GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
            this.targetScreen = ScreenState.CREATE_PROFILE_SCREEN;
            base.Initialize();
        }

        public override void createScreen()
        {
            setScreenColor(Color.Black);

            addImage(Gallery.BG_MAIN, 0, 0, 1f, 0.4f);
            addImage(welcomeLogo, 0, 10);

            addText(10, 15, "SELECT PROFILE", Color.White);
            

            expandContractBtn = addImageButton(GameConfig.APP_WIDTH - 60, 10, icon_expand, "Expand", StringAlignment.BOTTOM_CENTERED, true, false, Color.White, Color.Black, FontType.CG_12_REGULAR);

            addImageButton(530, 10, icon_back, "Back", StringAlignment.BOTTOM_CENTERED, true, false, null, null, FontType.CG_12_REGULAR);

            //addRectangle(20, 100, 50, 100, "Sample", FontType.CG_14_REGULAR, BasicRectangle.Hor_Orientation.CENTER, BasicRectangle.Vert_Orientation.BOTTOM, Color.Black, Color.White);

            profileSlider = addSlider(0, 100, GameConfig.APP_WIDTH, 30, 6, "PROFILES", Color.White, Color.Black, BasicSlider.SliderType.VERTICAL_SLIDER, Color.MidnightBlue, Color.DodgerBlue, Color.LightBlue);

            for (int index = 0; index < TAPDatabase.playerProfiles.Count; index++)
            {
                profileSlider.addItem(TAPDatabase.playerProfiles[index].username);
            }

            profileSlider.setSelectedItem(GameConfig.CURRENT_PROFILE);

            selectProfileBtn = addButton(5, 330, 200, 40, "SELECT PROFILE", true, StringAlignment.CENTER, Color.White, Color.Black, Color.MidnightBlue, Color.RoyalBlue);
            
            deleteProfileBtn = addButton(220, 330, 200, 40, "DELETE PROFILE", true, StringAlignment.CENTER, Color.White, Color.Black, Color.MidnightBlue, Color.RoyalBlue);
            createProfileBtn = addButton(430, 330, 200, 40, "NEW PROFILE", true, StringAlignment.CENTER, Color.White, Color.Black, Color.MidnightBlue, Color.RoyalBlue);
            
            confirmNewProfileBtn = addButton(430, 330, 200, 40, "CREATE", true, StringAlignment.CENTER, Color.White, Color.Black, Color.MidnightBlue, Color.RoyalBlue);
            confirmNewProfileBtn.hide();

            newProfileTF = addTextField(120, 330, 310, 40, FontType.CG_14_REGULAR, "ENTER NAME", "", Color.Black, Color.White);
            newProfileTF.hide();

            yesDeleteBtn = addButton(330, 380, 200, 40, "YES",true,StringAlignment.CENTER, Color.White, Color.Black, Color.MidnightBlue, Color.RoyalBlue);
            cancelDeleteBtn = addButton(120, 380, 200, 40, "CANCEL", true, StringAlignment.CENTER, Color.White, Color.Black, Color.MidnightBlue, Color.RoyalBlue);

            profileNameToBeDeletedTxt = addRectangle(0,343, GameConfig.APP_WIDTH, 30, "Profile Name", FontType.CG_14_REGULAR, BasicRectangle.Hor_Orientation.CENTER, BasicRectangle.Vert_Orientation.CENTER, Color.White, Color.White);
            profileNameToBeDeletedTxt.setBoxHoverEffect(Color.RoyalBlue, Color.RoyalBlue);
            profileNameToBeDeletedTxt.setTextHoverEffect(Color.Black, Color.Black);
            confirmDeleteTxt = addTextCenteredHorizontal(330, "Are you sure you want to delete this profile?",Color.White, FontType.CG_14_REGULAR);

            yesDeleteBtn.hide();
            cancelDeleteBtn.hide();
            profileNameToBeDeletedTxt.hide();
            confirmDeleteTxt.hide();
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            base.LoadContent(content);

            welcomeLogo = content.Load<Texture2D>("screens/menuscreen/img_welcomebanner");
        }

        public override void Update()
        {
            base.Update();

            switch (transitionState)
            {
                case TransitionState.TRANSITION_IN:
                    profileSlider.setSelectedItem(GameConfig.CURRENT_PROFILE);
                    newProfileTF.hide();
                    confirmNewProfileBtn.hide();
                    updateFullScreenButton();
                    updateProfileSliderBtns();                  
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
                                    hideDeleteConfirmationPane();
                                    break;
                                case "Contract":
                                case "Expand":
                                    graphics.ToggleFullScreen();
                                    updateFullScreenButton();
                                    break;
                                case "SELECT PROFILE":
                                    GameConfig.CURRENT_PROFILE = profileSlider.getSelectedItem();
                                    targetScreen = ScreenState.MENU_SCREEN;
                                    transitionState = TransitionState.TRANSITION_OUT;
                                    TAPDatabase.saveGameConfig();
                                    break;
                                case "DELETE PROFILE":
                                    selectProfileBtn.hide();
                                    deleteProfileBtn.hide();
                                    createProfileBtn.hide();
                                    yesDeleteBtn.show();
                                    cancelDeleteBtn.show();
                                    confirmDeleteTxt.show();
                                    profileNameToBeDeletedTxt.setLabel(profileSlider.getSelectedItem());
                                    profileNameToBeDeletedTxt.show();
                                    break;
                                case "NEW PROFILE":
                                    profileSlider.clearSelectedItem();
                                    createProfileBtn.hide();
                                    confirmNewProfileBtn.show();
                                    newProfileTF.show();
                                    newProfileTF.setAsActive();
                                    break;
                                case "CREATE":
                                    string newItem = newProfileTF.getText();
                                    if (newItem.Length > 0)
                                    {
                                        profileSlider.addItem(newItem);
                                        profileSlider.setSelectedItem(newItem);
                                        newProfileTF.clearText();
                                        confirmNewProfileBtn.hide();
                                        profileSlider.moveDown();
                                        TAPDatabase.saveProfile(new PlayerProfile(newItem));
                                    }
                                    break;
                                case "CANCEL":
                                    hideDeleteConfirmationPane();
                                    break;
                                case "YES":
                                    string item = profileSlider.getSelectedItem();
                                    profileSlider.deleteItem( item );
                                    TAPDatabase.deleteProfile(item);
                                    if (profileSlider.exists(GameConfig.CURRENT_PROFILE))
                                    {
                                        profileSlider.setSelectedItem(GameConfig.CURRENT_PROFILE);
                                    }
                                    else
                                    {
                                        profileSlider.setDefaultSelectedItem();
                                        GameConfig.CURRENT_PROFILE = profileSlider.getSelectedItem();
                                        TAPDatabase.saveGameConfig();
                                    }
                                    hideDeleteConfirmationPane();
                                    break;
                            }
                            break;
                        }

                    }
                    break;
                case TransitionState.GO_TO_TARGET_SCREEN:
                    profileSlider.clearSelectedItem();
                    break;
            }

            updateProfileSliderBtns();

            if (!confirmNewProfileBtn.hidden && newProfileTF.enterKeyPressed())
            {
                string newItem = newProfileTF.getText();
                if (newItem.Length > 0)
                {
                    profileSlider.addItem(newItem);
                    profileSlider.setSelectedItem(newItem);
                    newProfileTF.clearText();
                    confirmNewProfileBtn.hide();
                    profileSlider.moveDown();
                    TAPDatabase.saveProfile(new PlayerProfile(newItem));
                }
            }
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

        private void hideDeleteConfirmationPane()
        {
            yesDeleteBtn.hide();
            cancelDeleteBtn.hide();
            profileNameToBeDeletedTxt.hide();
            confirmDeleteTxt.hide();
        }

        private void updateProfileSliderBtns()
        {
            if (profileSlider.hasSelectedItem() && confirmDeleteTxt.hidden)
            {
                selectProfileBtn.show();
                deleteProfileBtn.show();
                createProfileBtn.show();

                confirmNewProfileBtn.hide();
                newProfileTF.hide();

            }
            else
            {
                selectProfileBtn.hide();
                deleteProfileBtn.hide();
            }
        }
    }
}
