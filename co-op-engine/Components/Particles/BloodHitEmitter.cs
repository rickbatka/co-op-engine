using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Particles
{
    class BloodHitEmitter : Emitter
    {
        private Vector2 Position;

        public BloodHitEmitter(Vector2 position)
        {
            Position = position;
            this.duration = TimeSpan.FromMilliseconds(10);
        }

        // this one emits in one big burst
        protected override void EmitParticle()
        {
            int numBloodParticles = MechanicSingleton.Instance.rand.Next(10, 20);
            for (int i = 0; i < numBloodParticles; i++)
            {
                var particle = new Particle(
                    lifetimeMS: 100,
                    position: GetEmitPosition(),
                    velocity: GetEmitVelocity()
                );
                particle.DrawColor = Color.Red;
                ParticleEngine.Instance.Add(particle);
            }
        }

        private Vector2 GetEmitPosition()
        {
            return new Vector2(
                Position.X + MechanicSingleton.Instance.rand.Next(-15, 15), 
                Position.Y + MechanicSingleton.Instance.rand.Next(-15, 15));
        }

        private Vector2 GetEmitVelocity()
        {
            return new Vector2(
                (float)MechanicSingleton.Instance.rand.Next(-3,3) / 10f,
                (float)MechanicSingleton.Instance.rand.Next(-3, 3) / 10f);
        }
    }
}
