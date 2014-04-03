using co_op_engine.Components.Particles.Decorators;
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
        private GameObject owner;
        private Vector2 HitRotation;

        public BloodHitEmitter(GameObject owner, Vector2 hitRotation)
        {
            this.owner = owner;
            HitRotation = hitRotation;
            this.duration = TimeSpan.FromMilliseconds(175);
        }

        // this one emits in one big burst
        protected override void EmitParticle()
        {
            int numBloodParticles = MechanicSingleton.Instance.rand.Next(1, 5);
            for (int i = 0; i < numBloodParticles; i++)
            {
                var particle = new Particle();
                particle.Lifetime = TimeSpan.FromMilliseconds(MechanicSingleton.Instance.rand.Next(50, 250));
                particle.Position = GetEmitPosition();
                particle.Velocity = GetEmitVelocity();
                particle.DrawColor = Color.Firebrick;

                var withSlowDown = new SlowDownDecorator(particle);

                ParticleEngine.Instance.Add(withSlowDown);
            }
        }

        private Vector2 GetEmitPosition()
        {
            return new Vector2(
                owner.BoundingBox.Center.X + MechanicSingleton.Instance.rand.Next(-10, 10),
                owner.BoundingBox.Center.Y + MechanicSingleton.Instance.rand.Next(-10, 10));
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
