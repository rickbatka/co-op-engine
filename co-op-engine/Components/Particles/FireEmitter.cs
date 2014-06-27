using co_op_engine.Components.Particles.Decorators;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Particles
{
    public class FireEmitter : Emitter
    {
        private GameObject owner;
        public FireEmitter(GameObject owner)
        {
            this.owner = owner;
            this.duration = TimeSpan.FromMilliseconds(4000);
            this.frequency = 5;
        }

        protected override void EmitParticle()
        {
            var particle = new Particle();
            particle.Lifetime = TimeSpan.FromMilliseconds(2550);
            particle.Position = GetEmitPosition();
            particle.Velocity = GetVelocity();
            particle.DrawColor = GetColor();

            var withDelayedStart = new DelayedStartDecorator(particle);
            var withVariableSize = new VariableSizeDecorator(withDelayedStart, 1, 7);

            ParticleEngine.Instance.Add(withVariableSize);
        }

        private Color GetColor()
        { 
            float lerpAmount = (float)MechanicSingleton.Instance.rand.Next(0, 100) / 100f;
            float alphaAmount = (float)MechanicSingleton.Instance.rand.Next(50, 100) / 100f;
            return new Color(Color.Lerp(Color.OrangeRed, Color.Firebrick, lerpAmount), alphaAmount);
        }

        private Vector2 GetVelocity()
        {
            var yVelocity = (float)MechanicSingleton.Instance.rand.Next(50, 300) / 1000f;
            return new Vector2(0f, -yVelocity);
        }

        private Vector2 GetEmitPosition() 
        {
            var spreadFactor = 0.2f / owner.PhysicsCollisionBox.Width;
            var minX = owner.PhysicsCollisionBox.Left - spreadFactor;
            var maxX = owner.PhysicsCollisionBox.Right + spreadFactor;
            var newX = MechanicSingleton.Instance.rand.Next((int)minX, (int)maxX);

            var newY = owner.PhysicsCollisionBox.Top;
            return new Vector2(newX, newY);
        }
    }
}
