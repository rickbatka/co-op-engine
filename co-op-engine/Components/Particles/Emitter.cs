using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Particles
{
    public abstract class Emitter
    {
        public bool IsAlive = true;
        protected TimeSpan duration = TimeSpan.FromMilliseconds(500);
        protected int frequency = 25;
        protected TimeSpan emitTimer = TimeSpan.Zero;

        public virtual void Update(GameTime gameTime)
        {
            duration -= gameTime.ElapsedGameTime;

            if (emitTimer == TimeSpan.Zero)
            {
                EmitParticle();
            }
            
            emitTimer += gameTime.ElapsedGameTime;

            if (emitTimer.Milliseconds >= frequency)
            {
                emitTimer = TimeSpan.Zero;
            }

            if (duration <= TimeSpan.Zero)
            {
                IsAlive = false;
            }
        }

        protected abstract void EmitParticle();
    }
}
