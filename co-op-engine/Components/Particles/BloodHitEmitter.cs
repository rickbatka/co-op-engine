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
        private Vector2 HitRotation;

        public BloodHitEmitter(Vector2 position, Vector2 hitRotation)
        {
            Position = position;
            HitRotation = hitRotation;
            this.duration = TimeSpan.FromMilliseconds(10);
        }

        // this one emits in one big burst
        protected override void EmitParticle()
        {
            int numBloodParticles = MechanicSingleton.Instance.rand.Next(20, 40);
            for (int i = 0; i < numBloodParticles; i++)
            {
                var particle = new Particle(
                    lifetimeMS: 150,
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
                Position.X + MechanicSingleton.Instance.rand.Next(-10, 10), 
                Position.Y + MechanicSingleton.Instance.rand.Next(-10, 10));
        }

        private Vector2 GetEmitVelocity()
        {
            Vector2 vel = HitRotation;
            vel.Normalize();

            // slow it down by 80 - 90%
            vel.X *= (float)MechanicSingleton.Instance.rand.Next(10, 20) / 100f;
            vel.Y *= (float)MechanicSingleton.Instance.rand.Next(10, 20) / 100f;

            return vel;
        }
    }
}
