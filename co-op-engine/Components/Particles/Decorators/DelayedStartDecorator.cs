using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Particles.Decorators
{
    class DelayedStartDecorator : ParticleDecorator
    {
        private TimeSpan BeginLifetime;
        private int DelayTimeMS;
        Vector2 BeginVelocity;
        bool hasBeenReleased = false;

        public DelayedStartDecorator(IParticle particle) 
            : base(particle){ }

        public override void Begin()
        {
            BeginLifetime = particle.Lifetime;
            BeginVelocity = particle.Velocity;
            particle.Velocity = new Vector2(0, 0);
            float delayPercentage = MechanicSingleton.Instance.rand.Next(50, 100) / 100f;
            DelayTimeMS = (int)(BeginLifetime.TotalMilliseconds * delayPercentage);

            base.Begin();
        }

        public override void Update(GameTime gameTime)
        {
            // hold the particle still for 20% of its lifetime
            if(!hasBeenReleased && particle.Lifetime.TotalMilliseconds <= (0.8f * DelayTimeMS))
            {
                particle.Velocity = BeginVelocity;
                hasBeenReleased = true;
            }
            
            base.Update(gameTime);
        }
    }
}
