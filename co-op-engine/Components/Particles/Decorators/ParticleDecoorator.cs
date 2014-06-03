using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Particles.Decorators
{
    class ParticleDecorator : IParticle
    {
        protected IParticle particle;

        public RectangleFloat DrawRectangle { get { return particle.DrawRectangle; } set { particle.DrawRectangle = value; } }
        public Texture2D Texture { get { return particle.Texture; } }
        public Color DrawColor { get { return particle.DrawColor; } }
        public bool IsAlive { get { return particle.IsAlive; } }
        public TimeSpan Lifetime { get { return particle.Lifetime; } set { particle.Lifetime = value; } }
        public Vector2 Velocity { get { return particle.Velocity; } set { particle.Velocity = value; } }
        public Vector2 Position { get { return particle.Position; } set { particle.Position = value; } }
        public int Width { get { return particle.Width; } set { particle.Width = value; } }
        public int Height { get { return particle.Height; } set { particle.Height = value; } }
        public float Transparency { get { return particle.Transparency; } set { particle.Transparency = value; } }
        public float Rotation { get { return particle.Rotation; } set { particle.Rotation = value; } }

        public ParticleDecorator(IParticle particle)
        {
            this.particle = particle;
        }

        public virtual void Begin()
        {
            particle.Begin();
        }

        public virtual void Update(GameTime gameTime)
        {
            particle.Update(gameTime);
        }

        public virtual void End() 
        {
            particle.End();
        }
        public virtual void Draw(SpriteBatch spriteBatch) 
        {
            particle.Draw(spriteBatch);
        }
    }
}
