using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace TouchAndPlay.input
{
    class MyKeyboard
    {
        private static KeyboardState previousKeyboardState = Keyboard.GetState();
        private static KeyboardState currentKeyboardState;

        public static void update()
        {
            previousKeyboardState = currentKeyboardState;

            currentKeyboardState = Keyboard.GetState();

            Keyboard.GetState().IsKeyDown(Keys.A);

        }

        public static bool isKeyPressed( Keys key )
        {
            return previousKeyboardState.IsKeyDown(key) && currentKeyboardState.IsKeyUp(key);
        }

        public static bool isKeyDown(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key);
        }

        /*
        public static Keys lastKeyPressed()
        {


        }
         * */

        public static Keys getPressedKey()
        {
            if (currentKeyboardState.GetPressedKeys().Count() > 0)
            {
                return currentKeyboardState.GetPressedKeys().ElementAt(0);
            }
            else
            {
                return Keys.None;
            }
        }

        public static bool shiftPressed()
        {
            
            return currentKeyboardState.IsKeyDown(Keys.LeftShift) || currentKeyboardState.IsKeyDown(Keys.RightShift); 
        }
    }
}
