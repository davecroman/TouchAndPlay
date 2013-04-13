using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TouchAndPlay.components
{
    class BasicComponent
    {
        protected float xPos;
        protected float yPos;

        protected Vector2 position;

        public bool hidden;

        public Vector2 getPos()
        {
            return position;
        }

        public float getXPos()
        {
            return xPos;
        }

        public float getYPos()
        {
            return yPos;
        }

        public virtual void Update()
        {
            throw new NotImplementedException();
        }

        public virtual void Draw(SpriteBatch sprite)
        {
            //throw new NotImplementedException();
        }

        public void hide()
        {
            hidden = true;
        }

        public void show()
        {
            hidden = false;
        }
    }
}
