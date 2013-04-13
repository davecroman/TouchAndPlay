using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TouchAndPlay
{
    public class GameConfigInstance
    {
        public int BUBBLE_WIDTH;

        public int SETBUBBLE_DURATION;
        public int DRAGBUBBLE_DURATION;
        public int BUBBLE_SOLO_DURATION;

        public float LINE_ALPHA;

        public int DRAG_BUBBLE_INTERVAL;
        public int SOLO_BUBBLE_INTERVAL;
        public int BUBBLE_SET_INTERVAL;

        public int SOLO_BUBBLES_TO_POP;
        public int BUBBLE_SETS_TO_POP;
        public int DRAG_BUBBLES_TO_POP;

        public int MIN_GAP_BETWEEN_BUBBLES;

        public int DRAG_BUBBLE_SPEED;
        public float DRAG_BUBBLE_RADIAN_RANGE;

        public Color DEFAULT_EFFECT_COLOR;

        public int LOCK_COUNT;

        public int DRAG_BUBBLE_POPCOUNT;

        public int BUBBLE_POP_TIME;

        public int MESSAGE_SWITCH_TIME;

        
        public int ANGLE_ADJUST_WAIT_TIME;

        public int RESUME_COUNT;

        public int APP_WIDTH;
        public int APP_HEIGHT;

        public string CURRENT_PROFILE;

        public GameConfigInstance()
        {
            
            this.BUBBLE_WIDTH = GameConfig.BUBBLE_WIDTH;

            this.SETBUBBLE_DURATION = GameConfig.SETBUBBLE_DURATION;
            this.DRAGBUBBLE_DURATION = GameConfig.DRAGBUBBLE_DURATION;
            this.BUBBLE_SOLO_DURATION = GameConfig.BUBBLE_SOLO_DURATION;

            this.LINE_ALPHA = GameConfig.LINE_ALPHA;
            this.DRAG_BUBBLE_INTERVAL = GameConfig.DRAG_BUBBLE_INTERVAL;
            this.DRAG_BUBBLE_INTERVAL = GameConfig.SOLO_BUBBLE_INTERVAL;
            this.DRAG_BUBBLE_INTERVAL =  GameConfig.BUBBLE_SET_INTERVAL;

            this.SOLO_BUBBLES_TO_POP = GameConfig.SOLO_BUBBLES_TO_POP;
            this.BUBBLE_SETS_TO_POP = GameConfig.BUBBLE_SETS_TO_POP;
            this.DRAG_BUBBLES_TO_POP = GameConfig.DRAG_BUBBLES_TO_POP;

            this.MIN_GAP_BETWEEN_BUBBLES = GameConfig.MIN_GAP_BETWEEN_BUBBLES;

            this.LOCK_COUNT = GameConfig.LOCK_COUNT;

            this.DRAG_BUBBLE_POPCOUNT = GameConfig.DRAG_BUBBLE_POPCOUNT;


            this.BUBBLE_POP_TIME = GameConfig.SOLOBUBBLE_POP_TIME;

            this.MESSAGE_SWITCH_TIME = GameConfig.MESSAGE_SWITCH_TIME;
            this.ANGLE_ADJUST_WAIT_TIME = GameConfig.ANGLE_ADJUST_WAIT_TIME;

            this.RESUME_COUNT = GameConfig.RESUME_COUNT;

            this.APP_WIDTH = GameConfig.APP_WIDTH;
            this.APP_HEIGHT = GameConfig.APP_HEIGHT;

            this.CURRENT_PROFILE = GameConfig.CURRENT_PROFILE;
        }
    }
}
