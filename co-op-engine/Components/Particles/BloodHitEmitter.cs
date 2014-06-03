using co_op_engine.Components.Particles.Decorators;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Particles.Emitters
{
    class BloodHitEmitter : Emitter
    {
        private GameObject owner;
        private Vector2 HitRotation;
        private int bloodForceMax = 8;

        public BloodHitEmitter(GameObject owner, Vector2 hitRotation)
        {
            this.owner = owner;
            HitRotation = hitRotation;
            this.duration = TimeSpan.FromMilliseconds(75);
        }

        // this one emits in one big burst
        protected override void EmitParticle()
        {
            int numBloodParticles = MechanicSingleton.Instance.rand.Next(10, 40);
            for (int i = 0; i < numBloodParticles; i++)
            {
                var particle = new Particle();
                //particle.Lifetime = TimeSpan.FromMilliseconds(MechanicSingleton.Instance.rand.Next(50, 250));
                particle.Lifetime = TimeSpan.FromMilliseconds(75);
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
                MechanicSingleton.Instance.rand.Next(owner.BoundingBox.Center.X - 10, owner.BoundingBox.Center.X + 10),
                MechanicSingleton.Instance.rand.Next(owner.BoundingBox.Center.Y - 10, owner.BoundingBox.Center.Y + 10));
        }

        private Vector2 GetEmitVelocity()
        {
            Vector2 vel = HitRotation;
            vel.Normalize();
            
            // speed it up!
            vel.X *= bloodForceMax * (float)MechanicSingleton.Instance.rand.Next(50, 90) / 100f;
            vel.Y *= bloodForceMax * (float)MechanicSingleton.Instance.rand.Next(50, 90) / 100f;

            return vel;
        }
    }
}
