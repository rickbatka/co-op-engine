using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Particles
{
    class Particle
    {
        public bool IsAlive;
        public TimeSpan Lifetime;
        public Vector2 Velocity;
        public Vector2 Position;
        public Rectangle DrawRectangle;

        //@TODO don't touch this constructor! we recycle particles. everything to set them up should happen in Initialize.
        public Particle() { }

        public void Initialize(int lifetimeMS)
        {
            this.IsAlive = true;
            this.Lifetime = TimeSpan.FromMilliseconds(lifetimeMS);
            this.Position = new Vector2(MechanicSingleton.Instance.rand.Next(100, 500), MechanicSingleton.Instance.rand.Next(100, 500));
            this.DrawRectangle = new Rectangle((int)this.Position.X, (int)this.Position.Y, 8, 8);
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
        }
    }
}
