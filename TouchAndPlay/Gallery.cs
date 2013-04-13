using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TouchAndPlay
{
    public class Gallery
    {
        public static Texture2D BG_MAIN;
        public static Texture2D MEDAL_OBTAINED;
        public static Texture2D MEDAL_NOT_EARNED;
        public static Texture2D LIFE_ICON;
        public static Texture2D MEDAL;
        public static Texture2D BG_OPTIONS;
        public static Texture2D BG_STATISTICS;

        public Gallery()
        {
            //nothing
        }

        public void LoadContent(ContentManager content)
        {
            BG_MAIN = content.Load<Texture2D>("bg/main_bg");
            BG_OPTIONS = content.Load<Texture2D>("bg/options_screen");
            BG_STATISTICS = content.Load<Texture2D>("bg/statistics_screen");
            MEDAL_OBTAINED = content.Load<Texture2D>("icons/medal_gold");
            MEDAL_NOT_EARNED = content.Load<Texture2D>("icons/medal_gold_gray");
            LIFE_ICON = content.Load<Texture2D>("icons/life_hands");
            MEDAL = content.Load<Texture2D>("icons/medal");
        }
    }
}
