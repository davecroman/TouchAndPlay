using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TouchAndPlay.utils;
using Microsoft.Xna.Framework;

namespace TouchAndPlay.screens
{
    class LineGraph:BasicScreen
    {
        Painter painter;
        List<Vector2> points;

        public LineGraph()
        {
            points = new List<Vector2>();

            for (int i = 1; i <= 7; i++)
            {
                points.Add(new Vector2(30 * i, Randomizer.random(100, 400)));
            }

            painter = new Painter();

        }

        public override void createScreen()
        {
            //create screen here
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sprite)
        {
            base.Draw(sprite);
        }
    }
}
