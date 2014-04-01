using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Particles
{
    public class Particle
    {
        public bool IsAlive;
        public TimeSpan Lifetime;
        public Vector2 Velocity;

        private Vector2 _position;
        public Vector2 Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                if (DrawRectangle != null)
                {
                    DrawRectangle.X = (int)_position.X;
                    DrawRectangle.Y = (int)_position.Y;
                }
            }
        }
        public Rectangle DrawRectangle;

        public Color DrawColor = Color.White;

        public Particle(int lifetimeMS, Vector2 position, Vector2 velocity) 
        {
            IsAlive = true;
            Lifetime = TimeSpan.FromMilliseconds(lifetimeMS);
            Position = position;
            DrawRectangle = new Rectangle((int)this.Position.X, (int)this.Position.Y, 3, 3);
            Velocity = velocity;
        }

        public void Update(GameTime gameTime)
        {
            if (Lifetime > TimeSpan.Zero)
            {
                Lifetime -= gameTime.ElapsedGameTime;
            }

            if (Lifetime <= TimeSpan.Zero)
            {
                Lifetime = TimeSpan.Zero;
                IsAlive = false;
            }

            Position += Velocity;
        }
    }
}
