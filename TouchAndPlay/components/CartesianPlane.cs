using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TouchAndPlay.utils
{
    class CartesianPlane:TouchAndPlay.components.BasicComponent
    {
        private List<Vector2> redPoints;
        private List<Vector2> bluePoints;

        //the coordinates of the cartesian plane's origin, i.e., (0,0)
        private Vector2 origin;

        public CartesianPlane(Vector2 origin, List<Vector2> redPoints, List<Vector2> bluePoints)
        {
            this.origin = origin;
            this.redPoints = redPoints;
            this.bluePoints = bluePoints;
        }

        public void LoadContentent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            //load your content here
            
        }

        public override void Update()
        {

        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sprite)
        {
            //draw cartesian plane here

        }
    }
}