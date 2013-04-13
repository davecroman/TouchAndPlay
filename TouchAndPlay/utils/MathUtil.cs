using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TouchAndPlay.utils
{
    class MathUtil
    {
        public static float getDistance(Vector2 pointA, Vector2 pointB)
        {
            return (float)Math.Sqrt(Math.Pow(pointA.X - pointB.X, 2) + Math.Pow(pointA.Y - pointB.Y, 2));
        }

        public static float getPIAngle(Vector2 pointA, Vector2 pointB)
        {
            return (float)Math.Atan2(pointB.Y - pointA.Y, pointB.X - pointA.X);
        }
    }
}
