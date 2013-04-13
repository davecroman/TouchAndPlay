using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using TouchAndPlay.screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace TouchAndPlay.components
{
	class MyUI
	{
        private Dictionary<StageBox, Texture2D> textures;

        private Vector2 preparationWindowPos;
        //private Vector2 preparationWindowTextPos;

        private Vector2 basicBoxScale;
        private Vector2 basicBoxPos;
        private int basicBoxWidth;
        private int basicBoxHeight;

        private SpriteFont basicFont;
        private SpriteFont notificationHeadingFont;
        private SpriteFont notificationMsgFont;

        private Color notifWindowColor;

        public MyUI(){

            Initialize();
        }

        private void Initialize()
        {
            textures = new Dictionary<StageBox, Texture2D>();
            
            preparationWindowPos = new Vector2(150, 200);

            basicBoxHeight = 120;
            basicBoxWidth = GameConfig.APP_WIDTH;

            notifWindowColor = new Color();
            
            basicBoxPos = new Vector2(0, GameConfig.APP_HEIGHT / 2 - basicBoxHeight / 2);
            
        }

        internal void LoadContent(ContentManager content)
        {
            textures[StageBox.BASIC_BOX] = content.Load<Texture2D>("stageboxes/basic_box_white");

            basicFont = content.Load<SpriteFont>("fonts/BasicFont");
            notificationHeadingFont = content.Load<SpriteFont>("fonts/NotificationHeadingFont");
            notificationMsgFont = content.Load<SpriteFont>("fonts/NotificationMsgFont");

            //do not change this
            basicBoxScale = new Vector2((float)basicBoxWidth / textures[StageBox.BASIC_BOX].Width, (float)basicBoxHeight/textures[StageBox.BASIC_BOX].Height);
        }

        public void writeNotificationWindow(SpriteBatch sprite, string header, string notifiation, StageScreen.StageScreenStates state = StageScreen.StageScreenStates.PAUSED)
        {
            switch( state ){
                case StageScreen.StageScreenStates.PAUSED:
                    notifWindowColor = Color.Tomato;
                    break;
                case StageScreen.StageScreenStates.RESUMING:
                    notifWindowColor = Color.GreenYellow;
                    break;
            }
            sprite.Draw(textures[StageBox.BASIC_BOX], basicBoxPos, null, notifWindowColor * 0.75f, 0f, Vector2.Zero, basicBoxScale, SpriteEffects.None, 0f);
            sprite.DrawString(notificationHeadingFont, header, basicBoxPos + Vector2.One * 20, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            sprite.DrawString(notificationMsgFont, "\n\n" +notifiation, basicBoxPos + Vector2.One * 20, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        public void writeNotificationWindow(SpriteBatch sprite, string header, string notifiation, Color boxColor, Color headingTextColor, Color bodyTextColor)
        {
            
            sprite.Draw(textures[StageBox.BASIC_BOX], basicBoxPos, null, boxColor * 0.75f, 0f, Vector2.Zero, basicBoxScale, SpriteEffects.None, 0f);
            sprite.DrawString(notificationHeadingFont, header, basicBoxPos + Vector2.One * 20, headingTextColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            sprite.DrawString(notificationMsgFont, "\n\n" + notifiation, basicBoxPos + Vector2.One * 20, bodyTextColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
