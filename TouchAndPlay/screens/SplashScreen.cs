using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace TouchAndPlay.screens
{
    class SplashScreen:BasicScreen
    {
        private Texture2D logoTexture;

        private int logoStayingDuration;

        private SplashScreenState currentState;

        private float logoAlpha;
        private Vector2 screenPosition;

        public SplashScreen()
        {
            base.Initialize();
            Init();
        }

        private void Init()
        {
            logoStayingDuration = 50;
            currentState = SplashScreenState.FADING_IN;
            logoAlpha = 0f;

        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadBasicContent(content);

            logoTexture = content.Load<Texture2D>("logo");
            screenPosition = new Vector2(GameConfig.APP_WIDTH / 2 - logoTexture.Width / 2, GameConfig.APP_HEIGHT / 2 - logoTexture.Height / 2);
        }

        public override void Update()
        {
            base.UpdateComponents();

            switch (currentState)
            {
                case SplashScreenState.FADING_IN:
                    logoAlpha += 0.01f;
                    if (logoAlpha >= 1.0f)
                    {
                        currentState = SplashScreenState.SHOWING;
                    }
                    break;
                case SplashScreenState.SHOWING:
                    logoStayingDuration -= 1;
                    if (logoStayingDuration <= 0)
                    {
                        currentState = SplashScreenState.FADING_OUT;
                    }
                    break;
                case SplashScreenState.FADING_OUT:
                    logoAlpha -= .01f;
                    if (logoAlpha <= 0f)
                    {
                        currentState = SplashScreenState.GOTO_NEXT_SCREEN;
                        base.transitionState = TransitionState.TRANSITION_OUT;
                        targetScreen = ScreenState.MENU_SCREEN;
                    }
                    break;
            }
        }

        public override void Draw(SpriteBatch sprite)
        {
            sprite.Draw(logoTexture, screenPosition, Color.White * logoAlpha);

            base.DrawTransitionBox(sprite);
        }

        public SplashScreenState getCurrentState(){
            return currentState;
        }

        internal void Dispose()
        {
            logoTexture = null;
        }
    }
}
