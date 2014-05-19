using co_op_engine.Components.Particles.Decorators;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Particles
{
    class DustFastEmitter : Emitter
    {
        private GameObject Owner;
        private RectangleFloat? ExactHitSpot;

        public DustFastEmitter(GameObject owner, RectangleFloat? exactHitSpot = null)
        {
            this.Owner = owner;
            ExactHitSpot = exactHitSpot;
            this.duration = TimeSpan.FromMilliseconds(100);
            this.frequency = 5;
        }

        protected override void EmitParticle()
        {
            var particle = new Particle();
            particle.Texture = AssetRepository.Instance.PlainWhiteTexture;
            particle.Lifetime = TimeSpan.FromMilliseconds(300);
            particle.Position = GetEmitPosition();
            particle.Velocity = GetEmitVelocity();
            particle.DrawColor = new Color(Color.WhiteSmoke, GetAlpha());

            var withVariableSize = new VariableSizeDecorator(particle, 2, 10);

            ParticleEngine.Instance.Add(withVariableSize);
        }

        private float GetAlpha()
        {
            return (float)MechanicSingleton.Instance.rand.Next(2, 6) / 10f;
        }

        private Vector2 GetEmitPosition()
        {
            var spot = ExactHitSpot != null ? ExactHitSpot.Value.Center : Owner.Position;
            var spreadFactor = 7f;
            var minX = spot.X - spreadFactor;
            var maxX = spot.X + spreadFactor;
            var newX = MechanicSingleton.Instance.rand.Next((int)minX, (int)maxX);

            spreadFactor = 3f;
            var minY = spot.Y - spreadFactor;
            var maxY = spot.Y + spreadFactor;
            var newY = MechanicSingleton.Instance.rand.Next((int)minY, (int)maxY);

            return new Vector2(newX, newY);
        }

        private Vector2 GetEmitVelocity()
        {
            float velX = MechanicSingleton.Instance.rand.Next(-100, 100) / 1000f;
            float velY = MechanicSingleton.Instance.rand.Next(-500, -100) / 1000f;
            return new Vector2(velX, velY);
        }
    }
}
