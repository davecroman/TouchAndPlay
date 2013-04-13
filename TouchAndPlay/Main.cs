using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TouchAndPlay.screens;
using TouchAndPlay.input;
using TouchAndPlay.db;
using TouchAndPlay.db.playerdata;

namespace TouchAndPlay
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Main : Microsoft.Xna.Framework.Game
    {
        const int APP_WIDTH = GameConfig.APP_WIDTH;
        const int APP_HEIGHT = GameConfig.APP_HEIGHT;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KinectManager kinector;

        //define your custom screens here

        Dictionary<ScreenState, BasicScreen> screens;

        private ScreenState currentScreen;
        private Gallery gallery;

        public Main()
        {
            TAPDatabase.setup();

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = APP_WIDTH;
            graphics.PreferredBackBufferHeight = APP_HEIGHT;

            kinector = new KinectManager(APP_WIDTH, APP_HEIGHT, this.graphics);
            //instantiate your custom screens here
            screens = new Dictionary<ScreenState, BasicScreen>();

            gallery = new Gallery();
            
            screens[ScreenState.MENU_SCREEN] = new MenuScreen(Color.Beige, graphics, this);
            screens[ScreenState.CREATE_PROFILE_SCREEN] = new CreateProfileScreen(this.graphics);
            screens[ScreenState.SPLASH_SCREEN] = new SplashScreen();
            screens[ScreenState.STAGE_SCREEN] = new StageScreen(kinector, this.graphics);
            screens[ScreenState.SELECT_GAME_SCREEN] = new SelectGameScreen(this.graphics, (StageScreen)screens[ScreenState.STAGE_SCREEN]);
            screens[ScreenState.OPTION_SCREEN] = new OptionScreen();

            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            
            kinector.Initialize();

            //change tracking mode here
            kinector.setTrackingMode(Microsoft.Kinect.SkeletonTrackingMode.Default);

            currentScreen = ScreenState.MENU_SCREEN;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            kinector.LoadContent(this.Content);
            gallery.LoadContent(this.Content); //always call this before the loop below
            
            foreach (KeyValuePair<ScreenState, BasicScreen> screen in screens)
            {
                screen.Value.LoadContent(this.Content);
                screen.Value.createScreen();
            }

            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
                kinector.Stop();
            }

            UpdateInputDevices();
            UpdateScreenState();

            base.Update(gameTime);
        }

        private void UpdateScreenState()
        {
            foreach (KeyValuePair<ScreenState, BasicScreen> screen in screens)
            {   
                //if the screen is the currently shown screen
                if (screen.Key == currentScreen)
                {
                    //we update the screen
                    screen.Value.Update();

                    //if the user clicked a button that leads to another screen
                    if (screen.Value.targetScreen != screen.Key && screen.Value.transitionState == TransitionState.GO_TO_TARGET_SCREEN)
                    {
                        //we set currentScreen to the new screen
                        currentScreen = screen.Value.targetScreen;

                        //we reset that transition state of the old screen
                        screens[screen.Value.targetScreen].resetTransitionState();
                        screens[screen.Value.targetScreen].targetScreen = screen.Value.targetScreen;

                        //we transition to the target screen
                        screen.Value.transitionState = TransitionState.TRANSITION_OUT;

                        /*
                        if (screen.Value.targetScreen == ScreenState.STAGE_SCREEN)
                        {
                            ((StageScreen)screens[screen.Value.targetScreen]).setState(StageScreenStates.PREPARING);
                        }*/
                    }

                    break;
                }
            }
        }

        private void UpdateInputDevices()
        {
            MyMouse.update();
            MyKeyboard.update();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            foreach (KeyValuePair<ScreenState, BasicScreen> screen in screens)
            {
                if (currentScreen == screen.Key)
                {
                    screen.Value.Draw(spriteBatch);
                }
                
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
