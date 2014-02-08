using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Conduct
{
    abstract class MoverBase : IMovable
    {
        protected GameObject owner;
        protected Vector2 movementDirection;
        public Vector2 MovementDirection { get { return movementDirection; } }

        protected Vector2 position;
        public Vector2 Position { get { return position; } }

        protected int width;
        public int Width { get { return width; } }

        protected int height;
        public int Height { get { return height; } }


        public MoverBase(GameObject owner)
        {
            this.owner = owner;
        }

        abstract public void Update(GameTime gameTime);
        abstract public void Draw(SpriteBatch spriteBatch);
    }
}
