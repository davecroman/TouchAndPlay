using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TouchAndPlay.utils
{
    class Painter
    {
        private Color DEFAULT_STROKE_COLOR = Color.Black;

        private float strokeLength;
        private Color defaultStrokeColor;

        private static Texture2D texture;

        public Painter()
        {
            
            //initialize default stroke length
            strokeLength = 1;

            //initialize default stroke color
            defaultStrokeColor = Color.Black;
        }

        /// <summary>
        /// Draws a line from pointA to pointB in the specified SpriteBatch instance
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="pointA"> Starting point of the line</param>
        /// <param name="pointB"> End point of the line </param>
        /// <param name="strokeLength"> Thickness of the line</param>
        /// <param name="color"> Color of the line</param>
        public void DrawLine(SpriteBatch batch, Vector2 pointA, Vector2 pointB, float strokeLength = 1, Color? color = null)
        {
            if (texture == null)
            {
                texture = new Texture2D(batch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                texture.SetData(new[] { Color.White });
            }

            float angle = (float)Math.Atan2(pointB.Y - pointA.Y, pointB.X - pointA.X);
            float length = Vector2.Distance(pointA, pointB);

            color = color.HasValue ? color : defaultStrokeColor;

            batch.Draw(texture, pointA, null, color.Value, angle, Vector2.Zero, new Vector2(length, strokeLength), SpriteEffects.None, 0f);

            //applies strokeLength
            for (int i = 1; i < strokeLength; i++)
            {
                batch.Draw(texture, new Vector2(pointA.X + i, pointA.Y), null, color.Value, angle, Vector2.Zero, new Vector2(length, 1.0f), SpriteEffects.None, 0f);
                batch.Draw(texture, new Vector2(pointA.X - i, pointA.Y), null, color.Value, angle, Vector2.Zero, new Vector2(length, 1.0f), SpriteEffects.None, 0f);
                batch.Draw(texture, new Vector2(pointA.X, pointA.Y + i), null, color.Value, angle, Vector2.Zero, new Vector2(length, 1.0f), SpriteEffects.None, 0f);
                batch.Draw(texture, new Vector2(pointA.X, pointA.Y - i), null, color.Value, angle, Vector2.Zero, new Vector2(length, 1.0f), SpriteEffects.None, 0f);
            }

        }

        /// <summary>
        /// Sets the thickness of the stroke
        /// </summary>
        /// <param name="strokeLength"></param>
        public void setStrokeLength(float strokeLength)
        {
            this.strokeLength = strokeLength;
        }



        /// <summary>
        /// Sets the color of the stroke
        /// </summary>
        /// <param name="c"></param>
        public void setStrokeColor(Color c)
        {
            this.defaultStrokeColor = c;
        }
    }
}
