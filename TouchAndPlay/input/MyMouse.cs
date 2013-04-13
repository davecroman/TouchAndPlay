using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace TouchAndPlay.input
{
    static class MyMouse
    {
        private static MouseState previousMouseState = Mouse.GetState();
        private static MouseState currentMouseState;

        private static Point currentPos = new Point();

        public static void update()
        {
            previousMouseState = currentMouseState;

            currentMouseState = Mouse.GetState();

            currentPos.X = currentMouseState.X;
            currentPos.Y = currentMouseState.Y;
        }

        public static bool leftClicked()
        {
            if (previousMouseState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Released)
            {
                return true;
            }

            return false;
        }

        public static Point getPos()
        {
            return currentPos;
        }

        public static Vector2 getPosVector()
        {
            return new Vector2(currentPos.X, currentPos.Y);
        }

        public static bool rightClicked()
        {
            if (previousMouseState.RightButton == ButtonState.Pressed && currentMouseState.RightButton == ButtonState.Released)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void setPosition(int newX, int newY)
        {
            Mouse.SetPosition(newX, newY);
        }

        public static int getX()
        {
            return currentMouseState.X;
        }

        public static int getY()
        {
            return currentMouseState.Y;
        }

        internal static bool isColliding(Rectangle collisionBox)
        {
            return collisionBox.Contains(currentPos);
        }
    }
}
