using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TouchAndPlay.utils;

namespace TouchAndPlay.screens
{
    class TestScreen:BasicScreen
    {
        private Texture2D dot;
        //private List<Vector2> points;

        private CartesianPlane plane;

        public TestScreen()
        {
            //do not change this
            this.targetScreen = ScreenState.TEST_SCREEN;

            base.Initialize();

            Init();
            
        }

        private void Init()
        {

            List<Vector2> redPoints = new List<Vector2>();
            List<Vector2> bluePoints = new List<Vector2>();

            for (int count = 0; count < 20; count++)
            {
                redPoints.Add(Randomizer.createRandomPoint());
            }

            plane = new CartesianPlane(new Vector2(GameConfig.APP_WIDTH / 2, GameConfig.APP_HEIGHT / 2), redPoints, bluePoints);
        }

        public void createSreen()
        {
            setScreenColor(Color.Green);

            //addComponent(plane);
        }


        public override void LoadContent(ContentManager content)
        {
            base.LoadBasicContent(content);

            dot = content.Load<Texture2D>("effects/basic_particle");
        }

        public override void Update()
        {
            base.UpdateComponents();
        }
    }
}
