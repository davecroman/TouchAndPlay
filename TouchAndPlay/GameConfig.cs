using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TouchAndPlay.engine;

namespace TouchAndPlay
{
    public class GameConfig
    {
        public const int BUBBLE_WIDTH = 100;
        public const float LINE_ALPHA = 0.60f;

        /* ================== APP SIZE DETAILS ===========================*/
        public const int APP_WIDTH = 640;
        public const int APP_HEIGHT = 480;

        /* ===================== GAME VARIABLES ======================== */
        public const int SETBUBBLE_DURATION = 1200;
        public const int DRAGBUBBLE_DURATION = 1200;
        public static int BUBBLE_SOLO_DURATION = 300;

        public const int DRAG_BUBBLE_INTERVAL = 1200;
        public const int SOLO_BUBBLE_INTERVAL = 300;
        public const int BUBBLE_SET_INTERVAL = 700;

        public const int SOLO_BUBBLES_TO_POP = 30;
        public const int BUBBLE_SETS_TO_POP = 5;
        public const int DRAG_BUBBLES_TO_POP = 60;

        public static int DRAG_BUBBLE_POPCOUNT = 30;
        public static int SOLOBUBBLE_POP_TIME = 60;

        public const int LOCK_COUNT = 10;

        public const int BUBBLE_SCORE = 5;

        public const int MIN_GAP_BETWEEN_BUBBLES = 5;

        /*==================PROFILE VARIABLES======================*/

        public static string CURRENT_PROFILE = "Guest";


        /*=================EFFECT CONSTANTS=====================*/
        public const int SCORE_TEXT_DURATION = 100;
 
        
        /* =============== UI vars ======================= */
        public static int MESSAGE_SWITCH_TIME = 40;
        public static int ANGLE_ADJUST_WAIT_TIME = 70;
        public static int RESUME_COUNT = 320;

        public static int CURRENT_LEVEL = 1;
        public static GameType CURRENT_GAME_TYPE = GameType.COORD_EXERCISE;

        

        
    }
}
