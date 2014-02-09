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
        protected float friction = 0.5f;
        protected float speedLimit = 2000f;
        protected float accelerationModifier = 400f;

        protected GameObject owner;

        protected Vector2 velocity;
        public Vector2 Velocity { get { return velocity; } }

        protected Vector2 acceleration;
        public Vector2 Acceleration { get { return acceleration; } }
        
        protected Vector2 inputMovementVector;
        public Vector2 InputMovementVector { get { return inputMovementVector; } }

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
