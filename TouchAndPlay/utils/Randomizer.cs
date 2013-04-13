using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TouchAndPlay.utils
{
    class Randomizer
    {
        static Random rand = new Random();

        public static Vector2 createRandomPoint()
        {
            return new Vector2(rand.Next(100,500), rand.Next(100,400));
        }

        public static int random(int minValue, int maxValue)
        {

            return rand.Next(minValue, maxValue);
        }

        public static double randomRadian()
        {
            return (rand.Next(0, 4));
        }

        public static List<Vector2> getEvenListOfPoints(float radius, float rDelta, float cDelta)
        {
            List<Vector2> points = new List<Vector2>();

            points.Add(Vector2.Zero);
            //approximate number of circles
            int rIntervals = (int)(radius / rDelta);
            float rDeltaAdjusted = (float) radius / rIntervals;

            for (int r = 1; r <= rIntervals; r++)
            {
                double c = 2 * Math.PI * (r * rDeltaAdjusted);

                int cIntervals = (int)(c / cDelta);
                //float cDeltaAdjusted = (float) c / cIntervals;

                float tDelta = (3.14f * 2) / cIntervals;
                
                float startDelta = (float) (Randomizer.rand.NextDouble() * 3.14f );

                for (int t = 1; t <= cIntervals; t++)
                {
                    float xPos = (float) (Math.Cos(t*tDelta + startDelta)*(r*rDeltaAdjusted));
                    float yPos = (float) (Math.Sin(t*tDelta + startDelta)*(r*rDeltaAdjusted));
                    points.Add(new Vector2(xPos, yPos));
                }

            }
            
            return points;
        }

        public static List<Vector2> getEvenListOfPoints(float radius, int n)
        {
            List<Vector2> points = new List<Vector2>();

            int count = 0;

            float d = radius * 2;

            while (count < n)
            {
                float xPos = (float) rand.NextDouble() * d;
                float yPos = (float) rand.NextDouble() * d;

                if (MathUtil.getDistance(new Vector2(radius,radius), new Vector2(xPos, yPos)) <= radius)
                {
                    points.Add(new Vector2(xPos, yPos));
                    count++;
                }

            }
            return points;
        }
    }
}
